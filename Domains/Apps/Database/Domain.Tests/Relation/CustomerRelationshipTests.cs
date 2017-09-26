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
    using Xunit;

    
    public class CustomerRelationshipTests : DomainTest
    {
        [Fact]
        public void GivenCustomerRelationship_WhenDerivingWithout_ThenAmountDueIsZero()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();

            

            this.DatabaseSession.Derive();

            Assert.Equal(0, customer.AmountDue);
        }

        [Fact]
        public void GivenCustomerRelationship_WhenDerivingWithout_ThenAmountOverDueIsZero()
        {
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();

            

            this.DatabaseSession.Derive();

            Assert.Equal(0, customer.AmountOverDue);
        }

        [Fact]
        public void GivenCustomerRelationshipBuilder_WhenBuild_ThenSubAccountNumerIsValidElevenTestNumber()
        {
            var internalOrganisation = Singleton.Instance(this.DatabaseSession);
            internalOrganisation.SubAccountCounter.Value = 1000;

            this.DatabaseSession.Commit();

            var customer1 = new PersonBuilder(this.DatabaseSession).WithLastName("customer1").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();

            this.DatabaseSession.Derive();

            Assert.Equal(1007, customer1.SubAccountNumber);

            var customer2 = new PersonBuilder(this.DatabaseSession).WithLastName("customer2").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();

            this.DatabaseSession.Derive();

            Assert.Equal(1015, customer2.SubAccountNumber);

            var customer3 = new PersonBuilder(this.DatabaseSession).WithLastName("customer3").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();

            this.DatabaseSession.Derive();

            Assert.Equal(1023, customer3.SubAccountNumber);
        }

        [Fact]
        public void GivenCustomerRelationship_WhenDeriving_ThenSubAccountNumberMustBeUniqueWithinSingleton()
        {
            var customer2 = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();

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

            var internalOrganisation2 = new SingletonBuilder(this.DatabaseSession)
                .WithName("internalOrganisation2")
                .WithPartyContactMechanism(billingAddress)
                .WithDefaultPaymentMethod(ownBankAccount)
                .WithPreferredCurrency(euro)
                .Build();
           
            customer2.SubAccountNumber = 19;

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenCustomerWithUnpaidInvoices_WhenDeriving_ThenAmountDueIsUpdated()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            
            var billToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Mechelen").Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive();

            var invoice1 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(billToContactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            this.DatabaseSession.Derive();

            var invoice2 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(billToContactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(200M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(300M, customer.AmountDue);

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(50)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice1.SalesInvoiceItems[0]).WithAmountApplied(50).Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(250, customer.AmountDue);

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(200)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice2.SalesInvoiceItems[0]).WithAmountApplied(200).Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(50, customer.AmountDue);

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(50)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice1.SalesInvoiceItems[0]).WithAmountApplied(50).Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(0, customer.AmountDue);
        }

        [Fact]
        public void GivenCustomerWithUnpaidInvoices_WhenDeriving_ThenAmountOverDueIsUpdated()
        {
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").WithPersonRole(new PersonRoles(this.DatabaseSession).Customer).Build();
            
            var billToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Mechelen").Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive();

            var invoice1 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(billToContactMechanism)
                .WithInvoiceDate(DateTime.UtcNow.AddDays(-30))
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            this.DatabaseSession.Derive();

            var invoice2 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(billToContactMechanism)
                .WithInvoiceDate(DateTime.UtcNow.AddDays(-5))
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(200M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(100M, customer.AmountOverDue);

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(20)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice1.SalesInvoiceItems[0]).WithAmountApplied(20).Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(80, customer.AmountOverDue);

            invoice2.InvoiceDate = DateTime.UtcNow.AddDays(-10);

            this.DatabaseSession.Derive();

            Assert.Equal(280, customer.AmountOverDue);
        }
    }
}
