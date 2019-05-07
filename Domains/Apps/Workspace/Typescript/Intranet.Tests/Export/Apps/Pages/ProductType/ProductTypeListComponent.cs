using src.app.main;

namespace Pages.ProductTypeTest
{
    using Components;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class ProductTypeListComponent : MainComponent
    {
        public ProductTypeListComponent(IWebDriver driver)
            : base(driver)
        {
        }

        public Input<ProductTypeListComponent> Name => this.Input(formControlName: "name");

        public Anchor<ProductTypeListComponent> AddNew => this.Anchor(By.LinkText("Add New"));
    }
}
