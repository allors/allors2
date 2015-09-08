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

    using NUnit.Framework;

    [TestFixture]
    public class PurchaseOrderItemTests : DomainTest
    {
        private FinishedGood finishedGood;
        private ProductPurchasePrice currentPurchasePrice;
        private PurchaseOrder order;
        private Organisation supplier;

        [SetUp]
        public override void Init()
        {
            base.Init();

            var euro = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            ContactMechanism takenViaContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var supplierContactMechanism = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(takenViaContactMechanism)
                .WithUseAsDefault(true)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                .Build();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            this.supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            this.supplier.AddPartyContactMechanism(supplierContactMechanism);

            new SupplierRelationshipBuilder(this.DatabaseSession).WithSupplier(supplier).WithInternalOrganisation(internalOrganisation).Build();

            this.finishedGood = new FinishedGoodBuilder(this.DatabaseSession)
                .WithManufacturerId("10101")
                .WithName("finished good")
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

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();
        }

        [Test]
        public void GivenOrderItem_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var buyer = new OrganisationBuilder(this.DatabaseSession).WithName("buyer").Build();
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var partyContactMechanism = new PartyContactMechanismBuilder(this.DatabaseSession).WithContactMechanism(shipToContactMechanism).Build();
            var part = new RawMaterialBuilder(this.DatabaseSession).WithName("raw stuff").Build();
            buyer.AddPartyContactMechanism(partyContactMechanism);

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var builder = new PurchaseOrderItemBuilder(this.DatabaseSession);
            order.AddPurchaseOrderItem(builder.Build());

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithPart(part);
            order.AddPurchaseOrderItem(builder.Build());

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            builder.WithProduct(new GoodBuilder(this.DatabaseSession).Build());
            var orderItem = builder.Build();
            order.AddPurchaseOrderItem(orderItem);

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            orderItem.RemovePart();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenConfirmedOrderItemForGood_WhenOrderItemIsRemoved_ThenItemIsRemovedFromValidOrderItems()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(VatRates.Meta.Rate, 21))
                .Build();

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddPurchaseOrderItem(item);

            this.DatabaseSession.Derive(true);

            this.order.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, this.order.ValidOrderItems.Count);

            item.Cancel();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(0, this.order.ValidOrderItems.Count);
        }

        [Test]
        public void GivenOrderItemForPart_WhenDerivingPrices_ThenUsePartCurrentPurchasePrice()
        {
            this.InstantiateObjects(this.DatabaseSession);

            const decimal QuantityOrdered = 3;
            var item1 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithPart(this.finishedGood).WithQuantityOrdered(QuantityOrdered).Build();
            this.order.AddPurchaseOrderItem(item1);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(this.currentPurchasePrice.Price, item1.UnitBasePrice);
            Assert.AreEqual(0, item1.UnitDiscount);
            Assert.AreEqual(0, item1.UnitSurcharge);
            Assert.AreEqual(this.currentPurchasePrice.Price, item1.CalculatedUnitPrice);

            Assert.AreEqual(this.currentPurchasePrice.Price * QuantityOrdered, item1.TotalBasePrice);
            Assert.AreEqual(0, item1.TotalDiscount);
            Assert.AreEqual(0, item1.TotalSurcharge);
            Assert.AreEqual(this.currentPurchasePrice.Price * QuantityOrdered, item1.TotalExVat);

            Assert.AreEqual(this.currentPurchasePrice.Price * QuantityOrdered, this.order.TotalBasePrice);
            Assert.AreEqual(0, this.order.TotalDiscount);
            Assert.AreEqual(0, this.order.TotalSurcharge);
            Assert.AreEqual(this.currentPurchasePrice.Price * QuantityOrdered, this.order.TotalExVat);
        }

        [Test]
        public void GivenOrderItemForProduct_WhenDerivingPrices_ThenUseProductCurrentPurchasePrice()
        {
            var euro = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(VatRates.Meta.Rate,21))
                .WithName("good")
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

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            const decimal QuantityOrdered = 3;
            var item1 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantityOrdered(QuantityOrdered).Build();
            this.order.AddPurchaseOrderItem(item1);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(currentPurchasePriceGood.Price, item1.UnitBasePrice);
            Assert.AreEqual(0, item1.UnitDiscount);
            Assert.AreEqual(0, item1.UnitSurcharge);
            Assert.AreEqual(currentPurchasePriceGood.Price, item1.CalculatedUnitPrice);

            Assert.AreEqual(currentPurchasePriceGood.Price * QuantityOrdered, item1.TotalBasePrice);
            Assert.AreEqual(0, item1.TotalDiscount);
            Assert.AreEqual(0, item1.TotalSurcharge);
            Assert.AreEqual(currentPurchasePriceGood.Price * QuantityOrdered, item1.TotalExVat);

            Assert.AreEqual(currentPurchasePriceGood.Price * QuantityOrdered, this.order.TotalBasePrice);
            Assert.AreEqual(0, this.order.TotalDiscount);
            Assert.AreEqual(0, this.order.TotalSurcharge);
            Assert.AreEqual(currentPurchasePriceGood.Price * QuantityOrdered, this.order.TotalExVat);
        }

        [Test]
        public void GivenOrderItemForPartWithActualPrice_WhenDerivingPrices_ThenUseActualPrice()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var item1 = new PurchaseOrderItemBuilder(this.DatabaseSession).WithPart(this.finishedGood).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            this.order.AddPurchaseOrderItem(item1);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(15, item1.UnitBasePrice);
            Assert.AreEqual(0, item1.UnitDiscount);
            Assert.AreEqual(0, item1.UnitSurcharge);
            Assert.AreEqual(15, item1.CalculatedUnitPrice);
            Assert.AreEqual(0, item1.UnitVat);
            Assert.AreEqual(45, item1.TotalBasePrice);
            Assert.AreEqual(0, item1.TotalDiscount);
            Assert.AreEqual(0, item1.TotalSurcharge);
            Assert.AreEqual(45, item1.TotalExVat);
            Assert.AreEqual(0, item1.TotalVat);
            Assert.AreEqual(45, item1.TotalIncVat);

            Assert.AreEqual(45, this.order.TotalBasePrice);
            Assert.AreEqual(0, this.order.TotalDiscount);
            Assert.AreEqual(0, this.order.TotalSurcharge);
            Assert.AreEqual(45, this.order.TotalExVat);
            Assert.AreEqual(0, this.order.TotalVat);
            Assert.AreEqual(45, this.order.TotalIncVat);
        }

        [Test]
        public void GivenOrderItem_WhenObjectStateIsCreated_ThenItemMayBeDeletedButNotCancelledOrRejected()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(VatRates.Meta.Rate,21))
                .Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("admin", "Forms"), new string[0]);

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddPurchaseOrderItem(item);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new PurchaseOrderItemObjectStates(this.DatabaseSession).Created, item.CurrentObjectState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsTrue(acl.CanExecute(PurchaseOrderItems.Meta.Delete));
            Assert.IsFalse(acl.CanExecute(PurchaseOrderItems.Meta.Cancel));
            Assert.IsFalse(acl.CanExecute(PurchaseOrderItems.Meta.Reject));
        }

        [Test]
        public void GivenOrderItem_WhenObjectStateIsConfirmed_ThenItemMayBeCancelledOrRejectedButNotDeleted()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(VatRates.Meta.Rate,21))
                .Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("admin", "Forms"), new string[0]);

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddPurchaseOrderItem(item);

            this.order.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new PurchaseOrderItemObjectStates(this.DatabaseSession).InProcess, item.CurrentObjectState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsTrue(acl.CanExecute(PurchaseOrderItems.Meta.Cancel));
            Assert.IsTrue(acl.CanExecute(PurchaseOrderItems.Meta.Reject));
            Assert.IsFalse(acl.CanExecute(PurchaseOrderItems.Meta.Delete));
        }

        [Test]
        public void GivenOrderItem_WhenObjectStateIsPartiallyReceived_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(VatRates.Meta.Rate,21))
                .Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("admin", "Forms"), new string[0]);

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(20)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddPurchaseOrderItem(item);

            this.order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipFromParty(this.supplier).Build();
            var shipmentItem = new ShipmentItemBuilder(this.DatabaseSession).WithGood(good).Build();
            shipment.AddShipmentItem(shipmentItem);

            new ShipmentReceiptBuilder(this.DatabaseSession)
                .WithQuantityAccepted(3)
                .WithShipmentItem(shipmentItem)
                .WithOrderItem(item)
                .Build();

            shipment.Complete();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new PurchaseOrderItemObjectStates(this.DatabaseSession).PartiallyReceived, item.CurrentObjectState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsFalse(acl.CanExecute(PurchaseOrderItems.Meta.Cancel));
            Assert.IsFalse(acl.CanExecute(PurchaseOrderItems.Meta.Reject));
            Assert.IsFalse(acl.CanExecute(PurchaseOrderItems.Meta.Delete));
        }

        [Test]
        public void GivenOrderItem_WhenObjectStateIsCancelled_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive(true);           
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(VatRates.Meta.Rate,21))
                .Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("admin", "Forms"), new string[0]);

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddPurchaseOrderItem(item);            

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            item.Cancel();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new PurchaseOrderItemObjectStates(this.DatabaseSession).Cancelled, item.CurrentObjectState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsFalse(acl.CanExecute(PurchaseOrderItems.Meta.Cancel));
            Assert.IsFalse(acl.CanExecute(PurchaseOrderItems.Meta.Reject));
            Assert.IsFalse(acl.CanExecute(PurchaseOrderItems.Meta.Delete));
        }

        [Test]
        public void GivenOrderItem_WhenObjectStateIsRejected_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(VatRates.Meta.Rate,21))
                .Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("admin", "Forms"), new string[0]);

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddPurchaseOrderItem(item);
            
            this.DatabaseSession.Derive(true);

            item.Reject();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new PurchaseOrderItemObjectStates(this.DatabaseSession).Rejected, item.CurrentObjectState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsFalse(acl.CanExecute(PurchaseOrderItems.Meta.Cancel));
            Assert.IsFalse(acl.CanExecute(PurchaseOrderItems.Meta.Reject));
            Assert.IsFalse(acl.CanExecute(PurchaseOrderItems.Meta.Delete));
        }

        [Test]
        public void GivenOrderItem_WhenObjectStateIsCompleted_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(VatRates.Meta.Rate,21))
                .Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("admin", "Forms"), new string[0]);

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddPurchaseOrderItem(item);
            
            this.order.Confirm();
            
            this.DatabaseSession.Derive(true);

            var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipFromParty(this.supplier).Build();
            var shipmentItem = new ShipmentItemBuilder(this.DatabaseSession).WithGood(good).Build();
            shipment.AddShipmentItem(shipmentItem);

            new ShipmentReceiptBuilder(this.DatabaseSession)
                .WithQuantityAccepted(3)
                .WithShipmentItem(shipmentItem)
                .WithOrderItem(item)
                .Build();

            shipment.Complete();
            
            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new PurchaseOrderItemObjectStates(this.DatabaseSession).Completed, item.CurrentObjectState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsFalse(acl.CanExecute(PurchaseOrderItems.Meta.Cancel));
            Assert.IsFalse(acl.CanExecute(PurchaseOrderItems.Meta.Reject));
            Assert.IsFalse(acl.CanExecute(PurchaseOrderItems.Meta.Delete));
        }

        [Test]
        public void GivenOrderItem_WhenObjectStateIsFinished_ThenItemMayNotBeCancelledOrRejectedOrDeleted()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(VatRates.Meta.Rate,21))
                .Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("admin", "Forms"), new string[0]);

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddPurchaseOrderItem(item);

            this.order.Finish();
            
            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new PurchaseOrderItemObjectStates(this.DatabaseSession).Finished, item.CurrentObjectState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsFalse(acl.CanExecute(PurchaseOrderItems.Meta.Cancel));
            Assert.IsFalse(acl.CanExecute(PurchaseOrderItems.Meta.Reject));
            Assert.IsFalse(acl.CanExecute(PurchaseOrderItems.Meta.Delete));
        }

        [Test]
        public void GivenOrderItem_WhenObjectStateIsPartiallyReceived_ThenProductChangeIsNotAllowed()
        {
            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            
            this.DatabaseSession.Derive(true); 
            this.DatabaseSession.Commit();

            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(VatRates.Meta.Rate,21))
                .Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("admin", "Forms"), new string[0]);

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            this.order.AddPurchaseOrderItem(item);

            this.order.Confirm();
            
            this.DatabaseSession.Derive(true);

            var shipment = new PurchaseShipmentBuilder(this.DatabaseSession).WithShipFromParty(this.supplier).Build();
            var shipmentItem = new ShipmentItemBuilder(this.DatabaseSession).WithGood(good).Build();
            shipment.AddShipmentItem(shipmentItem);

            new ShipmentReceiptBuilder(this.DatabaseSession)
                .WithQuantityAccepted(1)
                .WithShipmentItem(shipmentItem)
                .WithOrderItem(item)
                .Build();

            shipment.Complete();
            
            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new PurchaseOrderItemObjectStates(this.DatabaseSession).PartiallyReceived, item.CurrentObjectState);
            var acl = new AccessControlList(item, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsFalse(acl.CanWrite(PurchaseOrderItems.Meta.Product));
        }

        [Test]
        public void GivenOrderItemWithAssignedDeliveryDate_WhenDeriving_ThenDeliveryDateIsOrderItemAssignedDeliveryDate()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(VatRates.Meta.Rate,21))
                .Build();

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(1)
                .WithAssignedDeliveryDate(DateTime.UtcNow.AddMonths(1))
                .Build();

            this.order.AddPurchaseOrderItem(item);
            
            this.DatabaseSession.Derive(true);

            Assert.AreEqual(item.DeliveryDate, item.AssignedDeliveryDate);
        }

        [Test]
        public void GivenOrderItemWithoutDeliveryDate_WhenDeriving_ThenDerivedDeliveryDateIsOrderDeliveryDate()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithVatRate(new VatRates(this.DatabaseSession).FindBy(VatRates.Meta.Rate,21))
                .Build();

            var item = new PurchaseOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(1)
                .Build();

            this.order.AddPurchaseOrderItem(item);
            
            this.DatabaseSession.Derive(true);

            Assert.AreEqual(item.DeliveryDate, this.order.DeliveryDate);
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
