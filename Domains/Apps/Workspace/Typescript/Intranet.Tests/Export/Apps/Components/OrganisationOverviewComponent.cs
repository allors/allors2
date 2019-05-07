using src.allors.material.apps.objects.emailcommunication.edit;
using src.allors.material.apps.objects.facetofacecommunication.edit;
using src.allors.material.apps.objects.lettercorrespondence.edit;
using src.allors.material.apps.objects.organisation.create;
using src.allors.material.apps.objects.phonecommunication.edit;
using src.app.main;

namespace src.allors.material.apps.objects.organisation.overview
{
    using Allors.Domain;
    using Components;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;
    using Pages.PartyRelationshipTests;

    public partial class OrganisationOverviewComponent
    {
        public Element<OrganisationOverviewComponent> DetailPanel => this.Element(By.CssSelector("div[data-allors-panel='detail']"));

        public Element<OrganisationOverviewComponent> CommunicationEventPanel => this.Element(By.CssSelector("div[data-allors-panel='communicationevent']"));

        public Element<OrganisationOverviewComponent> PartyRelationshipPanel => this.Element(By.CssSelector("div[data-allors-panel='partyrelationship']"));

        public MatTable<OrganisationOverviewComponent> Table => this.MatTable();

        public Anchor<OrganisationOverviewComponent> AddNew => this.Anchor(By.CssSelector("[mat-fab]"));

        public Button<OrganisationOverviewComponent> BtnFaceToFaceCommunication => this.Button(By.CssSelector("button[data-allors-class='FaceToFaceCommunication']"));

        public Button<OrganisationOverviewComponent> BtnEmailCommunication => this.Button(By.CssSelector("button[data-allors-class='EmailCommunication']"));

        public Button<OrganisationOverviewComponent> BtnLetterCorrespondence => this.Button(By.CssSelector("button[data-allors-class='LetterCorrespondence']"));

        public Button<OrganisationOverviewComponent> BtnPhoneCommunication => this.Button(By.CssSelector("button[data-allors-class='PhoneCommunication']"));

        public Button<OrganisationOverviewComponent> BtnCustomerRelationship => this.Button(By.CssSelector("button[data-allors-class='CustomerRelationship']"));

        public Button<OrganisationOverviewComponent> BtnEmployment => this.Button(By.CssSelector("button[data-allors-class='Employment']"));

        public Button<OrganisationOverviewComponent> BtnSupplierRelationship => this.Button(By.CssSelector("button[data-allors-class='SupplierRelationship']"));

        public Button<OrganisationOverviewComponent> BtnOrganisationContactRelationship => this.Button(By.CssSelector("button[data-allors-class='OrganisationContactRelationship']"));

        public Anchor<OrganisationOverviewComponent> List => this.Anchor(By.LinkText("Organisations"));

        public OrganisationCreateComponent Edit()
        {
            this.DetailPanel.Click();
            return new OrganisationCreateComponent(this.Driver);
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
