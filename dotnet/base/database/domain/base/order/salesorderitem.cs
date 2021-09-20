// <copyright file="SalesOrderItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using Resources;
    using System.Linq;
    using Allors.Meta;
    using System.Text;
    using System.Collections.Generic;

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

        public bool IsValid => !(this.SalesOrderItemState.IsCancelled || this.SalesOrderItemState.IsRejected);

        public bool WasValid => this.ExistLastObjectStates && !(this.LastSalesOrderItemState.IsCancelled || this.LastSalesOrderItemState.IsRejected);

        internal bool IsDeletable =>
            (this.SalesOrderItemState.Equals(new SalesOrderItemStates(this.Strategy.Session).Provisional)
                || this.SalesOrderItemState.Equals(new SalesOrderItemStates(this.Strategy.Session).ReadyForPosting)
                || this.SalesOrderItemState.Equals(new SalesOrderItemStates(this.Strategy.Session).Cancelled)
                || this.SalesOrderItemState.Equals(new SalesOrderItemStates(this.Strategy.Session).Rejected))
            && !this.ExistOrderItemBillingsWhereOrderItem
            && !this.ExistOrderShipmentsWhereOrderItem
            && !this.ExistOrderRequirementCommitmentsWhereOrderItem
            && !this.ExistWorkEffortsWhereOrderItemFulfillment;

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
                method.SecurityTokens = this.SyncedOrder?.SecurityTokens.ToArray();
            }

            if (method.Restrictions == null)
            {
                method.Restrictions = this.SyncedOrder?.Restrictions.ToArray();
            }
        }

        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistSalesOrderItemState)
            {
                this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).Provisional;
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

        public void BaseOnInit(ObjectOnInit method)
        {
            if (this.ExistProduct && !this.ExistInvoiceItemType)
            {
                this.InvoiceItemType = new InvoiceItemTypes(this.Strategy.Session).ProductItem;
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
                    iteration.AddDependency(this, this.ReservedFromNonSerialisedInventoryItem);
                    iteration.Mark(this.ReservedFromNonSerialisedInventoryItem);
                }

                if (this.ExistReservedFromSerialisedInventoryItem)
                {
                    iteration.AddDependency(this, this.ReservedFromSerialisedInventoryItem);
                    iteration.Mark(this.ReservedFromSerialisedInventoryItem);
                }

                if (this.ExistSerialisedItem)
                {
                    iteration.AddDependency(this.SerialisedItem, this);
                    iteration.Mark(this.SerialisedItem);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var salesOrder = this.SalesOrderWhereSalesOrderItem;
            var shipped = new ShipmentStates(this.Session()).Shipped;

            if (this.SalesOrderItemState.IsProvisional)
            {
                this.DerivedShipFromAddress = this.AssignedShipFromAddress ?? salesOrder.DerivedShipFromAddress;
                this.DerivedShipToAddress = this.AssignedShipToAddress ?? this.AssignedShipToParty?.ShippingAddress ?? salesOrder.DerivedShipToAddress;
                this.DerivedShipToParty = this.AssignedShipToParty ?? salesOrder.ShipToCustomer;
                this.DerivedDeliveryDate = this.AssignedDeliveryDate ?? salesOrder.DeliveryDate;
                this.DerivedVatRegime = this.AssignedVatRegime ?? salesOrder.DerivedVatRegime;
                this.VatRate = this.DerivedVatRegime?.VatRates.First(v => v.FromDate <= salesOrder.OrderDate && (!v.ExistThroughDate || v.ThroughDate >= salesOrder.OrderDate));
                this.DerivedIrpfRegime = this.AssignedIrpfRegime ?? salesOrder.DerivedIrpfRegime;
                this.IrpfRate = this.DerivedIrpfRegime?.IrpfRates.First(v => v.FromDate <= salesOrder.OrderDate && (!v.ExistThroughDate || v.ThroughDate >= salesOrder.OrderDate));
            }

            if (!this.ExistDerivationTrigger)
            {
                this.DerivationTrigger = Guid.NewGuid();
            }

            if (this.ExistSerialisedItem && !this.ExistNextSerialisedItemAvailability)
            {
                derivation.Validation.AssertExists(this, this.Meta.NextSerialisedItemAvailability);
            }

            if (this.Part != null && this.Part.InventoryItemKind.IsSerialised && this.QuantityOrdered != 1)
            {
                derivation.Validation.AddError(this, M.SalesOrderItem.QuantityOrdered, ErrorMessages.InvalidQuantity);
            }

            if (this.Part != null && this.Part.InventoryItemKind.IsNonSerialised && this.QuantityOrdered == 0)
            {
                derivation.Validation.AddError(this, M.SalesOrderItem.QuantityOrdered, ErrorMessages.InvalidQuantity);
            }

            if (this.ExistInvoiceItemType && this.InvoiceItemType.MaxQuantity.HasValue && this.QuantityOrdered > this.InvoiceItemType.MaxQuantity.Value)
            {
                derivation.Validation.AddError(this, M.SalesOrderItem.QuantityOrdered, ErrorMessages.InvalidQuantity);
            }

            var salesOrderItemShipmentStates = new SalesOrderItemShipmentStates(derivation.Session);
            var salesOrderItemPaymentStates = new SalesOrderItemPaymentStates(derivation.Session);
            var salesOrderItemInvoiceStates = new SalesOrderItemInvoiceStates(derivation.Session);
            var salesOrderItemStates = new SalesOrderItemStates(derivation.Session);

            // Shipments
            this.QuantityPendingShipment = this.OrderShipmentsWhereOrderItem
                .Where(v => v.ExistShipmentItem && !((CustomerShipment)v.ShipmentItem.ShipmentWhereShipmentItem).ShipmentState.Equals(shipped))
                .Sum(v => v.Quantity);

            this.QuantityShipped = this.OrderShipmentsWhereOrderItem
                .Where(v => v.ExistShipmentItem && ((CustomerShipment)v.ShipmentItem.ShipmentWhereShipmentItem).ShipmentState.Equals(shipped))
                .Sum(v => v.Quantity);

            if (this.SalesOrderItemState.IsInProcess
                && this.ExistPreviousReservedFromNonSerialisedInventoryItem
                && this.ReservedFromNonSerialisedInventoryItem != this.PreviousReservedFromNonSerialisedInventoryItem)
            {
                derivation.Validation.AddError(this, this.Meta.ReservedFromNonSerialisedInventoryItem, ErrorMessages.ReservedFromNonSerialisedInventoryItem);
            }

            if (this.ExistSerialisedItem && this.QuantityOrdered != 1)
            {
                derivation.Validation.AddError(this, this.Meta.QuantityOrdered, ErrorMessages.SerializedItemQuantity);
            }

            if (this.QuantityOrdered < this.QuantityPendingShipment || this.QuantityOrdered < this.QuantityShipped)
            {
                derivation.Validation.AddError(this, M.SalesOrderItem.QuantityOrdered, ErrorMessages.SalesOrderItemQuantityToShipNowIsLargerThanQuantityRemaining);
            }

            if (this.SalesOrderItemInventoryAssignments.FirstOrDefault() != null)
            {
                this.QuantityCommittedOut = this.SalesOrderItemInventoryAssignments
                    .SelectMany(v => v.InventoryItemTransactions)
                    .Where(t => t.Reason.Equals(new InventoryTransactionReasons(this.Session()).Reservation))
                    .Sum(v => v.Quantity);
            }
            else
            {
                this.QuantityCommittedOut = 0;
            }

            // SalesOrderItem States
            if (this.IsValid)
            {
                if (salesOrder.SalesOrderState.IsProvisional)
                {
                    this.SalesOrderItemState = salesOrderItemStates.Provisional;
                }

                if (salesOrder.SalesOrderState.IsReadyForPosting &&
                    (this.SalesOrderItemState.IsProvisional || this.SalesOrderItemState.IsRequestsApproval || this.SalesOrderItemState.IsOnHold))
                {
                    this.SalesOrderItemState = salesOrderItemStates.ReadyForPosting;
                }

                if (salesOrder.SalesOrderState.IsRequestsApproval &&
                    (this.SalesOrderItemState.IsProvisional || this.SalesOrderItemState.IsOnHold))
                {
                    this.SalesOrderItemState = salesOrderItemStates.RequestsApproval;
                }

                if (salesOrder.SalesOrderState.IsAwaitingAcceptance
                    && this.SalesOrderItemState.IsReadyForPosting)
                {
                    this.SalesOrderItemState = salesOrderItemStates.AwaitingAcceptance;
                }

                if (salesOrder.SalesOrderState.IsInProcess
                    && this.SalesOrderItemState.IsAwaitingAcceptance || this.SalesOrderItemState.IsOnHold)
                {
                    this.SalesOrderItemState = salesOrderItemStates.InProcess;
                }

                if (salesOrder.SalesOrderState.IsOnHold && this.SalesOrderItemState.IsInProcess)
                {
                    this.SalesOrderItemState = salesOrderItemStates.OnHold;
                }

                if (salesOrder.SalesOrderState.IsFinished)
                {
                    this.SalesOrderItemState = salesOrderItemStates.Finished;
                }

                if (salesOrder.SalesOrderState.IsCancelled)
                {
                    this.SalesOrderItemState = salesOrderItemStates.Cancelled;
                }

                if (salesOrder.SalesOrderState.IsRejected)
                {
                    this.SalesOrderItemState = salesOrderItemStates.Rejected;
                }
            }

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
                    if (orderBilling.All(v => v.SalesInvoiceWhereSalesInvoiceItem.SalesInvoiceState.IsPaid))
                    {
                        this.SalesOrderItemPaymentState = salesOrderItemPaymentStates.Paid;
                    }
                    else if (orderBilling.Any(v => v.SalesInvoiceWhereSalesInvoiceItem.SalesInvoiceState.IsPartiallyPaid))
                    {
                        this.SalesOrderItemPaymentState = salesOrderItemPaymentStates.PartiallyPaid;
                    }
                    else
                    {
                        this.SalesOrderItemPaymentState = salesOrderItemPaymentStates.NotPaid;
                    }
                }
                else
                {
                    var shipmentBilling = this.OrderShipmentsWhereOrderItem.SelectMany(v => v.ShipmentItem.ShipmentItemBillingsWhereShipmentItem).Select(v => v.InvoiceItem).OfType<SalesInvoiceItem>().ToArray();
                    if (shipmentBilling.Any())
                    {
                        if (shipmentBilling.All(v => v.SalesInvoiceWhereSalesInvoiceItem.SalesInvoiceState.IsPaid))
                        {
                            this.SalesOrderItemPaymentState = salesOrderItemPaymentStates.Paid;
                        }
                        else if (shipmentBilling.Any(v => v.SalesInvoiceWhereSalesInvoiceItem.SalesInvoiceState.IsPartiallyPaid))
                        {
                            this.SalesOrderItemPaymentState = salesOrderItemPaymentStates.PartiallyPaid;
                        }
                        else
                        {
                            this.SalesOrderItemPaymentState = salesOrderItemPaymentStates.NotPaid;
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

                if (this.SalesOrderItemState.IsCompleted && this.SalesOrderItemPaymentState.Paid)
                {
                    this.SalesOrderItemState = salesOrderItemStates.Finished;
                }
            }

            this.CalculatePrice(derivation, salesOrder);

            if (!this.IsValid && this.WasValid)
            {
                if (this.ExistReservedFromNonSerialisedInventoryItem && this.ExistQuantityCommittedOut)
                {
                    var inventoryAssignment = this.SalesOrderItemInventoryAssignments.FirstOrDefault();
                    if (inventoryAssignment != null)
                    {
                        inventoryAssignment.Quantity = 0 - this.QuantityCommittedOut;
                    }
                }
            }

            // TODO: Move to Custom
            if (derivation.ChangeSet.IsCreated(this) && !this.ExistDescription)
            {
                if (this.ExistSerialisedItem)
                {
                    var builder = new StringBuilder();
                    var part = this.SerialisedItem.PartWhereSerialisedItem;

                    if (part != null && part.ExistManufacturedBy)
                    {
                        builder.Append($", Manufacturer: {part.ManufacturedBy.PartyName}");
                    }

                    if (part != null && part.ExistBrand)
                    {
                        builder.Append($", Brand: {part.Brand.Name}");
                    }

                    if (part != null && part.ExistModel)
                    {
                        builder.Append($", Model: {part.Model.Name}");
                    }

                    builder.Append($", SN: {this.SerialisedItem.SerialNumber}");

                    if (this.SerialisedItem.ExistManufacturingYear)
                    {
                        builder.Append($", YOM: {this.SerialisedItem.ManufacturingYear}");
                    }

                    foreach (SerialisedItemCharacteristic characteristic in this.SerialisedItem.SerialisedItemCharacteristics)
                    {
                        if (characteristic.ExistValue)
                        {
                            var characteristicType = characteristic.SerialisedItemCharacteristicType;
                            if (characteristicType.ExistUnitOfMeasure)
                            {
                                var uom = characteristicType.UnitOfMeasure.ExistAbbreviation
                                                ? characteristicType.UnitOfMeasure.Abbreviation
                                                : characteristicType.UnitOfMeasure.Name;
                                builder.Append(
                                    $", {characteristicType.Name}: {characteristic.Value} {uom}");
                            }
                            else
                            {
                                builder.Append($", {characteristicType.Name}: {characteristic.Value}");
                            }
                        }
                    }

                    var details = builder.ToString();

                    if (details.StartsWith(","))
                    {
                        details = details.Substring(2);
                    }

                    this.Description = details;

                }
                else if (this.ExistProduct && this.Product is UnifiedGood unifiedGood)
                {
                    var builder = new StringBuilder();

                    if (unifiedGood != null && unifiedGood.ExistManufacturedBy)
                    {
                        builder.Append($", Manufacturer: {unifiedGood.ManufacturedBy.PartyName}");
                    }

                    if (unifiedGood != null && unifiedGood.ExistBrand)
                    {
                        builder.Append($", Brand: {unifiedGood.Brand.Name}");
                    }

                    if (unifiedGood != null && unifiedGood.ExistModel)
                    {
                        builder.Append($", Model: {unifiedGood.Model.Name}");
                    }

                    foreach (SerialisedItemCharacteristic characteristic in unifiedGood.SerialisedItemCharacteristics)
                    {
                        if (characteristic.ExistValue)
                        {
                            var characteristicType = characteristic.SerialisedItemCharacteristicType;
                            if (characteristicType.ExistUnitOfMeasure)
                            {
                                var uom = characteristicType.UnitOfMeasure.ExistAbbreviation
                                                ? characteristicType.UnitOfMeasure.Abbreviation
                                                : characteristicType.UnitOfMeasure.Name;
                                builder.Append($", {characteristicType.Name}: {characteristic.Value} {uom}");
                            }
                            else
                            {
                                builder.Append($", {characteristicType.Name}: {characteristic.Value}");
                            }
                        }
                    }

                    var details = builder.ToString();

                    if (details.StartsWith(","))
                    {
                        details = details.Substring(2);
                    }

                    this.Description = details;
                }
            }

            this.Sync();
        }

        private void Sync()
        {
            if (this.IsValid)
            {
                var salesOrder = this.SalesOrderWhereSalesOrderItem;

                if (this.Part != null && salesOrder?.TakenBy != null)
                {
                    if (this.Part.InventoryItemKind.IsSerialised)
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

                if (this.SalesOrderItemState.IsInProcess && !this.SalesOrderItemShipmentState.Shipped)
                {
                    if (this.ExistReservedFromNonSerialisedInventoryItem)
                    {
                        if ((salesOrder.OrderKind?.ScheduleManually == true && this.QuantityPendingShipment > 0)
                            || !salesOrder.ExistOrderKind || salesOrder.OrderKind.ScheduleManually == false)
                        {
                            var committedOutSameProductOtherItem = salesOrder.SalesOrderItems
                                .Where(v => !Equals(v, this) && Equals(v.Product, this.Product))
                                .Sum(v => v.QuantityRequestsShipping);

                            var qoh = this.ReservedFromNonSerialisedInventoryItem.QuantityOnHand;

                            var atp = this.ReservedFromNonSerialisedInventoryItem.AvailableToPromise - committedOutSameProductOtherItem > 0 ?
                                this.ReservedFromNonSerialisedInventoryItem.AvailableToPromise - committedOutSameProductOtherItem :
                                0;

                            var quantityCommittedOut = this.SalesOrderItemInventoryAssignments
                                .SelectMany(v => v.InventoryItemTransactions)
                                .Where(t => t.Reason.Equals(new InventoryTransactionReasons(this.Session()).Reservation))
                                .Sum(v => v.Quantity);

                            var wantToShip = quantityCommittedOut - this.QuantityPendingShipment;

                            var inventoryAssignment = this.SalesOrderItemInventoryAssignments.FirstOrDefault(v => v.InventoryItem.Equals(this.ReservedFromNonSerialisedInventoryItem));
                            if (quantityCommittedOut > qoh)
                            {
                                wantToShip = qoh;
                            }

                            //if (this.ExistPreviousReservedFromNonSerialisedInventoryItem
                            //    && !Equals(this.ReservedFromNonSerialisedInventoryItem, this.PreviousReservedFromNonSerialisedInventoryItem))
                            //{
                            //    var previousInventoryAssignment = this.SalesOrderItemInventoryAssignments.FirstOrDefault(v => v.InventoryItem.Equals(this.PreviousReservedFromNonSerialisedInventoryItem));
                            //    previousInventoryAssignment.Quantity = 0;

                            //    foreach (OrderShipment orderShipment in this.OrderShipmentsWhereOrderItem)
                            //    {
                            //        orderShipment.Delete();
                            //    }

                            //    quantityCommittedOut = 0;
                            //    wantToShip = 0;
                            //}

                            var neededFromInventory = this.QuantityOrdered - this.QuantityShipped - quantityCommittedOut;
                            var availableFromInventory = neededFromInventory < atp ? neededFromInventory : atp;

                            if (neededFromInventory != 0 || this.QuantityShortFalled > 0)
                            {
                                if (inventoryAssignment == null)
                                {
                                    var salesOrderItemInventoryAssignment = new SalesOrderItemInventoryAssignmentBuilder(this.Session())
                                        .WithInventoryItem(this.ReservedFromNonSerialisedInventoryItem)
                                        .WithQuantity(wantToShip + availableFromInventory)
                                        .Build();

                                    this.AddSalesOrderItemInventoryAssignment(salesOrderItemInventoryAssignment);
                                }
                                else
                                {
                                    inventoryAssignment.InventoryItem = this.ReservedFromNonSerialisedInventoryItem;
                                    if (quantityCommittedOut > qoh)
                                    {
                                        inventoryAssignment.Quantity = qoh;
                                    }
                                    else
                                    {
                                        inventoryAssignment.Quantity = quantityCommittedOut + availableFromInventory;
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
                        var inventoryAssignment = this.SalesOrderItemInventoryAssignments.FirstOrDefault(v => v.InventoryItem.Equals(this.ReservedFromSerialisedInventoryItem));
                        if (inventoryAssignment == null)
                        {
                            var salesOrderItemInventoryAssignment = new SalesOrderItemInventoryAssignmentBuilder(this.Session())
                                 .WithInventoryItem(this.ReservedFromSerialisedInventoryItem)
                                 .WithQuantity(1)
                                 .Build();

                            this.AddSalesOrderItemInventoryAssignment(salesOrderItemInventoryAssignment);

                            this.QuantityRequestsShipping = 1;
                        }

                        this.QuantityReserved = 1;
                    }
                }
            }
        }

        public void BaseOnPostDerive(ObjectOnPostDerive method)
        {
            var derivation = method.Derivation;

            this.PreviousReservedFromNonSerialisedInventoryItem = this.ReservedFromNonSerialisedInventoryItem;
            this.PreviousQuantity = this.QuantityOrdered;
            this.PreviousProduct = this.Product;

            if (this.ExistReservedFromNonSerialisedInventoryItem)
            {
                if (!this.ReservedFromNonSerialisedInventoryItem.Equals(this.PreviousReservedFromNonSerialisedInventoryItem))
                {
                    derivation.Mark(this.PreviousReservedFromNonSerialisedInventoryItem);
                }
            }

            if (!this.SalesOrderItemInvoiceState.NotInvoiced || !this.SalesOrderItemShipmentState.NotShipped)
            {
                var deniablePermissionByOperandTypeId = new Dictionary<Guid, Permission>();

                foreach (Permission permission in this.Session().Extent<Permission>())
                {
                    if (permission.ConcreteClassPointer == this.strategy.Class.Id
                        && (permission.Operation == Operations.Write || permission.Operation == Operations.Execute))
                    {
                        deniablePermissionByOperandTypeId.Add(permission.OperandTypePointer, permission);
                    }
                }

                foreach (var keyValuePair in deniablePermissionByOperandTypeId)
                {
                    this.AddDeniedPermission(keyValuePair.Value);
                }
            }

            var deletePermission = new Permissions(this.Strategy.Session).Get(this.Meta.ObjectType, this.Meta.Delete, Operations.Execute);
            if (this.IsDeletable)
            {
                this.RemoveDeniedPermission(deletePermission);
            }
            else
            {
                this.AddDeniedPermission(deletePermission);
            }
        }

        public void BaseDelete(SalesOrderItemDelete method)
        {
            foreach (SalesTerm salesTerm in this.SalesTerms)
            {
                salesTerm.Delete();
            }

            if (this.ExistSerialisedItem)
            {
                this.SerialisedItem.DerivationTrigger = Guid.NewGuid();
            }
        }

        public void BaseCancel(OrderItemCancel method) => this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).Cancelled;

        public void BaseReject(OrderItemReject method) => this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).Rejected;

        public void BaseApprove(OrderItemApprove method) => this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).ReadyForPosting;

        public void BaseReopen(OrderItemReopen method) => this.SalesOrderItemState = this.PreviousSalesOrderItemState;

        public void SyncPrices(IDerivation derivation, SalesOrder salesOrder) => this.CalculatePrice(derivation, salesOrder, true);

        public void Sync(Order order) => this.SyncedOrder = order;

        private void CalculatePrice(IDerivation derivation, SalesOrder salesOrder, bool useValueOrdered = false)
        {
            var sameProductItems = salesOrder.SalesOrderItems
                .Where(v => v.IsValid && v.ExistProduct && v.Product.Equals(this.Product))
                .ToArray();

            var quantityOrdered = sameProductItems.Sum(w => w.QuantityOrdered);
            var valueOrdered = useValueOrdered ? sameProductItems.Sum(w => w.TotalBasePrice) : 0;

            var orderPriceComponents = new PriceComponents(this.Session()).CurrentPriceComponents(salesOrder.OrderDate);
            var orderItemPriceComponents = Array.Empty<PriceComponent>();
            if (this.ExistProduct)
            {
                orderItemPriceComponents = this.Product.GetPriceComponents(orderPriceComponents);
            }
            else if (this.ExistProductFeature)
            {
                orderItemPriceComponents = this.ProductFeature.GetPriceComponents(this.SalesOrderItemWhereOrderedWithFeature.Product, orderPriceComponents);
            }

            var priceComponents = orderItemPriceComponents.Where(
                v => PriceComponents.BaseIsApplicable(
                    new PriceComponents.IsApplicable
                    {
                        PriceComponent = v,
                        Customer = salesOrder.BillToCustomer,
                        Product = this.Product,
                        SalesOrder = salesOrder,
                        QuantityOrdered = quantityOrdered,
                        ValueOrdered = valueOrdered,
                    })).ToArray();

            var unitBasePrice = priceComponents.OfType<BasePrice>().Min(v => v.Price);

            // Calculate Unit Price (with Discounts and Surcharges)
            if (this.AssignedUnitPrice.HasValue)
            {
                this.UnitBasePrice = unitBasePrice ?? this.AssignedUnitPrice.Value;
                this.UnitDiscount = 0;
                this.UnitSurcharge = 0;
                this.UnitPrice = this.AssignedUnitPrice.Value;
            }
            else
            {
                if (!unitBasePrice.HasValue)
                {
                    derivation.Validation.AddError(this, M.SalesOrderItem.UnitBasePrice, "No BasePrice with a Price");
                    return;
                }

                this.UnitBasePrice = unitBasePrice.Value;

                this.UnitDiscount = priceComponents.OfType<DiscountComponent>().Sum(
                    v => v.Percentage.HasValue
                        ? this.UnitBasePrice * v.Percentage.Value / 100
                        : v.Price ?? 0);

                this.UnitSurcharge = priceComponents.OfType<SurchargeComponent>().Sum(
                    v => v.Percentage.HasValue
                        ? this.UnitBasePrice * v.Percentage.Value / 100
                        : v.Price ?? 0);

                this.UnitPrice = this.UnitBasePrice - this.UnitDiscount + this.UnitSurcharge;

                foreach(OrderAdjustment orderAdjustment in this.DiscountAdjustments)
                {
                    this.UnitDiscount += orderAdjustment.Percentage.HasValue
                        ? this.UnitPrice * orderAdjustment.Percentage.Value / 100
                        : orderAdjustment.Amount ?? 0;
                }

                foreach (OrderAdjustment orderAdjustment in this.SurchargeAdjustments)
                {
                    this.UnitSurcharge += orderAdjustment.Percentage.HasValue
                        ? this.UnitPrice * orderAdjustment.Percentage.Value / 100
                        : orderAdjustment.Amount ?? 0;
                }

                this.UnitPrice = this.UnitBasePrice - this.UnitDiscount + this.UnitSurcharge;
            }

            foreach (SalesOrderItem featureItem in this.OrderedWithFeatures)
            {
                this.UnitBasePrice += featureItem.UnitBasePrice;
                this.UnitPrice += featureItem.UnitPrice;
                this.UnitDiscount += featureItem.UnitDiscount;
                this.UnitSurcharge += featureItem.UnitSurcharge;
            }

            this.UnitVat = this.ExistVatRate ? this.UnitPrice * this.VatRate.Rate / 100 : 0;
            this.UnitIrpf = this.ExistIrpfRate ? this.UnitPrice * this.IrpfRate.Rate / 100 : 0;

            // Calculate Totals
            this.TotalBasePrice = this.UnitBasePrice * this.QuantityOrdered;
            this.TotalDiscount = this.UnitDiscount * this.QuantityOrdered;
            this.TotalSurcharge = this.UnitSurcharge * this.QuantityOrdered;
            this.TotalOrderAdjustment = this.TotalSurcharge - this.TotalDiscount;

            if (this.TotalBasePrice > 0)
            {
                this.TotalDiscountAsPercentage = Rounder.RoundDecimal(this.TotalDiscount / this.TotalBasePrice * 100, 2);
                this.TotalSurchargeAsPercentage = Rounder.RoundDecimal(this.TotalSurcharge / this.TotalBasePrice * 100, 2);
            }
            else
            {
                this.TotalDiscountAsPercentage = 0;
                this.TotalSurchargeAsPercentage = 0;
            }

            this.TotalExVat = this.UnitPrice * this.QuantityOrdered;
            this.TotalVat = this.UnitVat * this.QuantityOrdered;
            this.TotalIncVat = this.TotalExVat + this.TotalVat;
            this.TotalIrpf = this.UnitIrpf * this.QuantityOrdered;
            this.GrandTotal = this.TotalIncVat - this.TotalIrpf;
        }
    }
}
