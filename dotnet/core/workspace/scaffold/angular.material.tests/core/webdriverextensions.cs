// <copyright file="WebDriverExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System;
    using System.Threading;
    using OpenQA.Selenium;
    using Tests;

    public static partial class WebDriverExtensions
    {
        public static void WaitForAngular(this IWebDriver @this)
        {
            const string Function =
                @"
if(window.getAngularTestability){
    var app = document.querySelector('allors-root');
    var testability = window.getAngularTestability(app);
    if(testability){
        return testability.isStable();
    }
}

return false;
";
            var timeOut = DateTime.Now.AddMinutes(1);

            var javascriptExecutor = (IJavaScriptExecutor)@this;
            var isStable = false;
            var factor = 1;
            while (isStable == false && timeOut > DateTime.Now)
            {
                isStable = (bool)javascriptExecutor.ExecuteScript(Function);
                Thread.Sleep(Math.Min(10 * factor++, 100));
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

        public static string Locale(this IWebDriver @this)
        {
            const string Function = "return window.navigator.userLanguage || window.navigator.language;";

            var javascriptExecutor = (IJavaScriptExecutor)@this;
            var locale = (string)javascriptExecutor.ExecuteScript(Function);

            return locale;
        }
    }
}
