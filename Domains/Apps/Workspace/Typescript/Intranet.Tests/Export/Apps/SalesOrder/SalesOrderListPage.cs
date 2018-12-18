namespace Tests.Intranet.SalesOrderTest
{
    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;

    public class SalesOrderListPage : MainPage
    {
        public SalesOrderListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input Company => new Input(this.Driver, formControlName: "company");

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
