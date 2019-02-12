namespace Pages.Relations
{
    using OpenQA.Selenium;

    using Angular.Html;

    public class OrganisationListPage : MainPage
    {
        public OrganisationListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input Name => new Input(this.Driver, formControlName: "name");

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
