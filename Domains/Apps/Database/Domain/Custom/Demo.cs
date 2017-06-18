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
            this.SetupUser("admin1@allors.com", "administrator1", "", "x");

            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");

            var belgium = new Countries(this.Session).FindBy(M.Country.IsoCode, "BE");
            var usa = new Countries(this.Session).FindBy(M.Country.IsoCode, "US");

            var postalAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("Kleine Nieuwedijkstraat 4")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session).WithLocality("Mechelen").WithPostalCode("2800").WithCountry(belgium).Build())
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
                .WithBic("BBRUBEBB").WithCountry(belgium).Build();

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

            var acmePostalAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("Acme address 1")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session).WithLocality("Acme city").WithPostalCode("1111").WithCountry(usa).Build())
                .Build();

            var acmeBillingAddress = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(acmePostalAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralCorrespondence)
                .WithUseAsDefault(true)
                .Build();

            var acmeInquiries= new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(new TelecommunicationsNumberBuilder(this.Session).WithCountryCode("+1").WithContactNumber("111 222 333").Build())
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralPhoneNumber)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).OrderInquiriesPhone)
                .WithUseAsDefault(true)
                .Build();

            var acme = new OrganisationBuilder(this.Session)
                .WithName("Acme")
                .WithOrganisationRole(new OrganisationRoles(this.Session).Customer)
                .WithLocale(new Locales(this.Session).EnglishUnitedStates)
                .WithPartyContactMechanism(acmeBillingAddress)
                .WithPartyContactMechanism(acmeInquiries)
                .Build();

            var contact1Email = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(new EmailAddressBuilder(this.Session).WithElectronicAddressString("employee1@acme.com").Build())
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).PersonalEmailAddress)
                .WithUseAsDefault(true)
                .Build();

            var contact2PhoneNumber = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(new TelecommunicationsNumberBuilder(this.Session).WithCountryCode("+1").WithAreaCode("123").WithContactNumber("456").Build())
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).GeneralPhoneNumber)
                .WithUseAsDefault(true)
                .Build();

            var contact1 = new PersonBuilder(this.Session)
                .WithFirstName("contact1")
                .WithPersonRole(new PersonRoles(this.Session).Contact)
                .WithGender(new GenderTypes(this.Session).Male)
                .WithLocale(new Locales(this.Session).EnglishUnitedStates)
                .WithPartyContactMechanism(contact1Email)
                .Build();

            var contact2 = new PersonBuilder(this.Session)
                .WithFirstName("contact2")
                .WithPersonRole(new PersonRoles(this.Session).Contact)
                .WithGender(new GenderTypes(this.Session).Male)
                .WithLocale(new Locales(this.Session).EnglishUnitedStates)
                .WithPartyContactMechanism(contact2PhoneNumber)
                .Build();

            new OrganisationContactRelationshipBuilder(this.Session)
                .WithOrganisation(acme)
                .WithContact(contact1)
                .WithContactKind(new OrganisationContactKinds(this.Session).FindBy(M.OrganisationContactKind.Description, "General contact"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new OrganisationContactRelationshipBuilder(this.Session)
                .WithOrganisation(acme)
                .WithContact(contact2)
                .WithContactKind(new OrganisationContactKinds(this.Session).FindBy(M.OrganisationContactKind.Description, "General contact"))
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new FaceToFaceCommunicationBuilder(this.Session)
                .WithDescription("Meeting")
                .WithSubject("review")
                .WithEventPurpose(new CommunicationEventPurposes(this.Session).Meeting)
                .WithParticipant(contact1)
                .WithParticipant(contact2)
                .WithParticipant(new People(this.Session).FindBy(M.Person.UserName, "administrator1"))
                .WithActualStart(DateTime.UtcNow)
                .Build();

            var productCategory1 = new ProductCategoryBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Best selling gizmo's").WithLocale(Singleton.Instance(this.Session).DefaultLocale).Build())
                .Build();

            var productCategory2 = new ProductCategoryBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Big Gizmo's").WithLocale(Singleton.Instance(this.Session).DefaultLocale).Build())
                .Build();

            var productCategory3 = new ProductCategoryBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Small gizmo's").WithLocale(Singleton.Instance(this.Session).DefaultLocale).Build())
                .Build();

            new CatalogueBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("New gizmo's").WithLocale(Singleton.Instance(this.Session).DefaultLocale).Build())
                .WithLocalisedDescription(new LocalisedTextBuilder(this.Session).WithText("Latest in the world of Gizmo's").WithLocale(Singleton.Instance(this.Session).DefaultLocale).Build())
                .WithProductCategory(productCategory1)
                .Build();
        }

        private void SetupUser(string email, string firstName, string lastName, string password)
        {
            var userEmail = new EmailAddressBuilder(this.Session).WithElectronicAddressString(email).Build();

            var person = new PersonBuilder(this.Session)
                .WithPersonRole(new PersonRoles(this.Session).Employee)
                .WithUserName(email)
                .WithFirstName(firstName)
                .WithLastName(lastName)
                .WithUserEmail(userEmail.ElectronicAddressString)
                .WithUserEmailConfirmed(true)
                .Build();

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