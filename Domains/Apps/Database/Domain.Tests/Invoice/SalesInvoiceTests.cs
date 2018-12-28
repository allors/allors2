//------------------------------------------------------------------------------------------------- 
// <copyright file="SalesInvoiceTests.cs" company="Allors bvba">
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

using System.Linq;

namespace Allors.Domain
{
    using System;

    using Meta;

    using Resources;

    using Xunit;

    public class SalesInvoiceTests : DomainTest
    {
        [Fact]
        public void GivenSalesInvoice_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.Session.Derive();

            Assert.Equal(new SalesInvoiceStates(this.Session).ReadyForPosting, invoice.SalesInvoiceState);
            Assert.Equal(invoice.LastSalesInvoiceState, invoice.SalesInvoiceState);
        }

        [Fact]
        public void GivenSalesInvoice_WhenBuild_ThenPreviousObjectStateIsNull()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.Session.Derive();

            Assert.Null(invoice.PreviousSalesInvoiceState);
        }

        [Fact]
        public void GivenSalesInvoice_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            ContactMechanism billToContactMechanism = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).Build();

            this.Session.Commit();

            var builder = new SalesInvoiceBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithBillToCustomer(customer);
            builder.Build();
            
            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithBillToContactMechanism(billToContactMechanism);
            var invoice = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            Assert.Equal(invoice.SalesInvoiceState, new SalesInvoiceStates(this.Session).ReadyForPosting);
            Assert.Equal(invoice.SalesInvoiceState, invoice.LastSalesInvoiceState);

            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSalesInvoice_WhenDeriving_ThenBillToCustomerMustBeActiveCustomer()
        {
            var customer = new OrganisationBuilder(this.Session)
                .WithName("customer")
                
                .Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())
                .Build();

            new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            var expectedError = ErrorMessages.PartyIsNotACustomer;
            Assert.Equal(expectedError, this.Session.Derive(false).Errors[0].Message);

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSalesInvoice_WhenDeriving_ThenShipToCustomerMustBeActiveCustomer()
        {
            var billtoCcustomer = new OrganisationBuilder(this.Session).WithName("billToCustomer").Build();
            var shipToCustomer = new OrganisationBuilder(this.Session).WithName("shipToCustomer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(billtoCcustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            var expectedError = ErrorMessages.PartyIsNotACustomer;
            Assert.Equal(expectedError, this.Session.Derive(false).Errors[0].Message);

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSalesInvoice_WhenGettingInvoiceNumberWithoutFormat_ThenInvoiceNumberShouldBeReturned()
        {
            var store = new Stores(this.Session).Extent().First(v => Equals(v.InternalOrganisation, this.InternalOrganisation));
            store.RemoveSalesInvoiceNumberPrefix();

            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice1 = new SalesInvoiceBuilder(this.Session)
                .WithStore(store)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            this.Session.Derive();

            Assert.Equal("1", invoice1.InvoiceNumber);

            var invoice2 = new SalesInvoiceBuilder(this.Session)
                .WithStore(store)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            this.Session.Derive();

            Assert.Equal("2", invoice2.InvoiceNumber);
        }

        [Fact]
        public void GivenSingletonWithInvoiceSequenceFiscalYear_WhenCreatingInvoice_ThenInvoiceNumberFromFiscalYearMustBeUsed()
        {
            var store = new Stores(this.Session).Extent().First(v => Equals(v.InternalOrganisation, this.InternalOrganisation));
            store.RemoveSalesInvoiceNumberPrefix();

            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice1 = new SalesInvoiceBuilder(this.Session)
                .WithStore(store)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            this.Session.Derive();

            invoice1.Send();

            Assert.False(store.ExistSalesInvoiceCounter);
            Assert.Equal(DateTime.UtcNow.Year, store.FiscalYearInvoiceNumbers.First.FiscalYear);
            Assert.Equal("1", invoice1.InvoiceNumber);

            var invoice2 = new SalesInvoiceBuilder(this.Session)
                .WithStore(store)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            this.Session.Derive();

            invoice2.Send();

            Assert.False(store.ExistSalesInvoiceCounter);
            Assert.Equal(DateTime.UtcNow.Year, store.FiscalYearInvoiceNumbers.First.FiscalYear);
            Assert.Equal("2", invoice2.InvoiceNumber);
        }

        [Fact]
        public void GivenSalesInvoiceSend_WhenGettingInvoiceNumberWithFormat_ThenFormattedInvoiceNumberShouldBeReturned()
        {
            var store = new Stores(this.Session).Extent().First(v => Equals(v.InternalOrganisation, this.InternalOrganisation));
            store.SalesInvoiceNumberPrefix = "the format is ";
            store.SalesInvoiceTemporaryCounter = new CounterBuilder(this.Session).WithUniqueId(Guid.NewGuid()).WithValue(10).Build();

            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                    .WithLocality("Mechelen")
                    .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                    .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithStore(store)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            this.Session.Derive();

            invoice.Send();

            this.Session.Derive();

            Assert.Equal("the format is 1", invoice.InvoiceNumber);
        }

        [Fact]
        public void GivenSalesInvoiceNotSend_WhenGettingInvoiceNumberWithFormat_ThenTemporaryInvoiceNumberShouldBeReturned()
        {
            var store = new Stores(this.Session).Extent().First(v => Equals(v.InternalOrganisation, this.InternalOrganisation));
            store.SalesInvoiceNumberPrefix = "the format is ";
            store.SalesInvoiceTemporaryCounter = new CounterBuilder(this.Session).WithUniqueId(Guid.NewGuid()).WithValue(10).Build();

            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithStore(store)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            this.Session.Derive();

            Assert.Equal("11", invoice.InvoiceNumber);
        }

        [Fact]
        public void GivenSalesInvoice_WhenDeriving_ThenDerivedSalesRepMustExist()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var salesrep = new PersonBuilder(this.Session).WithLastName("salesrep").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            this.Session.Derive();
            this.Session.Commit();

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(customer)
                
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new SalesRepRelationshipBuilder(this.Session)
                .WithFromDate(DateTime.UtcNow)
                .WithCustomer(customer)
                .WithSalesRepresentative(salesrep)
                .Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build())
                .Build();

            this.Session.Derive(); 
            
            Assert.Contains(salesrep, invoice.SalesReps);
        }

        [Fact]
        public void GivenSalesInvoice_WhenDeriving_ThenBilledFromContactMechanismMustExist()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var homeAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("Sint-Lambertuslaan 78")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Muizen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var billingAddress = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(homeAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            customer.AddPartyContactMechanism(billingAddress);

            this.Session.Derive();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.Session.Derive();

            Assert.Equal(this.InternalOrganisation.BillingAddress, invoice.BilledFromContactMechanism);
        }

        [Fact]
        public void GivenSalesInvoiceWithBillToCustomerWithBillingAsdress_WhenDeriving_ThendBillToContactMechanismMustExist()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            ContactMechanism billToContactMechanism = new PostalAddressBuilder(this.Session).WithAddress1("Haverwerf 15").WithGeographicBoundary(mechelen).Build();

            var billingAddress = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(billToContactMechanism)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            customer.AddPartyContactMechanism(billingAddress);
            this.Session.Derive();

            var invoice = new SalesInvoiceBuilder(this.Session).WithBillToCustomer(customer).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.Session.Derive(); 

            Assert.Equal(billingAddress.ContactMechanism, invoice.BillToContactMechanism);
        }

        [Fact]
        public void GivenSalesInvoiceBuilderWithBillToCustomerWithPreferredCurrency_WhenBuilding_ThenDerivedCurrencyIsCustomersPreferredCurrency()
        {
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");

            var customer = new OrganisationBuilder(this.Session)
                .WithName("customer")
                .WithPreferredCurrency(euro)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            var billToContactMechanismMechelen = new WebAddressBuilder(this.Session).WithElectronicAddressString("dummy").Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(billToContactMechanismMechelen)
                .Build();

            Session.Derive();

            Assert.Equal(euro, invoice.Currency);
        }

        [Fact]
        public void GivenSalesInvoiceWithShipToCustomerWithShippingAddress_WhenDeriving_ThenShipToAddressMustExist()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            ContactMechanism shipToContactMechanism = new PostalAddressBuilder(this.Session).WithAddress1("Haverwerf 15").WithGeographicBoundary(mechelen).Build();

            var shippingAddress = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(shipToContactMechanism)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            customer.AddPartyContactMechanism(shippingAddress);

            this.Session.Derive();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithBillToContactMechanism(shipToContactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.Session.Derive();

            Assert.Equal(shippingAddress.ContactMechanism, invoice.ShipToAddress);
        }

        [Fact]
        public void GivenSalesInvoiceBuilderWithoutBillToCustomer_WhenBuilding_ThenDerivedCurrencyIsSingletonsPreferredCurrency()
        {
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");

            var customer = new OrganisationBuilder(this.Session)
                .WithName("customer")
                .WithLocale(new Locales(this.Session).DutchBelgium)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            Session.Derive();

            Assert.Equal(euro, invoice.Currency);
        }

        [Fact]
        public void GivenSalesInvoice_WhenDeriving_ThenLocaleMustExist()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice1 = new SalesInvoiceBuilder(this.Session).WithBillToCustomer(customer).WithBillToContactMechanism(contactMechanism).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.Session.Derive();

            Assert.Equal(this.Session.GetSingleton().DefaultLocale, invoice1.Locale);

            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            customer.Locale = dutchLocale;

            var invoice2 = new SalesInvoiceBuilder(this.Session).WithBillToCustomer(customer).WithBillToContactMechanism(contactMechanism).Build();

            this.Session.Derive();

            Assert.Equal(dutchLocale, invoice2.Locale);
        }

        [Fact]
        public void GivenSalesInvoice_WhenDeriving_ThenTotalAmountMustBeDerived()
        {
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();

            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            new SupplierOfferingBuilder(this.Session)
                .WithPart(good.Part)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrice(7)
                .WithCurrency(euro)
                .Build();

            var productItem = new InvoiceItemTypes(this.Session).ProductItem;

            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.Session.Derive();

            var invoice = new SalesInvoiceBuilder(this.Session).WithBillToCustomer(customer).WithBillToContactMechanism(contactMechanism).Build();

            var item1 = new SalesInvoiceItemBuilder(this.Session)
                .WithProduct(good)
                .WithInvoiceItemType(productItem)
                .WithQuantity(1)
                .WithActualUnitPrice(8)
                .Build();

            invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(8, invoice.TotalExVat);
            Assert.Equal(1.68M, invoice.TotalVat);
            Assert.Equal(9.68M, invoice.TotalIncVat);

            var item2 = new SalesInvoiceItemBuilder(this.Session)
                .WithProduct(good)
                .WithInvoiceItemType(productItem)
                .WithQuantity(1)
                .WithActualUnitPrice(8)
                .Build();

            var item3 = new SalesInvoiceItemBuilder(this.Session)
                .WithProduct(good)
                .WithInvoiceItemType(productItem)
                .WithQuantity(1)
                .WithActualUnitPrice(8)
                .Build();

            invoice.AddSalesInvoiceItem(item2);
            invoice.AddSalesInvoiceItem(item3);

            this.Session.Derive();

            Assert.Equal(24, invoice.TotalExVat);
            Assert.Equal(5.04M, invoice.TotalVat);
            Assert.Equal(29.04M, invoice.TotalIncVat);
            Assert.Equal(21, invoice.TotalPurchasePrice);
            Assert.Equal(invoice.TotalListPrice, invoice.TotalExVat);
        }

        [Fact]
        public void GivenSalesInvoice_WhenObjectStateIsReadyForPosting_ThenCheckTransitions()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var administrator = new PersonBuilder(this.Session).WithLastName("Administrator").WithUserName("administrator").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            this.Session.Derive();
            this.Session.Commit();

            SetIdentity("administrator");

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.Session.Derive();
            this.Session.Commit();

            var acl = new AccessControlList(invoice, this.Session.GetUser());
            Assert.True(acl.CanExecute(M.SalesInvoice.Send));
            Assert.True(acl.CanExecute(M.SalesInvoice.WriteOff));
            Assert.True(acl.CanExecute(M.SalesInvoice.CancelInvoice));
        }

        [Fact]
        public void GivenSalesInvoice_WhenObjectStateIsSent_ThenCheckTransitions()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var administrator = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("administrator").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            this.Session.Derive();
            this.Session.Commit();

            this.SetIdentity("administrator");

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.Session.Derive();

            invoice.Send();

            this.Session.Derive();
            this.Session.Commit();

            Assert.Equal(new SalesInvoiceStates(this.Session).Sent, invoice.SalesInvoiceState);

            var acl = new AccessControlList(invoice, this.Session.GetUser());
            Assert.False(acl.CanExecute(M.SalesInvoice.Send));
            Assert.True(acl.CanExecute(M.SalesInvoice.WriteOff));
            Assert.False(acl.CanExecute(M.SalesInvoice.CancelInvoice));
        }

        ////[Fact]
        ////public void GivenSalesInvoice_WhenObjectStateIsReceived_ThenCheckTransitions()
        ////{
        ////    SecurityPopulation securityPopulation = new SecurityPopulation(this.Session);
        ////    securityPopulation.CoreCreateUserGroups();
        ////    securityPopulation.CoreAssignDefaultPermissions();
        ////    new Invoices(this.Session).Populate();
        ////    new SalesInvoices(this.Session).Populate();

        ////    Person administrator = new PersonBuilder(this.Session).WithUserName("administrator").WithLastName("administrator").Build();
        ////    securityPopulation.CoreAdministrators.AddMember(administrator);

        ////    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("administrator", "Forms"), new string[0]);
        ////    SalesInvoice invoice = new SalesInvoiceBuilder(this.Session)
        ////        .WithInvoiceStatus(new InvoiceStatusBuilder(this.Session).WithObjectState(new Invoices(this.Session).Received).Build())
        ////        .Build();

        ////    AccessControlList acl = new AccessControlList(invoice, this.Session.GetUser()());
        ////    Assert.False(acl.CanExecute(Invoices.ReadyForPostingId));
        ////    Assert.False(acl.CanExecute(Invoices.ApproveId));
        ////    Assert.False(acl.CanExecute(Invoices.SendId));
        ////    Assert.False(acl.CanExecute(Invoices.WriteOffId));
        ////    Assert.False(acl.CanExecute(Invoices.CancelId));
        ////}

        [Fact]
        public void GivenSalesInvoice_WhenObjectStateIsPaid_ThenCheckTransitions()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var administrator = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("administrator").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            this.Session.Derive();
            this.Session.Commit();

            this.SetIdentity("administrator");

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build())
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.Session.Derive();

            new ReceiptBuilder(this.Session)
                .WithAmount(100)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(invoice.SalesInvoiceItems[0]).WithAmountApplied(100).Build())
                .Build();

            this.Session.Derive();

            var acl = new AccessControlList(invoice, this.Session.GetUser());
            Assert.False(acl.CanExecute(M.SalesInvoice.Send));
            Assert.False(acl.CanExecute(M.SalesInvoice.WriteOff));
            Assert.False(acl.CanExecute(M.SalesInvoice.CancelInvoice));
        }

        [Fact]
        public void GivenSalesInvoice_WhenObjectStateIsPartiallyPaid_ThenCheckTransitions()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var administrator = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("administrator").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            this.Session.Derive();
            this.Session.Commit();

            this.SetIdentity("administrator");

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build())
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.Session.Derive();

            new ReceiptBuilder(this.Session)
                .WithAmount(90)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(invoice.SalesInvoiceItems[0]).WithAmountApplied(90).Build())
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var acl = new AccessControlList(invoice, this.Session.GetUser());
            Assert.False(acl.CanExecute(M.SalesInvoice.Send));
            Assert.True(acl.CanExecute(M.SalesInvoice.WriteOff));
            Assert.False(acl.CanExecute(M.SalesInvoice.CancelInvoice));
        }

        [Fact]
        public void GivenSalesInvoice_WhenObjectStateIsWrittenOff_ThenCheckTransitions()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var administrator = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("administrator").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            this.Session.Derive();
            this.Session.Commit();

            this.SetIdentity("administrator");

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.Session.Derive();

            invoice.Send();
            invoice.WriteOff();

            this.Session.Derive();

            var acl = new AccessControlList(invoice, this.Session.GetUser());
            Assert.False(acl.CanExecute(M.SalesInvoice.Send));
            Assert.False(acl.CanExecute(M.SalesInvoice.WriteOff));
            Assert.False(acl.CanExecute(M.SalesInvoice.CancelInvoice));
        }

        [Fact]
        public void GivenSalesInvoice_WhenObjectStateIsCancelled_ThenCheckTransitions()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var administrator = new PersonBuilder(this.Session).WithFirstName("Koen").WithUserName("administrator").Build();
            var administrators = new UserGroups(this.Session).Administrators;
            administrators.AddMember(administrator);

            this.Session.Derive();
            this.Session.Commit();

            this.SetIdentity("administrator");

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            invoice.CancelInvoice();

            this.Session.Derive();

            var acl = new AccessControlList(invoice, this.Session.GetUser());
            Assert.Equal(new SalesInvoiceStates(this.Session).Cancelled, invoice.SalesInvoiceState);
            Assert.False(acl.CanExecute(M.SalesInvoice.Send));
            Assert.False(acl.CanExecute(M.SalesInvoice.WriteOff));
            Assert.False(acl.CanExecute(M.SalesInvoice.CancelInvoice));
        }

        [Fact]
        public void GivenSalesInvoiceWithShippingAndHandlingAmount_WhenDeriving_ThenInvoiceTotalsMustIncludeShippingAndHandlingAmount()
        {
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var adjustment = new ShippingAndHandlingChargeBuilder(this.Session).WithAmount(7.5M).WithVatRate(vatRate21).Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(new OrganisationBuilder(this.Session).WithName("customer").Build())
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithShippingAndHandlingCharge(adjustment)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(3).WithActualUnitPrice(15).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build();
            invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(45, invoice.TotalBasePrice);
            Assert.Equal(0, invoice.TotalDiscount);
            Assert.Equal(0, invoice.TotalSurcharge);
            Assert.Equal(7.5m, invoice.TotalShippingAndHandling);
            Assert.Equal(0, invoice.TotalFee);
            Assert.Equal(52.5m, invoice.TotalExVat);
            Assert.Equal(11.03m, invoice.TotalVat);
            Assert.Equal(63.53m, invoice.TotalIncVat);
        }

        [Fact]
        public void GivenSalesInvoiceWithShippingAndHandlingPercentage_WhenDeriving_ThenSalesInvoiceTotalsMustIncludeShippingAndHandlingAmount()
        {
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var adjustment = new ShippingAndHandlingChargeBuilder(this.Session).WithPercentage(5).WithVatRate(vatRate21).Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(new OrganisationBuilder(this.Session).WithName("customer").Build())
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithShippingAndHandlingCharge(adjustment)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(3).WithActualUnitPrice(15).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build();
            invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(45, invoice.TotalBasePrice);
            Assert.Equal(0, invoice.TotalDiscount);
            Assert.Equal(0, invoice.TotalSurcharge);
            Assert.Equal(2.25m, invoice.TotalShippingAndHandling);
            Assert.Equal(0, invoice.TotalFee);
            Assert.Equal(47.25m, invoice.TotalExVat);
            Assert.Equal(9.92m, invoice.TotalVat);
            Assert.Equal(57.17m, invoice.TotalIncVat);
        }

        [Fact]
        public void GivenSalesInvoiceWithFeeAmount_WhenDeriving_ThenSalesInvoiceTotalsMustIncludeFeeAmount()
        {
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var adjustment = new FeeBuilder(this.Session).WithAmount(7.5M).WithVatRate(vatRate21).Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(new OrganisationBuilder(this.Session).WithName("customer").Build())
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithFee(adjustment)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(3).WithActualUnitPrice(15).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build();
            invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(45, invoice.TotalBasePrice);
            Assert.Equal(0, invoice.TotalDiscount);
            Assert.Equal(0, invoice.TotalSurcharge);
            Assert.Equal(0, invoice.TotalShippingAndHandling);
            Assert.Equal(7.5m, invoice.TotalFee);
            Assert.Equal(52.5m, invoice.TotalExVat);
            Assert.Equal(11.03m, invoice.TotalVat);
            Assert.Equal(63.53m, invoice.TotalIncVat);
        }

        [Fact]
        public void GivenSalesInvoiceWithFeePercentage_WhenDeriving_ThenSalesInvoiceTotalsMustIncludeFeeAmount()
        {
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var adjustment = new FeeBuilder(this.Session).WithPercentage(5).WithVatRate(vatRate21).Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(new OrganisationBuilder(this.Session).WithName("customer").Build())
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithFee(adjustment)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(3).WithActualUnitPrice(15).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build();
            invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(45, invoice.TotalBasePrice);
            Assert.Equal(0, invoice.TotalDiscount);
            Assert.Equal(0, invoice.TotalSurcharge);
            Assert.Equal(0, invoice.TotalShippingAndHandling);
            Assert.Equal(2.25m, invoice.TotalFee);
            Assert.Equal(47.25m, invoice.TotalExVat);
            Assert.Equal(9.92m, invoice.TotalVat);
            Assert.Equal(57.17m, invoice.TotalIncVat);
        }

        [Fact]
        public void GivenSalesInvoice_WhenShipToAndBillToAreSameCustomer_ThenDerivedCustomersIsSingleCustomer()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithShipToCustomer(customer)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.Session.Derive(); 
            
            Assert.Equal(1, invoice.Customers.Count);
            Assert.Equal(customer, invoice.Customers.First);
        }

        [Fact]
        public void GivenSalesInvoice_WhenShipToAndBillToAreDifferentCustomers_ThenDerivedCustomersHoldsBothCustomers()
        {
            var billToCustomer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var shipToCustomer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithShipToCustomer(shipToCustomer)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.ShipToCustomer).Build();

            this.Session.Derive(); 
            
            Assert.Equal(2, invoice.Customers.Count);
            Assert.Contains(billToCustomer, invoice.Customers);
            Assert.Contains(shipToCustomer, invoice.Customers);
        }

        [Fact]
        public void GivenSalesInvoice_WhenDerivingSalesReps_ThenSalesRepsAreCollectedFromSalesInvoiceItems()
        {
            var salesrep1 = new PersonBuilder(this.Session).WithLastName("salesrep for child product category").Build();
            var salesrep2 = new PersonBuilder(this.Session).WithLastName("salesrep for parent category").Build();
            var salesrep3 = new PersonBuilder(this.Session).WithLastName("salesrep for customer").Build();
            var parentProductCategory = new ProductCategoryBuilder(this.Session)
                .WithName("parent")
                .Build();

            var childProductCategory = new ProductCategoryBuilder(this.Session)
                .WithName("child")
                .WithParent(parentProductCategory).
                Build();

            var billToCustomer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            new SalesRepRelationshipBuilder(this.Session)
                .WithSalesRepresentative(salesrep1)
                .WithCustomer(billToCustomer)
                .WithProductCategory(childProductCategory)
                .Build();

            new SalesRepRelationshipBuilder(this.Session)
                .WithSalesRepresentative(salesrep2)
                .WithCustomer(billToCustomer)
                .WithProductCategory(parentProductCategory)
                .Build();

            new SalesRepRelationshipBuilder(this.Session)
                .WithSalesRepresentative(salesrep3)
                .WithCustomer(billToCustomer)
                .Build();

            var good1 = new GoodBuilder(this.Session)
                .WithGoodIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("1")
                    .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Good).Build())
                .WithName("good")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(0).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPart(new PartBuilder(this.Session)
                    .WithGoodIdentification(new PartNumberBuilder(this.Session)
                        .WithIdentification("1")
                        .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Part).Build())
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            childProductCategory.AddProduct(good1);

            var good2 = new GoodBuilder(this.Session)
                .WithGoodIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("1")
                    .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Good).Build())
                .WithName("good")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(0).Build())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPart(new PartBuilder(this.Session)
                    .WithGoodIdentification(new PartNumberBuilder(this.Session)
                        .WithIdentification("2")
                        .WithGoodIdentificationType(new GoodIdentificationTypes(this.Session).Part).Build())
                    .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised).Build())
                .Build();

            parentProductCategory.AddProduct(good2);

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            var item1 = new SalesInvoiceItemBuilder(this.Session)
                .WithProduct(good1)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .WithQuantity(3)
                .WithActualUnitPrice(5)
                .Build();

            var item2 = new SalesInvoiceItemBuilder(this.Session)
                .WithProduct(good2)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .WithQuantity(3)
                .WithActualUnitPrice(5)
                .Build();

            invoice.AddSalesInvoiceItem(item2);

            this.Session.Derive();

            Assert.Equal(2, invoice.SalesReps.Count);
            Assert.Contains(salesrep2, invoice.SalesReps);
            Assert.Contains(salesrep3, invoice.SalesReps);

            invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(3, invoice.SalesReps.Count);
            Assert.Contains(salesrep1, invoice.SalesReps);
            Assert.Contains(salesrep2, invoice.SalesReps);
            Assert.Contains(salesrep3, invoice.SalesReps);
        }

        [Fact]
        public void GivenSalesInvoice_WhenPartialPaymentIsReceived_ThenInvoiceStateIsSetToPartiallyPaid()
        {
            var billToCustomer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            this.Session.Derive();
            this.Session.Commit();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build())
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(2).WithActualUnitPrice(100M).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build())
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            this.Session.Derive();

            new ReceiptBuilder(this.Session)
                .WithAmount(90)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(invoice.SalesInvoiceItems[0]).WithAmountApplied(90).Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(new SalesInvoiceStates(this.Session).PartiallyPaid, invoice.SalesInvoiceState);
        }

        [Fact]
        public void GiveninvoiceItem_WhenFullPaymentIsReceived_ThenInvoiceItemStateIsSetToPaid()
        {
            var billToCustomer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");
            good.VatRate = new VatRateBuilder(this.Session).WithRate(0).Build();

            this.Session.Derive();
            this.Session.Commit();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            invoice.AddSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build());

            this.Session.Derive();

            new ReceiptBuilder(this.Session)
                .WithAmount(100)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(invoice.InvoiceItems[0]).WithAmountApplied(100).Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(new SalesInvoiceStates(this.Session).Paid, invoice.SalesInvoiceState);
        }

        [Fact]
        public void GiveninvoiceItem_WhenCancelled_ThenInvoiceItemsAreCancelled()
        {
            var billToCustomer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            this.Session.Derive();
            this.Session.Commit();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build())
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build())
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            this.Session.Derive();

            invoice.CancelInvoice();

            this.Session.Derive();

            Assert.Equal(new SalesInvoiceStates(this.Session).Cancelled, invoice.SalesInvoiceState);
            Assert.Equal(new SalesInvoiceItemStates(this.Session).Cancelled, invoice.SalesInvoiceItems[0].SalesInvoiceItemState);
            Assert.Equal(new SalesInvoiceItemStates(this.Session).Cancelled, invoice.SalesInvoiceItems[1].SalesInvoiceItemState);
        }

        [Fact]
        public void GiveninvoiceItem_WhenWrittenOff_ThenInvoiceItemsAreWrittenOff()
        {
            var billToCustomer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            this.Session.Derive();
            this.Session.Commit();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build())
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build())
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            invoice.WriteOff();

            this.Session.Derive();

            Assert.Equal(new SalesInvoiceStates(this.Session).WrittenOff, invoice.SalesInvoiceState);
            Assert.Equal(new SalesInvoiceItemStates(this.Session).WrittenOff, invoice.SalesInvoiceItems[0].SalesInvoiceItemState);
            Assert.Equal(new SalesInvoiceItemStates(this.Session).WrittenOff, invoice.SalesInvoiceItems[1].SalesInvoiceItemState);
        }

        [Fact]
        public void GivenSalesInvoice_WhenDeriving_ThenPrintDocumentRendered()
        {
            // Arrange
            var demo = new Demo(this.Session, null);
            demo.Execute();

            // Act
            this.Session.Derive(true);

            // Assert
            var invoice = new SalesInvoices(this.Session).Extent().First;

            Assert.NotNull(invoice.PrintDocument);
            //var result = invoice.PrintDocument;

            //var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //var outputFile = System.IO.File.Create(System.IO.Path.Combine(desktopDir, "salesInvoice.odt"));
            //var stream = new System.IO.MemoryStream(result.MediaContent.Data);

            //stream.CopyTo(outputFile);
            //stream.Close();
        }
    }
}