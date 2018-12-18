namespace Tests.Intranet
{
    using OpenQA.Selenium;

    public abstract class Page
    {
        protected readonly IWebDriver Driver;

        protected Page(IWebDriver driver)
        {
            this.Driver = driver;
        }
    }
}
