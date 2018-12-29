namespace Tests.Intranet.PartyContactMachanismTests
{
    using Allors.Meta;

    using OpenQA.Selenium;

    using Tests.Components.Html;
    using Tests.Components.Material;

    public class PartyContactMachanismEditPage : MainPage
    {
        public PartyContactMachanismEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialDatePicker FromDate => new MaterialDatePicker(this.Driver, roleType: M.PartyContactMechanism.FromDate);

        public MaterialDatePicker ThroughDate => new MaterialDatePicker(this.Driver, roleType: M.PartyContactMechanism.ThroughDate);

        public MaterialMultipleSelect ContactPurposes => new MaterialMultipleSelect(this.Driver, roleType: M.PartyContactMechanism.ContactPurposes);

        public MaterialInput Address1 => new MaterialInput(this.Driver, roleType: M.PostalAddress.Address1);

        public MaterialInput Address2 => new MaterialInput(this.Driver, roleType: M.PostalAddress.Address2);

        public MaterialInput Address3 => new MaterialInput(this.Driver, roleType: M.PostalAddress.Address3);

        public MaterialInput Locality => new MaterialInput(this.Driver, roleType: M.PostalBoundary.Locality);

        public MaterialInput PostalCode => new MaterialInput(this.Driver, roleType: M.PostalBoundary.PostalCode);

        public MaterialSingleSelect Country => new MaterialSingleSelect(this.Driver, roleType: M.PostalBoundary.Country);

        public MaterialSlideToggle UseAsDefault => new MaterialSlideToggle(this.Driver, roleType: M.PartyContactMechanism.UseAsDefault);

        public MaterialSlideToggle NonSolicitationIndicator => new MaterialSlideToggle(this.Driver, roleType: M.PartyContactMechanism.NonSolicitationIndicator);

        public MaterialTextArea Description => new MaterialTextArea(this.Driver, roleType: M.ContactMechanism.Description);

        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor List => new Anchor(this.Driver, By.CssSelector("a[href='/contacts/people']"));

        public Button NewContactMechanism=> new Button(this.Driver, By.CssSelector("div[data-allors-class='contactmechanism']"));
    }
}
