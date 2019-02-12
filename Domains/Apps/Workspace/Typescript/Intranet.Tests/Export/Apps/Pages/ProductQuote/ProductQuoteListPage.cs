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

        public Input Company => new Input(this.Driver, formControlName: "company");

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
