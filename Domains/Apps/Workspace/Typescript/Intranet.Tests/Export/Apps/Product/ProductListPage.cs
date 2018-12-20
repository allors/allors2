namespace Tests.Intranet.ProductTest
{
    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;

    public class ProductListPage : MainPage
    {
        public ProductListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input Name => new Input(this.Driver, formControlName: "name");

        public Anchor AddNew => new Anchor(this.Driver, By.CssSelector("[mat-fab]"));
    }
}
