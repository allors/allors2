// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyProductCategoryRevenueHistoryTests.cs" company="Allors bvba">
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
    public class PartyProductCategoryRevenueHistoryTests : DomainTest
    {
        [Test]
        public void GivenSalesInvoice_WhenDerived_ThenTotalExVatIsAddedToPartyProductCategoryRevenueHistory()
        {
            var productItem = new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem;
            var contactMechanism = new ContactMechanisms(this.DatabaseSession).Extent().First;

            var euro = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var catMain = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("main cat").Build();
            var cat1 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("cat for good1").WithParent(catMain).Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good1")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithPrimaryProductCategory(cat1)
                .Build();

            var customer = new Organisations(this.DatabaseSession).FindBy(Organisations.Meta.Name, "customer");
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            internalOrganisation.PreferredCurrency = euro;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var date = DateTime.UtcNow.AddYears(-1).AddMonths(-1);
            decimal revenuePastTwelveMonths = 0;
            for (var i = 1; i <= 13; i++)
            {
                var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                    .WithInvoiceDate(date)
                    .WithBillToCustomer(customer)
                    .WithBillToContactMechanism(contactMechanism)
                    .WithSalesChannel(new SalesChannels(this.DatabaseSession).WebChannel)
                    .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                    .Build();

                var item = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantity(1).WithActualUnitPrice(i * 10M).WithSalesInvoiceItemType(productItem).Build();
                invoice.AddSalesInvoiceItem(item);

                this.DatabaseSession.Derive(true);
                this.DatabaseSession.Commit();

                var history = customer.PartyProductCategoryRevenueHistoriesWhereParty.First;

                //// first iteration is too old
                if (i > 1)
                {
                    revenuePastTwelveMonths += i * 10M;
                }

                ////date in first iteration is too old, no history yet.
                if (history != null)
                {
                    Assert.AreEqual(revenuePastTwelveMonths, history.Revenue);
                }

                date = date.AddMonths(1);
            }
        }

        [Test]
        public void DeriveHistory()
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
                .WithInvoiceDate(DateTime.UtcNow.AddMonths(-1))
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer1)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            var item1a = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item1a);

            var item1b = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item1b);

            var item1c = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantity(5).WithActualUnitPrice(10).WithSalesInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item1c);

            this.DatabaseSession.Derive(true);

            var invoice2 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceDate(DateTime.UtcNow)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer1)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            var item2a = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice2.AddSalesInvoiceItem(item2a);

            var item2b = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice2.AddSalesInvoiceItem(item2b);

            var item2c = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantity(5).WithActualUnitPrice(10).WithSalesInvoiceItemType(productItem).Build();
            invoice2.AddSalesInvoiceItem(item2c);

            this.DatabaseSession.Derive(true);

            Singleton.Instance(this.DatabaseSession).DeriveRevenues();

            var customer1Cat1RevenueHistories = customer1.PartyProductCategoryRevenueHistoriesWhereParty;
            customer1Cat1RevenueHistories.Filter.AddEquals(PartyProductCategoryRevenueHistories.Meta.ProductCategory, cat1);
            var customer1Cat1RevenueHistory = customer1Cat1RevenueHistories.First;

            var customer1Cat2RevenueHistories = customer1.PartyProductCategoryRevenueHistoriesWhereParty;
            customer1Cat2RevenueHistories.Filter.AddEquals(PartyProductCategoryRevenueHistories.Meta.ProductCategory, cat2);
            var customer1Cat2RevenueHistory = customer1Cat2RevenueHistories.First;

            var customer1CatMainRevenueHistories = customer1.PartyProductCategoryRevenueHistoriesWhereParty;
            customer1CatMainRevenueHistories.Filter.AddEquals(PartyProductCategoryRevenueHistories.Meta.ProductCategory, catMain);
            var customer1CatMainRevenueHistory = customer1CatMainRevenueHistories.First;

            Assert.AreEqual(180, customer1Cat1RevenueHistory.Revenue);
            Assert.AreEqual(100, customer1Cat2RevenueHistory.Revenue);
            Assert.AreEqual(280, customer1CatMainRevenueHistory.Revenue);

            var invoice3 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceDate(DateTime.UtcNow)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer2)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            var item3a = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantity(1).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice3.AddSalesInvoiceItem(item3a);

            var item3b = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantity(1).WithActualUnitPrice(10).WithSalesInvoiceItemType(productItem).Build();
            invoice3.AddSalesInvoiceItem(item3b);

            this.DatabaseSession.Derive(true);

            Singleton.Instance(this.DatabaseSession).DeriveRevenues();

            var customer2Cat1RevenueHistories = customer2.PartyProductCategoryRevenueHistoriesWhereParty;
            customer2Cat1RevenueHistories.Filter.AddEquals(PartyProductCategoryRevenueHistories.Meta.ProductCategory, cat1);
            var customer2Cat1RevenueHistory = customer2Cat1RevenueHistories.First;

            var customer2Cat2RevenueHistories = customer2.PartyProductCategoryRevenueHistoriesWhereParty;
            customer2Cat2RevenueHistories.Filter.AddEquals(PartyProductCategoryRevenueHistories.Meta.ProductCategory, cat2);
            var customer2Cat2RevenueHistory = customer2Cat2RevenueHistories.First;

            var customer2CatMainRevenueHistories = customer2.PartyProductCategoryRevenueHistoriesWhereParty;
            customer2CatMainRevenueHistories.Filter.AddEquals(PartyProductCategoryRevenueHistories.Meta.ProductCategory, catMain);
            var customer2CatMainRevenueHistory = customer2CatMainRevenueHistories.First;

            Assert.AreEqual(180, customer1Cat1RevenueHistory.Revenue);
            Assert.AreEqual(100, customer1Cat2RevenueHistory.Revenue);
            Assert.AreEqual(280, customer1CatMainRevenueHistory.Revenue);

            Assert.AreEqual(15, customer2Cat1RevenueHistory.Revenue);
            Assert.AreEqual(10, customer2Cat2RevenueHistory.Revenue);
            Assert.AreEqual(25, customer2CatMainRevenueHistory.Revenue);
        }
    }
}
