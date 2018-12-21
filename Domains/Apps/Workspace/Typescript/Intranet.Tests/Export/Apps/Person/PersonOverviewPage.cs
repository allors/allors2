namespace Tests.Intranet.PersonTests
{
    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;

    public class PersonOverviewPage : MainPage
    {
        public PersonOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Element DetailPanel => new Element(this.Driver, By.Id("detail"));

        public Anchor List => new Anchor(this.Driver, By.CssSelector("a[href='/contacts/people']"));

        public PersonEditPage Edit()
        {
            this.DetailPanel.Click();
            return new PersonEditPage(this.Driver);
        }
    }
}
