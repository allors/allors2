namespace Tests.Intranet.TelecommunicationsNumberTests
{
    using Allors.Meta;

    using OpenQA.Selenium;

    using Tests.Components.Html;
    using Tests.Components.Material;

    public class TelecommunicationsNumberEditPage : MainPage
    {
        public TelecommunicationsNumberEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialMultipleSelect ContactPurposes => new MaterialMultipleSelect(this.Driver, roleType: M.PartyContactMechanism.ContactPurposes);

        public MaterialInput CountryCode => new MaterialInput(this.Driver, roleType: M.TelecommunicationsNumber.CountryCode);

        public MaterialInput AreaCode => new MaterialInput(this.Driver, roleType: M.TelecommunicationsNumber.AreaCode);

        public MaterialInput ContactNumber => new MaterialInput(this.Driver, roleType: M.TelecommunicationsNumber.ContactNumber);

        public MaterialSingleSelect ContactMechanismType => new MaterialSingleSelect(this.Driver, roleType: M.TelecommunicationsNumber.ContactMechanismType);

        public MaterialTextArea Description => new MaterialTextArea(this.Driver, roleType: M.ContactMechanism.Description);

        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor List => new Anchor(this.Driver, By.CssSelector("a[href='/contacts/people']"));

        public Button NewContactMechanism=> new Button(this.Driver, By.CssSelector("div[data-allors-class='contactmechanism']"));
    }
}
