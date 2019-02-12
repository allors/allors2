namespace Pages.ElectronicAddressTests
{
    using Allors.Meta;

    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class ElectronicAddressEditPage : MainPage
    {
        public ElectronicAddressEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialMultipleSelect<ElectronicAddressEditPage> ContactPurposes => this.MaterialMultipleSelect(M.PartyContactMechanism.ContactPurposes);

        public MaterialInput<ElectronicAddressEditPage> ElectronicAddressString => this.MaterialInput(M.ElectronicAddress.ElectronicAddressString);

        public MaterialTextArea<ElectronicAddressEditPage> Description => this.MaterialTextArea(M.ContactMechanism.Description);

        public Button<ElectronicAddressEditPage> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor<ElectronicAddressEditPage> List => this.Anchor(By.CssSelector("a[href='/contacts/people']"));

        public Button<ElectronicAddressEditPage> NewContactMechanism => this.Button(By.CssSelector("div[data-allors-class='contactmechanism']"));
    }
}
