using src.app.main;

namespace Pages.PostalAddressTests
{
    using Allors.Meta;
    using Components;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class PostalAddressEditComponent : MainComponent
    {
        public PostalAddressEditComponent(IWebDriver driver)
            : base(driver)
        {
        }

        public MatSelect<PostalAddressEditComponent> ContactPurposes => this.MatSelect(roleType: M.PartyContactMechanism.ContactPurposes);

        public MatInput<PostalAddressEditComponent> Address1 => this.MatInput(roleType: M.PostalAddress.Address1);

        public MatInput<PostalAddressEditComponent> Address2 => this.MatInput(roleType: M.PostalAddress.Address2);

        public MatInput<PostalAddressEditComponent> Address3 => this.MatInput(roleType: M.PostalAddress.Address3);

        public MatInput<PostalAddressEditComponent> Locality => this.MatInput(roleType: M.PostalBoundary.Locality);

        public MatInput<PostalAddressEditComponent> PostalCode => this.MatInput(roleType: M.PostalBoundary.PostalCode);

        public MatSelect<PostalAddressEditComponent> Country => this.MatSelect(roleType: M.PostalBoundary.Country);

        public MatTextarea<PostalAddressEditComponent> Description => this.MatTextarea(roleType: M.ContactMechanism.Description);

        public Button<PostalAddressEditComponent> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor<PostalAddressEditComponent> List => this.Anchor(By.CssSelector("a[href='/contacts/people']"));

        public Button<PostalAddressEditComponent> NewContactMechanism => this.Button(By.CssSelector("div[data-allors-class='contactmechanism']"));
    }
}
