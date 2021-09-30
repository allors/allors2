// <copyright file="MatChips.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using Allors.Meta;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class MatChips : SelectorComponent
    {
        public MatChips(IWebDriver driver, RoleType roleType, params string[] scopes)
            : base(driver) =>
            this.Selector = By.XPath($".//a-mat-chips{this.ByScopesPredicate(scopes)}//*[@data-allors-roletype='{roleType.IdAsString}']");

        public override By Selector { get; }

        public IWebElement Input => this.Driver.FindElement(new ByChained(this.Selector, By.CssSelector("input")));

        public void Add(string value, string selection = null)
        {
            this.Driver.WaitForAngular();
            this.ScrollToElement(this.Input);
            this.Input.Clear();
            this.Input.SendKeys(value);

            this.Driver.WaitForAngular();

            value = this.CssEscape(value);

            var optionSelector = By.CssSelector($"mat-option[data-allors-option-display='{selection ?? value}'] span");
            var option = this.Driver.FindElement(optionSelector);
            option.Click();
        }

        public void Remove(string value)
        {
            this.Driver.WaitForAngular();

            var listItem = this.Driver.FindElement(new ByChained(this.Selector, By.CssSelector($"mat-chip[data-allors-chip-display='{value}'] mat-icon")));
            this.ScrollToElement(listItem);
            listItem.Click();
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MatChips<T> : MatChips where T : Component
    {
        public MatChips(T page, RoleType roleType, params string[] scopes)
            : base(page.Driver, roleType, scopes) =>
            this.Page = page;

        public T Page { get; }

        public new T Add(string value, string selection = null)
        {
            base.Add(value, selection);
            return this.Page;
        }

        public new T Remove(string value)
        {
            base.Remove(value);
            return this.Page;
        }
    }
}
