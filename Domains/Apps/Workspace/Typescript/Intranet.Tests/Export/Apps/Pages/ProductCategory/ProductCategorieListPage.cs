namespace Pages.ProductCategoryTest
{
    using Angular.Html;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class ProductCategorieListPage : MainPage
    {
        public ProductCategorieListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input<ProductCategorieListPage> Name => this.Input(formControlName: "name");

        public Anchor<ProductCategorieListPage> AddNew => this.Anchor(By.LinkText("Add New"));
    }
}
