// <copyright file="Anchor.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System.Diagnostics.CodeAnalysis;

    using OpenQA.Selenium;

    public class Anchor : SelectorComponent
    {
        public Anchor(IWebDriver driver, By selector)
        : base(driver) =>
            this.Selector = selector;

        public Anchor(IWebDriver driver, string kind, string value, params string[] scopes)
            : base(driver)
        {
            switch (kind.ToLowerInvariant())
            {
                case "innertext":
                    this.Selector = By.XPath($".//a[normalize-space()='{value}'{this.ByScopesAnd(scopes)}]");
                    break;

                case "routerlink":
                    this.Selector = By.XPath($".//a[@ng-reflect-router-link='{value}'{this.ByScopesAnd(scopes)}]");
                    break;

                default:
                    this.Selector = By.XPath($".//a'{this.ByScopesPredicate(scopes)}");
                    break;
            }
        }

        public override By Selector { get; }

        public bool IsVisible => this.Driver.SelectorIsVisible(this.Selector);

        public void Click()
        {
            this.Driver.WaitForAngular();
            var element = this.Driver.FindElement(this.Selector);
            this.ScrollToElement(element);
            element.Click();
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class Anchor<T> : Anchor where T : Component
    {
        public Anchor(T page, By selector)
            : base(page.Driver, selector) =>
            this.Page = page;

        public T Page { get; }

        public new T Click()
        {
            base.Click();
            return this.Page;
        }
    }
}
