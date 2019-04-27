// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrderItem.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Allors.Domain
{
    using System.Linq;

    using Meta;

    public partial class PurchaseOrderItem
    {
        #region Transitional
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.PurchaseOrderItem, M.PurchaseOrderItem.PurchaseOrderItemState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;
        #endregion

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

        public void AppsConfirm(OrderItemConfirm method)
        {
            this.PurchaseOrderItemState = new PurchaseOrderItemStates(this.Strategy.Session).InProcess;
        }

        public void AppsApprove(OrderItemApprove method)
        {
            this.PurchaseOrderItemState = new PurchaseOrderItemStates(this.Strategy.Session).InProcess;
        }

        public void CancelFromOrder()
        {
            this.PurchaseOrderItemState = new PurchaseOrderItemStates(this.Strategy.Session).CancelledByOrder;
        }

        public void AppsCancel(OrderItemCancel method)
        {
            this.PurchaseOrderItemState = new PurchaseOrderItemStates(this.Strategy.Session).Cancelled;
        }

        public void AppsReject(OrderItemReject method)
        {
            this.PurchaseOrderItemState = new PurchaseOrderItemStates(this.Strategy.Session).Rejected;
        }

        public void AppsOnBuild(ObjectOnBuild method)
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

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            // TODO:
            if (derivation.ChangeSet.Associations.Contains(this.Id))
            {
                if (this.ExistPurchaseOrderWherePurchaseOrderItem)
                {
                    derivation.AddDependency(this.PurchaseOrderWherePurchaseOrderItem, this);
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.AppsDeriveVatRegime(derivation);

            #region States
            var purchaseOrderItemShipmentStates = new PurchaseOrderItemShipmentStates(derivation.Session);
            var purchaseOrderItemPaymentStates = new PurchaseOrderItemPaymentStates(derivation.Session);
            var purchaseOrderItemStates = new PurchaseOrderItemStates(derivation.Session);

            if (this.IsValid)
            {
                // ShipmentState
                var quantityReceived = 0M;
                foreach (ShipmentReceipt shipmentReceipt in this.ShipmentReceiptsWhereOrderItem)
                {
                    quantityReceived += shipmentReceipt.QuantityAccepted;
                }

                this.QuantityReceived = quantityReceived;

                if (this.QuantityReceived == 0)
                {
                    this.PurchaseOrderItemShipmentState = new PurchaseOrderItemShipmentStates(this.Strategy.Session).NotReceived;
                }
                else
                {
                    this.PurchaseOrderItemShipmentState = this.QuantityReceived < this.QuantityOrdered ?
                        purchaseOrderItemShipmentStates.PartiallyReceived:
                        purchaseOrderItemShipmentStates.Received;
                }

                // PaymentState
                var orderBilling = this.OrderItemBillingsWhereOrderItem.Select(v => v.InvoiceItem).OfType<SalesInvoiceItem>().ToArray();

                if (orderBilling.Any())
                {
                    if (orderBilling.All(v => v.SalesInvoiceWhereSalesInvoiceItem.SalesInvoiceState.Paid))
                    {
                        this.PurchaseOrderItemPaymentState = purchaseOrderItemPaymentStates.Paid;
                    }
                    else if (orderBilling.All(v => !v.SalesInvoiceWhereSalesInvoiceItem.SalesInvoiceState.Paid))
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
                this.AppsOnDeriveQuantities(derivation);

                this.PreviousQuantity = this.QuantityOrdered;
            }

            if (this.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).Cancelled) ||
                this.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).Rejected))
            {
                this.AppsOnDeriveQuantities(derivation);
            }

            #endregion
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            if (this.PurchaseOrderItemShipmentState.IsReceived)
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.QuickReceive, Operations.Execute));
            }
        }

        public void AppsDeriveVatRegime(IDerivation derivation)
        {
            this.VatRegime = this.AssignedVatRegime ?? this.PurchaseOrderWherePurchaseOrderItem.VatRegime;
            this.VatRate = this.VatRegime?.VatRate;
        }

        public void AppsOnDeriveDeliveryDate(IDerivation derivation)
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

        public void AppsOnDerivePrices()
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

            this.UnitVat = this.ExistVatRate ? Math.Round((this.UnitPrice * this.VatRate.Rate) / 100, 2) : 0;
            this.TotalBasePrice = this.UnitBasePrice * this.QuantityOrdered;
            this.TotalDiscount = this.UnitDiscount * this.QuantityOrdered;
            this.TotalSurcharge = this.UnitSurcharge * this.QuantityOrdered;
            this.UnitPrice = this.UnitBasePrice - this.UnitDiscount + this.UnitSurcharge;
            this.TotalVat = this.UnitVat * this.QuantityOrdered;
            this.TotalExVat = this.UnitPrice * this.QuantityOrdered;
            this.TotalIncVat = this.TotalExVat + this.TotalVat;
        }

        public void AppsOnDeriveQuantities(IDerivation derivation)
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
    }
}