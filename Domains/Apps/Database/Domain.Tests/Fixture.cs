// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Fixture.cs" company="Allors bvba">
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors
{
    using System;
    using System.Globalization;
    using Domain;
    using Meta;

    public class Fixture
    {
        public static void Setup(IDatabase database)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-GB");
            CultureInfo.CurrentUICulture = new CultureInfo("en-GB");

            using (var session = database.CreateSession())
            {
                new Setup(session, null).Apply();

                var administrator = new People(session).FindBy(M.Person.UserName, Users.AdministratorUserName);
                new UserGroups(session).Administrators.AddMember(administrator);

                session.Derive();
                session.Commit();

                var singleton = Singleton.Instance(session);
                singleton.Guest = new PersonBuilder(session).WithUserName("guest").WithLastName("guest").WithPersonRole(new PersonRoles(session).Contact).Build();

                var belgium = new Countries(session).CountryByIsoCode["BE"];
                var euro = belgium.Currency;

                var bank = new BankBuilder(session).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

                var ownBankAccount = new OwnBankAccountBuilder(session)
                    .WithBankAccount(new BankAccountBuilder(session).WithBank(bank)
                                        .WithCurrency(euro)
                                        .WithIban("BE68539007547034")
                                        .WithNameOnAccount("Koen")
                                        .Build())
                    .WithDescription("Main bank account")
                    .Build();

                var postalBoundary = new PostalBoundaryBuilder(session).WithLocality("Mechelen").WithCountry(belgium).Build();
                var postalAddress = new PostalAddressBuilder(session).WithAddress1("Kleine Nieuwedijkstraat 2").WithPostalBoundary(postalBoundary).Build();

                var internalOrganisation = new InternalOrganisationBuilder(session)
                    .WithName("internalOrganisation")
                    .WithIncomingShipmentNumberPrefix("incoming shipmentno: ")
                    .WithPurchaseInvoiceNumberPrefix("incoming invoiceno: ")
                    .WithPurchaseOrderNumberPrefix("purchase orderno: ")
                    .WithBillingAddress(postalAddress)
                    .WithShippingAddress(postalAddress)
                    .WithDefaultPaymentMethod(ownBankAccount)
                    .Build();

                Singleton.Instance(session).InternalOrganisation = internalOrganisation;

                var facility = new FacilityBuilder(session).WithFacilityType(new FacilityTypes(session).Warehouse).WithName("facility").Build();
                internalOrganisation.DefaultFacility = facility;

                var paymentMethod = new PaymentMethods(session).Extent().First;

                new StoreBuilder(session)
                    .WithName("store")
                    .WithDefaultFacility(facility)
                    .WithOutgoingShipmentNumberPrefix("shipmentno: ")
                    .WithSalesInvoiceNumberPrefix("invoiceno: ")
                    .WithSalesOrderNumberPrefix("orderno: ")
                    .WithDefaultShipmentMethod(new ShipmentMethods(session).Ground)
                    .WithDefaultCarrier(new Carriers(session).Fedex)
                    .WithCreditLimit(500)
                    .WithPaymentGracePeriod(10)
                    .WithDefaultPaymentMethod(paymentMethod)
                    .Build();

                var customer = new OrganisationBuilder(session).WithName("customer").WithOrganisationRole(new OrganisationRoles(session).Customer).WithLocale(singleton.DefaultLocale).Build();
                var supplier = new OrganisationBuilder(session).WithName("supplier").WithOrganisationRole(new OrganisationRoles(session).Customer).WithLocale(singleton.DefaultLocale).Build();
                var purchaser = new PersonBuilder(session).WithLastName("purchaser").WithUserName("purchaser").WithPersonRole(new PersonRoles(session).Contact).Build();
                var salesrep = new PersonBuilder(session).WithLastName("salesRep").WithUserName("salesRep").WithPersonRole(new PersonRoles(session).Contact).Build();
                var orderProcessor = new PersonBuilder(session).WithLastName("orderProcessor").WithUserName("orderProcessor").WithPersonRole(new PersonRoles(session).Employee).Build();

                new CustomerRelationshipBuilder(session).WithCustomer(customer).WithFromDate(DateTime.UtcNow).Build();

                new SupplierRelationshipBuilder(session).WithSupplier(supplier).WithFromDate(DateTime.UtcNow).Build();

                new EmploymentBuilder(session).WithFromDate(DateTime.UtcNow).WithEmployee(purchaser).Build();

                new EmploymentBuilder(session).WithFromDate(DateTime.UtcNow).WithEmployee(salesrep).Build();

                new EmploymentBuilder(session).WithFromDate(DateTime.UtcNow).WithEmployee(orderProcessor).Build();

                new SalesRepRelationshipBuilder(session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithSalesRepresentative(salesrep).Build();

                session.Derive();
                session.Commit();
            }
        }
    }
}