namespace Components
{
    using System;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public static partial class WebDriverExtensions
    {
        public static void WaitForAngular(this IWebDriver driver)
        {
            const string Function =
                @"

var done = arguments[0];
window.getAngularTestability(document.querySelector('app-root'))
    .whenStable(function(didWork) {
        done(didWork);
});
";

            var javascriptExecutor = (IJavaScriptExecutor)driver;
            var didWork = true;
            while (didWork)
            {
                didWork = (bool)javascriptExecutor.ExecuteAsyncScript(Function);
            }
        }

        public static void WaitForCondition(this IWebDriver driver, Func<IWebDriver, bool> condition)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until(condition);
        }
    }

}
