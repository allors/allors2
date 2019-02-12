namespace Pages.ProductTypeTest
{
    using Angular.Html;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class ProductTypeListPage : MainPage
    {
        public ProductTypeListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input<ProductTypeListPage> Name => this.Input(formControlName: "name");

        public Anchor<ProductTypeListPage> AddNew => this.Anchor(By.LinkText("Add New"));
    }
}
