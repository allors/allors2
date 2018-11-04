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

        private void CreateInternalOrganisation(
            string name,
            string address,
            string postalCode,
            string locality,
            Country country,
            string emailAddress,
            string websiteAddress,
            string taxNumber,
            string bankName,
            string facilityName,
            string bic,
            string iban,
            Currency currency,
            string logo,
            string requestNumberPrefix,
            string quoteNumberPrefix,
            string articleNumberPrefix,
            int? requestCounterValue,
            int? quoteCounterValue)
        {
            var postalAddress1 = new PostalAddressBuilder(this.Session)
                    .WithAddress1(address)
                    .WithPostalBoundary(new PostalBoundaryBuilder(this.Session).WithPostalCode(postalCode).WithLocality(locality).WithCountry(country).Build())
                    .Build();

            var phoneSales = new TelecommunicationsNumberBuilder(this.Session)
                .WithCountryCode("+32")
                .WithContactNumber("471 94 27 80")
                .Build();

            var phoneOperations = new TelecommunicationsNumberBuilder(this.Session)
                .WithCountryCode("+31")
                .WithContactNumber("6 53 76 5332")
                .Build();

            var email = new EmailAddressBuilder(this.Session)
                .WithElectronicAddressString(emailAddress)
                .Build();

            var webSite = new WebAddressBuilder(this.Session)
                .WithElectronicAddressString(websiteAddress)
                .Build();

            var bank = new BankBuilder(this.Session).WithName(bankName).WithBic(bic).WithCountry(country).Build();
            var bankaccount = new BankAccountBuilder(this.Session)
                .WithBank(bank)
                .WithIban(iban)
                .WithNameOnAccount(name)
                .WithCurrency(currency)
                .Build();

            var organisation = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithTaxNumber(taxNumber)
                .WithName(name)
                .WithBankAccount(bankaccount)
                .WithDefaultCollectionMethod(new OwnBankAccountBuilder(this.Session).WithBankAccount(bankaccount).WithDescription("Huisbank").Build())
                .WithPreferredCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"))
                .WithInvoiceSequence(new InvoiceSequences(this.Session).EnforcedSequence)
                .WithFiscalYearStartMonth(01)
                .WithFiscalYearStartDay(01)
                .WithDoAccounting(false)
                .WithRequestNumberPrefix(requestNumberPrefix)
                .WithQuoteNumberPrefix(quoteNumberPrefix)
                .WithInternetAddress(webSite)
                .Build();

            if (requestCounterValue != null)
            {
                organisation.RequestCounter = new CounterBuilder(this.Session).WithValue(requestCounterValue).Build();
            }

            if (quoteCounterValue != null)
            {
                organisation.QuoteCounter = new CounterBuilder(this.Session).WithValue(quoteCounterValue).Build();
            }

            organisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(email)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralEmail)
                .Build());
            organisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(phoneSales)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).SalesOffice)
                .Build());
            organisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(phoneOperations)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).Operations)
                .Build());
            organisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(postalAddress1)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralCorrespondence)
                .Build());

            if (File.Exists(logo))
            {
                var fileInfo = new FileInfo(logo);

                var fileName = System.IO.Path.GetFileNameWithoutExtension(fileInfo.FullName).ToLowerInvariant();
                var content = File.ReadAllBytes(fileInfo.FullName);
                var image = new MediaBuilder(this.Session).WithFileName(fileName).WithInData(content).Build();
                organisation.LogoImage = image;
            }

            var magazijn = new FacilityBuilder(this.Session)
                .WithName(facilityName)
                .WithFacilityType(new FacilityTypes(this.Session).Warehouse)
                .WithOwner(organisation)
                .Build();

            organisation.DefaultFacility = magazijn;
        }
    }
}