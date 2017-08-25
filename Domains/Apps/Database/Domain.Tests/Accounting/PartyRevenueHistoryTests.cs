// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyRevenueHistoryTests.cs" company="Allors bvba">
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

    
    public class PartyRevenueHistoryTests : DomainTest
    {
        [Fact]
        public void GivenSalesInvoice_WhenDerived_ThenTotalExVatIsAddedToPartyRevenueHistory()
        {
            var productItem = new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem;
            var contactMechanism = new ContactMechanisms(this.DatabaseSession).Extent().First;

            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var catMain = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("main cat").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .Build();
            var cat1 = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("cat for good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithParent(catMain)
                .Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good1").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithPrimaryProductCategory(cat1)
                .Build();

            var customer = new Organisations(this.DatabaseSession).FindBy(M.Organisation.Name, "customer");
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            internalOrganisation.PreferredCurrency = euro;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            this.DatabaseSession.Derive();
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

                this.DatabaseSession.Derive();
                this.DatabaseSession.Commit();

                var history = customer.PartyRevenueHistoriesWhereParty.First;

                //// first iteration is too old
                if (i > 1)
                {
                    revenuePastTwelveMonths += i * 10M;
                }

                ////date in first iteration is too old, no history yet.
                if (history != null)
                {
                    Assert.Equal(revenuePastTwelveMonths, history.Revenue);
                }

                date = date.AddMonths(1);
            }
        }

        [Fact]
        public void DeriveHistory()
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

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            internalOrganisation.PreferredCurrency = euro;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer1).WithInternalOrganisation(internalOrganisation).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer2).WithInternalOrganisation(internalOrganisation).Build();

            this.DatabaseSession.Derive();

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

            this.DatabaseSession.Derive();

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

            this.DatabaseSession.Derive();

            Singleton.Instance(this.DatabaseSession).DeriveRevenues(new NonLogging.Derivation(this.DatabaseSession));

            var customer1RevenueHistory = customer1.PartyRevenueHistoriesWhereParty.First;
            Assert.Equal(280, customer1RevenueHistory.Revenue);

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

            this.DatabaseSession.Derive();

            Singleton.Instance(this.DatabaseSession).DeriveRevenues(new NonLogging.Derivation(this.DatabaseSession));

            var customer2RevenueHistory = customer2.PartyRevenueHistoriesWhereParty.First;
            Assert.Equal(280, customer1RevenueHistory.Revenue);
            Assert.Equal(25, customer2RevenueHistory.Revenue);
        }
    }
}
