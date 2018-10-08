// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesRepPartyRevenuesTests.cs" company="Allors bvba">
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

    
    public class SalesRepPartyRevenuesTests : DomainTest
    {
        [Fact(Skip = "to repair")]
        public void DeriveRevenues()
        {
            var productItem = new InvoiceItemTypes(this.Session).ProductItem;
            var contactMechanism = new ContactMechanisms(this.Session).Extent().First;

            var customer1 = new OrganisationBuilder(this.Session).WithName("customer1").Build();
            var customer2 = new OrganisationBuilder(this.Session).WithName("customer2").Build();
            var salesRep1 = new PersonBuilder(this.Session).WithLastName("salesRep1").Build();
            var salesRep2 = new PersonBuilder(this.Session).WithLastName("salesRep2").Build();
            var catMain = new ProductCategoryBuilder(this.Session)
                .WithName("main cat")
                .Build();
            var cat1 = new ProductCategoryBuilder(this.Session)
                .WithName("cat for good1")
                .WithParent(catMain)
                .Build();
            var cat2 = new ProductCategoryBuilder(this.Session)
                .WithName("cat for good2")
                .WithParent(catMain)
                .Build();

            new SalesRepRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer1).WithProductCategory(cat1).WithSalesRepresentative(salesRep1).Build();
            new SalesRepRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer1).WithProductCategory(cat2).WithSalesRepresentative(salesRep2).Build();

            this.Session.Derive();

            new SalesRepRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer2).WithProductCategory(cat1).WithSalesRepresentative(salesRep1).Build();
            new SalesRepRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer2).WithProductCategory(cat2).WithSalesRepresentative(salesRep2).Build();

            this.Session.Derive();

            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();

            var good1 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(cat1)
                .WithPart(new PartBuilder(this.Session).WithPartId("1").WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            var good2 = new GoodBuilder(this.Session)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(cat2)
                .WithPart(new PartBuilder(this.Session).WithPartId("2").WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer1).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer2).Build();

            this.Session.Derive();

            var invoice1 = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceDate(DateTime.UtcNow)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer1)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good1).WithQuantity(3).WithActualUnitPrice(15).WithInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item1);

            var item2 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good1).WithQuantity(3).WithActualUnitPrice(15).WithInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item2);

            var item3 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good2).WithQuantity(5).WithActualUnitPrice(10).WithInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item3);

            this.Session.Derive();

            this.Session.GetSingleton().DeriveRevenues(new NonLogging.Derivation(this.Session));

            var salesRep1Customer1Revenues = salesRep1.SalesRepPartyRevenuesWhereSalesRep;
            salesRep1Customer1Revenues.Filter.AddEquals(M.SalesRepPartyRevenue.Party, customer1);
            var salesRep1Customer1Revenue = salesRep1Customer1Revenues.First;

            var salesRep2Customer1Revenues = salesRep2.SalesRepPartyRevenuesWhereSalesRep;
            salesRep2Customer1Revenues.Filter.AddEquals(M.SalesRepPartyRevenue.Party, customer1);
            var salesRep2Customer1Revenue = salesRep2Customer1Revenues.First;

            Assert.Equal(90, salesRep1Customer1Revenue.Revenue);
            Assert.Equal(50, salesRep2Customer1Revenue.Revenue);

            var invoice2 = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceDate(DateTime.UtcNow)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer2)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            var item4 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good1).WithQuantity(1).WithActualUnitPrice(15).WithInvoiceItemType(productItem).Build();
            invoice2.AddSalesInvoiceItem(item4);

            this.Session.Derive();

            var item5 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good2).WithQuantity(1).WithActualUnitPrice(10).WithInvoiceItemType(productItem).Build();
            invoice2.AddSalesInvoiceItem(item5);

            this.Session.Derive();

            this.Session.GetSingleton().DeriveRevenues(new NonLogging.Derivation(this.Session));

            salesRep1Customer1Revenues = salesRep1.SalesRepPartyRevenuesWhereSalesRep;
            salesRep1Customer1Revenues.Filter.AddEquals(M.SalesRepPartyRevenue.Party, customer1);
            salesRep1Customer1Revenue = salesRep1Customer1Revenues.First;

            salesRep2Customer1Revenues = salesRep2.SalesRepPartyRevenuesWhereSalesRep;
            salesRep2Customer1Revenues.Filter.AddEquals(M.SalesRepPartyRevenue.Party, customer1);
            salesRep2Customer1Revenue = salesRep2Customer1Revenues.First;

            var salesRep1Customer2Revenues = salesRep1.SalesRepPartyRevenuesWhereSalesRep;
            salesRep1Customer2Revenues.Filter.AddEquals(M.SalesRepPartyRevenue.Party, customer2);
            var salesRep1Customer2Revenue = salesRep1Customer2Revenues.First;

            var salesRep2Customer2Revenues = salesRep2.SalesRepPartyRevenuesWhereSalesRep;
            salesRep2Customer2Revenues.Filter.AddEquals(M.SalesRepPartyRevenue.Party, customer2);
            var salesRep2Customer2Revenue = salesRep2Customer2Revenues.First;

            Assert.Equal(90, salesRep1Customer1Revenue.Revenue);
            Assert.Equal(50, salesRep2Customer1Revenue.Revenue);
            Assert.Equal(15, salesRep1Customer2Revenue.Revenue);
            Assert.Equal(10, salesRep2Customer2Revenue.Revenue);
        }
    }
}
