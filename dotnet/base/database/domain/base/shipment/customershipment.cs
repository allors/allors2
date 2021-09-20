// <copyright file="CustomerShipment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Meta;

    public partial class CustomerShipment
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.CustomerShipment, M.CustomerShipment.ShipmentState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public bool CanShip
        {
            get
            {
                if (!this.ShipmentState.Equals(new ShipmentStates(this.Strategy.Session).Packed))
                {
                    return false;
                }

                var picklists = this.ShipToParty.PickListsWhereShipToParty;
                picklists.Filter.AddEquals(M.PickList.Store, this.Store);
                picklists.Filter.AddNot().AddEquals(M.PickList.PickListState, new PickListStates(this.Strategy.Session).Picked);
                if (picklists.First != null)
                {
                    return false;
                }

                foreach (ShipmentItem shipmentItem in this.ShipmentItems)
                {
                    foreach (OrderShipment orderShipment in shipmentItem.OrderShipmentsWhereShipmentItem)
                    {
                        if (orderShipment.OrderItem is SalesOrderItem salesOrderItem
                            && salesOrderItem.SalesOrderWhereSalesOrderItem.SalesOrderState.Equals(new SalesOrderStates(this.Strategy.Session).OnHold))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        public string ShortShipDateString => this.EstimatedShipDate?.ToString("d");

        private PickList PendingPickList
        {
            get
            {
                var pickLists = this.ShipToParty.PickListsWhereShipToParty;
                pickLists.Filter.AddEquals(M.PickList.Store, this.Store);
                pickLists.Filter.AddNot().AddEquals(M.PickList.PickListState, new PickListStates(this.Session()).Picked);

                return pickLists.FirstOrDefault();
            }
        }

        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistShipmentState)
            {
                this.ShipmentState = new ShipmentStates(this.Strategy.Session).Created;
            }

            if (!this.ExistReleasedManually)
            {
                this.ReleasedManually = false;
            }

            if (!this.ExistHeldManually)
            {
                this.HeldManually = false;
            }

            if (!this.ExistWithoutCharges)
            {
                this.WithoutCharges = false;
            }

            if (!this.ExistStore)
            {
                this.Store = this.Strategy.Session.Extent<Store>().First;
            }

            if (!this.ExistEstimatedShipDate)
            {
                this.EstimatedShipDate = this.Session().Now().Date;
            }

            if (!this.ExistCarrier && this.ExistStore)
            {
                this.Carrier = this.Store.DefaultCarrier;
            }
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                foreach (ShipmentItem shipmentItem in this.ShipmentItems)
                {
                    iteration.AddDependency(this, shipmentItem);
                    iteration.Mark(shipmentItem);

                    foreach (OrderShipment orderShipment in shipmentItem.OrderShipmentsWhereShipmentItem)
                    {
                        if (orderShipment.ExistOrderItem && !orderShipment.Strategy.IsNewInSession)
                        {
                            iteration.AddDependency(this, orderShipment.OrderItem);
                            iteration.Mark(orderShipment.OrderItem);
                        }
                    }
                }

                foreach (ShipmentPackage shipmentPackage in this.ShipmentPackages)
                {
                    iteration.AddDependency(this, shipmentPackage);
                    iteration.Mark(shipmentPackage);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var internalOrganisations = new Organisations(this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistShipFromParty && internalOrganisations.Count() == 1)
            {
                this.ShipFromParty = internalOrganisations.First();
            }

            if (!this.ExistShipmentNumber && this.ExistStore)
            {
                var year = this.CreationDate.Value.Year;
                this.ShipmentNumber = this.Store.NextOutgoingShipmentNumber(year);

                var fiscalYearStoreSequenceNumbers = this.Store.FiscalYearsStoreSequenceNumbers.FirstOrDefault(v => v.FiscalYear == year);
                var prefix = ((InternalOrganisation)this.ShipFromParty).CustomerShipmentSequence.IsEnforcedSequence ? this.Store.OutgoingShipmentNumberPrefix : fiscalYearStoreSequenceNumbers.OutgoingShipmentNumberPrefix;
                this.SortableShipmentNumber = this.Session().GetSingleton().SortableNumber(prefix, this.ShipmentNumber, year.ToString());
            }

            derivation.Validation.AssertExists(this, this.Meta.ShipToParty);

            if (!this.ExistShipToAddress && this.ExistShipToParty)
            {
                this.ShipToAddress = this.ShipToParty.ShippingAddress;
            }

            if (!this.ExistShipFromAddress)
            {
                this.ShipFromAddress = this.ShipFromParty?.ShippingAddress;
            }

            if (!this.ExistShipFromFacility)
            {
                this.ShipFromFacility = ((Organisation)this.ShipFromParty)?.FacilitiesWhereOwner.FirstOrDefault();
            }

            this.BaseOnDeriveShipmentValue(derivation);
            this.BaseOnDeriveCurrentShipmentState(derivation);

            if (this.CanShip && this.Store.IsAutomaticallyShipped)
            {
                this.Ship();
            }

            this.BaseOnDeriveCurrentObjectState(derivation);

            if (this.ShipmentState.IsShipped
                && (!this.ExistLastShipmentState || !this.LastShipmentState.IsShipped))
            {
                foreach (var item in this.ShipmentItems.Where(v => v.ExistSerialisedItem))
                {
                    if (item.ExistNextSerialisedItemAvailability)
                    {
                        item.SerialisedItem.SerialisedItemAvailability = item.NextSerialisedItemAvailability;

                        if ((this.ShipFromParty as InternalOrganisation)?.SerialisedItemSoldOns.Contains(new SerialisedItemSoldOns(this.Session()).CustomerShipmentShip) == true
                            && item.NextSerialisedItemAvailability.Equals(new SerialisedItemAvailabilities(this.Session()).Sold))
                        {
                            item.SerialisedItem.OwnedBy = this.ShipToParty;
                            item.SerialisedItem.Ownership = new Ownerships(this.Session()).ThirdParty;
                            item.SerialisedItem.AvailableForSale = false;
                        }

                        if (item.NextSerialisedItemAvailability.Equals(new SerialisedItemAvailabilities(this.Session()).InRent))
                        {
                            item.SerialisedItem.RentedBy = this.ShipToParty;
                        }
                    }
                }
            }

            this.Sync(this.Session());
        }

        public void BaseCancel(CustomerShipmentCancel method) => this.ShipmentState = new ShipmentStates(this.Strategy.Session).Cancelled;

        public void BasePick(CustomerShipmentPick method)
        {
            if (!method.Overridden)
            {
                this.CreatePickList();

                foreach (ShipmentItem shipmentItem in this.ShipmentItems)
                {
                    shipmentItem.ShipmentItemState = new ShipmentItemStates(this.Session()).Picking;
                }

                this.ShipmentState = new ShipmentStates(this.Strategy.Session).Picking;
            }
        }

        public void BaseHold(CustomerShipmentHold method)
        {
            this.HeldManually = true;
            this.PutOnHold();
        }

        public void BasePutOnHold(CustomerShipmentPutOnHold method)
        {
            foreach (PickList pickList in this.ShipToParty.PickListsWhereShipToParty)
            {
                if (this.Store.Equals(pickList.Store) &&
                    !pickList.ExistPicker)
                {
                    pickList.Hold();
                }
            }

            this.ShipmentState = new ShipmentStates(this.Strategy.Session).OnHold;
        }

        public void BaseContinue(CustomerShipmentContinue method)
        {
            this.ReleasedManually = true;
            this.ProcessOnContinue();
        }

        public void BaseProcessOnContinue(CustomerShipmentProcessOnContinue method)
        {
            this.ShipmentState = this.ExistPreviousShipmentState ? this.PreviousShipmentState : new ShipmentStates(this.Strategy.Session).Created;

            foreach (PickList pickList in this.ShipToParty.PickListsWhereShipToParty)
            {
                if (this.Store.Equals(pickList.Store) && pickList.PickListState.Equals(new PickListStates(this.Strategy.Session).OnHold))
                {
                    pickList.Continue();
                }
            }
        }

        public void BaseSetPicked(CustomerShipmentSetPicked method)
        {
            this.ShipmentState = new ShipmentStates(this.Strategy.Session).Picked;

            foreach (ShipmentItem shipmentItem in this.ShipmentItems)
            {
                shipmentItem.ShipmentItemState = new ShipmentItemStates(this.Session()).Picked;
            }
        }

        public void BaseSetPacked(CustomerShipmentSetPacked method)
        {
            this.ShipmentState = new ShipmentStates(this.Strategy.Session).Packed;

            foreach (ShipmentItem shipmentItem in this.ShipmentItems)
            {
                shipmentItem.ShipmentItemState = new ShipmentItemStates(this.Session()).Packed;
            }
        }

        public void BaseShip(CustomerShipmentShip method)
        {
            if (this.CanShip)
            {
                this.ShipmentState = new ShipmentStates(this.Strategy.Session).Shipped;
                this.EstimatedShipDate = this.Session().Now().Date;

                foreach (ShipmentItem shipmentItem in this.ShipmentItems)
                {
                    shipmentItem.ShipmentItemState = new ShipmentItemStates(this.Session()).Shipped;

                    foreach (OrderShipment orderShipment in shipmentItem.OrderShipmentsWhereShipmentItem)
                    {
                        foreach (SalesOrderItemInventoryAssignment salesOrderItemInventoryAssignment in ((SalesOrderItem)orderShipment.OrderItem).SalesOrderItemInventoryAssignments)
                        {
                            // Quantity is used to calculate QuantityReserved (via inventoryItemTransactions)
                            salesOrderItemInventoryAssignment.Quantity -= orderShipment.Quantity;
                        }
                    }

                    foreach (InventoryItem inventoryItem in shipmentItem.ReservedFromInventoryItems)
                    {
                        if (inventoryItem.Part.InventoryItemKind.IsSerialised)
                        {
                            new InventoryItemTransactionBuilder(this.Session())
                                .WithPart(shipmentItem.Part)
                                .WithSerialisedItem(shipmentItem.SerialisedItem)
                                .WithUnitOfMeasure(inventoryItem.Part.UnitOfMeasure)
                                .WithFacility(inventoryItem.Facility)
                                .WithReason(new InventoryTransactionReasons(this.Strategy.Session).OutgoingShipment)
                                .WithSerialisedInventoryItemState(new SerialisedInventoryItemStates(this.Session()).Good)
                                .WithQuantity(1)
                                .Build();
                        }
                        else
                        {
                            new InventoryItemTransactionBuilder(this.Session())
                                .WithPart(inventoryItem.Part)
                                .WithUnitOfMeasure(inventoryItem.Part.UnitOfMeasure)
                                .WithFacility(inventoryItem.Facility)
                                .WithReason(new InventoryTransactionReasons(this.Strategy.Session).OutgoingShipment)
                                .WithQuantity(shipmentItem.Quantity)
                                .WithCost(inventoryItem.Part.PartWeightedAverage.AverageCost)
                                .Build();
                        }
                    }
                }
            }
        }

        public void BaseInvoice(CustomerShipmentInvoice method)
        {
            if (this.ShipmentState.Equals(new ShipmentStates(this.Strategy.Session).Shipped) &&
                   Equals(this.Store.BillingProcess, new BillingProcesses(this.Strategy.Session).BillingForShipmentItems))
            {
                var invoiceByOrder = new Dictionary<SalesOrder, SalesInvoice>();
                var costsInvoiced = false;

                foreach (ShipmentItem shipmentItem in this.ShipmentItems)
                {
                    foreach (OrderShipment orderShipment in shipmentItem.OrderShipmentsWhereShipmentItem)
                    {
                        var salesOrder = orderShipment.OrderItem.OrderWhereValidOrderItem as SalesOrder;

                        if (!invoiceByOrder.TryGetValue(salesOrder, out var salesInvoice))
                        {
                            salesInvoice = new SalesInvoiceBuilder(this.Strategy.Session)
                                .WithStore(salesOrder.Store)
                                .WithBilledFrom(salesOrder.TakenBy)
                                .WithAssignedBilledFromContactMechanism(salesOrder.DerivedTakenByContactMechanism)
                                .WithBilledFromContactPerson(salesOrder.TakenByContactPerson)
                                .WithBillToCustomer(salesOrder.BillToCustomer)
                                .WithAssignedBillToContactMechanism(salesOrder.DerivedBillToContactMechanism)
                                .WithBillToContactPerson(salesOrder.BillToContactPerson)
                                .WithBillToEndCustomer(salesOrder.BillToEndCustomer)
                                .WithAssignedBillToEndCustomerContactMechanism(salesOrder.DerivedBillToEndCustomerContactMechanism)
                                .WithBillToEndCustomerContactPerson(salesOrder.BillToEndCustomerContactPerson)
                                .WithShipToCustomer(salesOrder.ShipToCustomer)
                                .WithAssignedShipToAddress(salesOrder.DerivedShipToAddress)
                                .WithShipToContactPerson(salesOrder.ShipToContactPerson)
                                .WithShipToEndCustomer(salesOrder.ShipToEndCustomer)
                                .WithAssignedShipToEndCustomerAddress(salesOrder.DerivedShipToEndCustomerAddress)
                                .WithShipToEndCustomerContactPerson(salesOrder.ShipToEndCustomerContactPerson)
                                .WithInvoiceDate(this.Session().Now())
                                .WithSalesChannel(salesOrder.SalesChannel)
                                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Strategy.Session).SalesInvoice)
                                .WithAssignedVatRegime(salesOrder.DerivedVatRegime)
                                .WithAssignedIrpfRegime(salesOrder.DerivedIrpfRegime)
                                .WithCustomerReference(salesOrder.CustomerReference)
                                .WithAssignedPaymentMethod(this.PaymentMethod)
                                .Build();

                            invoiceByOrder.Add(salesOrder, salesInvoice);

                            foreach (OrderAdjustment orderAdjustment in salesOrder.OrderAdjustments)
                            {
                                OrderAdjustment newAdjustment = null;
                                if (orderAdjustment.GetType().Name.Equals(typeof(DiscountAdjustment).Name))
                                {
                                    newAdjustment = new DiscountAdjustmentBuilder(this.Session()).Build();
                                }

                                if (orderAdjustment.GetType().Name.Equals(typeof(SurchargeAdjustment).Name))
                                {
                                    newAdjustment = new SurchargeAdjustmentBuilder(this.Session()).Build();
                                }

                                if (orderAdjustment.GetType().Name.Equals(typeof(Fee).Name))
                                {
                                    newAdjustment = new FeeBuilder(this.Session()).Build();
                                }

                                if (orderAdjustment.GetType().Name.Equals(typeof(ShippingAndHandlingCharge).Name))
                                {
                                    newAdjustment = new ShippingAndHandlingChargeBuilder(this.Session()).Build();
                                }

                                if (orderAdjustment.GetType().Name.Equals(typeof(MiscellaneousCharge).Name))
                                {
                                    newAdjustment = new MiscellaneousChargeBuilder(this.Session()).Build();
                                }

                                newAdjustment.Amount ??= orderAdjustment.Amount;
                                newAdjustment.Percentage ??= orderAdjustment.Percentage;
                                salesInvoice.AddOrderAdjustment(newAdjustment);
                            }

                            if (!costsInvoiced)
                            {
                                var costs = this.BaseOnDeriveShippingAndHandlingCharges();
                                if (costs > 0)
                                {
                                    salesInvoice.AddOrderAdjustment(new ShippingAndHandlingChargeBuilder(this.Strategy.Session).WithAmount(costs).Build());
                                    costsInvoiced = true;
                                }
                            }

                            foreach (SalesTerm salesTerm in salesOrder.SalesTerms)
                            {
                                if (salesTerm.GetType().Name == typeof(IncoTerm).Name)
                                {
                                    salesInvoice.AddSalesTerm(new IncoTermBuilder(this.Strategy.Session)
                                        .WithTermType(salesTerm.TermType)
                                        .WithTermValue(salesTerm.TermValue)
                                        .WithDescription(salesTerm.Description)
                                        .Build());
                                }

                                if (salesTerm.GetType().Name == typeof(InvoiceTerm).Name)
                                {
                                    salesInvoice.AddSalesTerm(new InvoiceTermBuilder(this.Strategy.Session)
                                        .WithTermType(salesTerm.TermType)
                                        .WithTermValue(salesTerm.TermValue)
                                        .WithDescription(salesTerm.Description)
                                        .Build());
                                }

                                if (salesTerm.GetType().Name == typeof(OrderTerm).Name)
                                {
                                    salesInvoice.AddSalesTerm(new OrderTermBuilder(this.Strategy.Session)
                                        .WithTermType(salesTerm.TermType)
                                        .WithTermValue(salesTerm.TermValue)
                                        .WithDescription(salesTerm.Description)
                                        .Build());
                                }
                            }
                        }

                        var amountAlreadyInvoiced = shipmentItem.ShipmentItemBillingsWhereShipmentItem.Sum(v => v.Amount);
                        var leftToInvoice = (orderShipment.OrderItem.QuantityOrdered * orderShipment.OrderItem.AssignedUnitPrice) - amountAlreadyInvoiced;

                        if (leftToInvoice > 0)
                        {
                            if (orderShipment.OrderItem is SalesOrderItem salesOrderItem)
                            {
                                var invoiceItem = new SalesInvoiceItemBuilder(this.Strategy.Session)
                                    .WithInvoiceItemType(new InvoiceItemTypes(this.Strategy.Session).ProductItem)
                                    .WithProduct(salesOrderItem.Product)
                                    .WithQuantity(orderShipment.Quantity)
                                    .WithAssignedUnitPrice(salesOrderItem.UnitPrice)
                                    .WithAssignedVatRegime(salesOrderItem.AssignedVatRegime)
                                    .WithDescription(salesOrderItem.Description)
                                    .WithInternalComment(salesOrderItem.InternalComment)
                                    .WithMessage(salesOrderItem.Message)
                                    .Build();

                                salesInvoice.AddSalesInvoiceItem(invoiceItem);

                                new ShipmentItemBillingBuilder(this.Strategy.Session)
                                    .WithQuantity(shipmentItem.Quantity)
                                    .WithAmount(leftToInvoice)
                                    .WithShipmentItem(shipmentItem)
                                    .WithInvoiceItem(invoiceItem)
                                    .Build();
                            }
                        }
                    }
                }
            }
        }

        public void BaseOnDeriveShipmentValue(IDerivation derivation)
        {
            var shipmentValue = 0M;
            foreach (ShipmentItem shipmentItem in this.ShipmentItems)
            {
                foreach (OrderShipment orderShipment in shipmentItem.OrderShipmentsWhereShipmentItem)
                {
                    shipmentValue += orderShipment.Quantity * orderShipment.OrderItem.UnitPrice;
                }
            }

            this.ShipmentValue = shipmentValue;
        }

        public void BaseOnDeriveCurrentShipmentState(IDerivation derivation)
        {
            if (this.ExistShipToParty && this.ExistShipmentItems)
            {
                ////cancel shipment if nothing left to ship
                if (this.ExistShipmentItems && this.PendingPickList == null
                    && !this.ShipmentState.Equals(new ShipmentStates(this.Strategy.Session).Cancelled))
                {
                    var canCancel = true;
                    foreach (ShipmentItem shipmentItem in this.ShipmentItems)
                    {
                        if (shipmentItem.Quantity > 0)
                        {
                            canCancel = false;
                            break;
                        }
                    }

                    if (canCancel)
                    {
                        this.Cancel();
                    }
                }

                if ((this.ShipmentState.Equals(new ShipmentStates(this.Strategy.Session).Picking) ||
                     this.ShipmentState.Equals(new ShipmentStates(this.Strategy.Session).Picked) ||
                    this.ShipmentState.Equals(new ShipmentStates(this.Strategy.Session).Packed)) &&
                    this.ShipToParty.ExistPickListsWhereShipToParty)
                {
                    var isPicked = true;
                    foreach (PickList pickList in this.ShipToParty.PickListsWhereShipToParty)
                    {
                        if (this.Store.Equals(pickList.Store) &&
                            !pickList.PickListState.Equals(new PickListStates(this.Strategy.Session).Picked))
                        {
                            isPicked = false;
                        }
                    }

                    if (isPicked)
                    {
                        this.SetPicked();
                    }
                }

                if (this.ShipmentState.Equals(new ShipmentStates(this.Strategy.Session).Picked)
                    || this.ShipmentState.Equals(new ShipmentStates(this.Strategy.Session).Packed))
                {
                    var totalShippingQuantity = 0M;
                    var totalPackagedQuantity = 0M;
                    foreach (ShipmentItem shipmentItem in this.ShipmentItems)
                    {
                        totalShippingQuantity += shipmentItem.Quantity;
                        foreach (PackagingContent packagingContent in shipmentItem.PackagingContentsWhereShipmentItem)
                        {
                            totalPackagedQuantity += packagingContent.Quantity;
                        }
                    }

                    if (this.Store.IsImmediatelyPacked && totalPackagedQuantity == totalShippingQuantity)
                    {
                        this.SetPacked();
                    }
                }

                if (this.ShipmentState.Equals(new ShipmentStates(this.Strategy.Session).Created)
                    || this.ShipmentState.Equals(new ShipmentStates(this.Strategy.Session).Picked)
                    || this.ShipmentState.Equals(new ShipmentStates(this.Strategy.Session).Picked)
                    || this.ShipmentState.Equals(new ShipmentStates(this.Strategy.Session).Packed))
                {
                    if (this.ShipmentValue < this.Store.ShipmentThreshold && !this.ReleasedManually)
                    {
                        this.PutOnHold();
                    }
                }

                if (this.ShipmentState.Equals(new ShipmentStates(this.Strategy.Session).OnHold) &&
                    !this.HeldManually &&
                    ((this.ShipmentValue >= this.Store.ShipmentThreshold) || this.ReleasedManually))
                {
                    this.Continue();
                }
            }
        }

        public void BaseOnDeriveCurrentObjectState(IDerivation derivation)
        {
            if (this.ExistShipmentState && !this.ShipmentState.Equals(this.LastShipmentState) &&
                this.ShipmentState.Equals(new ShipmentStates(this.Strategy.Session).Shipped))
            {
                if (Equals(this.Store.BillingProcess, new BillingProcesses(this.Strategy.Session).BillingForShipmentItems))
                {
                    this.Invoice();
                }
            }
        }

        private void Sync(ISession session)
        {
            // session.Prefetch(this.SyncPrefetch, this);
            foreach (ShipmentItem shipmentItem in this.ShipmentItems)
            {
                shipmentItem.Sync(this);
            }
        }

        public void BaseOnDeriveQuantityDecreased(ShipmentItem shipmentItem, SalesOrderItem orderItem, decimal correction)
        {
            var remainingCorrection = correction;

            var inventoryAssignment = orderItem.SalesOrderItemInventoryAssignments.FirstOrDefault();
            if (inventoryAssignment != null)
            {
                inventoryAssignment.Quantity = orderItem.QuantityCommittedOut - correction;
            }

            foreach (OrderShipment orderShipment in shipmentItem.OrderShipmentsWhereShipmentItem)
            {
                if (orderShipment.OrderItem.Equals(orderItem) && remainingCorrection > 0)
                {
                    decimal quantity;
                    if (orderShipment.Quantity < remainingCorrection)
                    {
                        quantity = orderShipment.Quantity;
                        remainingCorrection -= quantity;
                    }
                    else
                    {
                        quantity = remainingCorrection;
                        remainingCorrection = 0;
                    }

                    shipmentItem.Quantity -= quantity;

                    var itemIssuanceCorrection = quantity;
                    foreach (ItemIssuance itemIssuance in shipmentItem.ItemIssuancesWhereShipmentItem)
                    {
                        decimal subQuantity;
                        if (itemIssuance.Quantity < itemIssuanceCorrection)
                        {
                            subQuantity = itemIssuance.Quantity;
                            itemIssuanceCorrection -= quantity;
                        }
                        else
                        {
                            subQuantity = itemIssuanceCorrection;
                            itemIssuanceCorrection = 0;
                        }

                        itemIssuance.Quantity -= subQuantity;

                        if (itemIssuanceCorrection == 0)
                        {
                            break;
                        }
                    }
                }
            }

            if (shipmentItem.Quantity == 0)
            {
                foreach (OrderShipment orderShipment in shipmentItem.OrderShipmentsWhereShipmentItem)
                {
                    foreach (ItemIssuance itemIssuance in orderShipment.ShipmentItem.ItemIssuancesWhereShipmentItem)
                    {
                        if (!itemIssuance.PickListItem.PickListWherePickListItem.ExistPicker && itemIssuance.Quantity == 0)
                        {
                            itemIssuance.Delete();
                        }
                    }

                    orderShipment.Delete();
                }

                shipmentItem.Delete();
            }

            if (!this.ExistShipmentItems)
            {
                this.Cancel();

                if (this.PendingPickList != null)
                {
                    this.PendingPickList.Cancel();
                }
            }
        }

        private void CreatePickList()
        {
            if (this.ExistShipmentItems && this.ExistShipToParty)
            {
                var pickList = new PickListBuilder(this.Strategy.Session).WithShipToParty(this.ShipToParty).WithStore(this.Store).Build();

                foreach (var shipmentItem in this.ShipmentItems
                    .Where(v => v.ShipmentItemState.Equals(new ShipmentItemStates(this.Session()).Created)
                                || v.ShipmentItemState.Equals(new ShipmentItemStates(this.Session()).Picking)))
                {
                    var quantityIssued = 0M;
                    foreach (ItemIssuance itemIssuance in shipmentItem.ItemIssuancesWhereShipmentItem)
                    {
                        quantityIssued += itemIssuance.Quantity;
                    }

                    var quantityToIssue = shipmentItem.Quantity - quantityIssued;
                    if (quantityToIssue == 0)
                    {
                        return;
                    }

                    var unifiedGood = shipmentItem.Good as UnifiedGood;
                    var nonUnifiedGood = shipmentItem.Good as NonUnifiedGood;
                    var serialized = unifiedGood?.InventoryItemKind.Equals(new InventoryItemKinds(this.Session()).Serialised);
                    var part = unifiedGood ?? nonUnifiedGood?.Part;

                    var facilities = ((InternalOrganisation)this.ShipFromParty).FacilitiesWhereOwner;
                    var inventoryItems = part.InventoryItemsWherePart.Where(v => facilities.Contains(v.Facility));
                    SerialisedInventoryItem issuedFromSerializedInventoryItem = null;

                    foreach (InventoryItem inventoryItem in shipmentItem.ReservedFromInventoryItems)
                    {
                        // shipment item originates from sales order. Sales order item has only 1 ReservedFromInventoryItem.
                        // Foreach loop wil execute once.
                        var pickListItem = new PickListItemBuilder(this.Strategy.Session)
                            .WithInventoryItem(inventoryItem)
                            .WithQuantity(quantityToIssue)
                            .Build();

                        new ItemIssuanceBuilder(this.Strategy.Session)
                            .WithInventoryItem(pickListItem.InventoryItem)
                            .WithShipmentItem(shipmentItem)
                            .WithQuantity(pickListItem.Quantity)
                            .WithPickListItem(pickListItem)
                            .Build();

                        pickList.AddPickListItem(pickListItem);

                        if (serialized.HasValue && serialized.Value)
                        {
                            issuedFromSerializedInventoryItem = (SerialisedInventoryItem)inventoryItem;
                        }
                    }

                    // shipment item is not linked to sales order item
                    if (!shipmentItem.ExistReservedFromInventoryItems)
                    {
                        var quantityLeftToIssue = quantityToIssue;
                        foreach (var inventoryItem in inventoryItems)
                        {
                            if (serialized.HasValue && serialized.Value && quantityLeftToIssue > 0)
                            {
                                var serializedInventoryItem = (SerialisedInventoryItem)inventoryItem;
                                if (serializedInventoryItem.AvailableToPromise == 1)
                                {
                                    var pickListItem = new PickListItemBuilder(this.Strategy.Session)
                                        .WithInventoryItem(inventoryItem)
                                        .WithQuantity(quantityLeftToIssue)
                                        .Build();

                                    new ItemIssuanceBuilder(this.Strategy.Session)
                                        .WithInventoryItem(inventoryItem)
                                        .WithShipmentItem(shipmentItem)
                                        .WithQuantity(pickListItem.Quantity)
                                        .WithPickListItem(pickListItem)
                                        .Build();

                                    pickList.AddPickListItem(pickListItem);
                                    quantityLeftToIssue = 0;
                                    issuedFromSerializedInventoryItem = serializedInventoryItem;
                                }
                            }
                            else if (quantityLeftToIssue > 0)
                            {
                                var nonSerializedInventoryItem = (NonSerialisedInventoryItem)inventoryItem;
                                var quantity = quantityLeftToIssue > nonSerializedInventoryItem.AvailableToPromise
                                    ? nonSerializedInventoryItem.AvailableToPromise
                                    : quantityLeftToIssue;

                                if (quantity > 0)
                                {
                                    var pickListItem = new PickListItemBuilder(this.Strategy.Session)
                                        .WithInventoryItem(inventoryItem)
                                        .WithQuantity(quantity)
                                        .Build();

                                    new ItemIssuanceBuilder(this.Strategy.Session)
                                        .WithInventoryItem(inventoryItem)
                                        .WithShipmentItem(shipmentItem)
                                        .WithQuantity(pickListItem.Quantity)
                                        .WithPickListItem(pickListItem)
                                        .Build();

                                    shipmentItem.AddReservedFromInventoryItem(inventoryItem);

                                    pickList.AddPickListItem(pickListItem);
                                    quantityLeftToIssue -= pickListItem.Quantity;
                                }
                            }
                        }
                    }
                }
            }
        }

        private decimal BaseOnDeriveShippingAndHandlingCharges()
        {
            var charges = 0M;

            if (!this.WithoutCharges)
            {
                foreach (ShippingAndHandlingComponent shippingAndHandlingComponent in new ShippingAndHandlingComponents(this.Strategy.Session).Extent())
                {
                    if (shippingAndHandlingComponent.FromDate <= this.Session().Now() &&
                        (!shippingAndHandlingComponent.ExistThroughDate || shippingAndHandlingComponent.ThroughDate >= this.Session().Now()))
                    {
                        if (ShippingAndHandlingComponents.BaseIsEligible(shippingAndHandlingComponent, this))
                        {
                            if (shippingAndHandlingComponent.Cost.HasValue)
                            {
                                if (charges == 0 || shippingAndHandlingComponent.Cost < charges)
                                {
                                    charges = shippingAndHandlingComponent.Cost.Value;
                                }
                            }
                        }
                    }
                }
            }

            return charges;
        }
    }
}
