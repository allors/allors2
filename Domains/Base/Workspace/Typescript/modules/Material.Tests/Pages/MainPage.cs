namespace Pages
{
    using Tests;

    using OpenQA.Selenium;

    using Angular;

    public abstract class MainPage : Page
    {
        protected MainPage(IWebDriver driver) : base(driver)
        {
        }
        
        public Sidenav Sidenav => new Sidenav(this.Driver);
    }
}
