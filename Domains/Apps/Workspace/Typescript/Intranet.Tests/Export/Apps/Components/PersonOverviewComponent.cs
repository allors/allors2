using src.allors.material.apps.objects.emailaddress.edit;
using src.allors.material.apps.objects.emailcommunication.edit;
using src.allors.material.apps.objects.facetofacecommunication.edit;
using src.allors.material.apps.objects.lettercorrespondence.edit;
using src.allors.material.apps.objects.person.create;
using src.allors.material.apps.objects.phonecommunication.edit;
using src.allors.material.apps.objects.postaladdress.edit;
using src.app.main;

namespace src.allors.material.apps.objects.person.overview
{
    using Allors.Domain;
    using Components;

    using OpenQA.Selenium;

    using Pages.PartyRelationshipTests;
    using Pages.TelecommunicationsNumberTests;

    public partial class PersonOverviewComponent 
    {
        public Element<PersonOverviewComponent> DetailPanel => this.Element(By.CssSelector("div[data-allors-panel='detail']"));

        public Element<PersonOverviewComponent> CommunicationEventPanel => this.Element(By.CssSelector("div[data-allors-panel='communicationevent']"));

        public Element<PersonOverviewComponent> ContactMechanismPanel => this.Element(By.CssSelector("div[data-allors-panel='contactmechanism']"));

        public Element<PersonOverviewComponent> PartyRelationshipPanel => this.Element(By.CssSelector("div[data-allors-panel='partyrelationship']"));

        public MatTable<PersonOverviewComponent> Table => this.MatTable();

        public Anchor<PersonOverviewComponent> AddNew => this.Anchor(By.CssSelector("[mat-fab]"));

        public Button<PersonOverviewComponent> BtnFaceToFaceCommunication => this.Button(By.CssSelector("button[data-allors-class='FaceToFaceCommunication']"));

        public Button<PersonOverviewComponent> BtnEmailCommunication => this.Button(By.CssSelector("button[data-allors-class='EmailCommunication']"));

        public Button<PersonOverviewComponent> BtnLetterCorrespondence => this.Button(By.CssSelector("button[data-allors-class='LetterCorrespondence']"));

        public Button<PersonOverviewComponent> BtnPhoneCommunication => this.Button(By.CssSelector("button[data-allors-class='PhoneCommunication']"));

        public Button<PersonOverviewComponent> BtnPostalAddress => this.Button(By.CssSelector("button[data-allors-class='PostalAddress']"));

        public Button<PersonOverviewComponent> BtnTelecommunicationsNumber => this.Button(By.CssSelector("button[data-allors-class='TelecommunicationsNumber']"));

        public Button<PersonOverviewComponent> BtnEmailAddress => this.Button(By.CssSelector("button[data-allors-class='EmailAddress']"));

        public Button<PersonOverviewComponent> BtnWebAddress => this.Button(By.CssSelector("button[data-allors-class='WebAddress']"));

        public Button<PersonOverviewComponent> BtnCustomerRelationship => this.Button(By.CssSelector("button[data-allors-class='CustomerRelationship']"));

        public Button<PersonOverviewComponent> BtnEmployment => this.Button(By.CssSelector("button[data-allors-class='Employment']"));

        public Button<PersonOverviewComponent> BtnSupplierRelationship => this.Button(By.CssSelector("button[data-allors-class='SupplierRelationship']"));

        public Button<PersonOverviewComponent> BtnOrganisationContactRelationship => this.Button(By.CssSelector("button[data-allors-class='OrganisationContactRelationship']"));

        public Anchor<PersonOverviewComponent> List => this.Anchor(By.LinkText("People"));

        public PersonCreateComponent Edit()
        {
            this.DetailPanel.Click();
            return new PersonCreateComponent(this.Driver);
        }

        public FaceToFaceCommunicationEditComponent NewFaceToFaceCommunication()
        {
            this.CommunicationEventPanel.Click();

            this.AddNew.Click();

            this.BtnFaceToFaceCommunication.Click();

            return new FaceToFaceCommunicationEditComponent(this.Driver);
        }

