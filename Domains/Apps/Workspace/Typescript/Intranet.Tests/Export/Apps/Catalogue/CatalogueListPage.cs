namespace Tests.Intranet.CatalogueTests
{
    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;

    public class CatalogueListPage : MainPage
    {
        public CatalogueListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input Name => new Input(this.Driver, formControlName: "name");

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
