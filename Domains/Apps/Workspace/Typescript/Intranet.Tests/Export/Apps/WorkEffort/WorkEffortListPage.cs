namespace Tests.Intranet.WorkEffortOverviewTests
{
    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;

    public class WorkEffortListPage : MainPage
    {
        public WorkEffortListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
