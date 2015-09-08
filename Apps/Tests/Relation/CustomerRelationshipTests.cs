// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerRelationshipTests.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
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
// <summary>
//   Defines the PersonTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerRelationshipTests : DomainTest
    {
        [Test]
        public void GivenCustomerRelationship_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new CustomerRelationshipBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithCustomer(new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build());
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenCustomerRelationship_WhenDerivingWithout_ThenAmountDueIsZero()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();

            var customerRelationship = new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(0, customerRelationship.AmountDue);
        }

        [Test]
        public void GivenCustomerRelationship_WhenDerivingWithout_ThenAmountOverDueIsZero()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();

            var customerRelationship = new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(0, customerRelationship.AmountOverDue);
        }

        [Test]
        public void GivenActiveCustomerRelationship_WhenDeriving_ThenInternalOrganisationCustomersContainsCustomer()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .WithInternalOrganisation(internalOrganisation)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.Contains(customer, internalOrganisation.Customers);
        }

        [Test]
        public void GivenCustomerRelationshipToCome_WhenDeriving_ThenInternalOrganisationCustomersDosNotContainCustomer()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(DateTime.UtcNow.AddDays(1))
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.IsFalse(internalOrganisation.Customers.Contains(customer));
        }

        [Test]
        public void GivenCustomerRelationshipThatHasEnded_WhenDeriving_ThenInternalOrganisationCustomersDosNotContainCustomer()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(DateTime.UtcNow.AddDays(-10))
                .WithThroughDate(DateTime.UtcNow.AddDays(-1))
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.IsFalse(internalOrganisation.Customers.Contains(customer));
        }

        [Test]
        public void GivenActiveEmployment_WhenDeriving_ThenInternalOrganisationEmployeesContainsEmployee()
        {
            var employee = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();
            var employer = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new EmploymentBuilder(this.DatabaseSession)
                .WithEmployee(employee)
                .WithEmployer(employer)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.Contains(employee, employer.Employees);
        }

        [Test]
        public void GivenEmploymentToCome_WhenDeriving_ThenInternalOrganisationEmployeesDosNotContainEmployee()
        {
            var employee = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();
            var employer = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new EmploymentBuilder(this.DatabaseSession)
                .WithEmployee(employee)
                .WithEmployer(employer)
                .WithFromDate(DateTime.UtcNow.AddDays(1))
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.IsFalse(employer.Customers.Contains(employee));
        }

        [Test]
        public void GivenEmploymentThatHasEnded_WhenDeriving_ThenInternalOrganisationEmployeesDosNotContainEmployee()
        {
            var employee = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();
            var employer = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;

            new EmploymentBuilder(this.DatabaseSession)
                .WithEmployee(employee)
                .WithEmployer(employer)
                .WithFromDate(DateTime.UtcNow.AddDays(-10))
                .WithThroughDate(DateTime.UtcNow.AddDays(-1))
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.IsFalse(employer.Customers.Contains(employee));
        }

        [Test]
        public void GivenCustomerRelationshipBuilder_WhenBuild_ThenSubAccountNumerIsValidElevenTestNumber()
        {
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;
            internalOrganisation.SubAccountCounter.Value = 1000;

            this.DatabaseSession.Commit();

            var customer1 = new PersonBuilder(this.DatabaseSession).WithLastName("customer1").Build();
            var customerRelationship1 = new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer1).Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1007, customerRelationship1.SubAccountNumber);

            var customer2 = new PersonBuilder(this.DatabaseSession).WithLastName("customer2").Build();
            var customerRelationship2 = new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer2).Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1015, customerRelationship2.SubAccountNumber);

            var customer3 = new PersonBuilder(this.DatabaseSession).WithLastName("customer3").Build();
            var customerRelationship3 = new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer3).Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1023, customerRelationship3.SubAccountNumber);
        }

        [Test]
        public void GivenCustomerRelationship_WhenDeriving_ThenSubAccountNumberMustBeUniqueWithinInternalOrganisation()
        {
            var customer2 = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();

            var belgium = new Countries(this.DatabaseSession).CountryByIsoCode["BE"];
            var euro = belgium.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("BE23 3300 6167 6391")
                .WithBankAccount(new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var address1 = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var billingAddress = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(address1)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var internalOrganisation2 = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("internalOrganisation2")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithPartyContactMechanism(billingAddress)
                .WithDefaultPaymentMethod(ownBankAccount)
                .WithPreferredCurrency(euro)
                .Build();

            var customerRelationship2 = new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer2)
                .WithInternalOrganisation(internalOrganisation2)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            customerRelationship2.SubAccountNumber = 19;

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenCustomerWithUnpaidInvoices_WhenDeriving_ThenAmountDueIsUpdated()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();
            var customerRelationship = new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).Build();
            var billToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Mechelen").Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive(true);

            var invoice1 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(billToContactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            this.DatabaseSession.Derive(true);

            var invoice2 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(billToContactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(200M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(300M, customerRelationship.AmountDue);

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(50)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice1.SalesInvoiceItems[0]).WithAmountApplied(50).Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(250, customerRelationship.AmountDue);

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(200)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice2.SalesInvoiceItems[0]).WithAmountApplied(200).Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(50, customerRelationship.AmountDue);

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(50)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice1.SalesInvoiceItems[0]).WithAmountApplied(50).Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(0, customerRelationship.AmountDue);
        }

        [Test]
        public void GivenCustomerWithUnpaidInvoices_WhenDeriving_ThenAmountOverDueIsUpdated()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();
            var customerRelationship = new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).Build();
            var billToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Mechelen").Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive(true);

            var invoice1 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(billToContactMechanism)
                .WithInvoiceDate(DateTime.UtcNow.AddDays(-30))
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            this.DatabaseSession.Derive(true);

            var invoice2 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(billToContactMechanism)
                .WithInvoiceDate(DateTime.UtcNow.AddDays(-5))
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(200M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(100M, customerRelationship.AmountOverDue);

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(20)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice1.SalesInvoiceItems[0]).WithAmountApplied(20).Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(80, customerRelationship.AmountOverDue);

            invoice2.InvoiceDate = DateTime.UtcNow.AddDays(-10);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(280, customerRelationship.AmountOverDue);
        }

        [Test]
        public void GivenCustomerRelationship_WhenDeriving_ThenSameSubAccountNumberIsAllowedAtInternalOrganisation()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();

            var customerRelationship = new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation)
                .Build();

            this.DatabaseSession.Derive(true);

            customerRelationship.SubAccountNumber = customerRelationship.InternalOrganisation.CustomerRelationshipsWhereInternalOrganisation.First.SubAccountNumber;

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);
        }
    }
}
