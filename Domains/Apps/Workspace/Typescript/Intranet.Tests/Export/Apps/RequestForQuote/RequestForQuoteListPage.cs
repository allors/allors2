namespace Tests.Intranet.RequestsForQuoteTest
{
    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;

    public class RequestForQuoteListPage : MainPage
    {
        public RequestForQuoteListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input Company => new Input(this.Driver, formControlName: "company");

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
