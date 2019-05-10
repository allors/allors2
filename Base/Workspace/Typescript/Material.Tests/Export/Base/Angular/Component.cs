using System.Collections.Generic;

namespace Components
{
    using System.Linq;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;

    public abstract class Component
    {
        protected Component(IWebDriver driver)
        {
            this.Driver = driver;
        }

        public IWebDriver Driver { get; }

        public static string[] ByScopesExpressions(params string[] scopes)
        {
            return scopes.Select((v, i) => $"ancestor::*[@data-test-scope][{i + 1}]/@data-test-scope='{v}'").ToArray();
        }

        public string ByScopesPredicate(string[] scopes)
        {
            return scopes.Length > 0 ? $"[{string.Join(" and ", ByScopesExpressions(scopes))}]" : string.Empty;
        }

        public string ByScopesAnd(string[] scopes)
        {
            return string.Concat(ByScopesExpressions(scopes).Select(v=>$" and {v}"));
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
