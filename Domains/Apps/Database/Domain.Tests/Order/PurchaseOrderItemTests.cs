//------------------------------------------------------------------------------------------------- 
// <copyright file="PurchaseOrderItemTests.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using System.Security.Principal;
    using System.Threading;
    using Meta;
    using Xunit;

    
    public class PurchaseOrderItemTests : DomainTest
    {
        private FinishedGood finishedGood;
        private ProductPurchasePrice currentPurchasePrice;
        private PurchaseOrder order;
        private Organisation supplier;
        
        public PurchaseOrderItemTests()
        {
            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            ContactMechanism takenViaContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var supplierContactMechanism = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(takenViaContactMechanism)
                .WithUseAsDefault(true)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                .Build();

            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            this.supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();
            this.supplier.AddPartyContactMechanism(supplierContactMechanism);

            new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier).Build();

            this.finishedGood = new FinishedGoodBuilder(this.DatabaseSession)
                .WithManufacturerId("10101")
                .WithName("finished good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            var supplierOffering = new SupplierOfferingBuilder(this.DatabaseSession)
                .WithPart(this.finishedGood)
                .WithSupplier(this.supplier)
                .WithFromDate(DateTime.UtcNow.AddYears(-1))
                .Build();

            var previousPurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithFromDate(DateTime.UtcNow.AddYears(-1))
                .WithThroughDate(DateTime.UtcNow.AddDays(-1))
                .WithPrice(8)
                .Build();

            this.currentPurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithFromDate(DateTime.UtcNow)
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .WithPrice(10)
                .Build();

            var futurePurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithFromDate(DateTime.UtcNow.AddYears(1))
                .WithPrice(8)
                .Build();

            supplierOffering.AddProductPurchasePrice(previousPurchasePrice);
            supplierOffering.AddProductPurchasePrice(this.currentPurchasePrice);
            supplierOffering.AddProductPurchasePrice(futurePurchasePrice);

            this.order = new PurchaseOrderBuilder(this.DatabaseSession)
                .WithTakenViaSupplier(this.supplier)
                .WithBillToContactMechanism(takenViaContactMechanism)
                .WithDeliveryDate(DateTime.UtcNow)
                .WithVatRegime(new VatRegimes(this.DatabaseSession).Exempt)
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();
        }

        [Fact]
        public void GivenOrderItem_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var buyer = new OrganisationBuilder(this.DatabaseSession).WithName("buyer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var partyContactMechanism = new PartyContactMechanismBuilder(this.DatabaseSession).WithContactMechanism(shipToContactMechanism).Build();
            var part = new RawMaterialBuilder(this.DatabaseSession).WithName("raw stuff").Build();
            buyer.AddPartyContactMechanism(partyContactMechanism);

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var builder = new PurchaseOrderItemBuilder(this.DatabaseSession);
            order.AddPurchaseOrderItem(builder.Build());

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithPart(part);
            order.AddPurchaseOrderItem(builder.Build());

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            builder.WithProduct(new GoodBuilder(this.DatabaseSession).WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised).Build());
            var orderItem = builder.Build();
            order.AddPurchaseOrderItem(orderItem);

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            orderItem.RemovePart();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenConfirmedOrderItemForGood_WhenOrderItemIsRemoved_ThenItemIsRemovedFromValidOrderItems()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(M.VatRate.Rate, 21))
                .Build();

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddPurchaseOrderItem(item);

            this.DatabaseSession.Derive();

            this.order.Confirm();

            this.DatabaseSession.Derive();

            Assert.Equal(1, this.order.ValidOrderItems.Count);

            item.Cancel();

            this.DatabaseSession.Derive();

            Assert.Equal(0, this.order.ValidOrderItems.Count);
        }

        [Fact]
        public void GivenOrderItemForPart_WhenDerivingPrices_ThenUsePartCurrentPurchasePrice()
        {
            this.InstantiateObjects(this.DatabaseSession);

            const decimal QuantityOrdered = 3;
            var item1 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithPart(this.finishedGood).WithQuantityOrdered(QuantityOrdered).Build();
            this.order.AddPurchaseOrderItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(this.currentPurchasePrice.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(this.currentPurchasePrice.Price, item1.CalculatedUnitPrice);

            Assert.Equal(this.currentPurchasePrice.Price * QuantityOrdered, item1.TotalBasePrice);
            Assert.Equal(0, item1.TotalDiscount);
            Assert.Equal(0, item1.TotalSurcharge);
            Assert.Equal(this.currentPurchasePrice.Price * QuantityOrdered, item1.TotalExVat);

            Assert.Equal(this.currentPurchasePrice.Price * QuantityOrdered, this.order.TotalBasePrice);
            Assert.Equal(0, this.order.TotalDiscount);
            Assert.Equal(0, this.order.TotalSurcharge);
            Assert.Equal(this.currentPurchasePrice.Price * QuantityOrdered, this.order.TotalExVat);
        }

        [Fact]
        public void GivenOrderItemForProduct_WhenDerivingPrices_ThenUseProductCurrentPurchasePrice()
        {
            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(M.VatRate.Rate,21))
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .Build();

            var supplierOffering = new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithSupplier(this.supplier)
                .WithFromDate(DateTime.UtcNow.AddYears(-1))
                .Build();

            var previousPurchasePriceGood = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithFromDate(DateTime.UtcNow.AddYears(-1))
                .WithThroughDate(DateTime.UtcNow.AddDays(-1))
                .WithPrice(8)
                .Build();

            var currentPurchasePriceGood = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithFromDate(DateTime.UtcNow.AddMinutes(-1))
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .WithPrice(10)
                .Build();

            var futurePurchasePriceGood = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithFromDate(DateTime.UtcNow.AddYears(1))
                .WithPrice(8)
                .Build();

            supplierOffering.AddProductPurchasePrice(previousPurchasePriceGood);
            supplierOffering.AddProductPurchasePrice(currentPurchasePriceGood);
            supplierOffering.AddProductPurchasePrice(futurePurchasePriceGood);

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            const decimal QuantityOrdered = 3;
            var item1 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantityOrdered(QuantityOrdered).Build();
            this.order.AddPurchaseOrderItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(currentPurchasePriceGood.Price, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(currentPurchasePriceGood.Price, item1.CalculatedUnitPrice);

            Assert.Equal(currentPurchasePriceGood.Price * QuantityOrdered, item1.TotalBasePrice);
            Assert.Equal(0, item1.TotalDiscount);
            Assert.Equal(0, item1.TotalSurcharge);
            Assert.Equal(currentPurchasePriceGood.Price * QuantityOrdered, item1.TotalExVat);

            Assert.Equal(currentPurchasePriceGood.Price * QuantityOrdered, this.order.TotalBasePrice);
            Assert.Equal(0, this.order.TotalDiscount);
            Assert.Equal(0, this.order.TotalSurcharge);
            Assert.Equal(currentPurchasePriceGood.Price * QuantityOrdered, this.order.TotalExVat);
        }

        [Fact]
        public void GivenOrderItemForPartWithActualPrice_WhenDerivingPrices_ThenUseActualPrice()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var item1 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithPart(this.finishedGood).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            this.order.AddPurchaseOrderItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(15, item1.UnitBasePrice);
            Assert.Equal(0, item1.UnitDiscount);
            Assert.Equal(0, item1.UnitSurcharge);
            Assert.Equal(15, item1.CalculatedUnitPrice);
            Assert.Equal(0, item1.UnitVat);
            Assert.Equal(45, item1.TotalBasePrice);
            Assert.Equal(0, item1.TotalDiscount);
            Assert.Equal(0, item1.TotalSurcharge);
            Assert.Equal(45, item1.TotalExVat);
            Assert.Equal(0, item1.TotalVat);
            Assert.Equal(45, item1.TotalIncVat);

            Assert.Equal(45, this.order.TotalBasePrice);
            Assert.Equal(0, this.order.TotalDiscount);
            Assert.Equal(0, this.order.TotalSurcharge);
            Assert.Equal(45, this.order.TotalExVat);
            Assert.Equal(0, this.order.TotalVat);
            Assert.Equal(45, this.order.TotalIncVat);
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsCreated_ThenItemMayBeDeletedButNotCancelledOrRejected()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(M.VatRate.Rate,21))
                .Build();

            this.SetIdentity("admin");

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddPurchaseOrderItem(item);

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            Assert.Equal(new PurchaseOrderItemStates(this.DatabaseSession).Created, item.PurchaseOrderItemState);
            var currentUser = new Users(this.DatabaseSession).CurrentUser;
            var acl = new AccessControlList(item, currentUser);

            Assert.True(acl.CanExecute(M.PurchaseOrderItem.Delete));
            Assert.False(acl.CanExecute(M.PurchaseOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.PurchaseOrderItem.Reject));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsConfirmed_ThenItemMayBeCancelledOrRejectedButNotDeleted()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(M.VatRate.Rate,21))
                .Build();

            this.SetIdentity("admin");

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddPurchaseOrderItem(item);

            this.order.Confirm();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            Assert.Equal(new PurchaseOrderItemStates(this.DatabaseSession).InProcess, item.PurchaseOrderItemState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).CurrentUser);
            Assert.True(acl.CanExecute(M.PurchaseOrderItem.Cancel));
            Assert.True(acl.CanExecute(M.PurchaseOrderItem.Reject));
            Assert.False(acl.CanExecute(M.PurchaseOrderItem.Delete));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsPartiallyReceived_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(M.VatRate.Rate,21))
                .Build();

            this.SetIdentity("admin");

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(20)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddPurchaseOrderItem(item);

            this.order.Confirm();

            this.DatabaseSession.Derive();

            var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground).WithShipFromParty(this.supplier).Build();
            var shipmentItem = new ShipmentItemBuilder(this.DatabaseSession).WithGood(good).Build();
            shipment.AddShipmentItem(shipmentItem);

            new ShipmentReceiptBuilder(this.DatabaseSession)
                .WithQuantityAccepted(3)
                .WithShipmentItem(shipmentItem)
                .WithOrderItem(item)
                .Build();

            this.DatabaseSession.Derive();

            shipment.AppsComplete();

            this.DatabaseSession.Derive();

            Assert.Equal(new PurchaseOrderItemStates(this.DatabaseSession).PartiallyReceived, item.PurchaseOrderItemState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanExecute(M.PurchaseOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.PurchaseOrderItem.Reject));
            Assert.False(acl.CanExecute(M.PurchaseOrderItem.Delete));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsCancelled_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive();           
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(M.VatRate.Rate,21))
                .Build();

            this.SetIdentity("admin");

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddPurchaseOrderItem(item);            

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            item.Cancel();

            this.DatabaseSession.Derive();

            Assert.Equal(new PurchaseOrderItemStates(this.DatabaseSession).Cancelled, item.PurchaseOrderItemState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanExecute(M.PurchaseOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.PurchaseOrderItem.Reject));
            Assert.False(acl.CanExecute(M.PurchaseOrderItem.Delete));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsRejected_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(M.VatRate.Rate,21))
                .Build();

            this.SetIdentity("admin");

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddPurchaseOrderItem(item);
            
            this.DatabaseSession.Derive();

            item.Reject();

            this.DatabaseSession.Derive();

            Assert.Equal(new PurchaseOrderItemStates(this.DatabaseSession).Rejected, item.PurchaseOrderItemState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanExecute(M.PurchaseOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.PurchaseOrderItem.Reject));
            Assert.False(acl.CanExecute(M.PurchaseOrderItem.Delete));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsCompleted_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(M.VatRate.Rate,21))
                .Build();

            this.SetIdentity("admin");

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddPurchaseOrderItem(item);
            
            this.order.Confirm();
            
            this.DatabaseSession.Derive();

            var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipFromParty(this.supplier).WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground).Build();
            var shipmentItem = new ShipmentItemBuilder(this.DatabaseSession).WithGood(good).Build();
            shipment.AddShipmentItem(shipmentItem);

            new ShipmentReceiptBuilder(this.DatabaseSession)
                .WithQuantityAccepted(3)
                .WithShipmentItem(shipmentItem)
                .WithOrderItem(item)
                .Build();

            this.DatabaseSession.Derive();

            shipment.AppsComplete();
            
            this.DatabaseSession.Derive();

            Assert.Equal(new PurchaseOrderItemStates(this.DatabaseSession).Completed, item.PurchaseOrderItemState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanExecute(M.PurchaseOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.PurchaseOrderItem.Reject));
            Assert.False(acl.CanExecute(M.PurchaseOrderItem.Delete));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsFinished_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(M.VatRate.Rate,21))
                .Build();

            this.SetIdentity("admin");

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddPurchaseOrderItem(item);

            this.DatabaseSession.Derive();

            this.order.PurchaseOrderState = new PurchaseOrderStates(this.DatabaseSession).Finished;
            
            this.DatabaseSession.Derive();

            Assert.Equal(new PurchaseOrderItemStates(this.DatabaseSession).Finished, item.PurchaseOrderItemState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanExecute(M.PurchaseOrderItem.Cancel));
            Assert.False(acl.CanExecute(M.PurchaseOrderItem.Reject));
            Assert.False(acl.CanExecute(M.PurchaseOrderItem.Delete));
        }

        [Fact]
        public void GivenOrderItem_WhenObjectStateIsPartiallyReceived_ThenProductChangeIsNotAllowed()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive(); 
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(M.VatRate.Rate,21))
                .Build();

            this.SetIdentity("admin");

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddPurchaseOrderItem(item);

            this.order.Confirm();
            
            this.DatabaseSession.Derive();

            var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground).WithShipFromParty(this.supplier).Build();
            var shipmentItem = new ShipmentItemBuilder(this.DatabaseSession).WithGood(good).Build();
            shipment.AddShipmentItem(shipmentItem);

            new ShipmentReceiptBuilder(this.DatabaseSession)
                .WithQuantityAccepted(1)
                .WithShipmentItem(shipmentItem)
                .WithOrderItem(item)
                .Build();

            this.DatabaseSession.Derive();

            shipment.AppsComplete();
            
            this.DatabaseSession.Derive();

            Assert.Equal(new PurchaseOrderItemStates(this.DatabaseSession).PartiallyReceived, item.PurchaseOrderItemState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanWrite(M.PurchaseOrderItem.Product));
        }

        [Fact]
        public void GivenOrderItemWithAssignedDeliveryDate_WhenDeriving_ThenDeliveryDateIsOrderItemAssignedDeliveryDate()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(M.VatRate.Rate,21))
                .Build();

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(1)
                .WithAssignedDeliveryDate(DateTime.UtcNow.AddMonths(1))
                .Build();

            this.order.AddPurchaseOrderItem(item);
            
            this.DatabaseSession.Derive();

            Assert.Equal(item.DeliveryDate, item.AssignedDeliveryDate);
        }

        [Fact]
        public void GivenOrderItemWithoutDeliveryDate_WhenDeriving_ThenDerivedDeliveryDateIsOrderDeliveryDate()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(M.VatRate.Rate,21))
                .Build();

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(1)
                .Build();

            this.order.AddPurchaseOrderItem(item);
            
            this.DatabaseSession.Derive();

            Assert.Equal(item.DeliveryDate, this.order.DeliveryDate);
        }

        private void InstantiateObjects(ISession session)
        {
            this.finishedGood = (FinishedGood)session.Instantiate(this.finishedGood);
            this.currentPurchasePrice = (ProductPurchasePrice)session.Instantiate(this.currentPurchasePrice);
            this.order = (PurchaseOrder)session.Instantiate(this.order);
            this.supplier = (Organisation)session.Instantiate(this.supplier);
        }
    }
}
