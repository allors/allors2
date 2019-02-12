namespace Pages.ProductQuoteTest
{
    using Angular.Html;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class ProductQuoteListPage : MainPage
    {
        public ProductQuoteListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input<ProductQuoteListPage> Company => this.Input(formControlName: "company");

        public Anchor<ProductQuoteListPage> AddNew => this.Anchor(By.LinkText("Add New"));
    }
}
