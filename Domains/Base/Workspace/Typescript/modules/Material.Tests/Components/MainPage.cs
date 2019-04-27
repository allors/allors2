namespace Pages
{
    using Tests;

    using OpenQA.Selenium;

    using Angular;

    public abstract class MainPage : Component
    {
        protected MainPage(IWebDriver driver) : base(driver)
        {
        }
        
        public Sidenav Sidenav => new Sidenav(this.Driver);
    }
}
