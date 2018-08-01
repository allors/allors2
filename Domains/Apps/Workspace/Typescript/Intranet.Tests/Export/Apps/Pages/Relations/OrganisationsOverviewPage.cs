namespace Intranet.Pages.Relations
{
    using Allors.Domain;

    using Intranet.Tests;

    using OpenQA.Selenium;

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
