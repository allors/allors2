namespace Tests.Intranet.ElectronicAddressTests
{
    using Allors.Meta;

    using OpenQA.Selenium;

    using Tests.Components.Html;
    using Tests.Components.Material;

    public class ElectronicAddressEditPage : MainPage
    {
        public ElectronicAddressEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialMultipleSelect ContactPurposes => new MaterialMultipleSelect(this.Driver, roleType: M.PartyContactMechanism.ContactPurposes);

        public MaterialInput ElectronicAddressString => new MaterialInput(this.Driver, roleType: M.ElectronicAddress.ElectronicAddressString);

        public MaterialTextArea Description => new MaterialTextArea(this.Driver, roleType: M.ContactMechanism.Description);

        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor List => new Anchor(this.Driver, By.CssSelector("a[href='/contacts/people']"));

        public Button NewContactMechanism=> new Button(this.Driver, By.CssSelector("div[data-allors-class='contactmechanism']"));
    }
}
