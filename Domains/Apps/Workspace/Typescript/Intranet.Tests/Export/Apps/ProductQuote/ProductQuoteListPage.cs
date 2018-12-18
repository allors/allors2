namespace Tests.Intranet.ProductQuoteTest
{
    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;

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
