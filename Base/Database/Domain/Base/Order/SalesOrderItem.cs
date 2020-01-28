// <copyright file="SalesOrderItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Resources;

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Meta;

    public partial class SalesOrderItem
    {
        #region Transitional

        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
        {
            new TransitionalConfiguration(M.SalesOrderItem, M.SalesOrderItem.SalesOrderItemState),
            new TransitionalConfiguration(M.SalesOrderItem, M.SalesOrderItem.SalesOrderItemShipmentState),
            new TransitionalConfiguration(M.SalesOrderItem, M.SalesOrderItem.SalesOrderItemInvoiceState),
            new TransitionalConfiguration(M.SalesOrderItem, M.SalesOrderItem.SalesOrderItemPaymentState),
        };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        #endregion Transitional

        public bool IsValid => !(this.SalesOrderItemState.Cancelled || this.SalesOrderItemState.Rejected);

        public Part Part
        {
            get
            {
                if (this.ExistProduct)
                {
                    var nonUnifiedGood = this.Product as NonUnifiedGood;
                    var unifiedGood = this.Product as UnifiedGood;
                    return unifiedGood ?? nonUnifiedGood?.Part;
                }

                return null;
            }
        }

        public void BaseDelegateAccess(DelegatedAccessControlledObjectDelegateAccess method)
        {
            if (method.SecurityTokens == null)
            {
                method.SecurityTokens = this.SalesOrderWhereSalesOrderItem?.SecurityTokens.ToArray();
            }

            if (method.DeniedPermissions == null)
            {
                method.DeniedPermissions = this.SalesOrderWhereSalesOrderItem?.DeniedPermissions.ToArray();
            }
        }

        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistSalesOrderItemState)
            {
                this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).Created;
            }

            if (this.ExistProduct && !this.ExistInvoiceItemType)
            {
                this.InvoiceItemType = new InvoiceItemTypes(this.Strategy.Session).ProductItem;
            }

            if (!this.ExistSalesOrderItemShipmentState)
            {
                this.SalesOrderItemShipmentState = new SalesOrderItemShipmentStates(this.Strategy.Session).NotShipped;
            }

            if (!this.ExistSalesOrderItemInvoiceState)
            {
                this.SalesOrderItemInvoiceState = new SalesOrderItemInvoiceStates(this.Strategy.Session).NotInvoiced;
            }

            if (!this.ExistSalesOrderItemPaymentState)
            {
                this.SalesOrderItemPaymentState = new SalesOrderItemPaymentStates(this.Strategy.Session).NotPaid;
            }
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;
            var salesOrder = this.SalesOrderWhereSalesOrderItem;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {

                iteration.AddDependency(salesOrder, this);
                iteration.Mark(salesOrder);

                foreach (SalesOrderItem featureItem in this.OrderedWithFeatures)
                {
                    iteration.AddDependency(this, featureItem);
                    iteration.Mark(featureItem);
                }

                if (this.ExistReservedFromNonSerialisedInventoryItem)
                {
                    iteration.AddDependency(this.ReservedFromNonSerialisedInventoryItem, this);
                    iteration.Mark(this.ReservedFromNonSerialisedInventoryItem);
                }

                if (this.ExistReservedFromSerialisedInventoryItem)
                {
                    iteration.AddDependency(this.ReservedFromSerialisedInventoryItem, this);
                    iteration.Mark(this.ReservedFromSerialisedInventoryItem);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var salesOrder = this.SalesOrderWhereSalesOrderItem;

            var reasons = new InventoryTransactionReasons(this.Strategy.Session);

            this.QuantityPendingShipment = this.OrderShipmentsWhereOrderItem
                .Where(v => v.ExistShipmentItem
                            && !((CustomerShipment)v.ShipmentItem.ShipmentWhereShipmentItem).ShipmentState.Equals(new ShipmentStates(this.Session()).Shipped))
                .Sum(v => v.Quantity);

            this.QuantityShipped = this.OrderShipmentsWhereOrderItem
                .Where(v => v.ExistShipmentItem
                            && ((CustomerShipment)v.ShipmentItem.ShipmentWhereShipmentItem).ShipmentState.Equals(new ShipmentStates(this.Session()).Shipped))
                .Sum(v => v.Quantity);

            if (this.ExistSerialisedItem && this.QuantityOrdered != 1)
            {
                derivation.Validation.AddError(this, this.Meta.QuantityOrdered, ErrorMessages.SerializedItemQuantity);
            }

            if (this.QuantityOrdered < this.QuantityPendingShipment || this.QuantityOrdered < this.QuantityShipped)
            {
                derivation.Validation.AddError(this, M.SalesOrderItem.QuantityOrdered, ErrorMessages.SalesOrderItemQuantityToShipNowIsLargerThanQuantityRemaining);
            }

            if (this.SalesOrderItemInventoryAssignmentsWhereSalesOrderItem.FirstOrDefault() != null)
            {
                this.QuantityCommittedOut = this.SalesOrderItemInventoryAssignmentsWhereSalesOrderItem.SelectMany(v => v.InventoryItemTransactions).Where(t => t.Reason.Equals(reasons.Reservation)).Sum(v => v.Quantity);
            }
            else
            {
                this.QuantityCommittedOut = 0;
            }

            var salesOrderItemShipmentStates = new SalesOrderItemShipmentStates(derivation.Session);
            var salesOrderItemPaymentStates = new SalesOrderItemPaymentStates(derivation.Session);
            var salesOrderItemInvoiceStates = new SalesOrderItemInvoiceStates(derivation.Session);
            var salesOrderItemStates = new SalesOrderItemStates(derivation.Session);

            // SalesOrderItem States
            if (this.IsValid)
            {
                // ShipmentState
                if (!this.ExistOrderShipmentsWhereOrderItem)
                {
                    this.SalesOrderItemShipmentState = salesOrderItemShipmentStates.NotShipped;
                }
                else if (this.QuantityShipped == 0 && this.QuantityPendingShipment > 0)
                {
                    this.SalesOrderItemShipmentState = salesOrderItemShipmentStates.InProgress;
                }
                else
                {
                    this.SalesOrderItemShipmentState = this.QuantityShipped < this.QuantityOrdered ?
                                                                     salesOrderItemShipmentStates.PartiallyShipped :
                                                                     salesOrderItemShipmentStates.Shipped;
                }

                // PaymentState
                var orderBilling = this.OrderItemBillingsWhereOrderItem.Select(v => v.InvoiceItem).OfType<SalesInvoiceItem>().ToArray();

                if (orderBilling.Any())
                {
                    if (orderBilling.All(v => v.SalesInvoiceWhereSalesInvoiceItem.SalesInvoiceState.Paid))
                    {
                        this.SalesOrderItemPaymentState = salesOrderItemPaymentStates.Paid;
                    }
                    else if (orderBilling.All(v => !v.SalesInvoiceWhereSalesInvoiceItem.SalesInvoiceState.Paid))
                    {
                        this.SalesOrderItemPaymentState = salesOrderItemPaymentStates.NotPaid;
                    }
                    else
                    {
                        this.SalesOrderItemPaymentState = salesOrderItemPaymentStates.PartiallyPaid;
                    }
                }
                else
                {
                    var shipmentBilling = this.OrderShipmentsWhereOrderItem.SelectMany(v => v.ShipmentItem.ShipmentItemBillingsWhereShipmentItem).Select(v => v.InvoiceItem).OfType<SalesInvoiceItem>().ToArray();
                    if (shipmentBilling.Any())
                    {
                        if (shipmentBilling.All(v => v.SalesInvoiceWhereSalesInvoiceItem.SalesInvoiceState.Paid))
                        {
                            this.SalesOrderItemPaymentState = salesOrderItemPaymentStates.Paid;
                        }
                        else if (shipmentBilling.All(v => !v.SalesInvoiceWhereSalesInvoiceItem.SalesInvoiceState.Paid))
                        {
                            this.SalesOrderItemPaymentState = salesOrderItemPaymentStates.NotPaid;
                        }
                        else
                        {
                            this.SalesOrderItemPaymentState = salesOrderItemPaymentStates.PartiallyPaid;
                        }
                    }
                    else
                    {
                        this.SalesOrderItemPaymentState = salesOrderItemPaymentStates.NotPaid;
                    }
                }

                // InvoiceState
                var amountAlreadyInvoiced = this.OrderItemBillingsWhereOrderItem?.Sum(v => v.Amount);
                if (amountAlreadyInvoiced == 0)
                {
                    amountAlreadyInvoiced = this.OrderShipmentsWhereOrderItem
                        .SelectMany(orderShipment => orderShipment.ShipmentItem.ShipmentItemBillingsWhereShipmentItem)
                        .Aggregate(amountAlreadyInvoiced, (current, shipmentItemBilling) => current + shipmentItemBilling.Amount);
                }

                var leftToInvoice = this.TotalExVat - amountAlreadyInvoiced;

                if (amountAlreadyInvoiced == 0)
                {
                    this.SalesOrderItemInvoiceState = salesOrderItemInvoiceStates.NotInvoiced;
                }
                else if (amountAlreadyInvoiced > 0 && leftToInvoice > 0)
                {
                    this.SalesOrderItemInvoiceState = salesOrderItemInvoiceStates.PartiallyInvoiced;
                }
                else
                {
                    this.SalesOrderItemInvoiceState = salesOrderItemInvoiceStates.Invoiced;
                }

                // SalesOrderItem States
                if (this.SalesOrderItemShipmentState.Shipped && this.SalesOrderItemInvoiceState.Invoiced)
                {
                    this.SalesOrderItemState = salesOrderItemStates.Completed;
                }

                if (this.SalesOrderItemState.Completed && this.SalesOrderItemPaymentState.Paid)
                {
                    this.SalesOrderItemState = salesOrderItemStates.Finished;
                }
            }

            if (this.IsValid)
            {
                if (this.Part != null && salesOrder?.TakenBy != null)
                {
                    if (this.Part.InventoryItemKind.Serialised)
                    {
                        if (!this.ExistReservedFromSerialisedInventoryItem)
                        {
                            if (this.ExistSerialisedItem)
                            {
                                if (this.SerialisedItem.ExistSerialisedInventoryItemsWhereSerialisedItem)
                                {
                                    this.ReservedFromSerialisedInventoryItem = this.SerialisedItem.SerialisedInventoryItemsWhereSerialisedItem.FirstOrDefault(v => v.Quantity == 1);
                                }
                            }
                            else
                            {
                                var inventoryItems = this.Part.InventoryItemsWherePart;
                                inventoryItems.Filter.AddEquals(M.InventoryItem.Facility, salesOrder.OriginFacility);
                                this.ReservedFromSerialisedInventoryItem = inventoryItems.FirstOrDefault() as SerialisedInventoryItem;
                            }
                        }
                    }
                    else
                    {
                        if (!this.ExistReservedFromNonSerialisedInventoryItem)
                        {
                            var inventoryItems = this.Part.InventoryItemsWherePart;
                            inventoryItems.Filter.AddEquals(M.InventoryItem.Facility, salesOrder.OriginFacility);
                            this.ReservedFromNonSerialisedInventoryItem = inventoryItems.FirstOrDefault() as NonSerialisedInventoryItem;
                        }
                    }
                }

                if (this.SalesOrderItemState.InProcess && !this.SalesOrderItemShipmentState.Shipped)
                {
                    if (this.ExistReservedFromNonSerialisedInventoryItem)
                    {
                        if ((salesOrder.OrderKind?.ScheduleManually == true && this.QuantityPendingShipment > 0)
                            || !salesOrder.ExistOrderKind || salesOrder.OrderKind.ScheduleManually == false)
                        {
                            var committedOutSameProductOtherItem = salesOrder.SalesOrderItems.Where(v => !Equals(v, this) && Equals(v.Product, this.Product)).Sum(v => v.QuantityRequestsShipping);
                            var qoh = this.ReservedFromNonSerialisedInventoryItem.QuantityOnHand;
                            var atp = this.ReservedFromNonSerialisedInventoryItem.AvailableToPromise - committedOutSameProductOtherItem > 0 ? this.ReservedFromNonSerialisedInventoryItem.AvailableToPromise - committedOutSameProductOtherItem : 0;
                            var wantToShip = this.QuantityCommittedOut - this.QuantityPendingShipment;

                            var inventoryAssignment = this.SalesOrderItemInventoryAssignmentsWhereSalesOrderItem.FirstOrDefault(v => v.InventoryItem.Equals(this.ReservedFromNonSerialisedInventoryItem));
                            if (this.QuantityCommittedOut > qoh)
                            {
                                wantToShip = qoh;
                            }

                            if (this.ExistPreviousReservedFromNonSerialisedInventoryItem
                                && !Equals(this.ReservedFromNonSerialisedInventoryItem, this.PreviousReservedFromNonSerialisedInventoryItem))
                            {
                                var previousInventoryAssignment = this.SalesOrderItemInventoryAssignmentsWhereSalesOrderItem.FirstOrDefault(v => v.InventoryItem.Equals(this.PreviousReservedFromNonSerialisedInventoryItem));
                                previousInventoryAssignment.Quantity = 0;

                                foreach (OrderShipment orderShipment in this.OrderShipmentsWhereOrderItem)
                                {
                                    orderShipment.Delete();
                                }

                                this.QuantityCommittedOut = 0;
                                wantToShip = 0;
                            }

                            var neededFromInventory = this.QuantityOrdered - this.QuantityShipped - this.QuantityCommittedOut;
                            var availableFromInventory = neededFromInventory < atp ? neededFromInventory : atp;

                            if (neededFromInventory != 0
                                || this.QuantityShortFalled > 0
                                || !Equals(this.ReservedFromNonSerialisedInventoryItem, this.PreviousReservedFromNonSerialisedInventoryItem))
                            {
                                if (inventoryAssignment == null)
                                {
                                    new SalesOrderItemInventoryAssignmentBuilder(this.Session())
                                        .WithSalesOrderItem(this)
                                        .WithInventoryItem(this.ReservedFromNonSerialisedInventoryItem)
                                        .WithQuantity(wantToShip + availableFromInventory)
                                        .Build();
                                }
                                else
                                {
                                    inventoryAssignment.InventoryItem = this.ReservedFromNonSerialisedInventoryItem;
                                    if (this.QuantityCommittedOut > qoh)
                                    {
                                        inventoryAssignment.Quantity = qoh;
                                    }
                                    else
                                    {
                                        inventoryAssignment.Quantity = this.QuantityCommittedOut + availableFromInventory;
                                    }
                                }

                                this.QuantityRequestsShipping = wantToShip + availableFromInventory;

                                if (this.QuantityRequestsShipping > qoh)
                                {
                                    this.QuantityRequestsShipping = qoh;
                                }

                                if (salesOrder.OrderKind?.ScheduleManually == true)
                                {
                                    this.QuantityRequestsShipping = 0;
                                }

                                this.QuantityReserved = this.QuantityOrdered - this.QuantityShipped;
                                this.QuantityShortFalled = neededFromInventory - availableFromInventory > 0 ? neededFromInventory - availableFromInventory : 0;
                            }
                        }
                    }

                    if (this.ExistReservedFromSerialisedInventoryItem)
                    {
                        var inventoryAssignment = this.SalesOrderItemInventoryAssignmentsWhereSalesOrderItem.FirstOrDefault(v => v.InventoryItem.Equals(this.ReservedFromSerialisedInventoryItem));
                        if (inventoryAssignment == null)
                        {
                            new SalesOrderItemInventoryAssignmentBuilder(this.Session())
                                .WithSalesOrderItem(this)
                                .WithInventoryItem(this.ReservedFromSerialisedInventoryItem)
                                .WithQuantity(1)
                                .Build();

                            this.QuantityRequestsShipping = 1;
                        }

                        this.ReservedFromSerialisedInventoryItem.SerialisedInventoryItemState = new SerialisedInventoryItemStates(this.Strategy.Session).Assigned;
                        this.QuantityReserved = 1;
                    }
                }

                // TODO: Move to Custom
                if (derivation.ChangeSet.IsCreated(this) && this.ExistSerialisedItem)
                {
                    this.Description = this.SerialisedItem.Details;
                }
            }
        }

        public void BaseOnPostDerive(ObjectOnPostDerive method)
        {
            this.PreviousReservedFromNonSerialisedInventoryItem = this.ReservedFromNonSerialisedInventoryItem;
            this.PreviousQuantity = this.QuantityOrdered;
            this.PreviousProduct = this.Product;

            if (this.ExistSalesOrderItemInvoiceState && this.SalesOrderItemInvoiceState.Equals(new SalesOrderItemInvoiceStates(this.Strategy.Session).Invoiced))
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.QuantityOrdered, Operations.Write));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.AssignedUnitPrice, Operations.Write));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.InvoiceItemType, Operations.Write));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Product, Operations.Write));
            }
        }

        public void BaseCancel(OrderItemCancel method)
        {
            this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).Cancelled;
            this.OnCancelOrReject();
        }

        public void BaseConfirm(OrderItemConfirm method) => this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).InProcess;

        public void BaseReject(OrderItemReject method)
        {
            this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).Rejected;
            this.OnCancelOrReject();
        }

        public void BaseApprove(OrderItemApprove method) => this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).InProcess;

        public void BaseContinue(SalesOrderItemContinue method) => this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).InProcess;

        private void OnCancelOrReject()
        {
            if (this.ExistReservedFromNonSerialisedInventoryItem && this.ExistQuantityCommittedOut)
            {
                var inventoryAssignment = this.SalesOrderItemInventoryAssignmentsWhereSalesOrderItem.FirstOrDefault();
                if (inventoryAssignment != null)
                {
                    inventoryAssignment.Quantity = 0 - this.QuantityCommittedOut;
                }
            }

            if (this.ExistReservedFromSerialisedInventoryItem)
            {
                this.ReservedFromSerialisedInventoryItem.SerialisedInventoryItemState = new SerialisedInventoryItemStates(this.Strategy.Session).Available;
            }
        }
    }
}