        public FaceToFaceCommunicationEditComponent SelectFaceToFaceCommunication(CommunicationEvent communication)
        {
            this.CommunicationEventPanel.Click();

            var row = this.Table.FindRow(communication);
            var cell = row.FindCell("description");
            cell.Click();

            return new FaceToFaceCommunicationEditComponent(this.Driver);
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

        public LetterCorrespondenceEditComponent NewLetterCorrespondence()
        {
            this.CommunicationEventPanel.Click();

            this.AddNew.Click();

            this.BtnLetterCorrespondence.Click();

            return new LetterCorrespondenceEditComponent(this.Driver);
        }

        public LetterCorrespondenceEditComponent SelectLetterCorrespondence(CommunicationEvent communication)
        {
            this.CommunicationEventPanel.Click();

            var row = this.Table.FindRow(communication);
            var cell = row.FindCell("description");
            cell.Click();

            return new LetterCorrespondenceEditComponent(this.Driver);
        }

        public PhoneCommunicationEditComponent NewPhoneCommunication()
        {
            this.CommunicationEventPanel.Click();

            this.AddNew.Click();

            this.BtnPhoneCommunication.Click();

            return new PhoneCommunicationEditComponent(this.Driver);
        }

        public PhoneCommunicationEditComponent SelectPhoneCommunication(CommunicationEvent communication)
        {
            this.CommunicationEventPanel.Click();

            var row = this.Table.FindRow(communication);
            var cell = row.FindCell("description");
            cell.Click();

            return new PhoneCommunicationEditComponent(this.Driver);
        }

        public PostalAddressEditComponent NewPostalAddress()
        {
            this.ContactMechanismPanel.Click();

            this.AddNew.Click();

            this.BtnPostalAddress.Click();

            return new PostalAddressEditComponent(this.Driver);
        }

        public PostalAddressEditComponent SelectPostalAddress(ContactMechanism ContactMechanism)
        {
            this.ContactMechanismPanel.Click();

            var row = this.Table.FindRow(ContactMechanism);
            var cell = row.FindCell("contact");
            cell.Click();

            return new PostalAddressEditComponent(this.Driver);
        }

        public TelecommunicationsNumberEditComponent NewTelecommunicationsNumber()
        {
            this.ContactMechanismPanel.Click();

            this.AddNew.Click();

            this.BtnTelecommunicationsNumber.Click();

            return new TelecommunicationsNumberEditComponent(this.Driver);
        }

        public TelecommunicationsNumberEditComponent SelectTelecommunicationsNumber(ContactMechanism ContactMechanism)
        {
            this.ContactMechanismPanel.Click();

            var row = this.Table.FindRow(ContactMechanism);
            var cell = row.FindCell("contact");
            cell.Click();

            return new TelecommunicationsNumberEditComponent(this.Driver);
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

        public PartyRelationshipEditComponent NewCustomerRelationship()
        {
            this.PartyRelationshipPanel.Click();

            this.AddNew.Click();

            this.BtnCustomerRelationship.Click();

            return new PartyRelationshipEditComponent(this.Driver);
        }

        public PartyRelationshipEditComponent NewEmployment()
        {
            this.PartyRelationshipPanel.Click();

            this.AddNew.Click();

            this.BtnEmployment.Click();

            return new PartyRelationshipEditComponent(this.Driver);
        }

        public PartyRelationshipEditComponent NewSupplierRelationship()
        {
            this.PartyRelationshipPanel.Click();

            this.AddNew.Click();

            this.BtnSupplierRelationship.Click();

            return new PartyRelationshipEditComponent(this.Driver);
        }

        public PartyRelationshipEditComponent NewOrganisationContactRelationship()
        {
            this.PartyRelationshipPanel.Click();

            this.AddNew.Click();

            this.BtnOrganisationContactRelationship.Click();

            return new PartyRelationshipEditComponent(this.Driver);
        }

        public PartyRelationshipEditComponent SelectPartyRelationship(PartyRelationship partyRelationship)
        {
            this.PartyRelationshipPanel.Click();

            var row = this.Table.FindRow(partyRelationship);
            var cell = row.FindCell("type");
            cell.Click();

            return new PartyRelationshipEditComponent(this.Driver);
        }
    }
}
