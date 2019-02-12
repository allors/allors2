namespace Pages.CatalogueTests
{
    using OpenQA.Selenium;

    using Angular.Html;

    using Pages.ApplicationTests;

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
