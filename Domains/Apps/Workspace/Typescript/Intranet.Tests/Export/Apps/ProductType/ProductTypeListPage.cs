namespace Tests.Intranet.ProductTypeTest
{
    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;

    public class ProductTypeListPage : MainPage
    {
        public ProductTypeListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input Name => new Input(this.Driver, formControlName: "name");

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
