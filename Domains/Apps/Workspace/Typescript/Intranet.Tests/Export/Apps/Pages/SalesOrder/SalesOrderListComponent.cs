using src.app.main;

namespace Pages.SalesOrderTest
{
    using OpenQA.Selenium;

    using Components;

    using Pages.ApplicationTests;

    public class SalesOrderListComponent : MainComponent
    {
        public SalesOrderListComponent(IWebDriver driver)
            : base(driver)
        {
        }

        public Input<SalesOrderListComponent> Company => this.Input(formControlName: "company");

        public Anchor<SalesOrderListComponent> AddNew => this.Anchor(By.LinkText("Add New"));
    }
}
