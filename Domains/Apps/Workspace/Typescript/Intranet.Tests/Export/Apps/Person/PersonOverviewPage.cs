namespace Tests.Intranet.PersonTests
{
    using Allors.Domain;

    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;
    using Tests.Components.Material;
    using Tests.Intranet.EmailCommunicationTests;
    using Tests.Intranet.FaceToFaceCommunicationTests;
    using Tests.Intranet.LetterCorrespondenceTests;
    using Tests.Intranet.PhoneCommunicationTests;
    using Tests.Intranet.PostalAddressTests;

    public class PersonOverviewPage : MainPage
    {
        public PersonOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Element DetailPanel => new Element(this.Driver, By.CssSelector("div[data-allors-panel='detail']"));

        public Element CommunicationEventPanel => new Element(this.Driver, By.CssSelector("div[data-allors-panel='communicationevent']"));

        public Element PartyContactMechanismPanel => new Element(this.Driver, By.CssSelector("div[data-allors-panel='partycontactmechanism']"));

        public MaterialTable Table => new MaterialTable(this.Driver);

        public Anchor AddNew => new Anchor(this.Driver, By.CssSelector("[mat-fab]"));

        public Button BtnFaceToFaceCommunication => new Button(this.Driver, By.CssSelector("button[data-allors-class='FaceToFaceCommunication']"));

        public Button BtnEmailCommunication => new Button(this.Driver, By.CssSelector("button[data-allors-class='EmailCommunication']"));

        public Button BtnLetterCorrespondence => new Button(this.Driver, By.CssSelector("button[data-allors-class='LetterCorrespondence']"));

        public Button BtnPhoneCommunication => new Button(this.Driver, By.CssSelector("button[data-allors-class='PhoneCommunication']"));

        public Button BtnPostalAddress => new Button(this.Driver, By.CssSelector("button[data-allors-class='PostalAddress']"));

        public Anchor List => new Anchor(this.Driver, By.CssSelector("a[href='/contacts/people']"));

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

        public PostalAddressEditPage SelectPostalAddress(PartyContactMechanism partyContactMechanism)
        {
            this.PartyContactMechanismPanel.Click();

            var row = this.Table.FindRow(partyContactMechanism);
            var cell = row.FindCell("contact");
            cell.Click();

            return new PostalAddressEditPage(this.Driver);
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
            this.PartyContactMechanismPanel.Click();

            this.AddNew.Click();

            this.BtnPostalAddress.Click();

            return new PostalAddressEditPage(this.Driver);
        }
    }
}
