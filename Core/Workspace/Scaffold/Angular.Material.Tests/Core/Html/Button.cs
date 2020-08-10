// <copyright file="Button.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System.Diagnostics.CodeAnalysis;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class Button : SelectorComponent
    {
        public Button(IWebDriver driver, params By[] selectors)
        : base(driver) =>
            this.Selector = selectors.Length == 1 ? selectors[0] : new ByChained(selectors);

        public Button(IWebDriver driver, string kind, string value, params string[] scopes)
            : base(driver)
        {
            switch (kind.ToLowerInvariant())
            {
                case "innertext":
                    this.Selector = By.XPath($".//button[normalize-space()='{value}'{this.ByScopesAnd(scopes)}]");
                    break;

                default:
                    this.Selector = By.XPath($".//button'{this.ByScopesPredicate(scopes)}");
                    break;
            }
        }

        public override By Selector { get; }

        public void Click()
        {
            this.Driver.WaitForAngular();
            var element = this.Driver.FindElement(this.Selector);
            this.ScrollToElement(element);
            element.Click();
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class Button<T> : Button where T : Component
    {
        public Button(T page, params By[] selectors)
            : base(page.Driver, selectors) =>
            this.Page = page;

        public T Page { get; }

        public new T Click()
        {
            base.Click();
            return this.Page;
        }
    }
}
