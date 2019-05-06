using src.allors.material.apps.objects.emailaddress.edit;
using src.allors.material.apps.objects.emailcommunication.edit;

namespace Pages.PersonTests
{
    using Allors.Domain;

    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;
    using Pages.FaceToFaceCommunicationTests;
    using Pages.LetterCorrespondenceTests;
    using Pages.PartyRelationshipTests;
    using Pages.PhoneCommunicationTests;
    using Pages.PostalAddressTests;
    using Pages.TelecommunicationsNumberTests;

    public class PersonOverviewPage : MainPage
    {
        public PersonOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Element<PersonOverviewPage> DetailPanel => this.Element(By.CssSelector("div[data-allors-panel='detail']"));

        public Element<PersonOverviewPage> CommunicationEventPanel => this.Element(By.CssSelector("div[data-allors-panel='communicationevent']"));

        public Element<PersonOverviewPage> ContactMechanismPanel => this.Element(By.CssSelector("div[data-allors-panel='contactmechanism']"));

        public Element<PersonOverviewPage> PartyRelationshipPanel => this.Element(By.CssSelector("div[data-allors-panel='partyrelationship']"));

        public MaterialTable<PersonOverviewPage> Table => this.MaterialTable();

        public Anchor<PersonOverviewPage> AddNew => this.Anchor(By.CssSelector("[mat-fab]"));

        public Button<PersonOverviewPage> BtnFaceToFaceCommunication => this.Button(By.CssSelector("button[data-allors-class='FaceToFaceCommunication']"));

        public Button<PersonOverviewPage> BtnEmailCommunication => this.Button(By.CssSelector("button[data-allors-class='EmailCommunication']"));

        public Button<PersonOverviewPage> BtnLetterCorrespondence => this.Button(By.CssSelector("button[data-allors-class='LetterCorrespondence']"));

        public Button<PersonOverviewPage> BtnPhoneCommunication => this.Button(By.CssSelector("button[data-allors-class='PhoneCommunication']"));

        public Button<PersonOverviewPage> BtnPostalAddress => this.Button(By.CssSelector("button[data-allors-class='PostalAddress']"));

        public Button<PersonOverviewPage> BtnTelecommunicationsNumber => this.Button(By.CssSelector("button[data-allors-class='TelecommunicationsNumber']"));

        public Button<PersonOverviewPage> BtnEmailAddress => this.Button(By.CssSelector("button[data-allors-class='EmailAddress']"));

        public Button<PersonOverviewPage> BtnWebAddress => this.Button(By.CssSelector("button[data-allors-class='WebAddress']"));

        public Button<PersonOverviewPage> BtnCustomerRelationship => this.Button(By.CssSelector("button[data-allors-class='CustomerRelationship']"));

        public Button<PersonOverviewPage> BtnEmployment => this.Button(By.CssSelector("button[data-allors-class='Employment']"));

        public Button<PersonOverviewPage> BtnSupplierRelationship => this.Button(By.CssSelector("button[data-allors-class='SupplierRelationship']"));

        public Button<PersonOverviewPage> BtnOrganisationContactRelationship => this.Button(By.CssSelector("button[data-allors-class='OrganisationContactRelationship']"));

        public Anchor<PersonOverviewPage> List => this.Anchor(By.LinkText("People"));

        public PersonEditPage Edit()
        {
            this.DetailPanel.Click();
            return new PersonEditPage(this.Driver);
        }

        public FaceToFaceCommunicationEditPage NewFaceToFaceCommunication()
        {
            this.CommunicationEventPanel.Click();

            this.AddNew.Click();

            this.BtnFaceToFaceCommunication.Click();

            return new FaceToFaceCommunicationEditPage(this.Driver);
        }

        public FaceToFaceCommunicationEditPage SelectFaceToFaceCommunication(CommunicationEvent communication)
        {
            this.CommunicationEventPanel.Click();

            var row = this.Table.FindRow(communication);
            var cell = row.FindCell("description");
            cell.Click();

            return new FaceToFaceCommunicationEditPage(this.Driver);
        }

        public EmailCommunicationEditComponent NewEmailCommunication()
        {
            this.CommunicationEventPanel.Click();

            this.AddNew.Click();

            this.BtnEmailCommunication.Click();

            return new EmailCommunicationEditComponent(this.Driver);
        }

        public EmailCommunicationEditComponent SelectEmailCommunication(CommunicationEvent communication)
        {
            this.CommunicationEventPanel.Click();

            var row = this.Table.FindRow(communication);
            var cell = row.FindCell("description");
            cell.Click();

            return new EmailCommunicationEditComponent(this.Driver);
        }

