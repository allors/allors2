namespace Intranet.Tests
{
    using OpenQA.Selenium;

    public static class PageExtensions
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
    }
}
