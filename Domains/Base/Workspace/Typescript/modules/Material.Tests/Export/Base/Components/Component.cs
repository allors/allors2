namespace Tests.Components
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Support.PageObjects;

    using Tests.Intranet;

    public abstract class Component
    {
        protected Component(IWebDriver driver)
        {
            this.Driver = driver;
        }

        public IWebDriver Driver { get; }

        protected void ScrollToElement(IWebElement element)
        {
            //const string ScrollToCommand = @"arguments[0].scrollIntoView(true);";
            //var javaScriptExecutor = (IJavaScriptExecutor)this.Driver;
            //javaScriptExecutor.ExecuteScript(ScrollToCommand, element);

            var actions = new Actions(this.Driver);
            actions.MoveToElement(element);
        }

        protected bool SelectorIsVisible(By selector)
        {
            this.Driver.WaitForAngular();
            var elements = this.Driver.FindElements(selector);
            return elements.Count == 1 && elements[0].Displayed;
        }
    }
}
