namespace Intranet.Tests
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;

    public abstract class Component
    {
        protected Component(IWebDriver driver)
        {
            this.Driver = driver;
        }

        public IWebDriver Driver { get; }

        public void ScrollToElement(IWebElement element)
        {
            //const string ScrollToCommand = @"arguments[0].scrollIntoView(true);";
            //var javaScriptExecutor = (IJavaScriptExecutor)this.Driver;
            //javaScriptExecutor.ExecuteScript(ScrollToCommand, element);

            var actions = new Actions(this.Driver);
            actions.MoveToElement(element);
        }
    }
}
