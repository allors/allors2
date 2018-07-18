namespace Intranet.Pages.Orders
{
    using Intranet.Tests;

    using OpenQA.Selenium;

    public class CategoriesOverviewPage : MainPage
    {
        public CategoriesOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input Name => new Input(this.Driver, formControlName: "name");

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
