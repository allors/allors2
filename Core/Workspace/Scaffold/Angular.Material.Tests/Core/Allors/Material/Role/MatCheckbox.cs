// <copyright file="MatCheckbox.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System.Diagnostics.CodeAnalysis;
    using Allors.Meta;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class MatCheckbox : SelectorComponent
    {
        public MatCheckbox(IWebDriver driver, RoleType roleType, params string[] scopes)
        : base(driver) =>
            this.Selector = By.XPath($".//a-mat-checkbox{this.ByScopesPredicate(scopes)}//*[@data-allors-roletype='{roleType.IdAsString}']");

        public override By Selector { get; }

        public IWebElement Label => this.Driver.FindElement(new ByChained(this.Selector, By.CssSelector("label")));

        public IWebElement Input => this.Driver.FindElement(new ByChained(this.Selector, By.CssSelector("input")));

        public bool Value
        {
            get
            {
                this.Driver.WaitForAngular();
                return this.Input.Selected;
            }

            set
            {
                this.Driver.WaitForAngular();
                this.ScrollToElement(this.Input);

                if (this.Input.Selected)
                {
                    if (!value)
                    {
                        this.Label.Click();
                    }
                }
                else
                {
                    if (value)
                    {
                        this.Label.Click();
                    }
                }
            }
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MatCheckbox<T> : MatCheckbox where T : Component
    {
        public MatCheckbox(T page, RoleType roleType, params string[] scopes)
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
