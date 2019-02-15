// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Organisations.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using System.IO;

    using Meta;

    public partial class Organisations
    {
        protected override void AppsPrepare(Setup setup)
        {
            setup.AddDependency(this.Meta.ObjectType, M.InventoryStrategy);
            setup.AddDependency(this.Meta.ObjectType, M.Role);
            setup.AddDependency(this.Meta.ObjectType, M.OrganisationRole);
            setup.AddDependency(this.Meta.ObjectType, M.InvoiceSequence);
            setup.AddDependency(this.Meta.ObjectType, M.Singleton);
            setup.AddDependency(this.Meta.ObjectType, M.FacilityType);
        }

        public static Organisation CreateInternalOrganisation(
            ISession session,
            string name,
            string address,
            string postalCode,
            string locality,
            Country country,
            string phone1CountryCode,
            string phone1,
            ContactMechanismPurpose phone1Purpose,
            string phone2CountryCode,
            string phone2,
            ContactMechanismPurpose phone2Purpose,
            string emailAddress,
            string websiteAddress,
            string taxNumber,
            string bankName,
            string facilityName,
            string bic,
            string iban,
            Currency currency,
            string logo,
            string storeName,
            string outgoingShipmentNumberPrefix,
            string salesInvoiceNumberPrefix,
            string salesOrderNumberPrefix,
            string requestNumberPrefix,
            string quoteNumberPrefix,
            string productNumberPrefix,
            int? requestCounterValue,
            int? quoteCounterValue,
            int? orderCounterValue,
            int? invoiceCounterValue)
        {
            var postalAddress1 = new PostalAddressBuilder(session)
                    .WithAddress1(address)
                    .WithPostalBoundary(new PostalBoundaryBuilder(session).WithPostalCode(postalCode).WithLocality(locality).WithCountry(country).Build())
                    .Build();

            var email = new EmailAddressBuilder(session)
                .WithElectronicAddressString(emailAddress)
                .Build();

            var webSite = new WebAddressBuilder(session)
                .WithElectronicAddressString(websiteAddress)
                .Build();

            var bank = new BankBuilder(session).WithName(bankName).WithBic(bic).WithCountry(country).Build();
            var bankaccount = new BankAccountBuilder(session)
                .WithBank(bank)
                .WithIban(iban)
                .WithNameOnAccount(name)
                .WithCurrency(currency)
                .Build();

            var organisation = new OrganisationBuilder(session)
                .WithIsInternalOrganisation(true)
                .WithTaxNumber(taxNumber)
                .WithName(name)
                .WithBankAccount(bankaccount)
                .WithDefaultCollectionMethod(new OwnBankAccountBuilder(session).WithBankAccount(bankaccount).WithDescription("Huisbank").Build())
                .WithPreferredCurrency(new Currencies(session).FindBy(M.Currency.IsoCode, "EUR"))
                .WithInvoiceSequence(new InvoiceSequences(session).EnforcedSequence)
                .WithFiscalYearStartMonth(01)
                .WithFiscalYearStartDay(01)
                .WithDoAccounting(false)
                .WithRequestNumberPrefix(requestNumberPrefix)
                .WithQuoteNumberPrefix(quoteNumberPrefix)
                .Build();

            if (requestCounterValue != null)
            {
                organisation.RequestCounter = new CounterBuilder(session).WithValue(requestCounterValue).Build();
            }

            if (quoteCounterValue != null)
            {
                organisation.QuoteCounter = new CounterBuilder(session).WithValue(quoteCounterValue).Build();
            }

            organisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(session)
                .WithUseAsDefault(true)
                .WithContactMechanism(email)
                .WithContactPurpose(new ContactMechanismPurposes(session).GeneralEmail)
                .Build());
            organisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(session)
                .WithUseAsDefault(true)
                .WithContactMechanism(postalAddress1)
                .WithContactPurpose(new ContactMechanismPurposes(session).GeneralCorrespondence)
                .Build());
            organisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(session)
                .WithUseAsDefault(true)
                .WithContactMechanism(webSite)
                .WithContactPurpose(new ContactMechanismPurposes(session).InternetAddress)
                .Build());

            TelecommunicationsNumber phoneNumber1 = null;
            if (!string.IsNullOrEmpty(phone1))
            {
                phoneNumber1 = new TelecommunicationsNumberBuilder(session).WithContactNumber(phone1).Build();
                if (!string.IsNullOrEmpty(phone1CountryCode))
                {
                    phoneNumber1.CountryCode = phone1CountryCode;
                }
            }

            if (phoneNumber1 != null)
            {
                organisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(session)
                    .WithUseAsDefault(true)
                    .WithContactMechanism(phoneNumber1)
                    .WithContactPurpose(phone1Purpose)
                    .Build());
            }

            TelecommunicationsNumber phoneNumber2 = null;
            if (!string.IsNullOrEmpty(phone2))
            {
                phoneNumber2 = new TelecommunicationsNumberBuilder(session).WithContactNumber(phone2).Build();
                if (!string.IsNullOrEmpty(phone2CountryCode))
                {
                    phoneNumber2.CountryCode = phone2CountryCode;
                }
            }

            if (phoneNumber2 != null)
            {
                organisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(session)
                    .WithUseAsDefault(true)
                    .WithContactMechanism(phoneNumber2)
                    .WithContactPurpose(phone2Purpose)
                    .Build());
            }

            if (File.Exists(logo))
            {
                var fileInfo = new FileInfo(logo);

                var fileName = System.IO.Path.GetFileNameWithoutExtension(fileInfo.FullName).ToLowerInvariant();
                var content = File.ReadAllBytes(fileInfo.FullName);
                var image = new MediaBuilder(session).WithFileName(fileName).WithInData(content).Build();
                organisation.LogoImage = image;
            }

            Facility facility = null;
            if (facilityName != null)
            { 
                facility = new FacilityBuilder(session)
                    .WithName(facilityName)
                    .WithFacilityType(new FacilityTypes(session).Warehouse)
                    .WithOwner(organisation)
                    .Build();
            }

            var paymentMethod = new OwnBankAccountBuilder(session).WithBankAccount(bankaccount).WithDescription("Hoofdbank").Build();

            var store = new StoreBuilder(session)
                .WithName(storeName)
                .WithOutgoingShipmentNumberPrefix(outgoingShipmentNumberPrefix)
                .WithSalesInvoiceNumberPrefix(salesInvoiceNumberPrefix)
                .WithSalesOrderNumberPrefix(salesOrderNumberPrefix)
                .WithDefaultCollectionMethod(paymentMethod)
                .WithDefaultShipmentMethod(new ShipmentMethods(session).Ground)
                .WithDefaultCarrier(new Carriers(session).Fedex)
                .WithBillingProcess(new BillingProcesses(session).BillingForOrderItems)
                .WithSalesInvoiceCounter(new CounterBuilder(session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build())
                .WithIsImmediatelyPicked(true)
                .WithInternalOrganisation(organisation)
                .Build();

            if (facility != null)
            {
                store.DefaultFacility = facility;
            }

            if (orderCounterValue != null)
            {
                store.SalesOrderCounter = new CounterBuilder(session).WithValue(orderCounterValue).Build();
            }

            if (invoiceCounterValue != null)
            {
                store.SalesInvoiceCounter = new CounterBuilder(session).WithValue(invoiceCounterValue).Build();
            }

            return organisation;
        }
    }
}