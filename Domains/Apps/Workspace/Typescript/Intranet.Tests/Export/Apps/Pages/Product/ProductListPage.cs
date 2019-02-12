namespace Pages.ProductTest
{
    using Angular.Html;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class ProductListPage : MainPage
    {
        public ProductListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input<ProductListPage> Name => this.Input(formControlName: "name");

        public Anchor<ProductListPage> AddNew => this.Anchor(By.CssSelector("[mat-fab]"));
    }
}
