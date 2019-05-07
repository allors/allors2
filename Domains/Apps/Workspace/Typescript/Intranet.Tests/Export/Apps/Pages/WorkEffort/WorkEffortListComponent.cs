using src.app.main;

namespace Pages.WorkEffortOverviewTests
{
    using Components;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class WorkEffortListComponent : MainComponent
    {
        public WorkEffortListComponent(IWebDriver driver)
            : base(driver)
        {
        }

        public Anchor<WorkEffortListComponent> AddNew => this.Anchor(By.LinkText("Add New"));
    }
}
