// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesRepPartyProductCategoryRevenuesTests.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class SalesRepPartyProductCategoryRevenuesTests : DomainTest
    {
        [Test]
        public void DeriveRevenues()
        {
            var productItem = new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem;
            var contactMechanism = new ContactMechanisms(this.DatabaseSession).Extent().First;

            var customer1 = new OrganisationBuilder(this.DatabaseSession).WithName("customer1").Build();
            var customer2 = new OrganisationBuilder(this.DatabaseSession).WithName("customer2").Build();
            var salesRep1 = new PersonBuilder(this.DatabaseSession).WithLastName("salesRep1").Build();
            var salesRep2 = new PersonBuilder(this.DatabaseSession).WithLastName("salesRep2").Build();
            var catMain = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("main cat").Build();
            var cat1 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("cat for good1").WithParent(catMain).Build();
            var cat2 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("cat for good2").WithParent(catMain).Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer1).WithProductCategory(cat1).WithSalesRepresentative(salesRep1).Build();
            new SalesRepRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer1).WithProductCategory(cat2).WithSalesRepresentative(salesRep2).Build();

            this.DatabaseSession.Derive(true);

            new SalesRepRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer2).WithProductCategory(cat1).WithSalesRepresentative(salesRep1).Build();
            new SalesRepRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer2).WithProductCategory(cat2).WithSalesRepresentative(salesRep2).Build();

            this.DatabaseSession.Derive(true);

            var euro = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithPrimaryProductCategory(cat1)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithPrimaryProductCategory(cat2)
                .Build();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            internalOrganisation.PreferredCurrency = euro;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer1).WithInternalOrganisation(internalOrganisation).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer2).WithInternalOrganisation(internalOrganisation).Build();

            var invoice1 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceDate(DateTime.UtcNow)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer1)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item1);

            var item2 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item2);

            var item3 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantity(5).WithActualUnitPrice(10).WithSalesInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item3);

            this.DatabaseSession.Derive(true);

            Singleton.Instance(this.DatabaseSession).DeriveRevenues();

            var salesRep1Customer1ProductCategoryRevenues = salesRep1.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            Assert.AreEqual(2, salesRep1Customer1ProductCategoryRevenues.Count);

            salesRep1Customer1ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.ProductCategory, cat1);
            salesRep1Customer1ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer1);
            var salesRep1Customer1Cat1Revenue = salesRep1Customer1ProductCategoryRevenues.First;

            salesRep1Customer1ProductCategoryRevenues = salesRep1.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            salesRep1Customer1ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.ProductCategory, catMain);
            salesRep1Customer1ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer1);
            var salesRep1Customer1CatMainRevenue = salesRep1Customer1ProductCategoryRevenues.First;

            var salesRep2Customer1ProductCategoryRevenues = salesRep2.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            Assert.AreEqual(2, salesRep2Customer1ProductCategoryRevenues.Count);

            salesRep2Customer1ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.ProductCategory, cat2);
            salesRep2Customer1ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer1);
            var salesRep2Customer1Cat2Revenue = salesRep2Customer1ProductCategoryRevenues.First;

            salesRep2Customer1ProductCategoryRevenues = salesRep2.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            salesRep2Customer1ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer1);
            salesRep2Customer1ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.ProductCategory, catMain);
            var salesRep2Customer1CatMainRevenue = salesRep2Customer1ProductCategoryRevenues.First;

            Assert.AreEqual(90, salesRep1Customer1Cat1Revenue.Revenue);
            Assert.AreEqual(90, salesRep1Customer1CatMainRevenue.Revenue);
            Assert.AreEqual(50, salesRep2Customer1Cat2Revenue.Revenue);
            Assert.AreEqual(50, salesRep2Customer1CatMainRevenue.Revenue);

            var invoice2 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceDate(DateTime.UtcNow)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer2)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            var item4 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantity(1).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice2.AddSalesInvoiceItem(item4);

            this.DatabaseSession.Derive(true);

            var item5 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantity(1).WithActualUnitPrice(10).WithSalesInvoiceItemType(productItem).Build();
            invoice2.AddSalesInvoiceItem(item5);

            this.DatabaseSession.Derive(true);

            Singleton.Instance(this.DatabaseSession).DeriveRevenues();

            var salesRep1Customer2ProductCategoryRevenues = salesRep1.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            salesRep1Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer2);
            Assert.AreEqual(2, salesRep1Customer2ProductCategoryRevenues.Count);

            salesRep1Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.ProductCategory, cat1);
            salesRep1Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer2);
            var salesRep1Customer2Cat1Revenue = salesRep1Customer2ProductCategoryRevenues.First;

            salesRep1Customer2ProductCategoryRevenues = salesRep1.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            salesRep1Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.ProductCategory, catMain);
            salesRep1Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer2);
            var salesRep1Customer2CatMainRevenue = salesRep1Customer2ProductCategoryRevenues.First;

            var salesRep2Customer2ProductCategoryRevenues = salesRep2.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            salesRep2Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer2);
            Assert.AreEqual(2, salesRep2Customer2ProductCategoryRevenues.Count);

            salesRep2Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.ProductCategory, cat2);
            salesRep2Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer2);
            var salesRep2Customer2Cat2Revenue = salesRep2Customer2ProductCategoryRevenues.First;

            salesRep2Customer2ProductCategoryRevenues = salesRep2.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            salesRep2Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer2);
            salesRep2Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.ProductCategory, catMain);
            var salesRep2Customer2CatMainRevenue = salesRep2Customer2ProductCategoryRevenues.First;

            Assert.AreEqual(90, salesRep1Customer1Cat1Revenue.Revenue);
            Assert.AreEqual(90, salesRep1Customer1CatMainRevenue.Revenue);
            Assert.AreEqual(50, salesRep2Customer1Cat2Revenue.Revenue);
            Assert.AreEqual(50, salesRep2Customer1CatMainRevenue.Revenue);

            Assert.AreEqual(15, salesRep1Customer2Cat1Revenue.Revenue);
            Assert.AreEqual(15, salesRep1Customer2CatMainRevenue.Revenue);
            Assert.AreEqual(10, salesRep2Customer2Cat2Revenue.Revenue);
            Assert.AreEqual(10, salesRep2Customer2CatMainRevenue.Revenue);
        }
    }
}
