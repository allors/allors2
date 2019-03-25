// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerShipment.cs" company="Allors bvba">
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

using System.Linq;

namespace Allors.Domain
{
    using Allors.Domain.NonLogging;
    using Meta;
    using System;
    using System.Collections.Generic;

    public partial class CustomerShipment
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.CustomerShipment, M.CustomerShipment.CustomerShipmentState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public bool CanShip
        {
            get
            {
                if (!this.CustomerShipmentState.Equals(new CustomerShipmentStates(this.Strategy.Session).Packed))
                {
                    return false;
                }

                if (this.PendingPickList != null)
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
                        if (orderShipment.OrderItem is SalesOrderItem salesOrderItem && salesOrderItem.SalesOrderWhereSalesOrderItem.SalesOrderState.Equals(new SalesOrderStates(this.Strategy.Session).OnHold))
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
                var picklists = this.ShipToParty.PickListsWhereShipToParty;
                picklists.Filter.AddNot().AddExists(M.PickList.Picker);

                PickList pendingPickList = null;
                foreach (PickList picklist in picklists)
                {
                    if (!picklist.IsNegativePickList && !this.Store.IsImmediatelyPicked)
                    {
                        pendingPickList = picklist;
                        break;
                    }
                }

                return pendingPickList;
            }
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCustomerShipmentState)
            {
                this.CustomerShipmentState = new CustomerShipmentStates(this.Strategy.Session).Created;
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
                this.EstimatedShipDate = DateTime.UtcNow.Date;
            }

            if (!this.ExistShipmentNumber && this.ExistStore)
            {
                this.ShipmentNumber = this.Store.DeriveNextShipmentNumber();
            }

