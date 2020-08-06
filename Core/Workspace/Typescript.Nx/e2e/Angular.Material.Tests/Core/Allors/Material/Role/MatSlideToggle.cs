// <copyright file="MatSlideToggle.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System.Diagnostics.CodeAnalysis;
    using Allors.Meta;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class MatSlideToggle
    : SelectorComponent
    {
        public MatSlideToggle(IWebDriver driver, RoleType roleType, params string[] scopes)
        : base(driver)
        {
            this.Selector = By.XPath($".//mat-slide-toggle[@data-allors-roletype='{roleType.IdAsString}'{this.ByScopesAnd(scopes)}]");
            this.InputSelector = new ByChained(this.Selector, By.XPath($".//input"));
        }

        public override By Selector { get; }

        public By InputSelector { get; }

        public bool Value
        {
            get
            {
                this.Driver.WaitForAngular();
                var element = this.Driver.FindElement(this.InputSelector);
                return element.Selected;
            }

            set
            {
                this.Driver.WaitForAngular();
                var container = this.Driver.FindElement(this.Selector);
                var element = this.Driver.FindElement(this.InputSelector);
                this.ScrollToElement(container);
                if (element.Selected)
                {
                    if (!value)
                    {
                        container.Click();
                    }
                }
                else
                {
                    if (value)
                    {
                        container.Click();
                    }
                }
            }
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MatSlidetoggle<T> : MatSlideToggle where T : Component
    {
        public MatSlidetoggle(T page, RoleType roleType, params string[] scopes)
            : base(page.Driver, roleType, scopes) =>
            this.Page = page;

        public T Page { get; }

        public T Set(bool value)
        {
            this.Value = value;
            return this.Page;
        }
    }
}
