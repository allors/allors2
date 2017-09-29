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
    using Meta;
    using Xunit;

    
    public class SalesRepPartyProductCategoryRevenuesTests : DomainTest
    {
        [Fact]
        public void DeriveRevenues()
        {
            var productItem = new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem;
            var contactMechanism = new ContactMechanisms(this.DatabaseSession).Extent().First;

            var customer1 = new OrganisationBuilder(this.DatabaseSession).WithName("customer1").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var customer2 = new OrganisationBuilder(this.DatabaseSession).WithName("customer2").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var salesRep1 = new PersonBuilder(this.DatabaseSession).WithLastName("salesRep1").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var salesRep2 = new PersonBuilder(this.DatabaseSession).WithLastName("salesRep2").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var catMain = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("main cat").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .Build();
            var cat1 = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("cat for good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithParent(catMain)
                .Build();
            var cat2 = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("cat for good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithParent(catMain)
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer1).WithProductCategory(cat1).WithSalesRepresentative(salesRep1).Build();
            new SalesRepRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer1).WithProductCategory(cat2).WithSalesRepresentative(salesRep2).Build();

            this.DatabaseSession.Derive();

            new SalesRepRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer2).WithProductCategory(cat1).WithSalesRepresentative(salesRep1).Build();
            new SalesRepRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer2).WithProductCategory(cat2).WithSalesRepresentative(salesRep2).Build();

            this.DatabaseSession.Derive();

            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithPrimaryProductCategory(cat1)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good2").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithPrimaryProductCategory(cat2)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer1).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer2).Build();

            this.DatabaseSession.Derive();

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

            this.DatabaseSession.Derive();

            Singleton.Instance(this.DatabaseSession).DeriveRevenues(new NonLogging.Derivation(this.DatabaseSession));

            var salesRep1Customer1ProductCategoryRevenues = salesRep1.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            Assert.Equal(2, salesRep1Customer1ProductCategoryRevenues.Count);

            salesRep1Customer1ProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.ProductCategory, cat1);
            salesRep1Customer1ProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.Party, customer1);
            var salesRep1Customer1Cat1Revenue = salesRep1Customer1ProductCategoryRevenues.First;

            salesRep1Customer1ProductCategoryRevenues = salesRep1.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            salesRep1Customer1ProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.ProductCategory, catMain);
            salesRep1Customer1ProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.Party, customer1);
            var salesRep1Customer1CatMainRevenue = salesRep1Customer1ProductCategoryRevenues.First;

            var salesRep2Customer1ProductCategoryRevenues = salesRep2.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            Assert.Equal(2, salesRep2Customer1ProductCategoryRevenues.Count);

            salesRep2Customer1ProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.ProductCategory, cat2);
            salesRep2Customer1ProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.Party, customer1);
            var salesRep2Customer1Cat2Revenue = salesRep2Customer1ProductCategoryRevenues.First;

            salesRep2Customer1ProductCategoryRevenues = salesRep2.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            salesRep2Customer1ProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.Party, customer1);
            salesRep2Customer1ProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.ProductCategory, catMain);
            var salesRep2Customer1CatMainRevenue = salesRep2Customer1ProductCategoryRevenues.First;

            Assert.Equal(90, salesRep1Customer1Cat1Revenue.Revenue);
            Assert.Equal(90, salesRep1Customer1CatMainRevenue.Revenue);
            Assert.Equal(50, salesRep2Customer1Cat2Revenue.Revenue);
            Assert.Equal(50, salesRep2Customer1CatMainRevenue.Revenue);

            var invoice2 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceDate(DateTime.UtcNow)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer2)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            var item4 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantity(1).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice2.AddSalesInvoiceItem(item4);

            this.DatabaseSession.Derive();

            var item5 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantity(1).WithActualUnitPrice(10).WithSalesInvoiceItemType(productItem).Build();
            invoice2.AddSalesInvoiceItem(item5);

            this.DatabaseSession.Derive();

            Singleton.Instance(this.DatabaseSession).DeriveRevenues(new NonLogging.Derivation(this.DatabaseSession));

            var salesRep1Customer2ProductCategoryRevenues = salesRep1.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            salesRep1Customer2ProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.Party, customer2);
            Assert.Equal(2, salesRep1Customer2ProductCategoryRevenues.Count);

            salesRep1Customer2ProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.ProductCategory, cat1);
            salesRep1Customer2ProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.Party, customer2);
            var salesRep1Customer2Cat1Revenue = salesRep1Customer2ProductCategoryRevenues.First;

            salesRep1Customer2ProductCategoryRevenues = salesRep1.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            salesRep1Customer2ProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.ProductCategory, catMain);
            salesRep1Customer2ProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.Party, customer2);
            var salesRep1Customer2CatMainRevenue = salesRep1Customer2ProductCategoryRevenues.First;

            var salesRep2Customer2ProductCategoryRevenues = salesRep2.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            salesRep2Customer2ProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.Party, customer2);
            Assert.Equal(2, salesRep2Customer2ProductCategoryRevenues.Count);

            salesRep2Customer2ProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.ProductCategory, cat2);
            salesRep2Customer2ProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.Party, customer2);
            var salesRep2Customer2Cat2Revenue = salesRep2Customer2ProductCategoryRevenues.First;

            salesRep2Customer2ProductCategoryRevenues = salesRep2.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            salesRep2Customer2ProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.Party, customer2);
            salesRep2Customer2ProductCategoryRevenues.Filter.AddEquals(M.SalesRepPartyProductCategoryRevenue.ProductCategory, catMain);
            var salesRep2Customer2CatMainRevenue = salesRep2Customer2ProductCategoryRevenues.First;

            Assert.Equal(90, salesRep1Customer1Cat1Revenue.Revenue);
            Assert.Equal(90, salesRep1Customer1CatMainRevenue.Revenue);
            Assert.Equal(50, salesRep2Customer1Cat2Revenue.Revenue);
            Assert.Equal(50, salesRep2Customer1CatMainRevenue.Revenue);

            Assert.Equal(15, salesRep1Customer2Cat1Revenue.Revenue);
            Assert.Equal(15, salesRep1Customer2CatMainRevenue.Revenue);
            Assert.Equal(10, salesRep2Customer2Cat2Revenue.Revenue);
            Assert.Equal(10, salesRep2Customer2CatMainRevenue.Revenue);
        }
    }
}
