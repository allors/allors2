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

        public Input<CatalogueListPage> Name => this.Input(formControlName: "name");

        public Anchor<CatalogueListPage> AddNew => this.Anchor(By.LinkText("Add New"));
    }
}
