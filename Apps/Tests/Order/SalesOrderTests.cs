//------------------------------------------------------------------------------------------------- 
// <copyright file="SalesOrderTests.cs" company="Allors bvba">
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

    using Resources;

    [TestFixture]
    public class SalesOrderTests : DomainTest
    {
        [Test]
        public void GivenSalesOrderBuilder_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).Provisional, order.CurrentObjectState);
            Assert.IsTrue(order.PartiallyShip);
            Assert.AreEqual(DateTime.UtcNow.Date, order.OrderDate.Date);
            Assert.AreEqual(DateTime.UtcNow.Date, order.EntryDate.Date);
            Assert.AreEqual(DateTime.UtcNow.Date, order.DeliveryDate.Date);
            Assert.AreEqual(order.PreviousBillToCustomer, order.BillToCustomer);
            Assert.AreEqual(order.PreviousShipToCustomer, order.ShipToCustomer);
            Assert.AreEqual(order.VatRegime, order.BillToCustomer.VatRegime);
            Assert.AreEqual(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"), order.TakenByInternalOrganisation);
            Assert.AreEqual(new Stores(this.DatabaseSession).FindBy(Stores.Meta.Name, "store"), order.Store);
            Assert.AreEqual(order.TakenByInternalOrganisation.PreferredCurrency, order.CustomerCurrency);
            Assert.AreEqual(order.Store.DefaultPaymentMethod, order.PaymentMethod);
            Assert.AreEqual(order.Store.DefaultShipmentMethod, order.ShipmentMethod);
        }

        [Test]
        public void GivenSalesOrderForItemsThatAreAvailable_WhenShipped_ThenOrderIsCompleted()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var billToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPartyContactMechanism(billToMechelen).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .Build();

            var good1InventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            good1InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var good2InventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good2).Build();
            good2InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(mechelenAddress)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)mechelenAddress.ShipmentsWhereShipToAddress[0];

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "orderProcessor");

            pickList.SetPicked();
            this.DatabaseSession.Derive(true);

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive(true);

            shipment.Ship();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).Completed, order.CurrentObjectState);
            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).Shipped, order.CurrentShipmentStatus.SalesOrderObjectState);
            Assert.IsFalse(order.ExistCurrentPaymentStatus);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Completed, item1.CurrentObjectState);
            Assert.IsFalse(item1.ExistCurrentPaymentStatus);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Shipped, item1.CurrentShipmentStatus.SalesOrderItemObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Completed, item2.CurrentObjectState);
            Assert.IsFalse(item2.ExistCurrentPaymentStatus);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Shipped, item2.CurrentShipmentStatus.SalesOrderItemObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Completed, item3.CurrentObjectState);
            Assert.IsFalse(item3.ExistCurrentPaymentStatus);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Shipped, item3.CurrentShipmentStatus.SalesOrderItemObjectState);
        }

        [Test]
        public void GivenSalesOrderCreatedByEmployee_WhenCurrentUserInAnotherEmployeeUserGroup_ThenAccessIsDenied()
        {
            var belgium = new Countries(this.DatabaseSession).CountryByIsoCode["BE"];
            var euro = belgium.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("BE23 3300 6167 6391")
                .WithBankAccount(new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            var customer = new Organisations(this.DatabaseSession).FindBy(Organisations.Meta.Name, "customer");
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var salesRep2 = new PersonBuilder(this.DatabaseSession).WithLastName("SalesRep2").WithUserName("salesRep2").Build();

            var address1 = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var billingAddress = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(address1)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var employer2 = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("employer2")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithPreferredCurrency(euro)
                .WithDefaultPaymentMethod(ownBankAccount)
                .WithPartyContactMechanism(billingAddress)
                .Build();

            new EmploymentBuilder(this.DatabaseSession).WithEmployee(salesRep2).WithEmployer(employer2).WithFromDate(DateTime.UtcNow).Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithSalesRepresentative(salesRep2).WithInternalOrganisation(employer2).Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("salesRep", "Forms"), new string[0]);
            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithTakenByInternalOrganisation(internalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanRead(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Confirm));

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("salesRep2", "Forms"), new string[0]);
            acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsFalse(acl.HasReadOperation);
        }

        [Test]
        public void GivenSalesOrderShippedInMultipleParts_WhenPaymentsAreReceived_ThenObjectStateCorrespondingSalesOrderIsUpdated()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .Build();

            var good1Inventory = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            good1Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(1).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate0)
                .WithName("good2")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .Build();

            var good2Inventory = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good2).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var billToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).WithPartyContactMechanism(billToMechelen).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)item1.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "orderProcessor");

            pickList.SetPicked();

            this.DatabaseSession.Derive(true);

            var package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive(true);

            shipment.Ship();

            this.DatabaseSession.Derive(true);

            var salesInvoiceitem = (SalesInvoiceItem)shipment.ShipmentItems[0].InvoiceItems[0];
            var invoice1 = salesInvoiceitem.SalesInvoiceWhereSalesInvoiceItem;

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(15)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice1.SalesInvoiceItems[0]).WithAmountApplied(15).Build())
                .WithEffectiveDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).InProcess, order.CurrentObjectState);
            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).PartiallyShipped, order.CurrentShipmentStatus.SalesOrderObjectState);
            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).PartiallyPaid, order.CurrentPaymentStatus.SalesOrderObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Completed, item1.CurrentObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Paid, item1.CurrentPaymentStatus.SalesOrderItemObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Shipped, item1.CurrentShipmentStatus.SalesOrderItemObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).InProcess, item2.CurrentObjectState);
            Assert.IsFalse(item2.ExistCurrentPaymentStatus);
            Assert.IsFalse(item2.ExistCurrentShipmentStatus);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).InProcess, item3.CurrentObjectState);
            Assert.IsFalse(item3.ExistCurrentPaymentStatus);
            Assert.IsFalse(item3.ExistCurrentShipmentStatus);

            good1Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            shipment = (CustomerShipment)item2.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "orderProcessor");

            pickList.SetPicked();

            this.DatabaseSession.Derive(true);

            package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive(true);

            shipment.Ship();

            this.DatabaseSession.Derive(true);

            salesInvoiceitem = (SalesInvoiceItem)shipment.ShipmentItems[0].InvoiceItems[0];
            var invoice2 = salesInvoiceitem.SalesInvoiceWhereSalesInvoiceItem;

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(30)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice2.SalesInvoiceItems[0]).WithAmountApplied(30).Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).InProcess, order.CurrentObjectState);
            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).PartiallyShipped, order.CurrentShipmentStatus.SalesOrderObjectState);
            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).PartiallyPaid, order.CurrentPaymentStatus.SalesOrderObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Completed, item1.CurrentObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Paid, item1.CurrentPaymentStatus.SalesOrderItemObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Shipped, item1.CurrentShipmentStatus.SalesOrderItemObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Completed, item2.CurrentObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Paid, item2.CurrentPaymentStatus.SalesOrderItemObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Shipped, item2.CurrentShipmentStatus.SalesOrderItemObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).InProcess, item3.CurrentObjectState);
            Assert.IsFalse(item3.ExistCurrentPaymentStatus);
            Assert.IsFalse(item3.ExistCurrentShipmentStatus);

            good2Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            shipment = (CustomerShipment)item3.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;

            pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            pickList.Picker = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "orderProcessor");

            pickList.SetPicked();

            this.DatabaseSession.Derive(true);

            package = new ShipmentPackageBuilder(this.DatabaseSession).Build();
            shipment.AddShipmentPackage(package);

            foreach (ShipmentItem shipmentItem in shipment.ShipmentItems)
            {
                package.AddPackagingContent(new PackagingContentBuilder(this.DatabaseSession).WithShipmentItem(shipmentItem).WithQuantity(shipmentItem.Quantity).Build());
            }

            this.DatabaseSession.Derive(true);

            shipment.Ship();

            this.DatabaseSession.Derive(true);

            salesInvoiceitem = (SalesInvoiceItem)shipment.ShipmentItems[0].InvoiceItems[0];
            var invoice3 = salesInvoiceitem.SalesInvoiceWhereSalesInvoiceItem;

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(75)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice3.SalesInvoiceItems[0]).WithAmountApplied(75).Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).Finished, order.CurrentObjectState);
            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).Shipped, order.CurrentShipmentStatus.SalesOrderObjectState);
            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).Paid, order.CurrentPaymentStatus.SalesOrderObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Finished, item1.CurrentObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Paid, item1.CurrentPaymentStatus.SalesOrderItemObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Shipped, item1.CurrentShipmentStatus.SalesOrderItemObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Finished, item2.CurrentObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Paid, item2.CurrentPaymentStatus.SalesOrderItemObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Shipped, item2.CurrentShipmentStatus.SalesOrderItemObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Finished, item3.CurrentObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Paid, item3.CurrentPaymentStatus.SalesOrderItemObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Shipped, item3.CurrentShipmentStatus.SalesOrderItemObjectState);
        }

        [Test]
        public void GivenPendingShipmentAndAssignedPickList_WhenNewOrderIsConfirmed_ThenNewPickListIsCreatedAndSingleOrderShipmentIsUpdated()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(10).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var order1 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(mechelenAddress)
                .WithVatRegime(assessable)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            order1.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order1.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.AreEqual(3, shipment.ShipmentItems[0].Quantity);

            var pickList1 = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.AreEqual(3, pickList1.PickListItems[0].RequestedQuantity);

            pickList1.Picker = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "orderProcessor");

            this.DatabaseSession.Derive(true);

            var order2 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(mechelenAddress)
                .WithVatRegime(assessable)
                .Build();

            item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(2)
                .WithActualUnitPrice(5)
                .Build();

            order2.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order2.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(5, shipment.ShipmentItems.First.Quantity);

            var pickList2 = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[1].PickListItem.PickListWherePickListItem;
            Assert.AreEqual(2, pickList2.PickListItems[0].RequestedQuantity);
        }

        [Test]
        public void GivenSalesOrderWithPendingShipmentAndAssignedPickList_WhenOrderIsCancelled_ThenNegativePickListIsCreatedAndSingleOrderShipmentIsCancelled()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(10).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(mechelenAddress)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)item.OrderShipmentsWhereSalesOrderItem[0].ShipmentItem.ShipmentWhereShipmentItem;
            Assert.AreEqual(10, shipment.ShipmentItems[0].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.AreEqual(10, pickList.PickListItems[0].RequestedQuantity);

            pickList.Picker = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "orderProcessor");

            this.DatabaseSession.Derive(true);

            order.Cancel();

            this.DatabaseSession.Derive(true);

            var negativePickList = order.ShipToCustomer.PickListsWhereShipToParty[1];
            Assert.AreEqual(-10, negativePickList.PickListItems[0].RequestedQuantity);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Cancelled, shipment.CurrentObjectState);
        }

        [Test]
        public void GivenSalesOrderWithPickList_WhenOrderIsCancelled_ThenPickListIsCancelledAndSingleOrderShipmentIsCancelled()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var order1 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(mechelenAddress)
                .WithVatRegime(assessable)
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            order1.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order1.Confirm();

            this.DatabaseSession.Derive(true);

            var order2 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(mechelenAddress)
                .WithVatRegime(assessable)
                .Build();

            item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(20)
                .WithActualUnitPrice(5)
                .Build();

            order2.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order2.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = (CustomerShipment)customer.ShipmentsWhereBillToParty[0];
            Assert.AreEqual(30, shipment.ShipmentItems[0].Quantity);

            var pickList = shipment.ShipmentItems[0].ItemIssuancesWhereShipmentItem[0].PickListItem.PickListWherePickListItem;
            Assert.AreEqual(30, pickList.PickListItems[0].RequestedQuantity);

            order1.Cancel();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Created, shipment.CurrentObjectState);
            Assert.AreEqual(new PickListObjectStates(this.DatabaseSession).Created, pickList.CurrentObjectState);
            Assert.AreEqual(20, pickList.PickListItems[0].RequestedQuantity);

            order2.Cancel();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new CustomerShipmentObjectStates(this.DatabaseSession).Cancelled, shipment.CurrentObjectState);
            Assert.AreEqual(new PickListObjectStates(this.DatabaseSession).Cancelled, pickList.CurrentObjectState);
        }

        [Test]
        public void GivenSalesOrderOnHold_WhenInventoryBecomesAvailable_ThenOrderIsNotSelectedForShipment()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .Build();

            var inventory = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            order.Hold();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).OnHold, order.CurrentObjectState);
            Assert.AreEqual(0, item.QuantityPendingShipment);
            Assert.AreEqual(10, item.QuantityShortFalled);

            inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).OnHold, order.CurrentObjectState);
            Assert.AreEqual(0, item.QuantityPendingShipment);
            Assert.AreEqual(10, item.QuantityRequestsShipping);
            Assert.AreEqual(0, item.QuantityShortFalled);
        }

        [Test]
        public void GivenSalesOrderOnHold_WhenOrderIsContinued_ThenOrderIsSelectedForShipment()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .Build();

            var inventory = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            order.Hold();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).OnHold, order.CurrentObjectState);
            Assert.AreEqual(0, item.QuantityPendingShipment);
            Assert.AreEqual(10, item.QuantityShortFalled);

            inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).OnHold, order.CurrentObjectState);
            Assert.AreEqual(0, item.QuantityPendingShipment);
            Assert.AreEqual(10, item.QuantityRequestsShipping);
            Assert.AreEqual(0, item.QuantityShortFalled);

            order.Continue();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).InProcess, order.CurrentObjectState);
            Assert.AreEqual(10, item.QuantityPendingShipment);
            Assert.AreEqual(0, item.QuantityRequestsShipping);
            Assert.AreEqual(0, item.QuantityShortFalled);
        }

        [Test]
        public void GivenSalesOrderNotPartiallyShipped_WhenInComplete_ThenOrderIsNotSelectedForShipment()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .Build();

            var inventoryGood1 = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            inventoryGood1.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(10).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("20202")
                .WithVatRate(vatRate0)
                .WithName("good2")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .Build();

            var inventoryGood2 = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good2).Build();
            inventoryGood2.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(10).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithPartiallyShip(false)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good1)
                .WithQuantityOrdered(20)
                .WithActualUnitPrice(5)
                .Build();

            var item2 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good2)
                .WithQuantityOrdered(20)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.IsFalse(customer.ExistShipmentsWhereShipToParty);

            Assert.AreEqual(10, item1.QuantityRequestsShipping);
            Assert.AreEqual(0, item1.QuantityPendingShipment);
            Assert.AreEqual(10, item1.QuantityShortFalled);

            Assert.AreEqual(10, item2.QuantityRequestsShipping);
            Assert.AreEqual(0, item2.QuantityPendingShipment);
            Assert.AreEqual(10, item2.QuantityShortFalled);

            inventoryGood1.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            Assert.IsFalse(customer.ExistShipmentsWhereShipToParty);

            Assert.AreEqual(20, item1.QuantityRequestsShipping);
            Assert.AreEqual(0, item1.QuantityPendingShipment);
            Assert.AreEqual(0, item1.QuantityShortFalled);

            Assert.AreEqual(10, item2.QuantityRequestsShipping);
            Assert.AreEqual(0, item2.QuantityPendingShipment);
            Assert.AreEqual(10, item2.QuantityShortFalled);

            inventoryGood2.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            Assert.IsTrue(customer.ExistShipmentsWhereShipToParty);

            Assert.AreEqual(0, item1.QuantityRequestsShipping);
            Assert.AreEqual(20, item1.QuantityPendingShipment);
            Assert.AreEqual(0, item1.QuantityShortFalled);

            Assert.AreEqual(0, item2.QuantityRequestsShipping);
            Assert.AreEqual(20, item2.QuantityPendingShipment);
            Assert.AreEqual(0, item2.QuantityShortFalled);
        }

        [Test]
        public void GivenSalesOrderForStoreExceedingCreditLimit_WhenOrderIsConfirmed_ThenOrderRequestsApproval()
        {
            var productItem = new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem;
            var contactMechanism = new ContactMechanisms(this.DatabaseSession).Extent().First;

            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithInvoiceDate(DateTime.UtcNow.AddYears(-1))
                .Build();

            var invoiceItem = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(10).WithActualUnitPrice(100).WithSalesInvoiceItemType(productItem).Build();
            invoice.AddSalesInvoiceItem(invoiceItem);

            this.DatabaseSession.Derive(true);

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).RequestsApproval, order.CurrentObjectState);
            Assert.AreEqual(0, item.QuantityReserved);
            Assert.AreEqual(0, item.QuantityPendingShipment);
            Assert.AreEqual(0, item.QuantityRequestsShipping);
            Assert.AreEqual(0, item.QuantityShortFalled);

            order.Approve();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).InProcess, order.CurrentObjectState);
            Assert.AreEqual(10, item.QuantityReserved);
            Assert.AreEqual(10, item.QuantityPendingShipment);
            Assert.AreEqual(0, item.QuantityRequestsShipping);
            Assert.AreEqual(0, item.QuantityShortFalled);
        }

        [Test]
        public void GivenSalesOrderForCustomerExceedingCreditLimit_WhenOrderIsConfirmed_ThenOrderRequestsApproval()
        {
            var productItem = new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem;
            var contactMechanism = new ContactMechanisms(this.DatabaseSession).Extent().First;

            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation)
                .WithCreditLimit(100M)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithInvoiceDate(DateTime.UtcNow.AddYears(-1))
                .Build();

            var invoiceItem = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(10).WithActualUnitPrice(11).WithSalesInvoiceItemType(productItem).Build();
            invoice.AddSalesInvoiceItem(invoiceItem);

            this.DatabaseSession.Derive(true);

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).RequestsApproval, order.CurrentObjectState);
            Assert.AreEqual(0, item.QuantityReserved);
            Assert.AreEqual(0, item.QuantityPendingShipment);
            Assert.AreEqual(0, item.QuantityRequestsShipping);
            Assert.AreEqual(0, item.QuantityShortFalled);

            order.Approve();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).InProcess, order.CurrentObjectState);
            Assert.AreEqual(10, item.QuantityReserved);
            Assert.AreEqual(10, item.QuantityPendingShipment);
            Assert.AreEqual(0, item.QuantityRequestsShipping);
            Assert.AreEqual(0, item.QuantityShortFalled);
        }

        [Test]
        public void GivenSalesOrderBelowOrderThreshold_WhenOrderIsConfirmed_ThenOrderIsNotShipped()
        {
            Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation.StoresWhereOwner.First.OrderThreshold = 1;

            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation)
                .Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(1)
                .WithActualUnitPrice(0.1M)
                .Build();

            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).RequestsApproval, order.CurrentObjectState);
        }

        [Test]
        public void GivenSalesOrderWithManualShipmentSchedule_WhenOrderIsConfirmed_ThenInventoryIsNotReservedAndOrderIsNotShipped()
        {
            var assessable = new VatRegimes(this.DatabaseSession).Assessable;
            var vatRate0 = new VatRateBuilder(this.DatabaseSession).WithRate(0).Build();
            assessable.VatRate = vatRate0;

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate0)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .Build();

            var inventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good).Build();
            inventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPartyContactMechanism(shipToMechelen).Build();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation)
                .Build();

            var manual = new OrderKindBuilder(this.DatabaseSession).WithDescription("manual").WithScheduleManually(true).Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithVatRegime(assessable)
                .WithOrderKind(manual)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(50)
                .WithActualUnitPrice(50)
                .Build();

            order.AddSalesOrderItem(item);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).InProcess, order.CurrentObjectState);
            Assert.AreEqual(0, item.QuantityReserved);
            Assert.AreEqual(0, item.QuantityPendingShipment);
            Assert.AreEqual(0, item.QuantityRequestsShipping);
            Assert.AreEqual(0, item.QuantityShortFalled);
            Assert.AreEqual(100, inventoryItem.QuantityOnHand);
            Assert.AreEqual(100, inventoryItem.AvailableToPromise);
        }

        [Test]
        public void GivenConfirmedOrder_WhenOrderIsRejected_ThenNonSerializedInventoryQuantitiesAreReleased()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(billToCustomer).WithInternalOrganisation(internalOrganisation).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(shipToCustomer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part1").Build();
            var part2 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part2").Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(shipToContactMechanism)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(4).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(3, item1.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.AreEqual(0, item1.ReservedFromInventoryItem.AvailableToPromise);
            Assert.AreEqual(7, item3.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.AreEqual(0, item3.ReservedFromInventoryItem.AvailableToPromise);

            order.Reject();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(0, item1.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.AreEqual(0, item1.ReservedFromInventoryItem.AvailableToPromise);
            Assert.AreEqual(0, item3.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.AreEqual(0, item3.ReservedFromInventoryItem.AvailableToPromise);
        }

        [Test]
        public void GivenConfirmedOrder_WhenOrderIsCancelled_ThenNonSerializedInventoryQuantitiesAreReleased()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(billToCustomer).WithInternalOrganisation(internalOrganisation).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(shipToCustomer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part1").Build();
            var part2 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part2").Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(shipToContactMechanism)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(4).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(3, item1.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.AreEqual(0, item1.ReservedFromInventoryItem.AvailableToPromise);
            Assert.AreEqual(7, item3.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.AreEqual(0, item3.ReservedFromInventoryItem.AvailableToPromise);

            order.Cancel();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(0, item1.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.AreEqual(0, item1.ReservedFromInventoryItem.AvailableToPromise);
            Assert.AreEqual(0, item3.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.AreEqual(0, item3.ReservedFromInventoryItem.AvailableToPromise);
        }

        [Test]
        public void GivenSalesOrder_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithTakenByInternalOrganisation(internalOrganisation)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).Provisional, order.CurrentObjectState);
            Assert.AreEqual(order.LastObjectState, order.CurrentObjectState);
        }

        [Test]
        public void GivenSalesOrder_WhenBuild_ThenPreviousObjectStateIsNull()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithTakenByInternalOrganisation(internalOrganisation)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.IsNull(order.PreviousObjectState);
        }

        [Test]
        public void GivenSalesOrder_WhenDeriving_ThenBillToCustomerMustBeInternalOrganisationCustomer()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithTakenByInternalOrganisation(internalOrganisation)
                .Build();

            var expectedError = ErrorMessages.PartyIsNotACustomer;
            Assert.AreEqual(expectedError, this.DatabaseSession.Derive().Errors[0].Message);

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenSalesOrder_WhenDeriving_ThenShipToCustomerMustBeInternalOrganisationCustomer()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var billToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("billToCustomer").Build();
            var shipToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("shipToCustomer").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithTakenByInternalOrganisation(internalOrganisation)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(billToCustomer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            var expectedError = ErrorMessages.PartyIsNotACustomer;
            Assert.AreEqual(expectedError, this.DatabaseSession.Derive().Errors[0].Message);

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(shipToCustomer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenSalesOrder_WhenConfirmed_ThenCurrentOrderStatusMustBeDerived()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithTakenByInternalOrganisation(internalOrganisation)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, order.OrderStatuses.Count);
            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).Provisional, order.CurrentOrderStatus.SalesOrderObjectState);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, order.OrderStatuses.Count);
            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).InProcess, order.CurrentOrderStatus.SalesOrderObjectState);
        }

        [Test]
        public void GivenSalesOrderCreatedByEmployee_WhenDerived_ThenOrderSecurityTokenEqualsEmployerOwnerSecurityToken()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var user = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("user").Build();

            new EmploymentBuilder(this.DatabaseSession)
                .WithEmployee(user)
                .WithEmployer(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("user", "Forms"), new string[0]);

            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.Contains(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation").OwnerSecurityToken, order.SecurityTokens);
        }

        [Test]
        public void GivenSalesOrderWithCancelledItem_WhenDeriving_ThenCancelledItemIsNotInValidOrderItems()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(billToCustomer).WithInternalOrganisation(internalOrganisation).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(shipToCustomer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part1").Build();
            var part2 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part2").Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            this.DatabaseSession.Derive(true);

            item4.Cancel();

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(3, order.ValidOrderItems.Count);
            Assert.Contains(item1, order.ValidOrderItems);
            Assert.Contains(item2, order.ValidOrderItems);
            Assert.Contains(item3, order.ValidOrderItems);
        }

        [Test]
        public void GivenSalesOrderBuilder_WhenBuild_ThenOrderMustBeValid()
        {
            new SalesOrderBuilder(this.DatabaseSession).Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenSalesOrder_WhenGettingOrderNumberWithoutFormat_ThenOrderNumberShouldBeReturned()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(billToCustomer).WithInternalOrganisation(internalOrganisation).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(shipToCustomer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            var store = new StoreBuilder(this.DatabaseSession).WithName("store")
                .WithDefaultFacility(new Warehouses(this.DatabaseSession).FindBy(Warehouses.Meta.Name, "facility"))
                .WithOwner(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .Build();

            var order1 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithTakenByInternalOrganisation(internalOrganisation)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithStore(store)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual("1", order1.OrderNumber);

            var order2 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithTakenByInternalOrganisation(internalOrganisation)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithStore(store)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual("2", order2.OrderNumber);
        }

        [Test]
        public void GivenSalesOrder_WhenGettingOrderNumberWithFormat_ThenFormattedOrderNumberShouldBeReturned()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(billToCustomer).WithInternalOrganisation(internalOrganisation).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(shipToCustomer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            var store = new StoreBuilder(this.DatabaseSession).WithName("store")
                .WithDefaultFacility(new Warehouses(this.DatabaseSession).FindBy(Warehouses.Meta.Name, "facility"))
                .WithOwner(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithSalesOrderNumberPrefix("the format is ")
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .Build();

            var order1 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithTakenByInternalOrganisation(internalOrganisation)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithStore(store)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual("the format is 1", order1.OrderNumber);

            var order2 = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithTakenByInternalOrganisation(internalOrganisation)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithStore(store)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual("the format is 2", order2.OrderNumber);
        }

        [Test]
        public void GivenSalesOrder_WhenDeriving_ThenDerivedSalesRepMustExist()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var salesrep = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep").Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithCustomer(customer)
                .WithSalesRepresentative(salesrep)
                .Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part1").Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            order.AddSalesOrderItem(new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(10).Build());

            this.DatabaseSession.Derive(true);

            Assert.Contains(salesrep, order.SalesReps);
        }

        [Test]
        public void GivenSalesOrder_WhenDeriving_ThenTakenByContactMechanismMustExist()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var orderContact = new EmailAddressBuilder(this.DatabaseSession).WithElectronicAddressString("orders@acme.com").Build();

            var orderContactMechanism = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(orderContact)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).OrderAddress)
                .WithUseAsDefault(true)
                .Build();

            internalOrganisation.AddPartyContactMechanism(orderContactMechanism);

            this.DatabaseSession.Derive(true);

            var order1 = new SalesOrderBuilder(this.DatabaseSession)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(orderContact, order1.TakenByContactMechanism);
        }

        [Test]
        public void GivenSalesOrder_WhenDeriving_ThenBillFromContactMechanismMustExist()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var billingContact = new EmailAddressBuilder(this.DatabaseSession).WithElectronicAddressString("orders@acme.com").Build();

            var billingContactMechanism = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(billingContact)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            internalOrganisation.AddPartyContactMechanism(billingContactMechanism);

            this.DatabaseSession.Derive(true);

            var order1 = new SalesOrderBuilder(this.DatabaseSession)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(billingContact, order1.BillFromContactMechanism);
        }

        [Test]
        public void GivenSalesOrderWithBillToCustomerWithPreferredCurrency_WhenBuild_ThenCurrencyIsFromCustomer()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var englischLocale = new Locales(this.DatabaseSession).EnglishGreatBritain;
            var poundSterling = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "GBP");

            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithLocale(englischLocale).WithPreferredCurrency(poundSterling).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            this.DatabaseSession.Derive(true);

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(poundSterling, order.CustomerCurrency);

            var euro = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");
            customer.PreferredCurrency = euro;

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(englischLocale.Country.Currency, order.CustomerCurrency);
        }

        [Test]
        public void GivenSalesOrder_WhenDeriving_ThenLocaleMustExist()
        {
            var englischLocale = new Locales(this.DatabaseSession).EnglishGreatBritain;

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var order = new SalesOrderBuilder(this.DatabaseSession).WithBillToCustomer(customer).WithShipToAddress(shipToContactMechanism).Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(englischLocale, order.Locale);
        }

        [Test]
        public void GivenSalesOrderCreatedByCustomer_WhenCurrentUserInSupplierRole_ThenAccessIsDenied()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithUserName("customer").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var supplierContact = new PersonBuilder(this.DatabaseSession).WithLastName("suppliercontact").WithUserName("suppliercontact").Build();
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();

            new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithSupplier(supplier)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new OrganisationContactRelationshipBuilder(this.DatabaseSession).WithContact(supplierContact).WithOrganisation(supplier).WithFromDate(DateTime.UtcNow).Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customer", "Forms"), new string[0]);
            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("suppliercontact", "Forms"), new string[0]);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsFalse(acl.HasReadOperation);
        }

        [Test]
        public void GivenSalesOrderCreatedByCustomerContact_WhenCurrentUserIsCreatorOfOrder_ThenAccessIsGranted()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var customerContact = new PersonBuilder(this.DatabaseSession).WithLastName("customercontact").WithUserName("customercontact").Build();
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new OrganisationContactRelationshipBuilder(this.DatabaseSession).WithContact(customerContact).WithOrganisation(customer).WithFromDate(DateTime.UtcNow).Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact", "Forms"), new string[0]);
            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithShipToCustomer(customer)
                .WithBillToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanRead(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Confirm));
        }

        [Test]
        public void GivenSalesOrderCreatedByCustomerContact_WhenCurrentUserIsAnotherCustomerContact_ThenAccessIsGranted()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var customerContact = new PersonBuilder(this.DatabaseSession).WithLastName("customercontact").WithUserName("customercontact").Build();
            var customerContact2 = new PersonBuilder(this.DatabaseSession).WithLastName("customercontact2").WithUserName("customercontact2").Build();
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new OrganisationContactRelationshipBuilder(this.DatabaseSession).WithContact(customerContact).WithOrganisation(customer).WithFromDate(DateTime.UtcNow).Build();
            new OrganisationContactRelationshipBuilder(this.DatabaseSession).WithContact(customerContact2).WithOrganisation(customer).WithFromDate(DateTime.UtcNow).Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact", "Forms"), new string[0]);
            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithShipToCustomer(customer)
                .WithBillToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact2", "Forms"), new string[0]);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanRead(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Confirm));
        }

        [Test]
        public void GivenSalesOrderCreatedByCustomerContact_WhenCurrentUserIsContactAtDifferentCustomer_ThenAccessIsDenied()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var customerContact = new PersonBuilder(this.DatabaseSession).WithLastName("customercontact").WithUserName("customercontact").Build();
            var customerContact2 = new PersonBuilder(this.DatabaseSession).WithLastName("customercontact2").WithUserName("customercontact2").Build();
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var customer2 = new OrganisationBuilder(this.DatabaseSession).WithName("customer2").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer2)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new OrganisationContactRelationshipBuilder(this.DatabaseSession).WithContact(customerContact).WithOrganisation(customer).WithFromDate(DateTime.UtcNow).Build();
            new OrganisationContactRelationshipBuilder(this.DatabaseSession).WithContact(customerContact2).WithOrganisation(customer2).WithFromDate(DateTime.UtcNow).Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact", "Forms"), new string[0]);
            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithShipToCustomer(customer)
                .WithBillToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact2", "Forms"), new string[0]);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsFalse(acl.HasReadOperation);
        }

        [Test]
        public void GivenSalesOrderCreatedByCustomer_WhenCurrentUserInAdministratorRole_ThenAccessIsGranted()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithUserName("customer").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);
            this.DatabaseSession.Derive(true);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customer", "Forms"), new string[0]);

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("admin", "Forms"), new string[0]);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanRead(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Confirm));
        }

        [Test]
        public void GivenSalesOrderCreatedByCustomer_WhenCurrentUserIsCreator_ThenAccessIsGranted()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithUserName("customer").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customer", "Forms"), new string[0]);

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanRead(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Confirm));
        }

        [Test]
        public void GivenSalesOrderCreatedByCustomer_WhenCurrentUserAnotherCustomer_ThenAccessIsDenied()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithUserName("customer").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            new PersonBuilder(this.DatabaseSession).WithUserName("customer2").WithLastName("customer2").Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customer1", "Forms"), new string[0]);

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customer2", "Forms"), new string[0]);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsFalse(acl.HasReadOperation);
        }

        [Test]
        public void GivenSalesOrderCreatedBySalesRep_WhenCurrentUserInSameSalesRepUserGroup_ThenAccessIsGranted()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithUserName("customer").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var salesrep2 = new PersonBuilder(this.DatabaseSession).WithLastName("salesRep2").WithUserName("salesRep2").Build();

            new EmploymentBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithEmployee(salesrep2)
                .WithEmployer(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithCustomer(customer)
                .WithSalesRepresentative(salesrep2)
                .Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("salesRep", "Forms"), new string[0]);

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanRead(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Confirm));

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("salesRep2", "Forms"), new string[0]);
            acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanRead(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Confirm));
        }

        [Test]
        public void GivenSalesOrderCreatedByEmployee_WhenCurrentUserIsCustomerContact_ThenAccessIsGranted()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var customerContact = new PersonBuilder(this.DatabaseSession).WithUserName("customercontact").WithLastName("customercontact").Build();

            new OrganisationContactRelationshipBuilder(this.DatabaseSession).WithContact(customerContact).WithOrganisation(customer).WithFromDate(DateTime.UtcNow).Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("salesRep", "Forms"), new string[0]);
            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithTakenByInternalOrganisation(internalOrganisation)
                .WithShipToCustomer(customer)
                .WithBillToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanRead(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Confirm));

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact", "Forms"), new string[0]);
            acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanRead(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Confirm));
        }

        [Test]
        public void GivenSalesOrderCreatedByCustomer_WhenCurrentUserIsSalesRepOfOrganisationThatTakesTheOrder_ThenAccessIsGranted()
        {
            var customer = new Organisations(this.DatabaseSession).FindBy(Organisations.Meta.Name, "customer");
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var customerContact = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("customerContact").Build();
            new OrganisationContactRelationshipBuilder(this.DatabaseSession)
                .WithContact(customerContact)
                .WithOrganisation(new Organisations(this.DatabaseSession).FindBy(Organisations.Meta.Name, "customer"))
                .Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customerContact", "Forms"), new string[0]);

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithTakenByInternalOrganisation(internalOrganisation)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("salesRep", "Forms"), new string[0]);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanRead(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Confirm));
        }

        [Test]
        public void GivenSalesOrder_WhenCustomerChangesValue_ThenAccessPreviousCustomerIsDenied()
        {
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var customerContact1 = new PersonBuilder(this.DatabaseSession).WithUserName("customercontact1").WithLastName("customercontact1").Build();
            var customerContact2 = new PersonBuilder(this.DatabaseSession).WithUserName("customercontact2").WithLastName("customercontact2").Build();
            var employee = new PersonBuilder(this.DatabaseSession).WithUserName("employee").WithLastName("employee").Build();
            var customer1 = new OrganisationBuilder(this.DatabaseSession).WithName("customer1").Build();
            var customer2 = new OrganisationBuilder(this.DatabaseSession).WithName("customer2").Build();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer1)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer2)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new OrganisationContactRelationshipBuilder(this.DatabaseSession).WithContact(customerContact1).WithOrganisation(customer1).WithFromDate(DateTime.UtcNow).Build();
            new OrganisationContactRelationshipBuilder(this.DatabaseSession).WithContact(customerContact2).WithOrganisation(customer2).WithFromDate(DateTime.UtcNow).Build();

            new EmploymentBuilder(this.DatabaseSession)
                .WithEmployee(employee)
                .WithEmployer(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("employee", "Forms"), new string[0]);
            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithTakenByInternalOrganisation(internalOrganisation)
                .WithShipToCustomer(customer1)
                .WithBillToCustomer(customer1)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact1", "Forms"), new string[0]);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanRead(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Confirm));

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact2", "Forms"), new string[0]);
            acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsFalse(acl.HasReadOperation);

            order.BillToCustomer = customer2;

            this.DatabaseSession.Derive(true);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact1", "Forms"), new string[0]);
            acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanRead(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Confirm));

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact2", "Forms"), new string[0]);
            acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanRead(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Confirm));

            order.ShipToCustomer = customer2;

            this.DatabaseSession.Derive(true);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact1", "Forms"), new string[0]);
            acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsFalse(acl.HasReadOperation);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact2", "Forms"), new string[0]);
            acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanRead(SalesOrders.Meta.Comment));
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Confirm));
        }

        [Test]
        public void GivenSalesOrder_WhenObjectStateIsCreated_ThenCheckTransitions()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("customer").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customer", "Forms"), new string[0]);
            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).Provisional, order.CurrentObjectState);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Confirm));
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Cancel));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Approve));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Reject));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Hold));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Continue));
        }

        [Test]
        public void GivenSalesOrder_WhenObjectStateIsInProcess_ThenCheckTransitions()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("customer").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customer", "Forms"), new string[0]);

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Cancel));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Reject));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Approve));
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Hold));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Confirm));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Continue));
        }

        [Test]
        public void GivenSalesOrder_WhenObjectStateIsCancelled_ThenCheckTransitions()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("customer").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customer", "Forms"), new string[0]);

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            order.Cancel();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).Cancelled, order.CurrentObjectState);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Confirm));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Cancel));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Reject));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Approve));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Continue));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Hold));
        }

        [Test]
        public void GivenSalesOrder_WhenObjectStateIsRejected_ThenCheckTransitions()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("customer").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customer", "Forms"), new string[0]);

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            order.Reject();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).Rejected, order.CurrentObjectState);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Confirm));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Cancel));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Reject));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Approve));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Continue));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Hold));
        }

        [Test]
        public void GivenSalesOrder_WhenObjectStateIsFinished_ThenCheckTransitions()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("customer").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customer", "Forms"), new string[0]);

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            order.Finish();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).Finished, order.CurrentObjectState);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Confirm));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Cancel));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Reject));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Approve));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Continue));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Hold));
        }

        [Test]
        public void GivenSalesOrder_WhenObjectStateIsOnHold_ThenCheckTransitions()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("customer").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customer", "Forms"), new string[0]);

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            order.Hold();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesOrderObjectStates(this.DatabaseSession).OnHold, order.CurrentObjectState);
            var acl = new AccessControlList(order, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Cancel));
            Assert.IsTrue(acl.CanExecute(SalesOrders.Meta.Continue));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Confirm));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Reject));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Approve));
            Assert.IsFalse(acl.CanExecute(SalesOrders.Meta.Hold));
        }

        [Test]
        public void GivenSalesOrderWithShippingAndHandlingAmount_WhenDeriving_ThenOrderTotalsMustIncludeShippingAndHandlingAmount()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(billToCustomer).WithInternalOrganisation(internalOrganisation).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(shipToCustomer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var euro = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var adjustment = new ShippingAndHandlingChargeBuilder(this.DatabaseSession).WithAmount(7.5M).WithVatRate(vatRate21).Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var goodPurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithProductPurchasePrice(goodPurchasePrice)
                .Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithShippingAndHandlingCharge(adjustment)
                .Build();

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantityOrdered(quantityOrdered).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(45, order.TotalBasePrice);
            Assert.AreEqual(0, order.TotalDiscount);
            Assert.AreEqual(0, order.TotalSurcharge);
            Assert.AreEqual(7.5, order.TotalShippingAndHandling);
            Assert.AreEqual(0, order.TotalFee);
            Assert.AreEqual(52.5, order.TotalExVat);
            Assert.AreEqual(11.03, order.TotalVat);
            Assert.AreEqual(63.53, order.TotalIncVat);
            Assert.AreEqual(goodPurchasePrice.Price, order.TotalPurchasePrice);
        }

        [Test]
        public void GivenSalesOrderWithShippingAndHandlingPercentage_WhenDeriving_ThenOrderTotalsMustIncludeShippingAndHandlingAmount()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(billToCustomer).WithInternalOrganisation(internalOrganisation).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(shipToCustomer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var euro = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var adjustment = new ShippingAndHandlingChargeBuilder(this.DatabaseSession).WithPercentage(5).WithVatRate(vatRate21).Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var goodPurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithProductPurchasePrice(goodPurchasePrice)
                .Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithShippingAndHandlingCharge(adjustment)
                .Build();

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantityOrdered(quantityOrdered).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(45, order.TotalBasePrice);
            Assert.AreEqual(0, order.TotalDiscount);
            Assert.AreEqual(0, order.TotalSurcharge);
            Assert.AreEqual(2.25, order.TotalShippingAndHandling);
            Assert.AreEqual(0, order.TotalFee);
            Assert.AreEqual(47.25, order.TotalExVat);
            Assert.AreEqual(9.92, order.TotalVat);
            Assert.AreEqual(57.17, order.TotalIncVat);
            Assert.AreEqual(goodPurchasePrice.Price, order.TotalPurchasePrice);
        }

        [Test]
        public void GivenSalesOrderWithFeeAmount_WhenDeriving_ThenOrderTotalsMustIncludeFeeAmount()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(billToCustomer).WithInternalOrganisation(internalOrganisation).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(shipToCustomer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var euro = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var adjustment = new FeeBuilder(this.DatabaseSession).WithAmount(7.5M).WithVatRate(vatRate21).Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var goodPurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithProductPurchasePrice(goodPurchasePrice)
                .Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithFee(adjustment)
                .Build();

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantityOrdered(quantityOrdered).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(45, order.TotalBasePrice);
            Assert.AreEqual(0, order.TotalDiscount);
            Assert.AreEqual(0, order.TotalSurcharge);
            Assert.AreEqual(0, order.TotalShippingAndHandling);
            Assert.AreEqual(7.5, order.TotalFee);
            Assert.AreEqual(52.5, order.TotalExVat);
            Assert.AreEqual(11.03, order.TotalVat);
            Assert.AreEqual(63.53, order.TotalIncVat);
            Assert.AreEqual(goodPurchasePrice.Price, order.TotalPurchasePrice);
        }

        [Test]
        public void GivenSalesOrderWithFeePercentage_WhenDeriving_ThenOrderTotalsMustIncludeFeeAmount()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(billToCustomer).WithInternalOrganisation(internalOrganisation).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(shipToCustomer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var euro = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var adjustment = new FeeBuilder(this.DatabaseSession).WithPercentage(5).WithVatRate(vatRate21).Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var goodPurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithProductPurchasePrice(goodPurchasePrice)
                .Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithFee(adjustment)
                .Build();

            const decimal quantityOrdered = 3;
            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantityOrdered(quantityOrdered).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(45, order.TotalBasePrice);
            Assert.AreEqual(0, order.TotalDiscount);
            Assert.AreEqual(0, order.TotalSurcharge);
            Assert.AreEqual(0, order.TotalShippingAndHandling);
            Assert.AreEqual(2.25, order.TotalFee);
            Assert.AreEqual(47.25, order.TotalExVat);
            Assert.AreEqual(9.92, order.TotalVat);
            Assert.AreEqual(57.17, order.TotalIncVat);
            Assert.AreEqual(goodPurchasePrice.Price, order.TotalPurchasePrice);
        }

        [Test]
        public void GivenSalesOrder_WhenConfirming_ThenInventoryItemsQuantityCommittedOutAndAvailableToPromiseMustBeUpdated()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(billToCustomer).WithInternalOrganisation(internalOrganisation).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(shipToCustomer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part1").Build();
            var part2 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part2").Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(6, item1.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.AreEqual(0, item1.ReservedFromInventoryItem.AvailableToPromise);
            Assert.AreEqual(3, item3.ReservedFromInventoryItem.QuantityCommittedOut);
            Assert.AreEqual(0, item3.ReservedFromInventoryItem.AvailableToPromise);
        }

        [Test]
        public void GivenSalesOrder_WhenChangingItemQuantityToZero_ThenItemIsInvalid()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(billToCustomer).WithInternalOrganisation(internalOrganisation).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(shipToCustomer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part1").Build();
            var part2 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part2").Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(4).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            this.DatabaseSession.Derive(true);
            Assert.AreEqual(4, order.ValidOrderItems.Count);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            item4.QuantityOrdered = 0;

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(3, order.ValidOrderItems.Count);
            Assert.Contains(item1, order.ValidOrderItems);
            Assert.Contains(item2, order.ValidOrderItems);
            Assert.Contains(item3, order.ValidOrderItems);
        }

        [Test]
        public void GivenSalesOrder_WhenOrderItemIsWithoutBasePrice_ThenItemIsInvalid()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(billToCustomer).WithInternalOrganisation(internalOrganisation).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(shipToCustomer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part1").Build();
            var part2 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part2").Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(4).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            item4.RemoveActualUnitPrice();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(0, item4.UnitBasePrice);
            Assert.AreEqual(3, order.ValidOrderItems.Count);
            Assert.Contains(item1, order.ValidOrderItems);
            Assert.Contains(item2, order.ValidOrderItems);
            Assert.Contains(item3, order.ValidOrderItems);
        }

        [Test]
        public void GivenSalesOrder_WhenConfirming_ThenAllValidItemsAreInConfirmedState()
        {
            var billToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").Build();
            var shipToCustomer = new PersonBuilder(this.DatabaseSession).WithLastName("person2").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(billToCustomer).WithInternalOrganisation(internalOrganisation).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(shipToCustomer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var shipToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var part1 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part1").Build();
            var part2 = new FinishedGoodBuilder(this.DatabaseSession).WithName("part2").Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part1)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part2)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithShipToAddress(shipToContactMechanism)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(3).WithActualUnitPrice(15).Build();
            var item4 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(4).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            item4.Cancel();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(3, order.ValidOrderItems.Count);
            Assert.Contains(item1, order.ValidOrderItems);
            Assert.Contains(item2, order.ValidOrderItems);
            Assert.Contains(item3, order.ValidOrderItems);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).InProcess, item1.CurrentObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).InProcess, item2.CurrentObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).InProcess, item3.CurrentObjectState);
            Assert.AreEqual(new SalesOrderItemObjectStates(this.DatabaseSession).Cancelled, item4.CurrentObjectState);
        }

        [Test]
        public void GivenSalesOrder_WhenConfirmed_ThenShipmentItemsAreCreated()
        {
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .Build();

            var good1InventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good1).Build();
            good1InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var good2InventoryItem = new NonSerializedInventoryItemBuilder(this.DatabaseSession).WithGood(good2).Build();
            good2InventoryItem.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipment = customer.ShipmentsWhereBillToParty.First;

            Assert.AreEqual(2, shipment.ShipmentItems.Count);
            Assert.AreEqual(1, item1.OrderShipmentsWhereSalesOrderItem[0].Quantity);
            Assert.AreEqual(2, item2.OrderShipmentsWhereSalesOrderItem[0].Quantity);
            Assert.AreEqual(5, item3.OrderShipmentsWhereSalesOrderItem[0].Quantity);
        }

        [Test]
        public void GivenSalesOrderWithMultipleRecipients_WhenConfirmed_ThenShipmentIsCreatedForEachRecipientAndPickListIsCreated()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var baal = new CityBuilder(this.DatabaseSession).WithName("Baal").Build();
            var baalAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(baal).WithAddress1("Haverwerf 15").Build();
            var shipToBaal = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(baalAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var person1 = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPartyContactMechanism(shipToMechelen).Build();
            var person2 = new PersonBuilder(this.DatabaseSession).WithLastName("person2").WithPartyContactMechanism(shipToBaal).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(person1).WithInternalOrganisation(internalOrganisation).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(person2).WithInternalOrganisation(internalOrganisation).Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var part = new FinishedGoodBuilder(this.DatabaseSession)
                .WithName("part1")
                .WithOwnedByParty(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            this.DatabaseSession.Derive(true);

            var partInventory = (NonSerializedInventoryItem)part.InventoryItemsWherePart[0];
            partInventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithFinishedGood(part)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithFinishedGood(part)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var colorBlack = new ColourBuilder(this.DatabaseSession)
                .WithName("white")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession)
                                            .WithText("White")
                                            .WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale)
                                            .Build())
                .Build();

            var extraLarge = new SizeBuilder(this.DatabaseSession)
                .WithName("Extra large")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession)
                                            .WithText("White")
                                            .WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale)
                                            .Build())
                .Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(person1)
                .WithShipToCustomer(person1)
                .WithShipToAddress(mechelenAddress)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProductFeature(colorBlack).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProductFeature(extraLarge).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            item1.AddOrderedWithFeature(item2);
            item1.AddOrderedWithFeature(item3);
            var item4 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item5 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(5).WithActualUnitPrice(15).WithAssignedShipToParty(person2).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);
            order.AddSalesOrderItem(item4);
            order.AddSalesOrderItem(item5);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var shipmentToMechelen = mechelenAddress.ShipmentsWhereShipToAddress[0];

            var shipmentToBaal = baalAddress.ShipmentsWhereShipToAddress[0];

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(mechelenAddress, shipmentToMechelen.ShipToAddress);
            Assert.AreEqual(1, shipmentToMechelen.ShipmentItems.Count);
            Assert.AreEqual(3, shipmentToMechelen.ShipmentItems[0].Quantity);

            Assert.AreEqual(baalAddress, shipmentToBaal.ShipToAddress);
            Assert.AreEqual(1, shipmentToBaal.ShipmentItems.Count);
            Assert.AreEqual(good2, shipmentToBaal.ShipmentItems[0].Good);
            Assert.AreEqual(5, shipmentToBaal.ShipmentItems[0].Quantity);
        }

        [Test]
        public void GivenSalesOrder_WhenShipToAndBillToAreSameCustomer_ThenDerivedCustomersIsSingleCustomer()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithShipToCustomer(customer)
                .WithBillToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, order.Customers.Count);
            Assert.AreEqual(customer, order.Customers.First);
        }

        [Test]
        public void GivenSalesOrder_WhenShipToAndBillToAreDifferentCustomers_ThenDerivedCustomersHoldsBothCustomers()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var billToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var shipToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(billToCustomer).WithInternalOrganisation(internalOrganisation).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(shipToCustomer).WithInternalOrganisation(internalOrganisation).Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithShipToCustomer(shipToCustomer)
                .WithBillToCustomer(billToCustomer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, order.Customers.Count);
            Assert.Contains(billToCustomer, order.Customers);
            Assert.Contains(shipToCustomer, order.Customers);
        }

        [Test]
        public void GivenSalesOrder_WhenDerivingSalesReps_ThenSalesRepsAreCollectedFromSalesOrderItems()
        {
            var salesrep1 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for child product category").Build();
            var salesrep2 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for parent category").Build();
            var salesrep3 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for everything else").Build();
            var parentProductCategory = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("parent").Build();
            var childProductCategory = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("child").WithParent(parentProductCategory).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("company").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithSalesRepresentative(salesrep1)
                .WithCustomer(customer)
                .WithProductCategory(childProductCategory)
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithSalesRepresentative(salesrep2)
                .WithCustomer(customer)
                .WithProductCategory(parentProductCategory)
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithSalesRepresentative(salesrep3)
                .WithCustomer(customer)
                .Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithProductCategory(childProductCategory)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithProductCategory(parentProductCategory)
                .Build();

            var good3 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good1)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item1);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, order.SalesReps.Count);
            Assert.Contains(salesrep1, order.SalesReps);

            var item2 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good2)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item2);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, order.SalesReps.Count);
            Assert.Contains(salesrep1, order.SalesReps);
            Assert.Contains(salesrep2, order.SalesReps);

            var item3 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good3)
                .WithQuantityOrdered(3)
                .WithActualUnitPrice(5)
                .Build();

            order.AddSalesOrderItem(item3);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(3, order.SalesReps.Count);
            Assert.Contains(salesrep1, order.SalesReps);
            Assert.Contains(salesrep2, order.SalesReps);
            Assert.Contains(salesrep3, order.SalesReps);
        }
    }
}