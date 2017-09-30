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
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesInvoiceStates(this.DatabaseSession).ReadyForPosting, invoice.SalesInvoiceState);
            Assert.Equal(invoice.LastSalesInvoiceState, invoice.SalesInvoiceState);
        }

        [Fact]
        public void GivenSalesInvoice_WhenBuild_ThenPreviousObjectStateIsNull()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.DatabaseSession.Derive();

            Assert.Null(invoice.PreviousSalesInvoiceState);
        }

        [Fact]
        public void GivenSalesInvoice_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            ContactMechanism billToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).Build();

            this.DatabaseSession.Commit();

            var builder = new SalesInvoiceBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithBillToCustomer(customer);
            builder.Build();
            
            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithBillToContactMechanism(billToContactMechanism);
            var invoice = builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            Assert.Equal(invoice.SalesInvoiceState, new SalesInvoiceStates(this.DatabaseSession).ReadyForPosting);
            Assert.Equal(invoice.SalesInvoiceState, invoice.LastSalesInvoiceState);

            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSalesInvoice_WhenDeriving_ThenBillToCustomerMustBeActiveCustomer()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession)
                .WithName("customer")
                .WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer)
                .Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())
                .Build();

            new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            var expectedError = ErrorMessages.PartyIsNotACustomer;
            Assert.Equal(expectedError, this.DatabaseSession.Derive(false).Errors[0].Message);

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSalesInvoice_WhenDeriving_ThenShipToCustomerMustBeActiveCustomer()
        {
            var billtoCcustomer = new OrganisationBuilder(this.DatabaseSession).WithName("billToCustomer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var shipToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("shipToCustomer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(billtoCcustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            var expectedError = ErrorMessages.PartyIsNotACustomer;
            Assert.Equal(expectedError, this.DatabaseSession.Derive(false).Errors[0].Message);

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(shipToCustomer).Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSalesInvoice_WhenGettingInvoiceNumberWithoutFormat_ThenInvoiceNumberShouldBeReturned()
        {
            var store = new StoreBuilder(this.DatabaseSession).WithName("store")
                .WithDefaultFacility(new Facilities(this.DatabaseSession).FindBy(M.Facility.FacilityType, new FacilityTypes(this.DatabaseSession).Warehouse))
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .Build();

            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice1 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithStore(store)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            Assert.Equal("1", invoice1.InvoiceNumber);

            var invoice2 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithStore(store)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            Assert.Equal("2", invoice2.InvoiceNumber);
        }

        [Fact]
        public void GivenSingletonWithInvoiceSequenceFiscalYear_WhenCreatingInvoice_ThenInvoiceNumberFromFiscalYearMustBeUsed()
        {
            var store = new StoreBuilder(this.DatabaseSession).WithName("store")
                .WithDefaultFacility(new Facilities(this.DatabaseSession).FindBy(M.Facility.FacilityType, new FacilityTypes(this.DatabaseSession).Warehouse))
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .Build();

            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice1 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithStore(store)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            Assert.False(store.ExistSalesInvoiceCounter);
            Assert.Equal(DateTime.UtcNow.Year, store.FiscalYearInvoiceNumbers.First.FiscalYear);
            Assert.Equal("1", invoice1.InvoiceNumber);

            var invoice2 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithStore(store)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            Assert.False(store.ExistSalesInvoiceCounter);
            Assert.Equal(DateTime.UtcNow.Year, store.FiscalYearInvoiceNumbers.First.FiscalYear);
            Assert.Equal("2", invoice2.InvoiceNumber);
        }

        [Fact]
        public void GivenSalesInvoice_WhenGettingInvoiceNumberWithFormat_ThenFormattedInvoiceNumberShouldBeReturned()
        {
            var store = new StoreBuilder(this.DatabaseSession).WithName("store")
                .WithDefaultFacility(new Facilities(this.DatabaseSession).FindBy(M.Facility.FacilityType, new FacilityTypes(this.DatabaseSession).Warehouse))
                .WithSalesInvoiceNumberPrefix("the format is ")
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .Build();

            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithStore(store)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            Assert.Equal("the format is 1", invoice.InvoiceNumber);
        }

        [Fact]
        public void GivenSalesInvoice_WhenDeriving_ThenDerivedSalesRepMustExist()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var salesrep = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithFromDate(DateTime.UtcNow)
                .WithCustomer(customer)
                .WithSalesRepresentative(salesrep)
                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            this.DatabaseSession.Derive(); 
            
            Assert.Contains(salesrep, invoice.SalesReps);
        }

        [Fact]
        public void GivenSalesInvoice_WhenDeriving_ThenBilledFromContactMechanismMustExist()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var homeAddress = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Sint-Lambertuslaan 78")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Muizen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;
            internalOrganisation.BillingAddress = homeAddress;

            this.DatabaseSession.Derive();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.DatabaseSession.Derive();

            Assert.Equal(homeAddress, invoice.BilledFromContactMechanism);
        }

        [Fact]
        public void GivenSalesInvoiceWithBillToCustomerWithBillingAsdress_WhenDeriving_ThendBillToContactMechanismMustExist()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            ContactMechanism billToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithAddress1("Haverwerf 15").WithGeographicBoundary(mechelen).Build();

            var billingAddress = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(billToContactMechanism)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            customer.AddPartyContactMechanism(billingAddress);
            this.DatabaseSession.Derive();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession).WithBillToCustomer(customer).Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.DatabaseSession.Derive(); 

            Assert.Equal(billingAddress.ContactMechanism, invoice.BillToContactMechanism);
        }

        [Fact]
        public void GivenSalesInvoiceBuilderWithBillToCustomerWithPreferredCurrency_WhenBuilding_ThenDerivedCurrencyIsCustomersPreferredCurrency()
        {
            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");

            var customer = new OrganisationBuilder(this.DatabaseSession)
                .WithName("customer")
                .WithPreferredCurrency(euro)
                .WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer)
                .Build();

            var billToContactMechanismMechelen = new PostalAddressBuilder(this.DatabaseSession).WithAddress1("Mechelen").Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(billToContactMechanismMechelen)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            Assert.Equal(euro, invoice.CustomerCurrency);
        }

        [Fact]
        public void GivenSalesInvoiceWithShipToCustomerWithShippingAddress_WhenDeriving_ThenShipToAddressMustExist()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            ContactMechanism shipToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithAddress1("Haverwerf 15").WithGeographicBoundary(mechelen).Build();

            var shippingAddress = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(shipToContactMechanism)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            customer.AddPartyContactMechanism(shippingAddress);

            this.DatabaseSession.Derive();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithBillToContactMechanism(shipToContactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.DatabaseSession.Derive();

            Assert.Equal(shippingAddress.ContactMechanism, invoice.ShipToAddress);
        }

        [Fact]
        public void GivenSalesInvoiceBuilderWithoutBillToCustomer_WhenBuilding_ThenDerivedCurrencyIsSingletonsPreferredCurrency()
        {
            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");

            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            Assert.Equal(euro, invoice.CustomerCurrency);
        }

        [Fact]
        public void GivenSalesInvoice_WhenDeriving_ThenLocaleMustExist()
        {
            var englischLocale = new Locales(this.DatabaseSession).EnglishGreatBritain;
            var dutchLocale = new Locales(this.DatabaseSession).DutchNetherlands;

            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice1 = new SalesInvoiceBuilder(this.DatabaseSession).WithBillToCustomer(customer).WithBillToContactMechanism(contactMechanism).Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.DatabaseSession.Derive();

            Assert.Equal(englischLocale, invoice1.Locale);

            customer.Locale = dutchLocale;

            var invoice2 = new SalesInvoiceBuilder(this.DatabaseSession).WithBillToCustomer(customer).WithBillToContactMechanism(contactMechanism).Build();

            this.DatabaseSession.Derive();

            Assert.Equal(dutchLocale, invoice2.Locale);
        }

        [Fact]
        public void GivenSalesInvoice_WhenDeriving_ThenTotalAmountMustBeDerived()
        {
            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Supplier).Build();
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(19).Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var goodPurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithProductPurchasePrice(goodPurchasePrice)
                .Build();

            var productItem = new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem;

            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.DatabaseSession.Derive();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession).WithBillToCustomer(customer).WithBillToContactMechanism(contactMechanism).Build();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithSalesInvoiceItemType(productItem)
                .WithQuantity(1)
                .WithActualUnitPrice(8)
                .Build();

            invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(8, invoice.TotalExVat);
            Assert.Equal(1.52M, invoice.TotalVat);
            Assert.Equal(9.52M, invoice.TotalIncVat);

            var item2 = new SalesInvoiceItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithSalesInvoiceItemType(productItem)
                .WithQuantity(1)
                .WithActualUnitPrice(8)
                .Build();

            var item3 = new SalesInvoiceItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithSalesInvoiceItemType(productItem)
                .WithQuantity(1)
                .WithActualUnitPrice(8)
                .Build();

            invoice.AddSalesInvoiceItem(item2);
            invoice.AddSalesInvoiceItem(item3);

            this.DatabaseSession.Derive();

            Assert.Equal(24, invoice.TotalExVat);
            Assert.Equal(4.56M, invoice.TotalVat);
            Assert.Equal(28.56M, invoice.TotalIncVat);
            Assert.Equal(21, invoice.TotalPurchasePrice);
            Assert.Equal(invoice.TotalListPrice, invoice.TotalExVat);
        }

        [Fact]
        public void GivenSalesInvoice_WhenObjectStateIsReadyForPosting_ThenCheckTransitions()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var administrator = new PersonBuilder(this.DatabaseSession).WithLastName("Administrator").WithUserName("administrator").Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            SetIdentity("administrator");

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).CurrentUser);
            Assert.True(acl.CanExecute(M.SalesInvoice.Send));
            Assert.True(acl.CanExecute(M.SalesInvoice.WriteOff));
            Assert.True(acl.CanExecute(M.SalesInvoice.CancelInvoice));
        }

        [Fact]
        public void GivenSalesInvoice_WhenObjectStateIsSent_ThenCheckTransitions()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("administrator").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.SetIdentity("administrator");

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            invoice.Send();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).CurrentUser);
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

        ////    AccessControlList acl = new AccessControlList(invoice, new Users(this.Session).CurrentUser());
        ////    Assert.False(acl.CanExecute(Invoices.ReadyForPostingId));
        ////    Assert.False(acl.CanExecute(Invoices.ApproveId));
        ////    Assert.False(acl.CanExecute(Invoices.SendId));
        ////    Assert.False(acl.CanExecute(Invoices.WriteOffId));
        ////    Assert.False(acl.CanExecute(Invoices.CancelId));
        ////}

        [Fact]
        public void GivenSalesInvoice_WhenObjectStateIsPaid_ThenCheckTransitions()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("administrator").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.SetIdentity("administrator");

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.DatabaseSession.Derive();

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(100)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice.SalesInvoiceItems[0]).WithAmountApplied(100).Build())
                .Build();

            this.DatabaseSession.Derive();

            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanExecute(M.SalesInvoice.Send));
            Assert.False(acl.CanExecute(M.SalesInvoice.WriteOff));
            Assert.False(acl.CanExecute(M.SalesInvoice.CancelInvoice));
        }

        [Fact]
        public void GivenSalesInvoice_WhenObjectStateIsPartiallyPaid_ThenCheckTransitions()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("administrator").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.SetIdentity("administrator");

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.DatabaseSession.Derive();

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(90)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice.SalesInvoiceItems[0]).WithAmountApplied(90).Build())
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanExecute(M.SalesInvoice.Send));
            Assert.True(acl.CanExecute(M.SalesInvoice.WriteOff));
            Assert.False(acl.CanExecute(M.SalesInvoice.CancelInvoice));
        }

        [Fact]
        public void GivenSalesInvoice_WhenObjectStateIsWrittenOff_ThenCheckTransitions()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("administrator").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.SetIdentity("administrator");

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            invoice.Send();
            invoice.WriteOff();

            this.DatabaseSession.Derive();

            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).CurrentUser);
            Assert.False(acl.CanExecute(M.SalesInvoice.Send));
            Assert.False(acl.CanExecute(M.SalesInvoice.WriteOff));
            Assert.False(acl.CanExecute(M.SalesInvoice.CancelInvoice));
        }

        [Fact]
        public void GivenSalesInvoice_WhenObjectStateIsCancelled_ThenCheckTransitions()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("administrator").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            this.SetIdentity("administrator");

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            invoice.CancelInvoice();

            this.DatabaseSession.Derive();

            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).CurrentUser);
            Assert.Equal(new SalesInvoiceStates(this.DatabaseSession).Cancelled, invoice.SalesInvoiceState);
            Assert.False(acl.CanExecute(M.SalesInvoice.Send));
            Assert.False(acl.CanExecute(M.SalesInvoice.WriteOff));
            Assert.False(acl.CanExecute(M.SalesInvoice.CancelInvoice));
        }

        [Fact]
        public void GivenSalesInvoiceWithShippingAndHandlingAmount_WhenDeriving_ThenInvoiceTotalsMustIncludeShippingAndHandlingAmount()
        {
            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var adjustment = new ShippingAndHandlingChargeBuilder(this.DatabaseSession).WithAmount(7.5M).WithVatRate(vatRate21).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build())
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithShippingAndHandlingCharge(adjustment)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build();
            invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

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
            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var adjustment = new ShippingAndHandlingChargeBuilder(this.DatabaseSession).WithPercentage(5).WithVatRate(vatRate21).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build())
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithShippingAndHandlingCharge(adjustment)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build();
            invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

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
            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var adjustment = new FeeBuilder(this.DatabaseSession).WithAmount(7.5M).WithVatRate(vatRate21).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build())
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithFee(adjustment)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build();
            invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

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
            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var adjustment = new FeeBuilder(this.DatabaseSession).WithPercentage(5).WithVatRate(vatRate21).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build())
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithFee(adjustment)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build();
            invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

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
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithShipToCustomer(customer)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.DatabaseSession.Derive(); 
            
            Assert.Equal(1, invoice.Customers.Count);
            Assert.Equal(customer, invoice.Customers.First);
        }

        [Fact]
        public void GivenSalesInvoice_WhenShipToAndBillToAreDifferentCustomers_ThenDerivedCustomersHoldsBothCustomers()
        {
            var billToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var shipToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithShipToCustomer(shipToCustomer)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.ShipToCustomer).Build();

            this.DatabaseSession.Derive(); 
            
            Assert.Equal(2, invoice.Customers.Count);
            Assert.Contains(billToCustomer, invoice.Customers);
            Assert.Contains(shipToCustomer, invoice.Customers);
        }

        [Fact]
        public void GivenSalesInvoice_WhenDerivingSalesReps_ThenSalesRepsAreCollectedFromSalesInvoiceItems()
        {
            var salesrep1 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for child product category").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var salesrep2 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for parent category").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var salesrep3 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for everything else").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var parentProductCategory = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("parent").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .Build();

            var childProductCategory = new ProductCategoryBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("child").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithParent(parentProductCategory).
                Build();

            var billToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithSalesRepresentative(salesrep1)
                .WithCustomer(billToCustomer)
                .WithProductCategory(childProductCategory)
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithSalesRepresentative(salesrep2)
                .WithCustomer(billToCustomer)
                .WithProductCategory(parentProductCategory)
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithSalesRepresentative(salesrep3)
                .WithCustomer(billToCustomer)
                .Build();

            var good1 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithProductCategory(childProductCategory)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithProductCategory(parentProductCategory)
                .Build();

            var good3 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession)
                .WithProduct(good1)
                .WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem)
                .WithQuantity(3)
                .WithActualUnitPrice(5)
                .Build();

            var item2 = new SalesInvoiceItemBuilder(this.DatabaseSession)
                .WithProduct(good2)
                .WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem)
                .WithQuantity(3)
                .WithActualUnitPrice(5)
                .Build();

            var item3 = new SalesInvoiceItemBuilder(this.DatabaseSession)
                .WithProduct(good3)
                .WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem)
                .WithQuantity(3)
                .WithActualUnitPrice(5)
                .Build();

            invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            Assert.Equal(1, invoice.SalesReps.Count);
            Assert.Contains(salesrep1, invoice.SalesReps);

            invoice.AddSalesInvoiceItem(item2);

            this.DatabaseSession.Derive();

            Assert.Equal(2, invoice.SalesReps.Count);
            Assert.Contains(salesrep1, invoice.SalesReps);
            Assert.Contains(salesrep2, invoice.SalesReps);

            invoice.AddSalesInvoiceItem(item3);

            this.DatabaseSession.Derive();

            Assert.Equal(3, invoice.SalesReps.Count);
            Assert.Contains(salesrep1, invoice.SalesReps);
            Assert.Contains(salesrep2, invoice.SalesReps);
            Assert.Contains(salesrep3, invoice.SalesReps);
        }

        [Fact]
        public void GivenSalesInvoice_WhenPartialPaymentIsReceived_ThenInvoiceStateIsSetToPartiallyPaid()
        {
            var billToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(2).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            this.DatabaseSession.Derive();

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(90)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice.SalesInvoiceItems[0]).WithAmountApplied(90).Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesInvoiceStates(this.DatabaseSession).PartiallyPaid, invoice.SalesInvoiceState);
        }

        [Fact]
        public void GiveninvoiceItem_WhenFullPaymentIsReceived_ThenInvoiceItemStateIsSetToPaid()
        {
            var billToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            invoice.AddSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build());

            this.DatabaseSession.Derive();

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(100)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice.InvoiceItems[0]).WithAmountApplied(100).Build())
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesInvoiceStates(this.DatabaseSession).Paid, invoice.SalesInvoiceState);
        }

        [Fact]
        public void GiveninvoiceItem_WhenCancelled_ThenInvoiceItemsAreCancelled()
        {
            var billToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            this.DatabaseSession.Derive();

            invoice.CancelInvoice();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesInvoiceStates(this.DatabaseSession).Cancelled, invoice.SalesInvoiceState);
            Assert.Equal(new SalesInvoiceItemStates(this.DatabaseSession).Cancelled, invoice.SalesInvoiceItems[0].SalesInvoiceItemState);
            Assert.Equal(new SalesInvoiceItemStates(this.DatabaseSession).Cancelled, invoice.SalesInvoiceItems[1].SalesInvoiceItemState);
        }

        [Fact]
        public void GiveninvoiceItem_WhenWrittenOff_ThenInvoiceItemsAreWrittenOff()
        {
            var billToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            invoice.WriteOff();

            this.DatabaseSession.Derive();

            Assert.Equal(new SalesInvoiceStates(this.DatabaseSession).WrittenOff, invoice.SalesInvoiceState);
            Assert.Equal(new SalesInvoiceItemStates(this.DatabaseSession).WrittenOff, invoice.SalesInvoiceItems[0].SalesInvoiceItemState);
            Assert.Equal(new SalesInvoiceItemStates(this.DatabaseSession).WrittenOff, invoice.SalesInvoiceItems[1].SalesInvoiceItemState);
        }

        [Fact]
        public void GivenSalesInvoice_WhenDeriving_ThenRevenuesAreCreatedAndUpdated()
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

            var invoice1 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceDate(DateTime.UtcNow)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer1)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesChannel(new SalesChannels(this.DatabaseSession).WebChannel)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(invoice1.BillToCustomer).Build();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item1);

            var item2 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item2);

            var item3 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantity(5).WithActualUnitPrice(10).WithSalesInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item3);

            this.DatabaseSession.Derive();

            var customer1Revenue = customer1.PartyRevenuesWhereParty[0];
            var internalOrganisationRevenue = new InternalOrganisationRevenues(this.DatabaseSession).Extent()[0];
            var storeRevenue = invoice1.Store.StoreRevenuesWhereStore[0];
            var salesChannelRevenue = invoice1.SalesChannel.SalesChannelRevenuesWhereSalesChannel[0];
            var salesRep1Revenue = salesRep1.SalesRepRevenuesWhereSalesRep[0];
            var salesRep2Revenue = salesRep2.SalesRepRevenuesWhereSalesRep[0];
            var good1Revenue = good1.ProductRevenuesWhereProduct[0];
            var good2Revenue = good2.ProductRevenuesWhereProduct[0];
            var cat1Revenue = cat1.ProductCategoryRevenuesWhereProductCategory[0];
            var cat2Revenue = cat2.ProductCategoryRevenuesWhereProductCategory[0];
            var catMainRevenue = catMain.ProductCategoryRevenuesWhereProductCategory[0];

            var customer1ProductRevenues = customer1.PartyProductRevenuesWhereParty;
            Assert.Equal(2, customer1ProductRevenues.Count);

            customer1ProductRevenues = customer1.PartyProductRevenuesWhereParty;
            customer1ProductRevenues.Filter.AddEquals(M.PartyProductRevenue.Product, good1);
            var customer1Good1Revenue = customer1ProductRevenues.First;

            customer1ProductRevenues = customer1.PartyProductRevenuesWhereParty;
            customer1ProductRevenues.Filter.AddEquals(M.PartyProductRevenue.Product, good2);
            var customer1Good2Revenue = customer1ProductRevenues.First;

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

            var salesRep1Customer1Revenues = salesRep1.SalesRepPartyRevenuesWhereSalesRep;
            salesRep1Customer1Revenues.Filter.AddEquals(M.SalesRepPartyRevenue.Party, customer1);
            var salesRep1Customer1Revenue = salesRep1Customer1Revenues.First;

            var salesRep2Customer1Revenues = salesRep2.SalesRepPartyRevenuesWhereSalesRep;
            salesRep2Customer1Revenues.Filter.AddEquals(M.SalesRepPartyRevenue.Party, customer1);
            var salesRep2Customer1Revenue = salesRep2Customer1Revenues.First;

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

            var salesRep1ProductCategoryRevenues = salesRep1.SalesRepProductCategoryRevenuesWhereSalesRep;
            Assert.Equal(2, salesRep1ProductCategoryRevenues.Count);

            salesRep1ProductCategoryRevenues.Filter.AddEquals(M.SalesRepProductCategoryRevenue.ProductCategory, cat1);
            var salesRep1Cat1Revenue = salesRep1ProductCategoryRevenues.First;

            salesRep1ProductCategoryRevenues = salesRep1.SalesRepProductCategoryRevenuesWhereSalesRep;
            salesRep1ProductCategoryRevenues.Filter.AddEquals(M.SalesRepProductCategoryRevenue.ProductCategory, catMain);
            var salesRep1CatMainRevenue = salesRep1ProductCategoryRevenues.First;

            var salesRep2ProductCategoryRevenues = salesRep2.SalesRepProductCategoryRevenuesWhereSalesRep;
            Assert.Equal(2, salesRep2ProductCategoryRevenues.Count);

            salesRep2ProductCategoryRevenues.Filter.AddEquals(M.SalesRepProductCategoryRevenue.ProductCategory, cat2);
            var salesRep2Cat2Revenue = salesRep2ProductCategoryRevenues.First;

            salesRep2ProductCategoryRevenues = salesRep2.SalesRepProductCategoryRevenuesWhereSalesRep;
            salesRep2ProductCategoryRevenues.Filter.AddEquals(M.SalesRepProductCategoryRevenue.ProductCategory, catMain);
            var salesRep2CatMainRevenue = salesRep2ProductCategoryRevenues.First;

            this.DatabaseSession.Derive();

            Assert.Equal(140, internalOrganisationRevenue.Revenue);
            Assert.Equal(140, storeRevenue.Revenue);
            Assert.Equal(140, salesChannelRevenue.Revenue);
            Assert.Equal(90, salesRep1Revenue.Revenue);
            Assert.Equal(50, salesRep2Revenue.Revenue);
            Assert.Equal(90, salesRep1Customer1Revenue.Revenue);
            Assert.Equal(50, salesRep2Customer1Revenue.Revenue);
            Assert.Equal(90, good1Revenue.Revenue);
            Assert.Equal(50, good2Revenue.Revenue);
            Assert.Equal(90, cat1Revenue.Revenue);
            Assert.Equal(50, cat2Revenue.Revenue);
            Assert.Equal(140, catMainRevenue.Revenue);
            Assert.Equal(90, customer1Cat1Revenue.Revenue);
            Assert.Equal(50, customer1Cat2Revenue.Revenue);
            Assert.Equal(140, customer1CatMainRevenue.Revenue);
            Assert.Equal(90, salesRep1Cat1Revenue.Revenue);
            Assert.Equal(90, salesRep1CatMainRevenue.Revenue);
            Assert.Equal(50, salesRep2Cat2Revenue.Revenue);
            Assert.Equal(50, salesRep2CatMainRevenue.Revenue);
            Assert.Equal(90, salesRep1Customer1Cat1Revenue.Revenue);
            Assert.Equal(90, salesRep1Customer1CatMainRevenue.Revenue);
            Assert.Equal(50, salesRep2Customer1Cat2Revenue.Revenue);
            Assert.Equal(50, salesRep2Customer1CatMainRevenue.Revenue);
            Assert.Equal(140, customer1Revenue.Revenue);
            Assert.Equal(90, customer1Good1Revenue.Revenue);
            Assert.Equal(50, customer1Good2Revenue.Revenue);

            var invoice2 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceDate(DateTime.UtcNow)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer2)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesChannel(new SalesChannels(this.DatabaseSession).WebChannel)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(invoice2.BillToCustomer).Build();

            var item4 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantity(1).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice2.AddSalesInvoiceItem(item4);

            this.DatabaseSession.Derive();

            var item5 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantity(1).WithActualUnitPrice(10).WithSalesInvoiceItemType(productItem).Build();
            invoice2.AddSalesInvoiceItem(item5);

            this.DatabaseSession.Derive();

            var customer2Revenue = customer2.PartyRevenuesWhereParty[0];

            var customer2ProductRevenues = customer2.PartyProductRevenuesWhereParty;
            Assert.Equal(2, customer2ProductRevenues.Count);

            customer2ProductRevenues.Filter.AddEquals(M.PartyProductRevenue.Product, good1);
            var customer2Good1Revenue = customer2ProductRevenues.First;

            customer2ProductRevenues = customer2.PartyProductRevenuesWhereParty;
            customer2ProductRevenues.Filter.AddEquals(M.PartyProductRevenue.Product, good2);
            var customer2Good2Revenue = customer2ProductRevenues.First;

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

            var salesRep1Customer2Revenues = salesRep1.SalesRepPartyRevenuesWhereSalesRep;
            salesRep1Customer2Revenues.Filter.AddEquals(M.SalesRepPartyRevenue.Party, customer2);
            var salesRep1Customer2Revenue = salesRep1Customer2Revenues.First;

            var salesRep2Customer2Revenues = salesRep2.SalesRepPartyRevenuesWhereSalesRep;
            salesRep2Customer2Revenues.Filter.AddEquals(M.SalesRepPartyRevenue.Party, customer2);
            var salesRep2Customer2Revenue = salesRep2Customer2Revenues.First;

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

            Assert.Equal(165, internalOrganisationRevenue.Revenue);
            Assert.Equal(165, storeRevenue.Revenue);
            Assert.Equal(165, salesChannelRevenue.Revenue);
            Assert.Equal(105, salesRep1Revenue.Revenue);
            Assert.Equal(60, salesRep2Revenue.Revenue);
            Assert.Equal(15, salesRep1Customer2Revenue.Revenue);
            Assert.Equal(10, salesRep2Customer2Revenue.Revenue);
            Assert.Equal(105, cat1Revenue.Revenue);
            Assert.Equal(60, cat2Revenue.Revenue);
            Assert.Equal(165, catMainRevenue.Revenue);
            Assert.Equal(15, customer2Cat1Revenue.Revenue);
            Assert.Equal(10, customer2Cat2Revenue.Revenue);
            Assert.Equal(25, customer2CatMainRevenue.Revenue);
            Assert.Equal(105, salesRep1Cat1Revenue.Revenue);
            Assert.Equal(105, salesRep1CatMainRevenue.Revenue);
            Assert.Equal(60, salesRep2Cat2Revenue.Revenue);
            Assert.Equal(60, salesRep2CatMainRevenue.Revenue);
            Assert.Equal(15, salesRep1Customer2Cat1Revenue.Revenue);
            Assert.Equal(15, salesRep1Customer2CatMainRevenue.Revenue);
            Assert.Equal(10, salesRep2Customer2Cat2Revenue.Revenue);
            Assert.Equal(10, salesRep2Customer2CatMainRevenue.Revenue);
            Assert.Equal(105, good1Revenue.Revenue);
            Assert.Equal(60, good2Revenue.Revenue);
            Assert.Equal(25, customer2Revenue.Revenue);
            Assert.Equal(15, customer2Good1Revenue.Revenue);
            Assert.Equal(10, customer2Good2Revenue.Revenue);
        }

        [Fact]
        public void GivenSalesInvoice_WhenWrittenOff_ThenRevenueIsUpdated()
        {
            var euro = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;

            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();

            var invoice1 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceDate(DateTimeFactory.CreateDate(2010, 01, 01))
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesChannel(new SalesChannels(this.DatabaseSession).WebChannel)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build();
            invoice1.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive();

            invoice1.Send();

            this.DatabaseSession.Derive();

            var invoice2 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceDate(DateTimeFactory.CreateDate(2010, 01, 01))
                .WithInvoiceNumber("2")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesChannel(new SalesChannels(this.DatabaseSession).WebChannel)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            var item2 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(15).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build();
            invoice2.AddSalesInvoiceItem(item2);

            this.DatabaseSession.Derive();

            var internalOrganisationRevenue = new InternalOrganisationRevenues(this.DatabaseSession).Extent()[0];
            var storeRevenue = invoice1.Store.StoreRevenuesWhereStore[0];
            var salesChannelRevenue = invoice1.SalesChannel.SalesChannelRevenuesWhereSalesChannel[0];
            var productRevenue = good.ProductRevenuesWhereProduct[0];
            var partyRevenue = customer.PartyRevenuesWhereParty[0];
            var partypPrductRevenue = customer.PartyProductRevenuesWhereParty[0];

            Assert.Equal(60, internalOrganisationRevenue.Revenue);
            Assert.Equal(2010, internalOrganisationRevenue.Year);
            Assert.Equal(60, storeRevenue.Revenue);
            Assert.Equal(2010, storeRevenue.Year);
            Assert.Equal(60, salesChannelRevenue.Revenue);
            Assert.Equal(2010, salesChannelRevenue.Year);
            Assert.Equal(60, productRevenue.Revenue);
            Assert.Equal(2010, productRevenue.Year);
            Assert.Equal(60, partyRevenue.Revenue);
            Assert.Equal(2010, partyRevenue.Year);
            Assert.Equal(60, partypPrductRevenue.Revenue);
            Assert.Equal(2010, partypPrductRevenue.Year);

            invoice2.WriteOff();

            this.DatabaseSession.Derive();

            internalOrganisationRevenue = new InternalOrganisationRevenues(this.DatabaseSession).Extent()[0];
            storeRevenue = invoice1.Store.StoreRevenuesWhereStore[0];
            salesChannelRevenue = invoice1.SalesChannel.SalesChannelRevenuesWhereSalesChannel[0];
            productRevenue = good.ProductRevenuesWhereProduct[0];
            partyRevenue = customer.PartyRevenuesWhereParty[0];
            partypPrductRevenue = customer.PartyProductRevenuesWhereParty[0];

            Assert.Equal(45, internalOrganisationRevenue.Revenue);
            Assert.Equal(2010, internalOrganisationRevenue.Year);
            Assert.Equal(45, storeRevenue.Revenue);
            Assert.Equal(2010, storeRevenue.Year);
            Assert.Equal(45, salesChannelRevenue.Revenue);
            Assert.Equal(2010, salesChannelRevenue.Year);
            Assert.Equal(45, productRevenue.Revenue);
            Assert.Equal(2010, productRevenue.Year);
            Assert.Equal(45, partyRevenue.Revenue);
            Assert.Equal(2010, partyRevenue.Year);
            Assert.Equal(45, partypPrductRevenue.Revenue);
            Assert.Equal(2010, partypPrductRevenue.Year);
        }
    }
}