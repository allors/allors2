namespace Pages.PostalAddressTests
{
    using Allors.Meta;

    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class PostalAddressEditPage : MainPage
    {
        public PostalAddressEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialMultipleSelect ContactPurposes => new MaterialMultipleSelect(this.Driver, roleType: M.PartyContactMechanism.ContactPurposes);

        public MaterialInput Address1 => new MaterialInput(this.Driver, roleType: M.PostalAddress.Address1);

        public MaterialInput Address2 => new MaterialInput(this.Driver, roleType: M.PostalAddress.Address2);

        public MaterialInput Address3 => new MaterialInput(this.Driver, roleType: M.PostalAddress.Address3);

        public MaterialInput Locality => new MaterialInput(this.Driver, roleType: M.PostalBoundary.Locality);

        public MaterialInput PostalCode => new MaterialInput(this.Driver, roleType: M.PostalBoundary.PostalCode);

        public MaterialSingleSelect Country => new MaterialSingleSelect(this.Driver, roleType: M.PostalBoundary.Country);

        public MaterialTextArea Description => new MaterialTextArea(this.Driver, roleType: M.ContactMechanism.Description);

        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor List => new Anchor(this.Driver, By.CssSelector("a[href='/contacts/people']"));

        public Button NewContactMechanism=> new Button(this.Driver, By.CssSelector("div[data-allors-class='contactmechanism']"));
    }
}
