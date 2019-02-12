namespace Pages.SalesOrderTest
{
    using OpenQA.Selenium;

    using Angular.Html;

    using Pages.ApplicationTests;

    public class SalesOrderListPage : MainPage
    {
        public SalesOrderListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input<SalesOrderListPage> Company => this.Input(formControlName: "company");

        public Anchor<SalesOrderListPage> AddNew => this.Anchor(By.LinkText("Add New"));
    }
}
