namespace Tests.Intranet.Relations
{
    using Allors.Domain;

    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;
    using Tests.Components.Material;

    public class OrganisationsOverviewPage : MainPage
    {
        public OrganisationsOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input Name => new Input(this.Driver, formControlName: "name");

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));

        public MaterialList List => new MaterialList(this.Driver);

        public OrganisationOverviewPage Select(Organisation organisation)
        {
            var listItem = this.List.FindListItem(organisation);
            listItem.Click();
            return new OrganisationOverviewPage(this.Driver);
        }
    }
}
