namespace Tests.Material.Pages
{
    using Tests.Material;

    using OpenQA.Selenium;

    public abstract class MainPage : Page
    {
        protected MainPage(IWebDriver driver) : base(driver)
        {
        }
        
        public Sidenav Sidenav => new Sidenav(this.Driver);
    }
}
