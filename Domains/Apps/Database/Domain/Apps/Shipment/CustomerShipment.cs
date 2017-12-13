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
namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using Meta;

    public partial class CustomerShipment
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.CustomerShipment.CustomerShipmentState),
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
                        if (orderShipment.SalesOrderItem.SalesOrderWhereSalesOrderItem.SalesOrderState.Equals(new SalesOrderStates(this.Strategy.Session).OnHold))
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
                    if (!picklist.IsNegativePickList)
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

            if (!this.ExistBillFromContactMechanism)
            {
                this.BillFromContactMechanism = this.Strategy.Session.GetSingleton().InternalOrganisation.BillingAddress;
            }

            if (!this.ExistStore)
            {
                this.Store = this.strategy.Session.Extent<Store>().First;
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
                    if (orderShipment.ExistSalesOrderItem)
                    {
                        derivation.AddDependency(this, orderShipment.SalesOrderItem);
                    }

                    if (orderShipment.ExistPurchaseOrderItem)
                    {
                        derivation.AddDependency(this, orderShipment.PurchaseOrderItem);
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

            derivation.Validation.AssertExists(this, this.Meta.ShipToParty);

            if (!this.ExistShipToAddress && this.ExistShipToParty)
            {
                this.ShipToAddress = this.ShipToParty.ShippingAddress;
            }

            if (!this.ExistShipFromAddress)
            {
                this.ShipFromAddress = this.strategy.Session.GetSingleton().InternalOrganisation.ShippingAddress;
            }

            if (!this.ExistBillFromContactMechanism)
            {
                this.BillFromContactMechanism = this.Strategy.Session.GetSingleton().InternalOrganisation.BillingAddress;
            }

            this.CreatePickList(derivation);
            this.AppsOnDeriveShipmentValue(derivation);
            this.AppsOnDeriveCurrentShipmentState(derivation);

            this.AppsOnDeriveCurrentObjectState(derivation);
        }

        public void AppsOnDeriveInvoices(IDerivation derivation)
        {
            var invoiceByOrder = new Dictionary<SalesOrder, SalesInvoice>();
            var costsInvoiced = false;

            foreach (ShipmentItem shipmentItem in this.ShipmentItems)
            {
                foreach (OrderShipment orderShipment in shipmentItem.OrderShipmentsWhereShipmentItem)
                {
                    var salesOrder = orderShipment.SalesOrderItem.SalesOrderWhereSalesOrderItem;

                    SalesInvoice salesInvoice;
                    if (!invoiceByOrder.TryGetValue(salesOrder, out salesInvoice))
                    {
                        salesInvoice = new SalesInvoiceBuilder(this.Strategy.Session)
                            .WithStore(salesOrder.Store)
                            .WithInvoiceDate(DateTime.UtcNow)
                            .WithSalesChannel(salesOrder.SalesChannel)
                            .WithSalesInvoiceType(new SalesInvoiceTypes(this.Strategy.Session).SalesInvoice)
                            .WithVatRegime(salesOrder.VatRegime)
                            .WithBilledFromContactMechanism(this.BillFromContactMechanism)
                            .WithBillToContactMechanism(this.BillToContactMechanism)
                            .WithBillToCustomer(this.BillToParty)
                            .WithShipToCustomer(this.ShipToParty)
                            .WithShipToAddress(this.ShipToAddress)
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
                    }

                    var invoiceItem = new SalesInvoiceItemBuilder(this.Strategy.Session)
                        .WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Strategy.Session).ProductItem)
                        .WithProduct(orderShipment.SalesOrderItem.Product)
                        .WithQuantity(orderShipment.Quantity)
                        .Build();

                    salesInvoice.AddSalesInvoiceItem(invoiceItem);
                    shipmentItem.AddInvoiceItem(invoiceItem);
                }

                foreach (var keyValuePair in invoiceByOrder)
                {
                    // TODO: put this in prepare
                    // keyValuePair.Value.Derive(x=>x.WithDerivation(derivation));
                }
            }
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
                        var orderItem = shipmentItem.OrderShipmentsWhereShipmentItem[0].SalesOrderItem;

                        if (pendingPickList == null)
                        {
                            pendingPickList = new PickListBuilder(this.Strategy.Session).WithShipToParty(this.ShipToParty).Build();
                        }

                        PickListItem pickListItem = null;
                        foreach (PickListItem item in pendingPickList.PickListItems)
                        {
                            if (item.InventoryItem.Equals(orderItem.ReservedFromInventoryItem))
                            {
                                pickListItem = item;
                                break;
                            }
                        }

                        if (pickListItem != null)
                        {
                            pickListItem.RequestedQuantity += shipmentItem.Quantity;

                            var itemIssuances = pickListItem.ItemIssuancesWherePickListItem;
                            itemIssuances.Filter.AddEquals(M.ItemIssuance.ShipmentItem, shipmentItem);
                            itemIssuances.First.Quantity = shipmentItem.Quantity;
                        }
                        else
                        {
                            var quantity = shipmentItem.Quantity - quantityIssued;
                            pickListItem = new PickListItemBuilder(this.Strategy.Session)
                                .WithInventoryItem(orderItem.ReservedFromInventoryItem)
                                .WithRequestedQuantity(quantity)
                                .WithActualQuantity(quantity)
                                .Build();

                            new ItemIssuanceBuilder(this.Strategy.Session)
                                .WithInventoryItem(orderItem.ReservedFromInventoryItem)
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

        private void CreateNegativePickList(IDerivation derivation, CustomerShipment shipment, SalesOrderItem orderItem, decimal quantity)
        {
            if (this.ExistShipToParty)
            {
                var pickList = new PickListBuilder(this.Strategy.Session)
                    .WithCustomerShipmentCorrection(shipment)
                    .WithShipToParty(this.ShipToParty)
                    .WithStore(this.Store)
                    .Build();

                pickList.AddPickListItem(new PickListItemBuilder(this.Strategy.Session)
                                        .WithInventoryItem(orderItem.ReservedFromInventoryItem)
                                        .WithRequestedQuantity(0 - quantity)
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

        public void AppsOnDeriveOrderItemQuantityShipped(IDerivation derivation)
        {
            var salesOrders = new List<SalesOrder>();

            foreach (ShipmentItem shipmentItem in this.ShipmentItems)
            {
                foreach (OrderShipment orderShipment in shipmentItem.OrderShipmentsWhereShipmentItem)
                {
                    var order = (SalesOrder)orderShipment.SalesOrderItem.SalesOrderWhereSalesOrderItem;
                    orderShipment.SalesOrderItem.AppsOnDeriveOnShipped(derivation, orderShipment.Quantity);
                    salesOrders.Add(order);
                }
            }
        }

        public void AppsOnDeriveQuantityDecreased(IDerivation derivation, ShipmentItem shipmentItem, SalesOrderItem orderItem, decimal correction)
        {
            var remainingCorrection = correction;

            foreach (OrderShipment orderShipment in shipmentItem.OrderShipmentsWhereShipmentItem)
            {
                if (orderShipment.SalesOrderItem.Equals(orderItem) && remainingCorrection > 0)
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

                    orderShipment.Quantity -= quantity;
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

                    if (orderShipment.Quantity == 0)
                    {
                        foreach (ItemIssuance itemIssuance in orderShipment.ShipmentItem.ItemIssuancesWhereShipmentItem)
                        {
                            if (!itemIssuance.PickListItem.PickListWherePickListItem.ExistPicker)
                            {
                                itemIssuance.Delete();
                            }
                        }

                        orderShipment.Delete();
                    }
                }
            }

            if (this.PendingPickList == null)
            {
                var shipment = (CustomerShipment)shipmentItem.ShipmentWhereShipmentItem;
                this.CreateNegativePickList(derivation, shipment, orderItem, correction);
            }

            if (shipmentItem.Quantity == 0)
            {
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
                    this.ShipmentValue += orderShipment.SalesOrderItem.TotalExVat;
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
            if (this.ExistCustomerShipmentState && !this.CustomerShipmentState.Equals(this.LastCustomerShipmentState))
            {
                if (this.CustomerShipmentState.Equals(new CustomerShipmentStates(this.Strategy.Session).Shipped))
                {
                    if (Equals(this.Store.BillingProcess, new BillingProcesses(this.strategy.Session).BillingForShipmentItems))
                    {
                        this.AppsOnDeriveInvoices(derivation);
                    }

                    this.AppsOnDeriveOrderItemQuantityShipped(derivation);
                }
            }
        }
    }
}