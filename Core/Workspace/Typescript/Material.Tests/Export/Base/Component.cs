// <copyright file="Component.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System.Linq;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;

    public abstract class Component
    {
        protected Component(IWebDriver driver) => this.Driver = driver;

        public IWebDriver Driver { get; }

        public static string[] ByScopesExpressions(params string[] scopes) => scopes.Select((v, i) => $"ancestor::*[@data-test-scope][{i + 1}]/@data-test-scope='{v}'").ToArray();

        public string ByScopesPredicate(string[] scopes) => scopes.Length > 0 ? $"[{string.Join(" and ", ByScopesExpressions(scopes))}]" : string.Empty;

        public string ByScopesAnd(string[] scopes) => string.Concat(ByScopesExpressions(scopes).Select(v => $" and {v}"));

        protected void ScrollToElement(IWebElement element)
        {
            // const string ScrollToCommand = @"arguments[0].scrollIntoView(true);";
            // var javaScriptExecutor = (IJavaScriptExecutor)this.Driver;
            // javaScriptExecutor.ExecuteScript(ScrollToCommand, element);
            var actions = new Actions(this.Driver);
            actions.MoveToElement(element);
        }
    }
}
