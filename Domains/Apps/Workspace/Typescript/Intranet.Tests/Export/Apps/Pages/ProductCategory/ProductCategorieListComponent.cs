using src.app.main;

namespace Pages.ProductCategoryTest
{
    using Components;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class ProductCategorieListComponent : MainComponent
    {
        public ProductCategorieListComponent(IWebDriver driver)
            : base(driver)
        {
        }

        public Input<ProductCategorieListComponent> Name => this.Input(formControlName: "name");

        public Anchor<ProductCategorieListComponent> AddNew => this.Anchor(By.LinkText("Add New"));
    }
}
