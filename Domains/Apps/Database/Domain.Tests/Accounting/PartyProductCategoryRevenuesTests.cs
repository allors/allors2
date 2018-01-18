// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyProductCategoryRevenuesTests.cs" company="Allors bvba">
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

    
    public class PartyProductCategoryRevenuesTests : DomainTest
    {
        [Fact]
        public void DeriveRevenues()
        {
            var productItem = new SalesInvoiceItemTypes(this.Session).ProductItem;
            var contactMechanism = new ContactMechanisms(this.Session).Extent().First;

            var customer1 = new OrganisationBuilder(this.Session).WithName("customer1").WithOrganisationRole(new OrganisationRoles(this.Session).Customer).Build();
            var customer2 = new OrganisationBuilder(this.Session).WithName("customer2").WithOrganisationRole(new OrganisationRoles(this.Session).Customer).Build();
            var salesRep1 = new PersonBuilder(this.Session).WithLastName("salesRep1").WithPersonRole(new PersonRoles(this.Session).Employee).Build();
            var salesRep2 = new PersonBuilder(this.Session).WithLastName("salesRep2").WithPersonRole(new PersonRoles(this.Session).Employee).Build();
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
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(cat1)
                .Build();

            var good2 = new GoodBuilder(this.Session)
                .WithSku("10102")
                .WithVatRate(vatRate21)
                .WithName("good2")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(cat2)
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

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good1).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item1);

            var item2 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good1).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item2);

            var item3 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good2).WithQuantity(5).WithActualUnitPrice(10).WithSalesInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item3);

            this.Session.Derive();

            this.Session.GetSingleton().DeriveRevenues(new NonLogging.Derivation(this.Session));

            var customer1ProductCategoryRevenues = customer1.PartyProductCategoryRevenuesWhereParty;
            Assert.Equal(3, customer1ProductCategoryRevenues.Count);

            customer1ProductCategoryRevenues.Filter.AddEquals(M.PartyProductCategoryRevenue.ProductCategory, cat1);
            var customer1Cat1Revenue = customer1ProductCategoryRevenues.First;

            customer1ProductCategoryRevenues = customer1.PartyProductCategoryRevenuesWhereParty;
            customer1ProductCategoryRevenues.Filter.AddEquals(M.PartyProductCategoryRevenue.ProductCategory, cat2);
            var customer1Cat2Revenue = customer1ProductCategoryRevenues.First;

            customer1ProductCategoryRevenues = customer1.PartyProductCategoryRevenuesWhereParty;
            customer1ProductCategoryRevenues.Filter.AddEquals(M.PartyProductCategoryRevenue.ProductCategory, catMain);
            var customer1CatMainRevenue = customer1ProductCategoryRevenues.First;

            Assert.Equal(90, customer1Cat1Revenue.Revenue);
            Assert.Equal(6, customer1Cat1Revenue.Quantity);
            Assert.Equal(50, customer1Cat2Revenue.Revenue);
            Assert.Equal(5, customer1Cat2Revenue.Quantity);
            Assert.Equal(140, customer1CatMainRevenue.Revenue);
            Assert.Equal(11, customer1CatMainRevenue.Quantity);

            var invoice2 = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceDate(DateTime.UtcNow)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer2)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            var item4 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good1).WithQuantity(1).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice2.AddSalesInvoiceItem(item4);

            this.Session.Derive();

            var item5 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good2).WithQuantity(1).WithActualUnitPrice(10).WithSalesInvoiceItemType(productItem).Build();
            invoice2.AddSalesInvoiceItem(item5);

            this.Session.Derive();

            this.Session.GetSingleton().DeriveRevenues(new NonLogging.Derivation(this.Session));

            var customer2ProductCategoryRevenues = customer2.PartyProductCategoryRevenuesWhereParty;
            Assert.Equal(3, customer2ProductCategoryRevenues.Count);

            customer2ProductCategoryRevenues.Filter.AddEquals(M.PartyProductCategoryRevenue.ProductCategory, cat1);
            var customer2Cat1Revenue = customer2ProductCategoryRevenues.First;

            customer2ProductCategoryRevenues = customer2.PartyProductCategoryRevenuesWhereParty;
            customer2ProductCategoryRevenues.Filter.AddEquals(M.PartyProductCategoryRevenue.ProductCategory, cat2);
            var customer2Cat2Revenue = customer2ProductCategoryRevenues.First;

            customer2ProductCategoryRevenues = customer2.PartyProductCategoryRevenuesWhereParty;
            customer2ProductCategoryRevenues.Filter.AddEquals(M.PartyProductCategoryRevenue.ProductCategory, catMain);
            var customer2CatMainRevenue = customer2ProductCategoryRevenues.First;

            Assert.Equal(15, customer2Cat1Revenue.Revenue);
            Assert.Equal(1, customer2Cat1Revenue.Quantity);
            Assert.Equal(10, customer2Cat2Revenue.Revenue);
            Assert.Equal(1, customer2Cat2Revenue.Quantity);
            Assert.Equal(25, customer2CatMainRevenue.Revenue);
            Assert.Equal(2, customer2CatMainRevenue.Quantity);
        }
    }
}
