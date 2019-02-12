namespace Pages.WorkEffortOverviewTests
{
    using Angular.Html;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class WorkEffortListPage : MainPage
    {
        public WorkEffortListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Anchor<WorkEffortListPage> AddNew => this.Anchor(By.LinkText("Add New"));
    }
}
