namespace Tests.Intranet.OrganisationTests
{
    using Allors.Domain;

    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;
    using Tests.Components.Material;
    using Tests.Intranet.EmailCommunicationTests;
    using Tests.Intranet.FaceToFaceCommunicationTests;
    using Tests.Intranet.LetterCorrespondenceTests;
    using Tests.Intranet.PartyRelationshipTests;
    using Tests.Intranet.PhoneCommunicationTests;

    public class OrganisationOverviewPage : MainPage
    {
        public OrganisationOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Element DetailPanel => new Element(this.Driver, By.CssSelector("div[data-allors-panel='detail']"));

        public Element CommunicationEventPanel => new Element(this.Driver, By.CssSelector("div[data-allors-panel='communicationevent']"));

        public Element PartyRelationshipPanel => new Element(this.Driver, By.CssSelector("div[data-allors-panel='partyrelationship']"));

        public MaterialTable Table => new MaterialTable(this.Driver);

        public Anchor AddNew => new Anchor(this.Driver, By.CssSelector("[mat-fab]"));

        public Button BtnFaceToFaceCommunication => new Button(this.Driver, By.CssSelector("button[data-allors-class='FaceToFaceCommunication']"));

        public Button BtnEmailCommunication => new Button(this.Driver, By.CssSelector("button[data-allors-class='EmailCommunication']"));

        public Button BtnLetterCorrespondence => new Button(this.Driver, By.CssSelector("button[data-allors-class='LetterCorrespondence']"));

        public Button BtnPhoneCommunication => new Button(this.Driver, By.CssSelector("button[data-allors-class='PhoneCommunication']"));

        public Button BtnCustomerRelationship => new Button(this.Driver, By.CssSelector("button[data-allors-class='CustomerRelationship']"));

        public Button BtnEmployment => new Button(this.Driver, By.CssSelector("button[data-allors-class='Employment']"));

        public Button BtnSupplierRelationship => new Button(this.Driver, By.CssSelector("button[data-allors-class='SupplierRelationship']"));

        public Button BtnOrganisationContactRelationship => new Button(this.Driver, By.CssSelector("button[data-allors-class='OrganisationContactRelationship']"));

        public Anchor List => new Anchor(this.Driver, By.LinkText("Organisations"));

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

        public EmailCommunicationEditPage NewEmailCommunication()
        {
            this.CommunicationEventPanel.Click();

            this.AddNew.Click();

            this.BtnEmailCommunication.Click();

            return new EmailCommunicationEditPage(this.Driver);
        }

        public EmailCommunicationEditPage SelectEmailCommunication(CommunicationEvent communication)
        {
            this.CommunicationEventPanel.Click();

            var row = this.Table.FindRow(communication);
            var cell = row.FindCell("description");
            cell.Click();

            return new EmailCommunicationEditPage(this.Driver);
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
