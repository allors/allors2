using src.allors.material.apps.objects.emailcommunication.edit;

namespace Pages.OrganisationTests
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

    public class OrganisationOverviewPage : MainPage
    {
        public OrganisationOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Element<OrganisationOverviewPage> DetailPanel => this.Element(By.CssSelector("div[data-allors-panel='detail']"));

        public Element<OrganisationOverviewPage> CommunicationEventPanel => this.Element(By.CssSelector("div[data-allors-panel='communicationevent']"));

        public Element<OrganisationOverviewPage> PartyRelationshipPanel => this.Element(By.CssSelector("div[data-allors-panel='partyrelationship']"));

        public MaterialTable<OrganisationOverviewPage> Table => this.MaterialTable();

        public Anchor<OrganisationOverviewPage> AddNew => this.Anchor(By.CssSelector("[mat-fab]"));

        public Button<OrganisationOverviewPage> BtnFaceToFaceCommunication => this.Button(By.CssSelector("button[data-allors-class='FaceToFaceCommunication']"));

        public Button<OrganisationOverviewPage> BtnEmailCommunication => this.Button(By.CssSelector("button[data-allors-class='EmailCommunication']"));

        public Button<OrganisationOverviewPage> BtnLetterCorrespondence => this.Button(By.CssSelector("button[data-allors-class='LetterCorrespondence']"));

        public Button<OrganisationOverviewPage> BtnPhoneCommunication => this.Button(By.CssSelector("button[data-allors-class='PhoneCommunication']"));

        public Button<OrganisationOverviewPage> BtnCustomerRelationship => this.Button(By.CssSelector("button[data-allors-class='CustomerRelationship']"));

        public Button<OrganisationOverviewPage> BtnEmployment => this.Button(By.CssSelector("button[data-allors-class='Employment']"));

        public Button<OrganisationOverviewPage> BtnSupplierRelationship => this.Button(By.CssSelector("button[data-allors-class='SupplierRelationship']"));

        public Button<OrganisationOverviewPage> BtnOrganisationContactRelationship => this.Button(By.CssSelector("button[data-allors-class='OrganisationContactRelationship']"));

        public Anchor<OrganisationOverviewPage> List => this.Anchor(By.LinkText("Organisations"));

        public OrganisationEditPage Edit()
        {
            this.DetailPanel.Click();
            return new OrganisationEditPage(this.Driver);
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
