namespace Angular
{
    using OpenQA.Selenium;

    public abstract class Page
    {
        public IWebDriver Driver { get; }

        protected Page(IWebDriver driver)
        {
            this.Driver = driver;
        }
    }
}
