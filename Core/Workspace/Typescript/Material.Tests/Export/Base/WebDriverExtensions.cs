// <copyright file="WebDriverExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Threading;

namespace Components
{
    using System;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public static partial class WebDriverExtensions
    {
        public static void WaitForAngular(this IWebDriver @this)
        {
            const string Function =
                @"

var done = arguments[0];
window.getAngularTestability(document.querySelector('app-root'))
    .whenStable(function(didWork) {
        done(didWork);
});
";

            var javascriptExecutor = (IJavaScriptExecutor)@this;
            var didWork = true;
            while (didWork)
            {
                didWork = (bool)javascriptExecutor.ExecuteAsyncScript(Function);
            }
        }

        public static void WaitForCondition(this IWebDriver @this, Func<IWebDriver, bool> condition)
        {
            for (var i = 0; i < 30; i++)
            {
                if (condition(@this))
                {
                    return;
                }

                Thread.Sleep(1000);
            }

            throw new Exception("Condition not met");
        }

        public static bool SelectorIsVisible(this IWebDriver @this, By selector)
        {
            @this.WaitForAngular();
            var elements = @this.FindElements(selector);
            return (elements.Count == 1) && elements[0].Displayed;
        }
    }
}
