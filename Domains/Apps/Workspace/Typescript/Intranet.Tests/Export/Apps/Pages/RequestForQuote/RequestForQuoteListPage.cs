namespace Pages.RequestsForQuoteTest
{
    using OpenQA.Selenium;

    using Angular.Html;

    using Pages.ApplicationTests;

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
