namespace Allors
{
    using System;
    using System.IO;

    using Allors.Domain;
    using Allors.Meta;

    public class Demo
    {
        private readonly ISession Session;

        private DirectoryInfo DataPath;

        public Demo(ISession session, DirectoryInfo dataPath)
        {
            this.Session = session;
            this.DataPath = dataPath;
        }

        public void Execute()
        {
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");
            var city = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var postalCode = new PostalCodeBuilder(this.Session).WithCode("2800").WithAbbreviation("Mechelen").Build();

            var belgie = new Countries(this.Session).FindBy(M.Country.IsoCode, "BE");

            var postalAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("Kleine Nieuwedijkstraat 4")
                .WithGeographicBoundary(postalCode)
                .WithGeographicBoundary(city)
                .WithGeographicBoundary(belgie)
                .Build();

            var phone = new TelecommunicationsNumberBuilder(this.Session)
                .WithCountryCode("+32")
                .WithContactNumber("2 335 2335")
                .Build();

            var email = new EmailAddressBuilder(this.Session)
                .WithElectronicAddressString("info@allors.com")
                .Build();

            var billing = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(postalAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var generalPhoneNumber = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(phone)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralPhoneNumber)
                .WithUseAsDefault(true)
                .Build();

            var generalEmail = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(email)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralEmail)
                .WithUseAsDefault(true)
                .Build();

            var ing = new BankBuilder(this.Session).WithName("ING België")
                .WithBic("BBRUBEBB").WithCountry(belgie).Build();

            var bankaccount = new BankAccountBuilder(this.Session)
                .WithBank(ing)
                .WithIban("BE89 3200 1467 7685")
                .WithNameOnAccount("Allors")
                .WithCurrency(euro)
                .Build();

            var allors = new InternalOrganisationBuilder(this.Session)
                .WithTaxNumber("BE 0476967014")
                .WithName("Allors")
                .WithPartyContactMechanism(billing)
                .WithPartyContactMechanism(generalPhoneNumber)
                .WithPartyContactMechanism(generalEmail)
                .WithBankAccount(bankaccount)
                .WithDefaultPaymentMethod(new OwnBankAccountBuilder(Session).WithBankAccount(bankaccount).WithDescription("Hoofdbank").Build())
                .WithLocale(new Locales(this.Session).FindBy(M.Locale.Name, "nl-BE"))
                .WithPreferredCurrency(euro)
                .WithInvoiceSequence(new InvoiceSequences(this.Session).EnforcedSequence)
                .WithFiscalYearStartMonth(01)
                .WithFiscalYearStartDay(01)
                .WithDoAccounting(false)
                .Build();

            var logo = this.DataPath + @"\admin\images\logo.png";

            if (File.Exists(logo))
            {
                var fileInfo = new FileInfo(logo);

                var fileName = System.IO.Path.GetFileNameWithoutExtension(fileInfo.FullName).ToLowerInvariant();
                var content = File.ReadAllBytes(fileInfo.FullName);
                var image = new ImageBuilder(this.Session).WithOriginalFilename(fileName).Build();
                var mediaContent = new MediaContentBuilder(this.Session).WithData(content).Build();
                image.Original = new MediaBuilder(this.Session).WithMediaContent(mediaContent).Build();
                allors.LogoImage = image;
            }

            Singleton.Instance(this.Session).DefaultInternalOrganisation = allors;

            var offices = new OfficeBuilder(this.Session).WithName("Headquarters").WithDescription("Allors HQ").WithOwner(allors).Build();
            allors.DefaultFacility = offices;
            
            new StoreBuilder(this.Session)
                .WithName("Allors store")
                .WithOwner(Singleton.Instance(this.Session).DefaultInternalOrganisation)
                .WithQuoteNumberPrefix("quoteno: ")
                .WithOutgoingShipmentNumberPrefix("shipmentno: ")
                .WithSalesInvoiceNumberPrefix("invoiceno: ")
                .WithSalesOrderNumberPrefix("orderno: ")
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .Build();

            this.SetupUser("koen@allors.com", "Koen", "Van Exem", "x");
        }

        private void SetupUser(string email, string firstName, string lastName, string password)
        {
            var userEmail = new EmailAddressBuilder(this.Session).WithElectronicAddressString(email).Build();

            var person = new PersonBuilder(this.Session).WithUserName(email).WithFirstName(firstName)
                .WithLastName(lastName).WithUserEmail(userEmail.ElectronicAddressString).WithUserEmailConfirmed(true).Build();

            person.AddPartyContactMechanism(
                new PartyContactMechanismBuilder(this.Session).WithContactMechanism(userEmail)
                    .WithContactPurpose(new ContactMechanismPurposes(this.Session).PersonalEmailAddress).WithUseAsDefault(true)
                    .Build());

            new UserGroups(this.Session).Administrators.AddMember(person);
            new UserGroups(this.Session).Creators.AddMember(person);

            person.SetPassword(password);
        }
    }
}