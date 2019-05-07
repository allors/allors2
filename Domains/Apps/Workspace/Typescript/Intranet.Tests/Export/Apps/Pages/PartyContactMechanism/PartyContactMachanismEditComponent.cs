using src.app.main;

namespace Pages.PartyContactMachanismTests
{
    using Allors.Meta;
    using Components;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class PartyContactMachanismEditComponent : MainComponent
    {
        public PartyContactMachanismEditComponent(IWebDriver driver)
            : base(driver)
        {
        }

        public MatDatepicker<PartyContactMachanismEditComponent> FromDate => this.MatDatepicker(roleType: M.PartyContactMechanism.FromDate);

        public MatDatepicker<PartyContactMachanismEditComponent> ThroughDate => this.MatDatepicker(roleType: M.PartyContactMechanism.ThroughDate);

        public MatSelect<PartyContactMachanismEditComponent> ContactPurposes => this.MatSelect(roleType: M.PartyContactMechanism.ContactPurposes);

        public MatInput<PartyContactMachanismEditComponent> Address1 => this.MatInput(roleType: M.PostalAddress.Address1);

        public MatInput<PartyContactMachanismEditComponent> Address2 => this.MatInput(roleType: M.PostalAddress.Address2);

        public MatInput<PartyContactMachanismEditComponent> Address3 => this.MatInput(roleType: M.PostalAddress.Address3);

        public MatInput<PartyContactMachanismEditComponent> Locality => this.MatInput(roleType: M.PostalBoundary.Locality);

        public MatInput<PartyContactMachanismEditComponent> PostalCode => this.MatInput(roleType: M.PostalBoundary.PostalCode);

        public MatSelect<PartyContactMachanismEditComponent> Country => this.MatSelect(roleType: M.PostalBoundary.Country);

        public MatSlidetoggle<PartyContactMachanismEditComponent> UseAsDefault => this.MatSlidetoggle(roleType: M.PartyContactMechanism.UseAsDefault);

        public MatSlidetoggle<PartyContactMachanismEditComponent> NonSolicitationIndicator => this.MatSlidetoggle(roleType: M.PartyContactMechanism.NonSolicitationIndicator);

        public MatTextarea<PartyContactMachanismEditComponent> Description => this.MatTextarea(roleType: M.ContactMechanism.Description);

        public Button<PartyContactMachanismEditComponent> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor<PartyContactMachanismEditComponent> List => this.Anchor(By.CssSelector("a[href='/contacts/people']"));

        public Button<PartyContactMachanismEditComponent> NewContactMechanism => this.Button(By.CssSelector("div[data-allors-class='contactmechanism']"));
    }
}