            if (!this.ExistCarrier && this.ExistStore)
            {
                this.Carrier = this.Store.DefaultCarrier;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            foreach (ShipmentItem shipmentItem in this.ShipmentItems)
            {
                derivation.AddDependency(this, shipmentItem);

                foreach (OrderShipment orderShipment in shipmentItem.OrderShipmentsWhereShipmentItem)
                {
                    if (orderShipment.ExistOrderItem && !orderShipment.OrderItem.Strategy.IsNewInSession)
                    {
                        derivation.AddDependency(this, orderShipment.OrderItem);
                    }
                }
            }

            foreach (ShipmentPackage shipmentPackage in this.ShipmentPackages)
            {
                derivation.AddDependency(this, shipmentPackage);
            }                
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var internalOrganisations = new Organisations(this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistShipFromParty && internalOrganisations.Count() == 1)
            {
                this.ShipFromParty = internalOrganisations.First();
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

            if (!this.ExistBillFromContactMechanism)
            {
                this.BillFromContactMechanism = this.ShipFromParty?.BillingAddress;
            }

            this.CreatePickList(derivation);
            this.AppsOnDeriveShipmentValue(derivation);
            this.AppsOnDeriveCurrentShipmentState(derivation);

            if (this.CanShip && this.Store.IsAutomaticallyShipped)
            {
                this.Ship();
            }

            this.AppsOnDeriveCurrentObjectState(derivation);
        }

        public void AppsCancel(CustomerShipmentCancel method)
        {
            this.CustomerShipmentState = new CustomerShipmentStates(this.Strategy.Session).Cancelled;
        }

        public void AppsHold(CustomerShipmentHold method)
        {
            this.HeldManually = true;
            this.PutOnHold();
        }

        public void AppsPutOnHold(CustomerShipmentPutOnHold method)
        {
            foreach (PickList pickList in this.ShipToParty.PickListsWhereShipToParty)
            {
                if (this.Store.Equals(pickList.Store) &&
                    !pickList.IsNegativePickList &&
                    !pickList.ExistPicker)
                {
                    pickList.Hold();
                }
            }

            this.CustomerShipmentState = new CustomerShipmentStates(this.Strategy.Session).OnHold;
        }

        public void AppsContinue(CustomerShipmentContinue method)
        {
            this.ReleasedManually = true;
            this.ProcessOnContinue();
        }

        public void AppsProcessOnContinue(CustomerShipmentProcessOnContinue method)
        {
            this.CustomerShipmentState = new CustomerShipmentStates(this.Strategy.Session).Created;

            foreach (PickList pickList in this.ShipToParty.PickListsWhereShipToParty)
            {
                if (this.Store.Equals(pickList.Store) && pickList.PickListState.Equals(new PickListStates(this.Strategy.Session).OnHold))
                {
                    pickList.Continue();
                }
            }
        }

        public void AppsSetPicked(CustomerShipmentSetPicked method)
        {
            this.CustomerShipmentState = new CustomerShipmentStates(this.Strategy.Session).Picked;
        }

        public void AppsSetPacked(CustomerShipmentSetPacked method)
        {
            this.CustomerShipmentState = new CustomerShipmentStates(this.Strategy.Session).Packed;
        }

        public void AppsShip(CustomerShipmentShip method)
        {
            if (this.CanShip)
            {
                this.CustomerShipmentState = new CustomerShipmentStates(this.Strategy.Session).Shipped;
                this.EstimatedShipDate = DateTime.UtcNow.Date;

                foreach (ShipmentItem shipmentItem in this.ShipmentItems)
                {
                    foreach (OrderShipment orderShipment in shipmentItem.OrderShipmentsWhereShipmentItem)
                    {
                        var inventoryAssignment = ((SalesOrderItem)orderShipment.OrderItem).SalesOrderItemInventoryAssignmentsWhereSalesOrderItem.FirstOrDefault();
                        if (inventoryAssignment != null)
                        {
                            inventoryAssignment.Quantity -= orderShipment.Quantity;
                        }
                    }
                }
            }
        }

        public void AppsInvoice(CustomerShipmentInvoice method)
        {
            var derivation = new Derivation(this.Strategy.Session);

            if (this.CustomerShipmentState.Equals(new CustomerShipmentStates(this.Strategy.Session).Shipped) &&
                Equals(this.Store.BillingProcess, new BillingProcesses(this.Strategy.Session).BillingForShipmentItems))
            {
                this.AppsInvoiceThis(derivation);
            }
        }

        public void AppsInvoiceThis(IDerivation derivation)
        {
            var invoiceByOrder = new Dictionary<SalesOrder, SalesInvoice>();
            var costsInvoiced = false;

            foreach (ShipmentItem shipmentItem in this.ShipmentItems)
            {
                foreach (OrderShipment orderShipment in shipmentItem.OrderShipmentsWhereShipmentItem)
                {
                    var salesOrder = orderShipment.OrderItem.OrderWhereValidOrderItem as SalesOrder;
                    var salesOrderItem = orderShipment.OrderItem as SalesOrderItem;

                    if (!invoiceByOrder.TryGetValue(salesOrder, out var salesInvoice))
                    {
                        salesInvoice = new SalesInvoiceBuilder(this.Strategy.Session)
                            .WithStore(salesOrder.Store)
                            .WithBilledFrom(salesOrder.TakenBy)
                            .WithBilledFromContactMechanism(salesOrder.TakenByContactMechanism)
                            .WithBilledFromContactPerson(salesOrder.TakenByContactPerson)
                            .WithBillToCustomer(salesOrder.BillToCustomer)
                            .WithBillToContactMechanism(salesOrder.BillToContactMechanism)
                            .WithBillToContactPerson(salesOrder.BillToContactPerson)
                            .WithBillToEndCustomer(salesOrder.BillToEndCustomer)
                            .WithBillToEndCustomerContactMechanism(salesOrder.BillToEndCustomerContactMechanism)
                            .WithBillToEndCustomerContactPerson(salesOrder.BillToEndCustomerContactPerson)
                            .WithShipToCustomer(salesOrder.ShipToCustomer)
                            .WithShipToAddress(salesOrder.ShipToAddress)
                            .WithShipToContactPerson(salesOrder.ShipToContactPerson)
                            .WithShipToEndCustomer(salesOrder.ShipToEndCustomer)
                            .WithShipToEndCustomerAddress(salesOrder.ShipToEndCustomerAddress)
                            .WithShipToEndCustomerContactPerson(salesOrder.ShipToEndCustomerContactPerson)
                            .WithInvoiceDate(DateTime.UtcNow)
                            .WithSalesChannel(salesOrder.SalesChannel)
                            .WithSalesInvoiceType(new SalesInvoiceTypes(this.Strategy.Session).SalesInvoice)
                            .WithVatRegime(salesOrder.VatRegime)
                            .WithDiscountAdjustment(salesOrder.DiscountAdjustment)
                            .WithSurchargeAdjustment(salesOrder.SurchargeAdjustment)
                            .WithShippingAndHandlingCharge(salesOrder.ShippingAndHandlingCharge)
                            .WithFee(salesOrder.Fee)
                            .WithCustomerReference(salesOrder.CustomerReference)
                            .WithPaymentMethod(this.PaymentMethod)
                            .Build();

                        invoiceByOrder.Add(salesOrder, salesInvoice);

                        if (!costsInvoiced)
                        {
                            var costs = this.AppsOnDeriveShippingAndHandlingCharges(derivation);
                            if (costs > 0)
                            {
                                salesInvoice.ShippingAndHandlingCharge = new ShippingAndHandlingChargeBuilder(this.Strategy.Session).WithAmount(costs).Build();
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
                        if (salesOrderItem != null)
                        {
                            var invoiceItem = new SalesInvoiceItemBuilder(this.Strategy.Session)
                                .WithInvoiceItemType(new InvoiceItemTypes(this.Strategy.Session).ProductItem)
                                .WithProduct(salesOrderItem.Product)
                                .WithQuantity(orderShipment.Quantity)
                                .WithAssignedUnitPrice(salesOrderItem.UnitPrice)
                                .WithDetails(salesOrderItem.Details)
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

        private void CreatePickList(IDerivation derivation)
        {
            if (this.ExistShipToParty)
            {
                var pendingPickList = this.PendingPickList;

                if (pendingPickList != null)
                {
                    foreach (PickListItem pickListItem in pendingPickList.PickListItems)
                    {
                        foreach (ItemIssuance itemIssuance in pickListItem.ItemIssuancesWherePickListItem)
                        {
                            itemIssuance.Delete();
                        }

                        pendingPickList.RemovePickListItem(pickListItem);
                        pickListItem.Delete();
                    }
                }

                foreach (ShipmentItem shipmentItem in this.ShipmentItems)
                {
                    var quantityIssued = 0M;
                    foreach (ItemIssuance itemIssuance in shipmentItem.ItemIssuancesWhereShipmentItem)
                    {
                        quantityIssued += itemIssuance.Quantity;
                    }

                    if (!shipmentItem.ExistItemIssuancesWhereShipmentItem || shipmentItem.Quantity > quantityIssued)
                    {
                        var salesOrderItem = shipmentItem.OrderShipmentsWhereShipmentItem[0].OrderItem as SalesOrderItem;

                        if (this.PendingPickList == null)
                        {
                            pendingPickList = new PickListBuilder(this.Strategy.Session).WithShipToParty(this.ShipToParty).Build();
                        }

                        PickListItem pickListItem = null;
                        foreach (PickListItem item in pendingPickList.PickListItems)
                        {
                            if (salesOrderItem != null && item.InventoryItem.Equals(salesOrderItem.ReservedFromNonSerialisedInventoryItem))
                            {
                                pickListItem = item;
                                break;
                            }
                        }

                        if (pickListItem != null)
                        {
                            pickListItem.Quantity += shipmentItem.Quantity;

                            var itemIssuances = pickListItem.ItemIssuancesWherePickListItem;
                            itemIssuances.Filter.AddEquals(M.ItemIssuance.ShipmentItem, shipmentItem);
                            itemIssuances.First.Quantity = shipmentItem.Quantity;
                        }
                        else
                        {
                            var quantity = shipmentItem.Quantity - quantityIssued;
                            pickListItem = new PickListItemBuilder(this.Strategy.Session)
                                .WithInventoryItem(salesOrderItem.ReservedFromNonSerialisedInventoryItem)
                                .WithQuantity(quantity)
                                .Build();

                            if (salesOrderItem.ExistReservedFromNonSerialisedInventoryItem)
                            {
                                pickListItem.InventoryItem = salesOrderItem.ReservedFromNonSerialisedInventoryItem;
                            }

                            if (salesOrderItem.ExistReservedFromSerialisedInventoryItem)
                            {
                                pickListItem.InventoryItem = salesOrderItem.ReservedFromSerialisedInventoryItem;
                            }

                            if (salesOrderItem.ExistSerialisedItem)
                            {
                                salesOrderItem.ReservedFromSerialisedInventoryItem.SerialisedItem.AvailableForSale = false;

                                if (salesOrderItem.ExistNewSerialisedItemState)
                                {
                                    salesOrderItem.ReservedFromSerialisedInventoryItem.SerialisedItem.SerialisedItemState = salesOrderItem.NewSerialisedItemState;
                                }

                                if (salesOrderItem.NewSerialisedItemState.Equals(
                                    new SerialisedItemStates(this.strategy.Session).Sold))
                                {
                                    salesOrderItem.SerialisedItem.OwnedBy = this.ShipToParty;
                                }
                            }

                            new ItemIssuanceBuilder(this.Strategy.Session)
                                .WithInventoryItem(pickListItem.InventoryItem)
                                .WithShipmentItem(shipmentItem)
                                .WithQuantity(quantity)
                                .WithPickListItem(pickListItem)
                                .Build();
                        }

                        pendingPickList.AddPickListItem(pickListItem);
                    }
                }

                if (pendingPickList != null)
                {
                    pendingPickList.OnDerive(x=>x.WithDerivation(derivation));
                }
            }
        }

        private void CreateNegativePickList(CustomerShipment shipment, SalesOrderItem orderItem, decimal quantity)
        {
            if (this.ExistShipToParty)
            {
                var pickList = new PickListBuilder(this.Strategy.Session)
                    .WithCustomerShipmentCorrection(shipment)
                    .WithShipToParty(this.ShipToParty)
                    .WithStore(this.Store)
                    .Build();

                pickList.AddPickListItem(new PickListItemBuilder(this.Strategy.Session)
                                        .WithInventoryItem(orderItem.ReservedFromNonSerialisedInventoryItem)
                                        .WithQuantity(0 - quantity)
                                        .Build());
            }
        }

        private decimal AppsOnDeriveShippingAndHandlingCharges(IDerivation derivation)
        {
            var charges = 0M;

            if (!this.WithoutCharges)
            {
                foreach (ShippingAndHandlingComponent shippingAndHandlingComponent in new ShippingAndHandlingComponents(this.Strategy.Session).Extent())
                {
                    if (shippingAndHandlingComponent.FromDate <= DateTime.UtcNow &&
                        (!shippingAndHandlingComponent.ExistThroughDate || shippingAndHandlingComponent.ThroughDate >= DateTime.UtcNow))
                    {
                        if (ShippingAndHandlingComponents.AppsIsEligible(shippingAndHandlingComponent, this))
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
        
        public void AppsOnDeriveQuantityDecreased(ShipmentItem shipmentItem, SalesOrderItem orderItem, decimal correction)
        {
            var remainingCorrection = correction;

            var inventoryAssignment = orderItem.SalesOrderItemInventoryAssignmentsWhereSalesOrderItem.FirstOrDefault();
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
                        if (!itemIssuance.PickListItem.PickListWherePickListItem.ExistPicker)
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
            }

            if (this.PendingPickList == null)
            {
                var shipment = (CustomerShipment)shipmentItem.ShipmentWhereShipmentItem;
                this.CreateNegativePickList(shipment, orderItem, correction);
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

        public void AppsOnDeriveShipmentValue(IDerivation derivation)
        {
            this.ShipmentValue = 0;
            foreach (ShipmentItem shipmentItem in this.ShipmentItems)
            {
                foreach (OrderShipment orderShipment in shipmentItem.OrderShipmentsWhereShipmentItem)
                {
                    this.ShipmentValue += orderShipment.OrderItem.TotalExVat;
                }
            }
        }

        public void AppsOnDeriveCurrentShipmentState(IDerivation derivation)
        {
            if (this.ExistShipToParty && this.ExistShipmentItems)
            {
                ////cancel shipment if nothing left to ship
                if (this.ExistShipmentItems && this.PendingPickList == null
                    && !this.CustomerShipmentState.Equals(new CustomerShipmentStates(this.Strategy.Session).Cancelled))
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

                if ((this.CustomerShipmentState.Equals(new CustomerShipmentStates(this.Strategy.Session).Created) || 
                    this.CustomerShipmentState.Equals(new CustomerShipmentStates(this.Strategy.Session).Packed)) &&
                    this.ShipToParty.ExistPickListsWhereShipToParty)
                {
                    var isPicked = true;
                    foreach (PickList pickList in this.ShipToParty.PickListsWhereShipToParty)
                    {
                        if (this.Store.Equals(pickList.Store) && 
                            !pickList.IsNegativePickList &&
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

                if (this.CustomerShipmentState.Equals(new CustomerShipmentStates(this.Strategy.Session).Picked)
                    || this.CustomerShipmentState.Equals(new CustomerShipmentStates(this.Strategy.Session).Packed))
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

                    if (totalPackagedQuantity == totalShippingQuantity)
                    {
                        this.SetPacked();
                    }
                }

                if (this.CustomerShipmentState.Equals(new CustomerShipmentStates(this.Strategy.Session).Created)
                    || this.CustomerShipmentState.Equals(new CustomerShipmentStates(this.Strategy.Session).Picked)
                    || this.CustomerShipmentState.Equals(new CustomerShipmentStates(this.Strategy.Session).Packed))
                {
                    if (this.ShipmentValue < this.Store.ShipmentThreshold && !this.ReleasedManually)
                    {                      
                        this.PutOnHold();
                    }
                }

                if (this.CustomerShipmentState.Equals(new CustomerShipmentStates(this.Strategy.Session).OnHold) && 
                    !this.HeldManually &&
                    ((this.ShipmentValue >= this.Store.ShipmentThreshold) || this.ReleasedManually))
                {
                    this.Continue();
                }
            }
        }

        public void AppsOnDeriveCurrentObjectState(IDerivation derivation)
        {
            if (this.ExistCustomerShipmentState && !this.CustomerShipmentState.Equals(this.LastCustomerShipmentState) &&
                this.CustomerShipmentState.Equals(new CustomerShipmentStates(this.Strategy.Session).Shipped))
            {
                if (Equals(this.Store.BillingProcess, new BillingProcesses(this.Strategy.Session).BillingForShipmentItems))
                {
                    this.Invoice();
                }
            }
        }
    }
}