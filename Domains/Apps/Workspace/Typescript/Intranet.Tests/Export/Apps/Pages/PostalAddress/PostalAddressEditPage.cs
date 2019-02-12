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

        public MaterialMultipleSelect<PostalAddressEditPage> ContactPurposes => this.MaterialMultipleSelect(roleType: M.PartyContactMechanism.ContactPurposes);

        public MaterialInput<PostalAddressEditPage> Address1 => this.MaterialInput(roleType: M.PostalAddress.Address1);

        public MaterialInput<PostalAddressEditPage> Address2 => this.MaterialInput(roleType: M.PostalAddress.Address2);

        public MaterialInput<PostalAddressEditPage> Address3 => this.MaterialInput(roleType: M.PostalAddress.Address3);

        public MaterialInput<PostalAddressEditPage> Locality => this.MaterialInput(roleType: M.PostalBoundary.Locality);

        public MaterialInput<PostalAddressEditPage> PostalCode => this.MaterialInput(roleType: M.PostalBoundary.PostalCode);

        public MaterialSingleSelect<PostalAddressEditPage> Country => this.MaterialSingleSelect(roleType: M.PostalBoundary.Country);

        public MaterialTextArea<PostalAddressEditPage> Description => this.MaterialTextArea(roleType: M.ContactMechanism.Description);

        public Button<PostalAddressEditPage> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor<PostalAddressEditPage> List => this.Anchor(By.CssSelector("a[href='/contacts/people']"));

        public Button<PostalAddressEditPage> NewContactMechanism => this.Button(By.CssSelector("div[data-allors-class='contactmechanism']"));
    }
}
