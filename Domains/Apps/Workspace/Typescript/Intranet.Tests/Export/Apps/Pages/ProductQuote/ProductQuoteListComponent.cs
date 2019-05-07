using src.app.main;

namespace Pages.ProductQuoteTest
{
    using Components;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class ProductQuoteListComponent : MainComponent
    {
        public ProductQuoteListComponent(IWebDriver driver)
            : base(driver)
        {
        }

        public Input<ProductQuoteListComponent> Company => this.Input(formControlName: "company");

        public Anchor<ProductQuoteListComponent> AddNew => this.Anchor(By.LinkText("Add New"));
    }
}
