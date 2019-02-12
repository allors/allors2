namespace Pages.TelecommunicationsNumberTests
{
    using Allors.Meta;

    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class TelecommunicationsNumberEditPage : MainPage
    {
        public TelecommunicationsNumberEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialMultipleSelect<TelecommunicationsNumberEditPage> ContactPurposes => this.MaterialMultipleSelect(roleType: M.PartyContactMechanism.ContactPurposes);

        public MaterialInput<TelecommunicationsNumberEditPage> CountryCode => this.MaterialInput(roleType: M.TelecommunicationsNumber.CountryCode);

        public MaterialInput<TelecommunicationsNumberEditPage> AreaCode => this.MaterialInput(roleType: M.TelecommunicationsNumber.AreaCode);

        public MaterialInput<TelecommunicationsNumberEditPage> ContactNumber => this.MaterialInput(roleType: M.TelecommunicationsNumber.ContactNumber);

        public MaterialSingleSelect<TelecommunicationsNumberEditPage> ContactMechanismType => this.MaterialSingleSelect(roleType: M.TelecommunicationsNumber.ContactMechanismType);

        public MaterialTextArea<TelecommunicationsNumberEditPage> Description => this.MaterialTextArea(roleType: M.ContactMechanism.Description);

        public Button<TelecommunicationsNumberEditPage> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor<TelecommunicationsNumberEditPage> List => this.Anchor(By.CssSelector("a[href='/contacts/people']"));

        public Button<TelecommunicationsNumberEditPage> NewContactMechanism => this.Button(By.CssSelector("div[data-allors-class='contactmechanism']"));
    }
}
