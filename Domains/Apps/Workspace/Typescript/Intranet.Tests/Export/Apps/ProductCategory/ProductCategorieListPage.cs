namespace Tests.Intranet.ProductCategoryTest
{
    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;

    public class ProductCategorieListPage : MainPage
    {
        public ProductCategorieListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input Name => new Input(this.Driver, formControlName: "name");

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
