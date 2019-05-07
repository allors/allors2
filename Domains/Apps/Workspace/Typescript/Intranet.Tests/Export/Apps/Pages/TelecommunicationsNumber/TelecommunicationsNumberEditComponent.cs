using src.app.main;

namespace Pages.TelecommunicationsNumberTests
{
    using Allors.Meta;
    using Components;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class TelecommunicationsNumberEditComponent : MainComponent
    {
        public TelecommunicationsNumberEditComponent(IWebDriver driver)
            : base(driver)
        {
        }

        public MatSelect<TelecommunicationsNumberEditComponent> ContactPurposes => this.MatSelect(roleType: M.PartyContactMechanism.ContactPurposes);

        public MatInput<TelecommunicationsNumberEditComponent> CountryCode => this.MatInput(roleType: M.TelecommunicationsNumber.CountryCode);

        public MatInput<TelecommunicationsNumberEditComponent> AreaCode => this.MatInput(roleType: M.TelecommunicationsNumber.AreaCode);

        public MatInput<TelecommunicationsNumberEditComponent> ContactNumber => this.MatInput(roleType: M.TelecommunicationsNumber.ContactNumber);

        public MatSelect<TelecommunicationsNumberEditComponent> ContactMechanismType => this.MatSelect(roleType: M.TelecommunicationsNumber.ContactMechanismType);

        public MatTextarea<TelecommunicationsNumberEditComponent> Description => this.MatTextarea(roleType: M.ContactMechanism.Description);

        public Button<TelecommunicationsNumberEditComponent> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor<TelecommunicationsNumberEditComponent> List => this.Anchor(By.CssSelector("a[href='/contacts/people']"));

        public Button<TelecommunicationsNumberEditComponent> NewContactMechanism => this.Button(By.CssSelector("div[data-allors-class='contactmechanism']"));
    }
}
