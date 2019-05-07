using src.app.main;

namespace Pages.RequestsForQuoteTest
{
    using OpenQA.Selenium;

    using Components;

    using Pages.ApplicationTests;

    public class RequestForQuoteListComponent : MainComponent
    {
        public RequestForQuoteListComponent(IWebDriver driver)
            : base(driver)
        {
        }

        public Input<RequestForQuoteListComponent> Company => this.Input(formControlName: "company");

        public Anchor<RequestForQuoteListComponent> AddNew => this.Anchor(By.LinkText("Add New"));
    }
}
