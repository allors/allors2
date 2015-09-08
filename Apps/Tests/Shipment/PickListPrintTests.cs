//------------------------------------------------------------------------------------------------- 
// <copyright file="PickListPrintTests.cs" company="Allors bvba">
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
    using System.Reflection;
    using System.Text.RegularExpressions;

    using NUnit.Framework;

    [TestFixture]
    public class PickListPrintTests : DomainTest
    {
        [Test]
        public void GivenPickList_WhenDeriving_ThenFormattedPickListIsCreated()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithInternalOrganisation(internalOrganisation)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var good1PurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR"))
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good1)
                .WithProductPurchasePrice(good1PurchasePrice)
                .WithSupplier(supplier)
                .Build();

            var good2PurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR"))
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good2)
                .WithProductPurchasePrice(good2PurchasePrice)
                .WithSupplier(supplier)
                .Build();

            this.DatabaseSession.Derive(true);

            var good1Inventory = (NonSerializedInventoryItem)good1.InventoryItemsWhereGood[0];
            good1Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            var good2Inventory = (NonSerializedInventoryItem)good2.InventoryItemsWhereGood[0];
            good2Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            var item3 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantityOrdered(5).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);
            order.AddSalesOrderItem(item3);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var pickList = good1.InventoryItemsWhereGood[0].PickListItemsWhereInventoryItem[0].PickListWherePickListItem;

            Assert.IsTrue(pickList.ExistPrintContent);
            this.AssertXml(ExpectedPickListLayout.pickListEnglisch, pickList.PrintContent);
        }

        [Test]
        public void GivenOrderItemWithPendingShipmentAndAssignedPickList_WhenQuantityOrderedIsDecreased_ThenNegativePickListIsCreated()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var shipToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("person1").WithPartyContactMechanism(shipToMechelen).Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithInternalOrganisation(internalOrganisation)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var goodPurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR"))
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good1)
                .WithProductPurchasePrice(goodPurchasePrice)
                .WithSupplier(supplier)
                .Build();

            this.DatabaseSession.Derive(true);

            var good1Inventory = (NonSerializedInventoryItem)good1.InventoryItemsWhereGood[0];
            good1Inventory.AddInventoryItemVariance(new InventoryItemVarianceBuilder(this.DatabaseSession).WithQuantity(100).WithReason(new VarianceReasons(this.DatabaseSession).Unknown).Build());

            this.DatabaseSession.Derive(true);

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(1).WithActualUnitPrice(15).Build();
            var item2 = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantityOrdered(2).WithActualUnitPrice(15).Build();
            order.AddSalesOrderItem(item1);
            order.AddSalesOrderItem(item2);

            this.DatabaseSession.Derive(true);

            order.Confirm();

            this.DatabaseSession.Derive(true);

            var pickList = good1.InventoryItemsWhereGood[0].PickListItemsWhereInventoryItem[0].PickListWherePickListItem;
            pickList.Picker = new Persons(this.DatabaseSession).FindBy(Persons.Meta.LastName, "orderProcessor");

            this.DatabaseSession.Derive(true);

            item2.Cancel();

            this.DatabaseSession.Derive(true);

            var negativePickList = order.ShipToCustomer.PickListsWhereShipToParty[1];

            Assert.IsTrue(negativePickList.ExistPrintContent);
            this.AssertXml(ExpectedPickListLayout.negativePickListEnglisch, negativePickList.PrintContent);
        }

        private void AssertXml(string expectedXml, string actualXml)
        {
            Assert.AreEqual(this.ToComparableXml(expectedXml), this.ToComparableXml(actualXml));
        }

        private string ToComparableXml(string xml)
        {
            var strictXml = new XmlStrictValidation(xml);
            strictXml.XmlDocument.Normalize();

            var stripWhiteSpace = new Regex(@">(\n|\s)*<");
            return stripWhiteSpace.Replace(strictXml.XmlDocument.InnerXml, "><");
        }

        private byte[] GetEmbeddedResource(string resourceName)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            var content = new byte[stream.Length];
            stream.Read(content, 0, content.Length);
            return content;
        }
    }
}
