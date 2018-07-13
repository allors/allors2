namespace Intranet.Pages.WorkEfforts
{
    using Intranet.Tests;

    using OpenQA.Selenium;

    public class TasksOverviewPage : MainPage
    {
        public TasksOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
