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

        public Input<RequestForQuoteListPage> Company => this.Input(formControlName: "company");

        public Anchor<RequestForQuoteListPage> AddNew => this.Anchor(By.LinkText("Add New"));
    }
}
