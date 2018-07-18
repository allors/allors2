namespace Intranet.Pages
{
    using Intranet.Tests;

    using OpenQA.Selenium;

    public abstract class MainPage : Page
    {
        protected MainPage(IWebDriver driver) : base(driver)
        {
        }
        
        public Sidenav Sidenav => new Sidenav(this.Driver);
    }
}
