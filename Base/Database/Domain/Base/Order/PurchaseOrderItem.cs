// <copyright file="PurchaseOrderItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;

    using Allors.Meta;

    public partial class PurchaseOrderItem
    {
        #region Transitional

        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.PurchaseOrderItem, M.PurchaseOrderItem.PurchaseOrderItemState),
                new TransitionalConfiguration(M.PurchaseOrderItem, M.PurchaseOrderItem.PurchaseOrderItemShipmentState),
                new TransitionalConfiguration(M.PurchaseOrderItem, M.PurchaseOrderItem.PurchaseOrderItemPaymentState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        #endregion Transitional

        public bool IsValid => !(this.PurchaseOrderItemState.IsCancelled || this.PurchaseOrderItemState.IsRejected);

        public string SupplierReference
        {
            get
            {
                Extent<SupplierOffering> offerings = null;

                if (this.ExistPart)
                {
                    offerings = this.Part.SupplierOfferingsWherePart;
                }

                if (offerings != null)
                {
                    offerings.Filter.AddEquals(M.SupplierOffering.Supplier, this.PurchaseOrderWherePurchaseOrderItem.TakenViaSupplier);
                    foreach (SupplierOffering offering in offerings)
                    {
                        if (offering.FromDate <= this.PurchaseOrderWherePurchaseOrderItem.OrderDate &&
                            (!offering.ExistThroughDate || offering.ThroughDate >= this.PurchaseOrderWherePurchaseOrderItem.OrderDate))
                        {
                            return offering.SupplierProductId;
                        }
                    }
                }

                return string.Empty;
            }
        }

        public void BaseDelegateAccess(DelegatedAccessControlledObjectDelegateAccess method)
        {
            if (method.SecurityTokens == null)
            {
                method.SecurityTokens = this.SyncedOrder?.SecurityTokens.ToArray();
            }

            if (method.DeniedPermissions == null)
            {
                method.DeniedPermissions = this.SyncedOrder?.DeniedPermissions.ToArray();
            }
        }

        public void BaseConfirm(OrderItemConfirm method) => this.PurchaseOrderItemState = new PurchaseOrderItemStates(this.Strategy.Session).InProcess;

        public void BaseApprove(OrderItemApprove method) => this.PurchaseOrderItemState = new PurchaseOrderItemStates(this.Strategy.Session).InProcess;

        public void BaseQuickReceive(PurchaseOrderItemQuickReceive method)
        {
            var session = this.strategy.Session;

            if (this.ExistPart)
            {
                var shipment = new PurchaseShipmentBuilder(session)
                    .WithShipmentMethod(new ShipmentMethods(session).Ground)
                    .WithShipToParty(this.PurchaseOrderWherePurchaseOrderItem.OrderedBy)
                    .WithShipFromParty(this.PurchaseOrderWherePurchaseOrderItem.TakenViaSupplier)
                    .WithShipToFacility(this.PurchaseOrderWherePurchaseOrderItem.Facility)
                    .Build();

                var shipmentItem = new ShipmentItemBuilder(session)
                    .WithPart(this.Part)
                    .WithQuantity(this.QuantityOrdered)
                    .WithContentsDescription($"{this.QuantityOrdered} * {this.Part.Name}")
                    .Build();

                shipment.AddShipmentItem(shipmentItem);

                new OrderShipmentBuilder(session)
                    .WithOrderItem(this)
                    .WithShipmentItem(shipmentItem)
                    .WithQuantity(this.QuantityOrdered)
                    .Build();

                new ShipmentReceiptBuilder(session)
                    .WithQuantityAccepted(this.QuantityOrdered)
                    .WithShipmentItem(shipmentItem)
                    .WithOrderItem(this)
                    .Build();
            }
            else
            {
                this.QuantityReceived = 1;
            }
        }

        public void BaseCancel(OrderItemCancel method) => this.PurchaseOrderItemState = new PurchaseOrderItemStates(this.Strategy.Session).Cancelled;

        public void BaseReject(OrderItemReject method) => this.PurchaseOrderItemState = new PurchaseOrderItemStates(this.Strategy.Session).Rejected;

        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistPurchaseOrderItemState)
            {
                this.PurchaseOrderItemState = new PurchaseOrderItemStates(this.Strategy.Session).Created;
            }

            if (!this.ExistPurchaseOrderItemShipmentState)
            {
                this.PurchaseOrderItemShipmentState = new PurchaseOrderItemShipmentStates(this.Strategy.Session).NotReceived;
            }

            if (!this.ExistPurchaseOrderItemPaymentState)
            {
                this.PurchaseOrderItemPaymentState = new PurchaseOrderItemPaymentStates(this.Strategy.Session).NotPaid;
            }
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            // TODO:
            // if (derivation.ChangeSet.Associations.Contains(this.Id))
            // {
            if (this.ExistPurchaseOrderWherePurchaseOrderItem)
            {
                derivation.AddDependency(this.PurchaseOrderWherePurchaseOrderItem, this);
            }

            // }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.BaseDeriveVatRegime(derivation);

            if (this.ExistPart && this.Part.InventoryItemKind.Serialised)
            {
                derivation.Validation.AssertAtLeastOne(this, M.PurchaseOrderItem.SerialisedItem, M.PurchaseOrderItem.SerialNumber);
                derivation.Validation.AssertExistsAtMostOne(this, M.PurchaseOrderItem.SerialisedItem, M.PurchaseOrderItem.SerialNumber);
            }

            if ((this.IsValid && !this.ExistOrderItemBillingsWhereOrderItem &&
                this.PurchaseOrderItemShipmentState.IsReceived) || this.PurchaseOrderItemShipmentState.IsPartiallyReceived || (!this.ExistPart && this.QuantityReceived == 1))
            {
                this.CanInvoice = true;
            }
            else
            {
                this.CanInvoice = false;
            }

            var purchaseOrderItemShipmentStates = new PurchaseOrderItemShipmentStates(derivation.Session);
            var purchaseOrderItemPaymentStates = new PurchaseOrderItemPaymentStates(derivation.Session);
            var purchaseOrderItemStates = new PurchaseOrderItemStates(derivation.Session);

            if (this.IsValid)
            {
                // ShipmentState
                if (this.ExistPart)
                {
                    var quantityReceived = 0M;
                    foreach (ShipmentReceipt shipmentReceipt in this.ShipmentReceiptsWhereOrderItem)
                    {
                        quantityReceived += shipmentReceipt.QuantityAccepted;
                    }

                    this.QuantityReceived = quantityReceived;
                }

                if (this.QuantityReceived == 0)
                {
                    this.PurchaseOrderItemShipmentState = new PurchaseOrderItemShipmentStates(this.Strategy.Session).NotReceived;
                }
                else
                {
                    this.PurchaseOrderItemShipmentState = this.QuantityReceived < this.QuantityOrdered ?
                        purchaseOrderItemShipmentStates.PartiallyReceived :
                        purchaseOrderItemShipmentStates.Received;
                }

                // PaymentState
                var orderBilling = this.OrderItemBillingsWhereOrderItem.Select(v => v.InvoiceItem).OfType<PurchaseInvoiceItem>().ToArray();

                if (orderBilling.Any())
                {
                    if (orderBilling.All(v => v.PurchaseInvoiceWherePurchaseInvoiceItem.PurchaseInvoiceState.IsPaid))
                    {
                        this.PurchaseOrderItemPaymentState = purchaseOrderItemPaymentStates.Paid;
                    }
                    else if (orderBilling.All(v => !v.PurchaseInvoiceWherePurchaseInvoiceItem.PurchaseInvoiceState.IsPaid))
                    {
                        this.PurchaseOrderItemPaymentState = purchaseOrderItemPaymentStates.NotPaid;
                    }
                    else
                    {
                        this.PurchaseOrderItemPaymentState = purchaseOrderItemPaymentStates.PartiallyPaid;
                    }
                }

                // PurchaseOrderItem States
                if (this.PurchaseOrderItemShipmentState.IsReceived)
                {
                    this.PurchaseOrderItemState = purchaseOrderItemStates.Completed;
                }

                if (this.PurchaseOrderItemState.IsCompleted && this.PurchaseOrderItemPaymentState.IsPaid)
                {
                    this.PurchaseOrderItemState = purchaseOrderItemStates.Finished;
                }
            }

            if (this.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).InProcess))
            {
                this.BaseOnDeriveQuantities(derivation);

                this.PreviousQuantity = this.QuantityOrdered;
            }

            if (this.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).Cancelled) ||
                this.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).Rejected))
            {
                this.BaseOnDeriveQuantities(derivation);
            }
        }

        public void BaseOnPostDerive(ObjectOnPostDerive method)
        {
            if (this.PurchaseOrderItemShipmentState.IsReceived)
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.QuickReceive, Operations.Execute));
            }
        }

        public void BaseDeriveVatRegime(IDerivation derivation)
        {
            this.VatRegime = this.AssignedVatRegime ?? this.PurchaseOrderWherePurchaseOrderItem.VatRegime;
            this.VatRate = this.VatRegime?.VatRate;
        }

        public void BaseOnDeriveDeliveryDate(IDerivation derivation)
        {
            if (this.AssignedDeliveryDate.HasValue)
            {
                this.DeliveryDate = this.AssignedDeliveryDate.Value;
            }
            else if (this.PurchaseOrderWherePurchaseOrderItem.DeliveryDate.HasValue)
            {
                this.DeliveryDate = this.PurchaseOrderWherePurchaseOrderItem.DeliveryDate.Value;
            }
        }

        public void BaseOnDerivePrices()
        {
            this.UnitBasePrice = 0;
            this.UnitDiscount = 0;
            this.UnitSurcharge = 0;

            if (this.AssignedUnitPrice.HasValue)
            {
                this.UnitBasePrice = this.AssignedUnitPrice.Value;
                this.UnitPrice = this.AssignedUnitPrice.Value;
            }
            else
            {
                var order = this.PurchaseOrderWherePurchaseOrderItem;
                this.UnitBasePrice = new SupplierOfferings(this.Strategy.Session).PurchasePrice(order.TakenViaSupplier, order.OrderDate, this.Part);
            }

            this.UnitVat = this.ExistVatRate ? Math.Round(this.UnitPrice * this.VatRate.Rate / 100, 2) : 0;
            this.TotalBasePrice = this.UnitBasePrice * this.QuantityOrdered;
            this.TotalDiscount = this.UnitDiscount * this.QuantityOrdered;
            this.TotalSurcharge = this.UnitSurcharge * this.QuantityOrdered;
            this.UnitPrice = this.UnitBasePrice - this.UnitDiscount + this.UnitSurcharge;
            this.TotalVat = this.UnitVat * this.QuantityOrdered;
            this.TotalExVat = this.UnitPrice * this.QuantityOrdered;
            this.TotalIncVat = this.TotalExVat + this.TotalVat;
        }

        public void BaseOnDeriveQuantities(IDerivation derivation)
        {
            NonSerialisedInventoryItem inventoryItem = null;

            if (this.ExistPart)
            {
                var inventoryItems = this.Part.InventoryItemsWherePart;
                inventoryItems.Filter.AddEquals(M.InventoryItem.Facility, this.PurchaseOrderWherePurchaseOrderItem.Facility);
                inventoryItem = inventoryItems.First as NonSerialisedInventoryItem;
            }

            if (this.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).InProcess))
            {
                if (!this.ExistPreviousQuantity || !this.QuantityOrdered.Equals(this.PreviousQuantity))
                {
                    if (inventoryItem != null)
                    {
                        inventoryItem.OnDerive(x => x.WithDerivation(derivation));
                    }
                }
            }

            if (this.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).Cancelled) ||
                this.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).Rejected))
            {
                if (inventoryItem != null)
                {
                    inventoryItem.OnDerive(x => x.WithDerivation(derivation));
                }
            }
        }

        public void Sync(Order order) => this.SyncedOrder = order;
    }
}
