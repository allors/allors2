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
    using System.Security.Principal;
    using System.Threading;
    using NUnit.Framework;

    using Resources;

    [TestFixture]
    public class SalesInvoiceTests : DomainTest
    {
        [Test]
        public void GivenSalesInvoice_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesInvoiceObjectStates(this.DatabaseSession).ReadyForPosting, invoice.CurrentObjectState);
            Assert.AreEqual(invoice.LastObjectState, invoice.CurrentObjectState);
        }

        [Test]
        public void GivenSalesInvoice_WhenBuild_ThenPreviousObjectStateIsNull()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true);

            Assert.IsNull(invoice.PreviousObjectState);
        }

        [Test]
        public void GivenSalesInvoice_WhenPaid_ThenCurrentInvoiceStatusMustBeDerived()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, invoice.InvoiceStatuses.Count);
            Assert.AreEqual(new SalesInvoiceObjectStates(this.DatabaseSession).ReadyForPosting, invoice.CurrentInvoiceStatus.SalesInvoiceObjectState);

            this.DatabaseSession.Derive(true);

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(100)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice.SalesInvoiceItems[0]).WithAmountApplied(100).Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, invoice.InvoiceStatuses.Count);
            Assert.AreEqual(new SalesInvoiceObjectStates(this.DatabaseSession).Paid, invoice.CurrentInvoiceStatus.SalesInvoiceObjectState);
        }

        [Test]
        public void GivenSalesInvoice_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            ContactMechanism billToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation)
                .Build();

            this.DatabaseSession.Commit();

            var builder = new SalesInvoiceBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithBillToCustomer(customer);
            builder.Build();
            
            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithBillToContactMechanism(billToContactMechanism);
            var invoice = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            Assert.AreEqual(invoice.CurrentInvoiceStatus.SalesInvoiceObjectState, new SalesInvoiceObjectStates(this.DatabaseSession).ReadyForPosting);
            Assert.AreEqual(invoice.CurrentObjectState, new SalesInvoiceObjectStates(this.DatabaseSession).ReadyForPosting);
            Assert.AreEqual(invoice.CurrentObjectState, invoice.LastObjectState);

            builder.WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"));
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenSalesInvoice_WhenDeriving_ThenBillToCustomerMustBeInternalOrganisationCustomer()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            var expectedError = ErrorMessages.PartyIsNotACustomer;
            Assert.AreEqual(expectedError, this.DatabaseSession.Derive().Errors[0].Message);

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenSalesInvoice_WhenDeriving_ThenShipToCustomerMustBeInternalOrganisationCustomer()
        {
            var billtoCcustomer = new OrganisationBuilder(this.DatabaseSession).WithName("billToCustomer").Build();
            var shipToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("shipToCustomer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(billtoCcustomer)
                .WithShipToCustomer(shipToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(billtoCcustomer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            var expectedError = ErrorMessages.PartyIsNotACustomer;
            Assert.AreEqual(expectedError, this.DatabaseSession.Derive().Errors[0].Message);

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(shipToCustomer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenSalesInvoice_WhenGettingInvoiceNumberWithoutFormat_ThenInvoiceNumberShouldBeReturned()
        {
            var store = new StoreBuilder(this.DatabaseSession).WithName("store")
                .WithDefaultFacility(new Warehouses(this.DatabaseSession).FindBy(Warehouses.Meta.Name, "facility"))
                .WithOwner(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .Build();

            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice1 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithStore(store)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            Assert.AreEqual("1", invoice1.InvoiceNumber);

            var invoice2 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithStore(store)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            Assert.AreEqual("2", invoice2.InvoiceNumber);
        }

        [Test]
        public void GivenInternalOrganisationWithInvoiceSequenceFiscalYear_WhenCreatingInvoice_ThenInvoiceNumberFromFiscalYearMustBeUsed()
        {
            var store = new StoreBuilder(this.DatabaseSession).WithName("store")
                .WithDefaultFacility(new Warehouses(this.DatabaseSession).FindBy(Warehouses.Meta.Name, "facility"))
                .WithOwner(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .Build();

            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice1 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithStore(store)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            Assert.IsFalse(store.ExistSalesInvoiceCounter);
            Assert.AreEqual(DateTime.UtcNow.Year, store.FiscalYearInvoiceNumbers.First.FiscalYear);
            Assert.AreEqual("1", invoice1.InvoiceNumber);

            var invoice2 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithStore(store)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            Assert.IsFalse(store.ExistSalesInvoiceCounter);
            Assert.AreEqual(DateTime.UtcNow.Year, store.FiscalYearInvoiceNumbers.First.FiscalYear);
            Assert.AreEqual("2", invoice2.InvoiceNumber);
        }

        [Test]
        public void GivenSalesInvoice_WhenGettingInvoiceNumberWithFormat_ThenFormattedInvoiceNumberShouldBeReturned()
        {
            var store = new StoreBuilder(this.DatabaseSession).WithName("store")
                .WithDefaultFacility(new Warehouses(this.DatabaseSession).FindBy(Warehouses.Meta.Name, "facility"))
                .WithOwner(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithSalesInvoiceNumberPrefix("the format is ")
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .Build();

            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithStore(store)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            Assert.AreEqual("the format is 1", invoice.InvoiceNumber);
        }

        [Test]
        public void GivenSalesInvoice_WhenDeriving_ThenDerivedSalesRepMustExist()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var salesrep = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .WithInternalOrganisation(internalOrganisation)
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

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true); 
            
            Assert.Contains(salesrep, invoice.SalesReps);
        }

        [Test]
        public void GivenSalesInvoice_WhenDeriving_ThenBilledFromContactMechanismMustExist()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var homeAddress = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Sint-Lambertuslaan 78")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Muizen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var home = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(homeAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            internalOrganisation.RemovePartyContactMechanisms();
            internalOrganisation.AddPartyContactMechanism(home);

            this.DatabaseSession.Derive(true);

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(internalOrganisation)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(homeAddress, invoice.BilledFromContactMechanism);
        }

        [Test]
        public void GivenSalesInvoiceWithBillToCustomerWithBillingAsdress_WhenDeriving_ThendBillToContactMechanismMustExist()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            ContactMechanism billToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithAddress1("Haverwerf 15").WithGeographicBoundary(mechelen).Build();

            var billingAddress = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(billToContactMechanism)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            customer.AddPartyContactMechanism(billingAddress);
            this.DatabaseSession.Derive(true);

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession).WithBillToCustomer(customer).Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true); 

            Assert.AreEqual(billingAddress.ContactMechanism, invoice.BillToContactMechanism);
        }

        [Test]
        public void GivenSalesInvoiceBuilderWithBillToCustomerWithPreferredCurrency_WhenBuilding_ThenDerivedCurrencyIsCustomersPreferredCurrency()
        {
            var euro = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");

            var customer = new OrganisationBuilder(this.DatabaseSession)
                .WithName("customer")
                .WithPreferredCurrency(euro)
                .Build();

            var billToContactMechanismMechelen = new PostalAddressBuilder(this.DatabaseSession).WithAddress1("Mechelen").Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(billToContactMechanismMechelen)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            Assert.AreEqual(euro, invoice.CustomerCurrency);
        }

        [Test]
        public void GivenSalesInvoiceWithShipToCustomerWithShippingAddress_WhenDeriving_ThenShipToAddressMustExist()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            ContactMechanism shipToContactMechanism = new PostalAddressBuilder(this.DatabaseSession).WithAddress1("Haverwerf 15").WithGeographicBoundary(mechelen).Build();

            var shippingAddress = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(shipToContactMechanism)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            customer.AddPartyContactMechanism(shippingAddress);

            this.DatabaseSession.Derive(true);

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithShipToCustomer(customer)
                .WithBillToContactMechanism(shipToContactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(shippingAddress.ContactMechanism, invoice.ShipToAddress);
        }

        [Test]
        public void GivenSalesInvoiceBuilderWithoutBillToCustomer_WhenBuilding_ThenDerivedCurrencyIsInternalOrganisationsPreferredCurrency()
        {
            var euro = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            internalOrganisation.PreferredCurrency = euro;
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(internalOrganisation)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            Assert.AreEqual(euro, invoice.CustomerCurrency);
        }

        [Test]
        public void GivenSalesInvoice_WhenDeriving_ThenLocaleMustExist()
        {
            var englischLocale = new Locales(this.DatabaseSession).EnglishGreatBritain;
            var dutchLocale = new Locales(this.DatabaseSession).DutchNetherlands;

            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice1 = new SalesInvoiceBuilder(this.DatabaseSession).WithBillToCustomer(customer).WithBillToContactMechanism(contactMechanism).Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(englischLocale, invoice1.Locale);

            customer.Locale = dutchLocale;

            var invoice2 = new SalesInvoiceBuilder(this.DatabaseSession).WithBillToCustomer(customer).WithBillToContactMechanism(contactMechanism).Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(dutchLocale, invoice2.Locale);
        }

        [Test]
        public void GivenSalesInvoice_WhenDeriving_ThenTotalAmountMustBeDerived()
        {
            var euro = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(19).Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
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

            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession).WithBillToCustomer(customer).WithBillToContactMechanism(contactMechanism).Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithSalesInvoiceItemType(productItem)
                .WithQuantity(1)
                .WithActualUnitPrice(8)
                .Build();

            invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(8, invoice.TotalExVat);
            Assert.AreEqual(1.52M, invoice.TotalVat);
            Assert.AreEqual(9.52M, invoice.TotalIncVat);

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

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(24, invoice.TotalExVat);
            Assert.AreEqual(4.56M, invoice.TotalVat);
            Assert.AreEqual(28.56M, invoice.TotalIncVat);
            Assert.AreEqual(21, invoice.TotalPurchasePrice);
            Assert.AreEqual(invoice.TotalListPrice, invoice.TotalExVat);
        }

        [Test]
        public void GivenSalesInvoice_WhenObjectStateIsReadyForPosting_ThenCheckTransitions()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var administrator = new PersonBuilder(this.DatabaseSession).WithLastName("Administrator").WithUserName("administrator").Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();
            
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("administrator", "Forms"), new string[0]);

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsTrue(acl.CanExecute(SalesInvoices.Meta.Send));
            Assert.IsTrue(acl.CanExecute(SalesInvoices.Meta.WriteOff));
            Assert.IsTrue(acl.CanExecute(SalesInvoices.Meta.CancelInvoice));
        }

        [Test]
        public void GivenSalesInvoice_WhenObjectStateIsSent_ThenCheckTransitions()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("administrator").Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("administrator", "Forms"), new string[0]);

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            invoice.Send();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsFalse(acl.CanExecute(SalesInvoices.Meta.Send));
            Assert.IsTrue(acl.CanExecute(SalesInvoices.Meta.WriteOff));
            Assert.IsFalse(acl.CanExecute(SalesInvoices.Meta.CancelInvoice));
        }

        ////[Test]
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
        ////    Assert.IsFalse(acl.CanExecute(Invoices.ReadyForPostingId));
        ////    Assert.IsFalse(acl.CanExecute(Invoices.ApproveId));
        ////    Assert.IsFalse(acl.CanExecute(Invoices.SendId));
        ////    Assert.IsFalse(acl.CanExecute(Invoices.WriteOffId));
        ////    Assert.IsFalse(acl.CanExecute(Invoices.CancelId));
        ////}

        [Test]
        public void GivenSalesInvoice_WhenObjectStateIsPaid_ThenCheckTransitions()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("administrator").Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("administrator", "Forms"), new string[0]);

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true);

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(100)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice.SalesInvoiceItems[0]).WithAmountApplied(100).Build())
                .Build();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsFalse(acl.CanExecute(SalesInvoices.Meta.Send));
            Assert.IsFalse(acl.CanExecute(SalesInvoices.Meta.WriteOff));
            Assert.IsFalse(acl.CanExecute(SalesInvoices.Meta.CancelInvoice));
        }

        [Test]
        public void GivenSalesInvoice_WhenObjectStateIsPartiallyPaid_ThenCheckTransitions()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("administrator").Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("administrator", "Forms"), new string[0]);

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true);

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(90)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice.SalesInvoiceItems[0]).WithAmountApplied(90).Build())
                .Build();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsFalse(acl.CanExecute(SalesInvoices.Meta.Send));
            Assert.IsTrue(acl.CanExecute(SalesInvoices.Meta.WriteOff));
            Assert.IsFalse(acl.CanExecute(SalesInvoices.Meta.CancelInvoice));
        }

        [Test]
        public void GivenSalesInvoice_WhenObjectStateIsWrittenOff_ThenCheckTransitions()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("administrator").Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("administrator", "Forms"), new string[0]);

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            invoice.Send();
            invoice.WriteOff();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.IsFalse(acl.CanExecute(SalesInvoices.Meta.Send));
            Assert.IsFalse(acl.CanExecute(SalesInvoices.Meta.WriteOff));
            Assert.IsFalse(acl.CanExecute(SalesInvoices.Meta.CancelInvoice));
        }

        [Test]
        public void GivenSalesInvoice_WhenObjectStateIsCancelled_ThenCheckTransitions()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("administrator").Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("administrator", "Forms"), new string[0]);

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            invoice.CancelInvoice();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).GetCurrentUser());
            Assert.AreEqual(new SalesInvoiceObjectStates(this.DatabaseSession).Cancelled, invoice.CurrentObjectState);
            Assert.IsFalse(acl.CanExecute(SalesInvoices.Meta.Send));
            Assert.IsFalse(acl.CanExecute(SalesInvoices.Meta.WriteOff));
            Assert.IsFalse(acl.CanExecute(SalesInvoices.Meta.CancelInvoice));
        }

        [Test]
        public void GivenSalesInvoiceCreatedByEmloyee_WhenCurrentUserIsCustomerContactForThisInvoice_ThenReadAccessIsGranted()
        {
            var customer = new Organisations(this.DatabaseSession).FindBy(Organisations.Meta.Name, "customer");
            var customerContact = new PersonBuilder(this.DatabaseSession).WithUserName("customercontact").WithLastName("customercontact").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            new OrganisationContactRelationshipBuilder(this.DatabaseSession).WithContact(customerContact).WithOrganisation(customer).WithFromDate(DateTime.UtcNow).Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("salesRep", "Forms"), new string[0]);
            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact", "Forms"), new string[0]);
            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsFalse(acl.CanWrite(SalesInvoices.Meta.InvoiceDate));
            Assert.IsTrue(acl.CanRead(SalesInvoices.Meta.InvoiceDate));
            Assert.IsTrue(acl.CanRead(SalesInvoices.Meta.InvoiceNumber));
            Assert.IsTrue(acl.CanRead(SalesInvoices.Meta.TotalExVat));
            Assert.IsFalse(acl.CanExecute(SalesInvoices.Meta.Send));
        }

        [Test]
        public void GivenSalesInvoiceCreatedByEmloyee_WhenCurrentUserIsCustomerContactForDifferentCustomer_ThenAccessIsDenied()
        {
            var customer = new Organisations(this.DatabaseSession).FindBy(Organisations.Meta.Name, "customer");
            var customerContact = new PersonBuilder(this.DatabaseSession).WithUserName("customercontact").WithLastName("customercontact").Build();
            var customerContact2 = new PersonBuilder(this.DatabaseSession).WithUserName("customercontact2").WithLastName("customercontact2").Build();
            var customer2 = new OrganisationBuilder(this.DatabaseSession).WithName("customer2").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer2)
                .WithInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new OrganisationContactRelationshipBuilder(this.DatabaseSession).WithContact(customerContact).WithOrganisation(customer).WithFromDate(DateTime.UtcNow).Build();
            new OrganisationContactRelationshipBuilder(this.DatabaseSession).WithContact(customerContact2).WithOrganisation(customer2).WithFromDate(DateTime.UtcNow).Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("salesRep", "Forms"), new string[0]);

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            this.DatabaseSession.Derive(true);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact2", "Forms"), new string[0]);
            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsFalse(acl.HasReadOperation);
        }

        [Test]
        public void GivenSalesInvoiceCreatedBySalesRep_WhenCurrentUserInAdministratorRole_ThenAccessIsGranted()
        {
            var customer = new Organisations(this.DatabaseSession).FindBy(Organisations.Meta.Name, "customer");
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var administrator = new PersonBuilder(this.DatabaseSession).WithFirstName("Koen").WithUserName("admin").Build();
            var administrators = new UserGroups(this.DatabaseSession).Administrators;
            administrators.AddMember(administrator);

            this.DatabaseSession.Derive(true);

            var supplierContact = new PersonBuilder(this.DatabaseSession).WithUserName("suppliercontact").WithLastName("suppliercontact").Build();
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();

            new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithSupplier(supplier)
                .WithInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new OrganisationContactRelationshipBuilder(this.DatabaseSession).WithContact(supplierContact).WithOrganisation(supplier).WithFromDate(DateTime.UtcNow).Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("salesRep", "Forms"), new string[0]);

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("admin", "Forms"), new string[0]);
            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(SalesInvoices.Meta.Comment));
            Assert.IsTrue(acl.CanRead(SalesInvoices.Meta.Comment));
            Assert.IsTrue(acl.CanExecute(SalesInvoices.Meta.Send));
        }

        [Test]
        public void GivenSalesInvoiceCreatedBySalesRep_WhenCurrentUserIsCustomer_ThenAccessIsGranted()
        {
            var customer = new Organisations(this.DatabaseSession).FindBy(Organisations.Meta.Name, "customer");
            var customerContact = new PersonBuilder(this.DatabaseSession).WithLastName("customercontact").WithUserName("customercontact").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            new OrganisationContactRelationshipBuilder(this.DatabaseSession).WithContact(customerContact).WithOrganisation(customer).WithFromDate(DateTime.UtcNow).Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("salesRep", "Forms"), new string[0]);

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact", "Forms"), new string[0]);
            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.HasReadOperation);
        }

        [Test]
        public void GivenSalesInvoiceCreatedBySalesRep_WhenCurrentUserIsSupplier_ThenAccessIsDenied()
        {
            var customer = new Organisations(this.DatabaseSession).FindBy(Organisations.Meta.Name, "customer");
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var supplierContact = new PersonBuilder(this.DatabaseSession).WithUserName("suppliercontact").WithLastName("suppliercontact").Build();
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();

            new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithSupplier(supplier)
                .WithInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new OrganisationContactRelationshipBuilder(this.DatabaseSession).WithContact(supplierContact).WithOrganisation(supplier).WithFromDate(DateTime.UtcNow).Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("salesRep", "Forms"), new string[0]);

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("suppliercontact", "Forms"), new string[0]);
            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsFalse(acl.HasReadOperation);
        }

        [Test]
        public void GivenSalesInvoiceCreatedBySalesRep_WhenCurrentUserInSameSalesRepUserGroup_ThenAccessIsGranted()
        {
            var customer = new Organisations(this.DatabaseSession).FindBy(Organisations.Meta.Name, "customer");
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var salesRep2 = new PersonBuilder(this.DatabaseSession).WithUserName("salesRep2").WithLastName("salesRep2").Build();

            new EmploymentBuilder(this.DatabaseSession)
                .WithEmployee(salesRep2)
                .WithEmployer(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithSalesRepresentative(salesRep2)
                .WithCustomer(customer)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("salesRep", "Forms"), new string[0]);

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(SalesInvoices.Meta.Comment));
            Assert.IsTrue(acl.CanRead(SalesInvoices.Meta.Comment));
            Assert.IsTrue(acl.CanExecute(SalesInvoices.Meta.Send));

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("salesRep2", "Forms"), new string[0]);
            acl = new AccessControlList(invoice, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(SalesInvoices.Meta.Comment));
            Assert.IsTrue(acl.CanRead(SalesInvoices.Meta.Comment));
            Assert.IsTrue(acl.CanExecute(SalesInvoices.Meta.Send));
        }

        [Test]
        public void GivenSalesInvoiceCreatedBySalesRep_WhenCurrentUserInAnotherSalesRepUserGroup_ThenAccessIsDenied()
        {
            var salesRep2 = new PersonBuilder(this.DatabaseSession).WithUserName("salesRep2").WithLastName("salesRep2").Build();
            var customer2 = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var belgium = new Countries(this.DatabaseSession).CountryByIsoCode["BE"];
            var euro = belgium.Currency;

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var mechelenAddress = new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();

            var billToMechelen = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(mechelenAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("BE23 3300 6167 6391")
                .WithBankAccount(new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            var internalOrganisation2 = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("internalOrganisation2")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Sales)
                .WithDefaultPaymentMethod(ownBankAccount)
                .WithPreferredCurrency(euro)
                .WithPartyContactMechanism(billToMechelen)
                .Build();

            var facility = new WarehouseBuilder(this.DatabaseSession).WithName("facility").WithOwner(internalOrganisation2).Build();
            internalOrganisation2.DefaultFacility = facility;

            new StoreBuilder(this.DatabaseSession)
                .WithName("store")
                .WithDefaultFacility(facility)
                .WithOwner(internalOrganisation2)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .WithSalesInvoiceTemplate(new StringTemplates(this.DatabaseSession).FindBy(UniquelyIdentifiables.Meta.UniqueId, SalesInvoices.SalesInvoiceTemplateEnId))
                .WithSalesInvoiceTemplate(new StringTemplates(this.DatabaseSession).FindBy(UniquelyIdentifiables.Meta.UniqueId, SalesInvoices.SalesInvoiceTemplateNlId))
                .WithSalesOrderTemplate(new StringTemplates(this.DatabaseSession).FindBy(UniquelyIdentifiables.Meta.UniqueId, SalesOrders.SalesOrderTemplateEnId))
                .WithSalesOrderTemplate(new StringTemplates(this.DatabaseSession).FindBy(UniquelyIdentifiables.Meta.UniqueId, SalesOrders.SalesOrderTemplateNlId))
                .WithCreditLimit(500)
                .WithPaymentGracePeriod(10)
                .Build();

            new EmploymentBuilder(this.DatabaseSession).WithEmployee(salesRep2).WithEmployer(internalOrganisation2).WithFromDate(DateTime.UtcNow).Build();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer2)
                .WithInternalOrganisation(internalOrganisation2)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithSalesRepresentative(salesRep2)
                .WithCustomer(customer2)
                .WithInternalOrganisation(internalOrganisation2)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("salesRep2", "Forms"), new string[0]);

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBilledFromInternalOrganisation(internalOrganisation2)
                .WithBillToCustomer(customer2)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            this.DatabaseSession.Derive(true);

            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsTrue(acl.CanWrite(SalesInvoices.Meta.Comment));
            Assert.IsTrue(acl.CanRead(SalesInvoices.Meta.Comment));
            Assert.IsTrue(acl.CanExecute(SalesInvoices.Meta.Send));

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("salesRep", "Forms"), new string[0]);
            acl = new AccessControlList(invoice, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsFalse(acl.HasReadOperation);
        }

        [Test]
        public void GivenSalesInvoice_WhenBillToCustomerChangesValue_ThenAccessPreviousCustomerIsDenied()
        {
            var customer = new Organisations(this.DatabaseSession).FindBy(Organisations.Meta.Name, "customer");
            var customer2 = new OrganisationBuilder(this.DatabaseSession).WithName("customer2").Build();
            var customerContact = new PersonBuilder(this.DatabaseSession).WithUserName("customercontact").WithLastName("customercontact").Build();
            var customerContact2 = new PersonBuilder(this.DatabaseSession).WithUserName("customercontact2").WithLastName("customercontact2").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer2)
                .WithInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .WithFromDate(DateTime.UtcNow.Date)
                .Build();

            new OrganisationContactRelationshipBuilder(this.DatabaseSession).WithContact(customerContact).WithOrganisation(customer).WithFromDate(DateTime.UtcNow).Build();
            new OrganisationContactRelationshipBuilder(this.DatabaseSession).WithContact(customerContact2).WithOrganisation(customer2).WithFromDate(DateTime.UtcNow).Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("salesRep", "Forms"), new string[0]);
            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithBilledFromInternalOrganisation(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"))
                .Build();

            this.DatabaseSession.Derive(true);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact", "Forms"), new string[0]);
            var acl = new AccessControlList(invoice, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsFalse(acl.CanWrite(SalesInvoices.Meta.InvoiceDate));
            Assert.IsTrue(acl.CanRead(SalesInvoices.Meta.InvoiceDate));
            Assert.IsTrue(acl.CanRead(SalesInvoices.Meta.InvoiceNumber));
            Assert.IsTrue(acl.CanRead(SalesInvoices.Meta.TotalExVat));
            Assert.IsFalse(acl.CanExecute(SalesInvoices.Meta.Send));

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact2", "Forms"), new string[0]);
            acl = new AccessControlList(invoice, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsFalse(acl.HasReadOperation);

            invoice.BillToCustomer = customer2;

            this.DatabaseSession.Derive(true);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact", "Forms"), new string[0]);
            acl = new AccessControlList(invoice, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsFalse(acl.HasReadOperation);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("customercontact2", "Forms"), new string[0]);
            acl = new AccessControlList(invoice, new Users(this.DatabaseSession).GetCurrentUser());

            Assert.IsFalse(acl.CanWrite(SalesInvoices.Meta.InvoiceDate));
            Assert.IsTrue(acl.CanRead(SalesInvoices.Meta.InvoiceDate));
            Assert.IsTrue(acl.CanRead(SalesInvoices.Meta.InvoiceNumber));
            Assert.IsTrue(acl.CanRead(SalesInvoices.Meta.TotalExVat));
            Assert.IsFalse(acl.CanExecute(SalesInvoices.Meta.Send));
        }

        [Test]
        public void GivenSalesInvoiceWithShippingAndHandlingAmount_WhenDeriving_ThenInvoiceTotalsMustIncludeShippingAndHandlingAmount()
        {
            var euro = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var adjustment = new ShippingAndHandlingChargeBuilder(this.DatabaseSession).WithAmount(7.5M).WithVatRate(vatRate21).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            internalOrganisation.PreferredCurrency = euro;

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build())
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithShippingAndHandlingCharge(adjustment)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(invoice.BillToCustomer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build();
            invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(45, invoice.TotalBasePrice);
            Assert.AreEqual(0, invoice.TotalDiscount);
            Assert.AreEqual(0, invoice.TotalSurcharge);
            Assert.AreEqual(7.5, invoice.TotalShippingAndHandling);
            Assert.AreEqual(0, invoice.TotalFee);
            Assert.AreEqual(52.5, invoice.TotalExVat);
            Assert.AreEqual(11.03, invoice.TotalVat);
            Assert.AreEqual(63.53, invoice.TotalIncVat);
        }

        [Test]
        public void GivenSalesInvoiceWithShippingAndHandlingPercentage_WhenDeriving_ThenSalesInvoiceTotalsMustIncludeShippingAndHandlingAmount()
        {
            var euro = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var adjustment = new ShippingAndHandlingChargeBuilder(this.DatabaseSession).WithPercentage(5).WithVatRate(vatRate21).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            internalOrganisation.PreferredCurrency = euro;

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build())
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithShippingAndHandlingCharge(adjustment)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(invoice.BillToCustomer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build();
            invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(45, invoice.TotalBasePrice);
            Assert.AreEqual(0, invoice.TotalDiscount);
            Assert.AreEqual(0, invoice.TotalSurcharge);
            Assert.AreEqual(2.25, invoice.TotalShippingAndHandling);
            Assert.AreEqual(0, invoice.TotalFee);
            Assert.AreEqual(47.25, invoice.TotalExVat);
            Assert.AreEqual(9.92, invoice.TotalVat);
            Assert.AreEqual(57.17, invoice.TotalIncVat);
        }

        [Test]
        public void GivenSalesInvoiceWithFeeAmount_WhenDeriving_ThenSalesInvoiceTotalsMustIncludeFeeAmount()
        {
            var euro = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var adjustment = new FeeBuilder(this.DatabaseSession).WithAmount(7.5M).WithVatRate(vatRate21).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            internalOrganisation.PreferredCurrency = euro;

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build())
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithFee(adjustment)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(invoice.BillToCustomer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build();
            invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(45, invoice.TotalBasePrice);
            Assert.AreEqual(0, invoice.TotalDiscount);
            Assert.AreEqual(0, invoice.TotalSurcharge);
            Assert.AreEqual(0, invoice.TotalShippingAndHandling);
            Assert.AreEqual(7.5, invoice.TotalFee);
            Assert.AreEqual(52.5, invoice.TotalExVat);
            Assert.AreEqual(11.03, invoice.TotalVat);
            Assert.AreEqual(63.53, invoice.TotalIncVat);
        }

        [Test]
        public void GivenSalesInvoiceWithFeePercentage_WhenDeriving_ThenSalesInvoiceTotalsMustIncludeFeeAmount()
        {
            var euro = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var adjustment = new FeeBuilder(this.DatabaseSession).WithPercentage(5).WithVatRate(vatRate21).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            internalOrganisation.PreferredCurrency = euro;

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build())
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithFee(adjustment)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(invoice.BillToCustomer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build();
            invoice.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(45, invoice.TotalBasePrice);
            Assert.AreEqual(0, invoice.TotalDiscount);
            Assert.AreEqual(0, invoice.TotalSurcharge);
            Assert.AreEqual(0, invoice.TotalShippingAndHandling);
            Assert.AreEqual(2.25, invoice.TotalFee);
            Assert.AreEqual(47.25, invoice.TotalExVat);
            Assert.AreEqual(9.92, invoice.TotalVat);
            Assert.AreEqual(57.17, invoice.TotalIncVat);
        }

        [Test]
        public void GivenSalesInvoice_WhenShipToAndBillToAreSameCustomer_ThenDerivedCustomersIsSingleCustomer()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithShipToCustomer(customer)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true); 
            
            Assert.AreEqual(1, invoice.Customers.Count);
            Assert.AreEqual(customer, invoice.Customers.First);
        }

        [Test]
        public void GivenSalesInvoice_WhenShipToAndBillToAreDifferentCustomers_ThenDerivedCustomersHoldsBothCustomers()
        {
            var billToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var shipToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithShipToCustomer(shipToCustomer)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(invoice.BillToCustomer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(invoice.ShipToCustomer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true); 
            
            Assert.AreEqual(2, invoice.Customers.Count);
            Assert.Contains(billToCustomer, invoice.Customers);
            Assert.Contains(shipToCustomer, invoice.Customers);
        }

        [Test]
        public void GivenSalesInvoice_WhenDerivingSalesReps_ThenSalesRepsAreCollectedFromSalesInvoiceItems()
        {
            var salesrep1 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for child product category").Build();
            var salesrep2 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for parent category").Build();
            var salesrep3 = new PersonBuilder(this.DatabaseSession).WithLastName("salesrep for everything else").Build();
            var parentProductCategory = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("parent").Build();
            var childProductCategory = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("child").WithParent(parentProductCategory).Build();

            var billToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
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
                .WithName("good")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithProductCategory(childProductCategory)
                .Build();

            var good2 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .WithProductCategory(parentProductCategory)
                .Build();

            var good3 = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithName("good")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(invoice.BillToCustomer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

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

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, invoice.SalesReps.Count);
            Assert.Contains(salesrep1, invoice.SalesReps);

            invoice.AddSalesInvoiceItem(item2);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, invoice.SalesReps.Count);
            Assert.Contains(salesrep1, invoice.SalesReps);
            Assert.Contains(salesrep2, invoice.SalesReps);

            invoice.AddSalesInvoiceItem(item3);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(3, invoice.SalesReps.Count);
            Assert.Contains(salesrep1, invoice.SalesReps);
            Assert.Contains(salesrep2, invoice.SalesReps);
            Assert.Contains(salesrep3, invoice.SalesReps);
        }

        [Test]
        public void GivenSalesInvoice_WhenPartialPaymentIsReceived_ThenInvoiceStateIsSetToPartiallyPaid()
        {
            var billToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(2).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(invoice.BillToCustomer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true);

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(90)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice.SalesInvoiceItems[0]).WithAmountApplied(90).Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesInvoiceObjectStates(this.DatabaseSession).PartiallyPaid, invoice.CurrentObjectState);
        }

        [Test]
        public void GiveninvoiceItem_WhenFullPaymentIsReceived_ThenInvoiceItemStateIsSetToPaid()
        {
            var billToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(invoice.BillToCustomer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            invoice.AddSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build());

            this.DatabaseSession.Derive(true);

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(100)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice.InvoiceItems[0]).WithAmountApplied(100).Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesInvoiceObjectStates(this.DatabaseSession).Paid, invoice.CurrentObjectState);
        }

        [Test]
        public void GiveninvoiceItem_WhenCancelled_ThenInvoiceItemsAreCancelled()
        {
            var billToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(invoice.BillToCustomer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            this.DatabaseSession.Derive(true);

            invoice.CancelInvoice();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesInvoiceObjectStates(this.DatabaseSession).Cancelled, invoice.CurrentObjectState);
            Assert.AreEqual(new SalesInvoiceItemObjectStates(this.DatabaseSession).Cancelled, invoice.SalesInvoiceItems[0].CurrentObjectState);
            Assert.AreEqual(new SalesInvoiceItemObjectStates(this.DatabaseSession).Cancelled, invoice.SalesInvoiceItems[1].CurrentObjectState);
        }

        [Test]
        public void GiveninvoiceItem_WhenWrittenOff_ThenInvoiceItemsAreWrittenOff()
        {
            var billToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(invoice.BillToCustomer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            invoice.WriteOff();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new SalesInvoiceObjectStates(this.DatabaseSession).WrittenOff, invoice.CurrentObjectState);
            Assert.AreEqual(new SalesInvoiceItemObjectStates(this.DatabaseSession).WrittenOff, invoice.SalesInvoiceItems[0].CurrentObjectState);
            Assert.AreEqual(new SalesInvoiceItemObjectStates(this.DatabaseSession).WrittenOff, invoice.SalesInvoiceItems[1].CurrentObjectState);
        }

        [Test]
        public void GivenSalesInvoice_WhenDeriving_ThenRevenuesAreCreatedAndUpdated()
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

            var invoice1 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceDate(DateTime.UtcNow)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer1)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesChannel(new SalesChannels(this.DatabaseSession).WebChannel)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(invoice1.BillToCustomer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item1);

            var item2 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item2);

            var item3 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantity(5).WithActualUnitPrice(10).WithSalesInvoiceItemType(productItem).Build();
            invoice1.AddSalesInvoiceItem(item3);

            this.DatabaseSession.Derive(true);

            var customer1Revenue = customer1.PartyRevenuesWhereParty[0];
            var internalOrganisationRevenue = internalOrganisation.InternalOrganisationRevenuesWhereInternalOrganisation[0];
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
            Assert.AreEqual(2, customer1ProductRevenues.Count);

            customer1ProductRevenues = customer1.PartyProductRevenuesWhereParty;
            customer1ProductRevenues.Filter.AddEquals(PartyProductRevenues.Meta.Product, good1);
            var customer1Good1Revenue = customer1ProductRevenues.First;

            customer1ProductRevenues = customer1.PartyProductRevenuesWhereParty;
            customer1ProductRevenues.Filter.AddEquals(PartyProductRevenues.Meta.Product, good2);
            var customer1Good2Revenue = customer1ProductRevenues.First;

            var customer1ProductCategoryRevenues = customer1.PartyProductCategoryRevenuesWhereParty;
            Assert.AreEqual(3, customer1ProductCategoryRevenues.Count);

            customer1ProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.ProductCategory, cat1);
            var customer1Cat1Revenue = customer1ProductCategoryRevenues.First;

            customer1ProductCategoryRevenues = customer1.PartyProductCategoryRevenuesWhereParty;
            customer1ProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.ProductCategory, cat2);
            var customer1Cat2Revenue = customer1ProductCategoryRevenues.First;

            customer1ProductCategoryRevenues = customer1.PartyProductCategoryRevenuesWhereParty;
            customer1ProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.ProductCategory, catMain);
            var customer1CatMainRevenue = customer1ProductCategoryRevenues.First;

            var salesRep1Customer1Revenues = salesRep1.SalesRepPartyRevenuesWhereSalesRep;
            salesRep1Customer1Revenues.Filter.AddEquals(SalesRepPartyRevenues.Meta.Party, customer1);
            var salesRep1Customer1Revenue = salesRep1Customer1Revenues.First;

            var salesRep2Customer1Revenues = salesRep2.SalesRepPartyRevenuesWhereSalesRep;
            salesRep2Customer1Revenues.Filter.AddEquals(SalesRepPartyRevenues.Meta.Party, customer1);
            var salesRep2Customer1Revenue = salesRep2Customer1Revenues.First;

            var salesRep1Customer1ProductCategoryRevenues = salesRep1.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            Assert.AreEqual(2, salesRep1Customer1ProductCategoryRevenues.Count);

            salesRep1Customer1ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.ProductCategory, cat1);
            salesRep1Customer1ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer1);
            var salesRep1Customer1Cat1Revenue = salesRep1Customer1ProductCategoryRevenues.First;

            salesRep1Customer1ProductCategoryRevenues = salesRep1.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            salesRep1Customer1ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.ProductCategory, catMain);
            salesRep1Customer1ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer1);
            var salesRep1Customer1CatMainRevenue = salesRep1Customer1ProductCategoryRevenues.First;

            var salesRep2Customer1ProductCategoryRevenues = salesRep2.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            Assert.AreEqual(2, salesRep2Customer1ProductCategoryRevenues.Count);

            salesRep2Customer1ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.ProductCategory, cat2);
            salesRep2Customer1ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer1);
            var salesRep2Customer1Cat2Revenue = salesRep2Customer1ProductCategoryRevenues.First;

            salesRep2Customer1ProductCategoryRevenues = salesRep2.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            salesRep2Customer1ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer1);
            salesRep2Customer1ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.ProductCategory, catMain);
            var salesRep2Customer1CatMainRevenue = salesRep2Customer1ProductCategoryRevenues.First;

            var salesRep1ProductCategoryRevenues = salesRep1.SalesRepProductCategoryRevenuesWhereSalesRep;
            Assert.AreEqual(2, salesRep1ProductCategoryRevenues.Count);

            salesRep1ProductCategoryRevenues.Filter.AddEquals(SalesRepProductCategoryRevenues.Meta.ProductCategory, cat1);
            var salesRep1Cat1Revenue = salesRep1ProductCategoryRevenues.First;

            salesRep1ProductCategoryRevenues = salesRep1.SalesRepProductCategoryRevenuesWhereSalesRep;
            salesRep1ProductCategoryRevenues.Filter.AddEquals(SalesRepProductCategoryRevenues.Meta.ProductCategory, catMain);
            var salesRep1CatMainRevenue = salesRep1ProductCategoryRevenues.First;

            var salesRep2ProductCategoryRevenues = salesRep2.SalesRepProductCategoryRevenuesWhereSalesRep;
            Assert.AreEqual(2, salesRep2ProductCategoryRevenues.Count);

            salesRep2ProductCategoryRevenues.Filter.AddEquals(SalesRepProductCategoryRevenues.Meta.ProductCategory, cat2);
            var salesRep2Cat2Revenue = salesRep2ProductCategoryRevenues.First;

            salesRep2ProductCategoryRevenues = salesRep2.SalesRepProductCategoryRevenuesWhereSalesRep;
            salesRep2ProductCategoryRevenues.Filter.AddEquals(SalesRepProductCategoryRevenues.Meta.ProductCategory, catMain);
            var salesRep2CatMainRevenue = salesRep2ProductCategoryRevenues.First;

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(140, internalOrganisationRevenue.Revenue);
            Assert.AreEqual(140, storeRevenue.Revenue);
            Assert.AreEqual(140, salesChannelRevenue.Revenue);
            Assert.AreEqual(90, salesRep1Revenue.Revenue);
            Assert.AreEqual(50, salesRep2Revenue.Revenue);
            Assert.AreEqual(90, salesRep1Customer1Revenue.Revenue);
            Assert.AreEqual(50, salesRep2Customer1Revenue.Revenue);
            Assert.AreEqual(90, good1Revenue.Revenue);
            Assert.AreEqual(50, good2Revenue.Revenue);
            Assert.AreEqual(90, cat1Revenue.Revenue);
            Assert.AreEqual(50, cat2Revenue.Revenue);
            Assert.AreEqual(140, catMainRevenue.Revenue);
            Assert.AreEqual(90, customer1Cat1Revenue.Revenue);
            Assert.AreEqual(50, customer1Cat2Revenue.Revenue);
            Assert.AreEqual(140, customer1CatMainRevenue.Revenue);
            Assert.AreEqual(90, salesRep1Cat1Revenue.Revenue);
            Assert.AreEqual(90, salesRep1CatMainRevenue.Revenue);
            Assert.AreEqual(50, salesRep2Cat2Revenue.Revenue);
            Assert.AreEqual(50, salesRep2CatMainRevenue.Revenue);
            Assert.AreEqual(90, salesRep1Customer1Cat1Revenue.Revenue);
            Assert.AreEqual(90, salesRep1Customer1CatMainRevenue.Revenue);
            Assert.AreEqual(50, salesRep2Customer1Cat2Revenue.Revenue);
            Assert.AreEqual(50, salesRep2Customer1CatMainRevenue.Revenue);
            Assert.AreEqual(140, customer1Revenue.Revenue);
            Assert.AreEqual(90, customer1Good1Revenue.Revenue);
            Assert.AreEqual(50, customer1Good2Revenue.Revenue);

            var invoice2 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceDate(DateTime.UtcNow)
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer2)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesChannel(new SalesChannels(this.DatabaseSession).WebChannel)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(invoice2.BillToCustomer).WithInternalOrganisation(invoice2.BilledFromInternalOrganisation).Build();

            var item4 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good1).WithQuantity(1).WithActualUnitPrice(15).WithSalesInvoiceItemType(productItem).Build();
            invoice2.AddSalesInvoiceItem(item4);

            this.DatabaseSession.Derive(true);

            var item5 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good2).WithQuantity(1).WithActualUnitPrice(10).WithSalesInvoiceItemType(productItem).Build();
            invoice2.AddSalesInvoiceItem(item5);

            this.DatabaseSession.Derive(true);

            var customer2Revenue = customer2.PartyRevenuesWhereParty[0];

            var customer2ProductRevenues = customer2.PartyProductRevenuesWhereParty;
            Assert.AreEqual(2, customer2ProductRevenues.Count);

            customer2ProductRevenues.Filter.AddEquals(PartyProductRevenues.Meta.Product, good1);
            var customer2Good1Revenue = customer2ProductRevenues.First;

            customer2ProductRevenues = customer2.PartyProductRevenuesWhereParty;
            customer2ProductRevenues.Filter.AddEquals(PartyProductRevenues.Meta.Product, good2);
            var customer2Good2Revenue = customer2ProductRevenues.First;

            var customer2ProductCategoryRevenues = customer2.PartyProductCategoryRevenuesWhereParty;
            Assert.AreEqual(3, customer2ProductCategoryRevenues.Count);

            customer2ProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.ProductCategory, cat1);
            var customer2Cat1Revenue = customer2ProductCategoryRevenues.First;

            customer2ProductCategoryRevenues = customer2.PartyProductCategoryRevenuesWhereParty;
            customer2ProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.ProductCategory, cat2);
            var customer2Cat2Revenue = customer2ProductCategoryRevenues.First;

            customer2ProductCategoryRevenues = customer2.PartyProductCategoryRevenuesWhereParty;
            customer2ProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.ProductCategory, catMain);
            var customer2CatMainRevenue = customer2ProductCategoryRevenues.First;

            var salesRep1Customer2Revenues = salesRep1.SalesRepPartyRevenuesWhereSalesRep;
            salesRep1Customer2Revenues.Filter.AddEquals(SalesRepPartyRevenues.Meta.Party, customer2);
            var salesRep1Customer2Revenue = salesRep1Customer2Revenues.First;

            var salesRep2Customer2Revenues = salesRep2.SalesRepPartyRevenuesWhereSalesRep;
            salesRep2Customer2Revenues.Filter.AddEquals(SalesRepPartyRevenues.Meta.Party, customer2);
            var salesRep2Customer2Revenue = salesRep2Customer2Revenues.First;

            var salesRep1Customer2ProductCategoryRevenues = salesRep1.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            salesRep1Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer2);
            Assert.AreEqual(2, salesRep1Customer2ProductCategoryRevenues.Count);

            salesRep1Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.ProductCategory, cat1);
            salesRep1Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer2);
            var salesRep1Customer2Cat1Revenue = salesRep1Customer2ProductCategoryRevenues.First;

            salesRep1Customer2ProductCategoryRevenues = salesRep1.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            salesRep1Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.ProductCategory, catMain);
            salesRep1Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer2);
            var salesRep1Customer2CatMainRevenue = salesRep1Customer2ProductCategoryRevenues.First;

            var salesRep2Customer2ProductCategoryRevenues = salesRep2.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            salesRep2Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer2);
            Assert.AreEqual(2, salesRep2Customer2ProductCategoryRevenues.Count);

            salesRep2Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.ProductCategory, cat2);
            salesRep2Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer2);
            var salesRep2Customer2Cat2Revenue = salesRep2Customer2ProductCategoryRevenues.First;

            salesRep2Customer2ProductCategoryRevenues = salesRep2.SalesRepPartyProductCategoryRevenuesWhereSalesRep;
            salesRep2Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.Party, customer2);
            salesRep2Customer2ProductCategoryRevenues.Filter.AddEquals(SalesRepPartyProductCategoryRevenues.Meta.ProductCategory, catMain);
            var salesRep2Customer2CatMainRevenue = salesRep2Customer2ProductCategoryRevenues.First;

            Assert.AreEqual(165, internalOrganisationRevenue.Revenue);
            Assert.AreEqual(165, storeRevenue.Revenue);
            Assert.AreEqual(165, salesChannelRevenue.Revenue);
            Assert.AreEqual(105, salesRep1Revenue.Revenue);
            Assert.AreEqual(60, salesRep2Revenue.Revenue);
            Assert.AreEqual(15, salesRep1Customer2Revenue.Revenue);
            Assert.AreEqual(10, salesRep2Customer2Revenue.Revenue);
            Assert.AreEqual(105, cat1Revenue.Revenue);
            Assert.AreEqual(60, cat2Revenue.Revenue);
            Assert.AreEqual(165, catMainRevenue.Revenue);
            Assert.AreEqual(15, customer2Cat1Revenue.Revenue);
            Assert.AreEqual(10, customer2Cat2Revenue.Revenue);
            Assert.AreEqual(25, customer2CatMainRevenue.Revenue);
            Assert.AreEqual(105, salesRep1Cat1Revenue.Revenue);
            Assert.AreEqual(105, salesRep1CatMainRevenue.Revenue);
            Assert.AreEqual(60, salesRep2Cat2Revenue.Revenue);
            Assert.AreEqual(60, salesRep2CatMainRevenue.Revenue);
            Assert.AreEqual(15, salesRep1Customer2Cat1Revenue.Revenue);
            Assert.AreEqual(15, salesRep1Customer2CatMainRevenue.Revenue);
            Assert.AreEqual(10, salesRep2Customer2Cat2Revenue.Revenue);
            Assert.AreEqual(10, salesRep2Customer2CatMainRevenue.Revenue);
            Assert.AreEqual(105, good1Revenue.Revenue);
            Assert.AreEqual(60, good2Revenue.Revenue);
            Assert.AreEqual(25, customer2Revenue.Revenue);
            Assert.AreEqual(15, customer2Good1Revenue.Revenue);
            Assert.AreEqual(10, customer2Good2Revenue.Revenue);
        }

        [Test]
        public void GivenSalesInvoice_WhenWrittenOff_ThenRevenueIsUpdated()
        {
            var euro = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");
            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var contactMechanism = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.DatabaseSession)
                                        .WithLocality("Mechelen")
                                        .WithCountry(new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE"))
                                        .Build())

                .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            internalOrganisation.PreferredCurrency = euro;

            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();

            var invoice1 = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithInvoiceDate(DateTimeFactory.CreateDate(2010, 01, 01))
                .WithInvoiceNumber("1")
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesChannel(new SalesChannels(this.DatabaseSession).WebChannel)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation).Build();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(3).WithActualUnitPrice(15).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build();
            invoice1.AddSalesInvoiceItem(item1);

            this.DatabaseSession.Derive(true);

            invoice1.Send();

            this.DatabaseSession.Derive(true);

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

            this.DatabaseSession.Derive(true);

            var internalOrganisationRevenue = internalOrganisation.InternalOrganisationRevenuesWhereInternalOrganisation[0];
            var storeRevenue = invoice1.Store.StoreRevenuesWhereStore[0];
            var salesChannelRevenue = invoice1.SalesChannel.SalesChannelRevenuesWhereSalesChannel[0];
            var productRevenue = good.ProductRevenuesWhereProduct[0];
            var partyRevenue = customer.PartyRevenuesWhereParty[0];
            var partypPrductRevenue = customer.PartyProductRevenuesWhereParty[0];

            Assert.AreEqual(60, internalOrganisationRevenue.Revenue);
            Assert.AreEqual(2010, internalOrganisationRevenue.Year);
            Assert.AreEqual(60, storeRevenue.Revenue);
            Assert.AreEqual(2010, storeRevenue.Year);
            Assert.AreEqual(60, salesChannelRevenue.Revenue);
            Assert.AreEqual(2010, salesChannelRevenue.Year);
            Assert.AreEqual(60, productRevenue.Revenue);
            Assert.AreEqual(2010, productRevenue.Year);
            Assert.AreEqual(60, partyRevenue.Revenue);
            Assert.AreEqual(2010, partyRevenue.Year);
            Assert.AreEqual(60, partypPrductRevenue.Revenue);
            Assert.AreEqual(2010, partypPrductRevenue.Year);

            invoice2.WriteOff();

            this.DatabaseSession.Derive(true);

            internalOrganisationRevenue = internalOrganisation.InternalOrganisationRevenuesWhereInternalOrganisation[0];
            storeRevenue = invoice1.Store.StoreRevenuesWhereStore[0];
            salesChannelRevenue = invoice1.SalesChannel.SalesChannelRevenuesWhereSalesChannel[0];
            productRevenue = good.ProductRevenuesWhereProduct[0];
            partyRevenue = customer.PartyRevenuesWhereParty[0];
            partypPrductRevenue = customer.PartyProductRevenuesWhereParty[0];

            Assert.AreEqual(45, internalOrganisationRevenue.Revenue);
            Assert.AreEqual(2010, internalOrganisationRevenue.Year);
            Assert.AreEqual(45, storeRevenue.Revenue);
            Assert.AreEqual(2010, storeRevenue.Year);
            Assert.AreEqual(45, salesChannelRevenue.Revenue);
            Assert.AreEqual(2010, salesChannelRevenue.Year);
            Assert.AreEqual(45, productRevenue.Revenue);
            Assert.AreEqual(2010, productRevenue.Year);
            Assert.AreEqual(45, partyRevenue.Revenue);
            Assert.AreEqual(2010, partyRevenue.Year);
            Assert.AreEqual(45, partypPrductRevenue.Revenue);
            Assert.AreEqual(2010, partypPrductRevenue.Year);
        }
    }
}