namespace Pages.PartyContactMachanismTests
{
    using Allors.Meta;

    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class PartyContactMachanismEditPage : MainPage
    {
        public PartyContactMachanismEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialDatePicker<PartyContactMachanismEditPage> FromDate => this.MaterialDatePicker(roleType: M.PartyContactMechanism.FromDate);

        public MaterialDatePicker<PartyContactMachanismEditPage> ThroughDate => this.MaterialDatePicker(roleType: M.PartyContactMechanism.ThroughDate);

        public MaterialMultipleSelect<PartyContactMachanismEditPage> ContactPurposes => this.MaterialMultipleSelect(roleType: M.PartyContactMechanism.ContactPurposes);

        public MaterialInput<PartyContactMachanismEditPage> Address1 => this.MaterialInput(roleType: M.PostalAddress.Address1);

        public MaterialInput<PartyContactMachanismEditPage> Address2 => this.MaterialInput(roleType: M.PostalAddress.Address2);

        public MaterialInput<PartyContactMachanismEditPage> Address3 => this.MaterialInput(roleType: M.PostalAddress.Address3);

        public MaterialInput<PartyContactMachanismEditPage> Locality => this.MaterialInput(roleType: M.PostalBoundary.Locality);

        public MaterialInput<PartyContactMachanismEditPage> PostalCode => this.MaterialInput(roleType: M.PostalBoundary.PostalCode);

        public MaterialSingleSelect<PartyContactMachanismEditPage> Country => this.MaterialSingleSelect(roleType: M.PostalBoundary.Country);

        public MaterialSlideToggle<PartyContactMachanismEditPage> UseAsDefault => this.MaterialSlideToggle(roleType: M.PartyContactMechanism.UseAsDefault);

        public MaterialSlideToggle<PartyContactMachanismEditPage> NonSolicitationIndicator => this.MaterialSlideToggle(roleType: M.PartyContactMechanism.NonSolicitationIndicator);

        public MaterialTextArea<PartyContactMachanismEditPage> Description => this.MaterialTextArea(roleType: M.ContactMechanism.Description);

        public Button<PartyContactMachanismEditPage> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor<PartyContactMachanismEditPage> List => this.Anchor(By.CssSelector("a[href='/contacts/people']"));

        public Button<PartyContactMachanismEditPage> NewContactMechanism => this.Button(By.CssSelector("div[data-allors-class='contactmechanism']"));
    }
}
