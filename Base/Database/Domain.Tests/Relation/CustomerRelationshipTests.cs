// <copyright file="CustomerRelationshipTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using Allors.Meta;
    using Xunit;

    public class CustomerRelationshipTests : DomainTest
    {
        [Fact]
        public void GivenCustomerRelationship_WhenDerivingWithout_ThenAmountDueIsZero()
        {
            var customer = new PersonBuilder(this.Session).WithLastName("customer").Build();

            var customerRelationship = new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var partyFinancial = customer.PartyFinancialRelationshipsWhereParty.First(v => Equals(v.InternalOrganisation, customerRelationship.InternalOrganisation));
            Assert.Equal(0, partyFinancial.AmountDue);
        }

        [Fact]
        public void GivenCustomerRelationship_WhenDerivingWithout_ThenAmountOverDueIsZero()
        {
            var customer = new PersonBuilder(this.Session).WithLastName("customer").Build();

            var customerRelationship = new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var partyFinancial = customer.PartyFinancialRelationshipsWhereParty.First(v => Equals(v.InternalOrganisation, customerRelationship.InternalOrganisation));

            Assert.Equal(0, partyFinancial.AmountOverDue);
        }

        [Fact]
        public void GivenCustomerRelationshipToCome_WhenDeriving_ThenInternalOrganisationCustomersDosNotContainCustomer()
        {
            var customer = new PersonBuilder(this.Session).WithLastName("customer").Build();
            var internalOrganisation = this.InternalOrganisation;

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(customer)

                .WithFromDate(this.Session.Now().AddDays(1))
                .Build();

            this.Session.Derive();

            Assert.False(internalOrganisation.CurrentCustomers.Contains(customer));
        }

        [Fact]
        public void GivenCustomerRelationshipThatHasEnded_WhenDeriving_ThenInternalOrganisationCustomersDosNotContainCustomer()
        {
            var customer = new PersonBuilder(this.Session).WithLastName("customer").Build();
            var internalOrganisation = this.InternalOrganisation;

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(customer)

                .WithFromDate(this.Session.Now().AddDays(-10))
                .WithThroughDate(this.Session.Now().AddDays(-1))
                .Build();

            this.Session.Derive();

            Assert.False(internalOrganisation.CurrentCustomers.Contains(customer));
        }

        [Fact]
        public void GivenCustomerRelationshipBuilder_WhenBuild_ThenSubAccountNumerIsValidElevenTestNumber()
        {
            var internalOrganisation = this.InternalOrganisation;
            internalOrganisation.SubAccountCounter.Value = 1000;

            this.Session.Commit();

            var customer1 = new PersonBuilder(this.Session).WithLastName("customer1").Build();
            var customerRelationship1 = new CustomerRelationshipBuilder(this.Session)
                .WithFromDate(this.Session.Now())
                .WithCustomer(customer1)
                .Build();

            this.Session.Derive();

            var partyFinancial1 = customer1.PartyFinancialRelationshipsWhereParty.First(v => Equals(v.InternalOrganisation, customerRelationship1.InternalOrganisation));

            Assert.Equal(1007, partyFinancial1.SubAccountNumber);

            var customer2 = new PersonBuilder(this.Session).WithLastName("customer2").Build();
            var customerRelationship2 = new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer2).Build();

            this.Session.Derive();

            var partyFinancial2 = customer2.PartyFinancialRelationshipsWhereParty.First(v => Equals(v.InternalOrganisation, customerRelationship2.InternalOrganisation));
            Assert.Equal(1015, partyFinancial2.SubAccountNumber);

            var customer3 = new PersonBuilder(this.Session).WithLastName("customer3").Build();
            var customerRelationship3 = new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer3).Build();

            this.Session.Derive();

            var partyFinancial3 = customer3.PartyFinancialRelationshipsWhereParty.First(v => Equals(v.InternalOrganisation, customerRelationship3.InternalOrganisation));
            Assert.Equal(1023, partyFinancial3.SubAccountNumber);
        }

        [Fact]
        public void GivenCustomerRelationship_WhenDeriving_ThenSubAccountNumberMustBeUniqueWithinSingleton()
        {
            var customer2 = new OrganisationBuilder(this.Session).WithName("customer").Build();

            var belgium = new Countries(this.Session).CountryByIsoCode["BE"];
            var euro = belgium.Currency;

            var bank = new BankBuilder(this.Session).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.Session)
                .WithDescription("BE23 3300 6167 6391")
                .WithBankAccount(new BankAccountBuilder(this.Session).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var address1 = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var internalOrganisation2 = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithDoAccounting(true)
                .WithName("internalOrganisation2")
                .WithDefaultCollectionMethod(ownBankAccount)
                .WithSubAccountCounter(new CounterBuilder(this.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build())
                .Build();

            var customerRelationship2 = new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(customer2)
                .WithInternalOrganisation(internalOrganisation2)
                .WithFromDate(this.Session.Now())
                .Build();

            this.Session.Derive();

            var partyFinancial = customer2.PartyFinancialRelationshipsWhereParty.First(v => Equals(v.InternalOrganisation, customerRelationship2.InternalOrganisation));
            partyFinancial.SubAccountNumber = 19;

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenCustomerWithUnpaidInvoices_WhenDeriving_ThenAmountDueIsUpdated()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var customerRelationship = new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).Build();

            this.Session.Derive();

            var partyFinancial = customer.PartyFinancialRelationshipsWhereParty.First(v => Equals(v.InternalOrganisation, customerRelationship.InternalOrganisation));

            var billToContactMechanism = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Mechelen").Build();

            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");
            good.VatRegime = new VatRegimes(this.Session).ZeroRated;

            this.Session.Derive();

            var invoice1 = new SalesInvoiceBuilder(this.Session)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithAssignedBillToContactMechanism(billToContactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithAssignedUnitPrice(100M).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build())
                .Build();

            this.Session.Derive();

            var invoice2 = new SalesInvoiceBuilder(this.Session)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithAssignedBillToContactMechanism(billToContactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithAssignedUnitPrice(200M).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(300M, partyFinancial.AmountDue);

            new ReceiptBuilder(this.Session)
                .WithAmount(50)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(invoice1.SalesInvoiceItems[0]).WithAmountApplied(50).Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(250, partyFinancial.AmountDue);

            new ReceiptBuilder(this.Session)
                .WithAmount(200)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(invoice2.SalesInvoiceItems[0]).WithAmountApplied(200).Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(50, partyFinancial.AmountDue);

            new ReceiptBuilder(this.Session)
                .WithAmount(50)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(invoice1.SalesInvoiceItems[0]).WithAmountApplied(50).Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(0, partyFinancial.AmountDue);
        }

        [Fact]
        public void GivenCustomerWithUnpaidInvoices_WhenDeriving_ThenAmountOverDueIsUpdated()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var customerRelationship = new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now().AddDays(-31)).WithCustomer(customer).Build();

            this.Session.Derive();

            var partyFinancial = customer.PartyFinancialRelationshipsWhereParty.First(v => Equals(v.InternalOrganisation, customerRelationship.InternalOrganisation));

            var billToContactMechanism = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Mechelen").Build();

            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");
            good.VatRegime = new VatRegimes(this.Session).ZeroRated;

            this.Session.Derive();

            var invoice1 = new SalesInvoiceBuilder(this.Session)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithAssignedBillToContactMechanism(billToContactMechanism)
                .WithInvoiceDate(this.Session.Now().AddDays(-30))
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithAssignedUnitPrice(100M).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build())
                .Build();

            this.Session.Derive();

            var invoice2 = new SalesInvoiceBuilder(this.Session)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithAssignedBillToContactMechanism(billToContactMechanism)
                .WithInvoiceDate(this.Session.Now().AddDays(-5))
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithAssignedUnitPrice(200M).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(100M, partyFinancial.AmountOverDue);

            new ReceiptBuilder(this.Session)
                .WithAmount(20)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(invoice1.SalesInvoiceItems[0]).WithAmountApplied(20).Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(80, partyFinancial.AmountOverDue);

            invoice2.InvoiceDate = this.Session.Now().AddDays(-10);

            this.Session.Derive();

            Assert.Equal(280, partyFinancial.AmountOverDue);
        }
    }
}