        public LetterCorrespondenceEditPage NewLetterCorrespondence()
        {
            this.CommunicationEventPanel.Click();

            this.AddNew.Click();

            this.BtnLetterCorrespondence.Click();

            return new LetterCorrespondenceEditPage(this.Driver);
        }

        public LetterCorrespondenceEditPage SelectLetterCorrespondence(CommunicationEvent communication)
        {
            this.CommunicationEventPanel.Click();

            var row = this.Table.FindRow(communication);
            var cell = row.FindCell("description");
            cell.Click();

            return new LetterCorrespondenceEditPage(this.Driver);
        }

        public PhoneCommunicationEditPage NewPhoneCommunication()
        {
            this.CommunicationEventPanel.Click();

            this.AddNew.Click();

            this.BtnPhoneCommunication.Click();

            return new PhoneCommunicationEditPage(this.Driver);
        }

        public PhoneCommunicationEditPage SelectPhoneCommunication(CommunicationEvent communication)
        {
            this.CommunicationEventPanel.Click();

            var row = this.Table.FindRow(communication);
            var cell = row.FindCell("description");
            cell.Click();

            return new PhoneCommunicationEditPage(this.Driver);
        }

        public PostalAddressEditPage NewPostalAddress()
        {
            this.ContactMechanismPanel.Click();

            this.AddNew.Click();

            this.BtnPostalAddress.Click();

            return new PostalAddressEditPage(this.Driver);
        }

        public PostalAddressEditPage SelectPostalAddress(ContactMechanism ContactMechanism)
        {
            this.ContactMechanismPanel.Click();

            var row = this.Table.FindRow(ContactMechanism);
            var cell = row.FindCell("contact");
            cell.Click();

            return new PostalAddressEditPage(this.Driver);
        }

        public TelecommunicationsNumberEditPage NewTelecommunicationsNumber()
        {
            this.ContactMechanismPanel.Click();

            this.AddNew.Click();

            this.BtnTelecommunicationsNumber.Click();

            return new TelecommunicationsNumberEditPage(this.Driver);
        }

        public TelecommunicationsNumberEditPage SelectTelecommunicationsNumber(ContactMechanism ContactMechanism)
        {
            this.ContactMechanismPanel.Click();

            var row = this.Table.FindRow(ContactMechanism);
            var cell = row.FindCell("contact");
            cell.Click();

            return new TelecommunicationsNumberEditPage(this.Driver);
        }

        public EmailAddressEditComponent NewEmailAddress()
        {
            this.ContactMechanismPanel.Click();

            this.AddNew.Click();

            this.BtnEmailAddress.Click();

            return new EmailAddressEditComponent(this.Driver);
        }

        public EmailAddressEditComponent SelectElectronicAddress(ContactMechanism contactMechanism)
        {
            this.ContactMechanismPanel.Click();

            var row = this.Table.FindRow(contactMechanism);
            var cell = row.FindCell("contact");
            cell.Click();

            return new EmailAddressEditComponent(this.Driver);
        }

        public EmailAddressEditComponent NewWebAddress()
        {
            this.ContactMechanismPanel.Click();

            this.AddNew.Click();

            this.BtnWebAddress.Click();

            return new EmailAddressEditComponent(this.Driver);
        }

        public PartyRelationshipEditPage NewCustomerRelationship()
        {
            this.PartyRelationshipPanel.Click();

            this.AddNew.Click();

            this.BtnCustomerRelationship.Click();

            return new PartyRelationshipEditPage(this.Driver);
        }

        public PartyRelationshipEditPage NewEmployment()
        {
            this.PartyRelationshipPanel.Click();

            this.AddNew.Click();

            this.BtnEmployment.Click();

            return new PartyRelationshipEditPage(this.Driver);
        }

        public PartyRelationshipEditPage NewSupplierRelationship()
        {
            this.PartyRelationshipPanel.Click();

            this.AddNew.Click();

            this.BtnSupplierRelationship.Click();

            return new PartyRelationshipEditPage(this.Driver);
        }

        public PartyRelationshipEditPage NewOrganisationContactRelationship()
        {
            this.PartyRelationshipPanel.Click();

            this.AddNew.Click();

            this.BtnOrganisationContactRelationship.Click();

            return new PartyRelationshipEditPage(this.Driver);
        }

        public PartyRelationshipEditPage SelectPartyRelationship(PartyRelationship partyRelationship)
        {
            this.PartyRelationshipPanel.Click();

            var row = this.Table.FindRow(partyRelationship);
            var cell = row.FindCell("type");
            cell.Click();

            return new PartyRelationshipEditPage(this.Driver);
        }
    }
}
