
namespace Angular
{
    using System.Linq;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;

    public abstract class Directive
    {
        protected Directive(IWebDriver driver)
        {
            this.Driver = driver;
        }

        public IWebDriver Driver { get; }

        public string ByScopePredicate(string[] scopes)
        {
            if (scopes.Length > 0)
            {
                var expressions = scopes.Select((v, i) => $"ancestor::*[@data-test-scope][{i + 1}]/@data-test-scope='{v}'");
                return $"[{string.Join(" and ", expressions)}]";
            }

            return string.Empty;
        }

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
