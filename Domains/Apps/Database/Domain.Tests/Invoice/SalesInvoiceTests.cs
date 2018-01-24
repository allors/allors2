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
            var store = new StoreBuilder(this.Session).WithName("store")
                .WithDefaultFacility(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse))
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .Build();

            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
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

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            Assert.Equal("1", invoice1.InvoiceNumber);

            var invoice2 = new SalesInvoiceBuilder(this.Session)
                .WithStore(store)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            Assert.Equal("2", invoice2.InvoiceNumber);
        }

        [Fact]
        public void GivenSingletonWithInvoiceSequenceFiscalYear_WhenCreatingInvoice_ThenInvoiceNumberFromFiscalYearMustBeUsed()
        {
            var store = new StoreBuilder(this.Session).WithName("store")
                .WithDefaultFacility(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse))
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .Build();

            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
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

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            Assert.False(store.ExistSalesInvoiceCounter);
            Assert.Equal(DateTime.UtcNow.Year, store.FiscalYearInvoiceNumbers.First.FiscalYear);
            Assert.Equal("1", invoice1.InvoiceNumber);

            var invoice2 = new SalesInvoiceBuilder(this.Session)
                .WithStore(store)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            Assert.False(store.ExistSalesInvoiceCounter);
            Assert.Equal(DateTime.UtcNow.Year, store.FiscalYearInvoiceNumbers.First.FiscalYear);
            Assert.Equal("2", invoice2.InvoiceNumber);
        }

        [Fact]
        public void GivenSalesInvoice_WhenGettingInvoiceNumberWithFormat_ThenFormattedInvoiceNumberShouldBeReturned()
        {
            var store = new StoreBuilder(this.Session).WithName("store")
                .WithDefaultFacility(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse))
                .WithSalesInvoiceNumberPrefix("the format is ")
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .Build();

            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
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

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            Assert.Equal("the format is 1", invoice.InvoiceNumber);
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

            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(0).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

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
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem).Build())
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

            var internalOrganisation = this.InternalOrganisation;
            internalOrganisation.BillingAddress = homeAddress;

            this.Session.Derive();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

            this.Session.Derive();

            Assert.Equal(homeAddress, invoice.BilledFromContactMechanism);
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

            var billToContactMechanismMechelen = new PostalAddressBuilder(this.Session).WithAddress1("Mechelen").Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(billToContactMechanismMechelen)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).Build();

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
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(19).Build();

            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var goodPurchasePrice = new ProductPurchasePriceBuilder(this.Session)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            new SupplierOfferingBuilder(this.Session)
                .WithProduct(good)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithProductPurchasePrice(goodPurchasePrice)
                .Build();

            var productItem = new SalesInvoiceItemTypes(this.Session).ProductItem;

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
                .WithSalesInvoiceItemType(productItem)
                .WithQuantity(1)
                .WithActualUnitPrice(8)
                .Build();

            invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(8, invoice.TotalExVat);
            Assert.Equal(1.52M, invoice.TotalVat);
            Assert.Equal(9.52M, invoice.TotalIncVat);

            var item2 = new SalesInvoiceItemBuilder(this.Session)
                .WithProduct(good)
                .WithSalesInvoiceItemType(productItem)
                .WithQuantity(1)
                .WithActualUnitPrice(8)
                .Build();

            var item3 = new SalesInvoiceItemBuilder(this.Session)
                .WithProduct(good)
                .WithSalesInvoiceItemType(productItem)
                .WithQuantity(1)
                .WithActualUnitPrice(8)
                .Build();

            invoice.AddSalesInvoiceItem(item2);
            invoice.AddSalesInvoiceItem(item3);

            this.Session.Derive();

            Assert.Equal(24, invoice.TotalExVat);
            Assert.Equal(4.56M, invoice.TotalVat);
            Assert.Equal(28.56M, invoice.TotalIncVat);
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

            invoice.Send();

            this.Session.Derive();
            this.Session.Commit();

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

            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(0).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.SetIdentity("administrator");

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem).Build())
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

            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(0).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.SetIdentity("administrator");

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem).Build())
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

            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(new OrganisationBuilder(this.Session).WithName("customer").Build())
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithShippingAndHandlingCharge(adjustment)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem).Build();
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

            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(new OrganisationBuilder(this.Session).WithName("customer").Build())
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithShippingAndHandlingCharge(adjustment)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem).Build();
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

            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(new OrganisationBuilder(this.Session).WithName("customer").Build())
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithFee(adjustment)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem).Build();
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

            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(new OrganisationBuilder(this.Session).WithName("customer").Build())
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithFee(adjustment)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem).Build();
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
            var salesrep3 = new PersonBuilder(this.Session).WithLastName("salesrep for everything else").Build();
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
                .WithSku("10101")
                .WithName("good")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(0).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithProductCategory(childProductCategory)
                .Build();

            var good2 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithName("good")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(0).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithProductCategory(parentProductCategory)
                .Build();

            var good3 = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithName("good")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(0).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            var item1 = new SalesInvoiceItemBuilder(this.Session)
                .WithProduct(good1)
                .WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem)
                .WithQuantity(3)
                .WithActualUnitPrice(5)
                .Build();

            var item2 = new SalesInvoiceItemBuilder(this.Session)
                .WithProduct(good2)
                .WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem)
                .WithQuantity(3)
                .WithActualUnitPrice(5)
                .Build();

            var item3 = new SalesInvoiceItemBuilder(this.Session)
                .WithProduct(good3)
                .WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem)
                .WithQuantity(3)
                .WithActualUnitPrice(5)
                .Build();

            invoice.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            Assert.Equal(1, invoice.SalesReps.Count);
            Assert.Contains(salesrep1, invoice.SalesReps);

            invoice.AddSalesInvoiceItem(item2);

            this.Session.Derive();

            Assert.Equal(2, invoice.SalesReps.Count);
            Assert.Contains(salesrep1, invoice.SalesReps);
            Assert.Contains(salesrep2, invoice.SalesReps);

            invoice.AddSalesInvoiceItem(item3);

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

            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(0).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem).Build())
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(2).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem).Build())
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

            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(0).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            invoice.AddSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem).Build());

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

            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(0).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem).Build())
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem).Build())
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

            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(0).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem).Build())
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem).Build())
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice.BillToCustomer).Build();

            invoice.WriteOff();

            this.Session.Derive();

            Assert.Equal(new SalesInvoiceStates(this.Session).WrittenOff, invoice.SalesInvoiceState);
            Assert.Equal(new SalesInvoiceItemStates(this.Session).WrittenOff, invoice.SalesInvoiceItems[0].SalesInvoiceItemState);
            Assert.Equal(new SalesInvoiceItemStates(this.Session).WrittenOff, invoice.SalesInvoiceItems[1].SalesInvoiceItemState);
        }

        [Fact]
        public void GivenSalesInvoice_WhenDeriving_ThenRevenuesAreCreatedAndUpdated()
        {
            var productItem = new SalesInvoiceItemTypes(this.Session).ProductItem;
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

            var invoice1 = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceDate(DateTime.UtcNow)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer1)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesChannel(new SalesChannels(this.Session).WebChannel)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice1.BillToCustomer).Build();

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good1).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item1);

            var item2 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good1).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item2);

            var item3 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good2).WithQuantity(5).WithActualUnitPrice(10).WithSalesInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item3);

            this.Session.Derive();

            var customer1Revenue = customer1.PartyRevenuesWhereParty[0];
            var internalOrganisationRevenue = new InternalOrganisationRevenues(this.Session).Extent()[0];
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

            this.Session.Derive();

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

            var invoice2 = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceDate(DateTime.UtcNow)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer2)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesChannel(new SalesChannels(this.Session).WebChannel)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(invoice2.BillToCustomer).Build();

            var item4 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good1).WithQuantity(1).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice2.AddSalesInvoiceItem(item4);

            this.Session.Derive();

            var item5 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good2).WithQuantity(1).WithActualUnitPrice(10).WithSalesInvoiceItemType(productItem).Build();
            invoice2.AddSalesInvoiceItem(item5);

            this.Session.Derive();

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
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var contactMechanism = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTimeFactory.CreateDate(2009, 01, 01)).WithCustomer(customer).Build();

            var invoice1 = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceDate(DateTimeFactory.CreateDate(2010, 01, 01))
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesChannel(new SalesChannels(this.Session).WebChannel)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem).Build();
            invoice1.AddSalesInvoiceItem(item1);

            this.Session.Derive();

            invoice1.Send();

            this.Session.Derive();

            var invoice2 = new SalesInvoiceBuilder(this.Session)
                .WithInvoiceDate(DateTimeFactory.CreateDate(2010, 01, 01))
                .WithInvoiceNumber("2")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesChannel(new SalesChannels(this.Session).WebChannel)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .Build();

            var item2 = new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithActualUnitPrice(15).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem).Build();
            invoice2.AddSalesInvoiceItem(item2);

            this.Session.Derive();

            var internalOrganisationRevenue = new InternalOrganisationRevenues(this.Session).Extent()[0];
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

            this.Session.Derive();

            internalOrganisationRevenue = new InternalOrganisationRevenues(this.Session).Extent()[0];
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