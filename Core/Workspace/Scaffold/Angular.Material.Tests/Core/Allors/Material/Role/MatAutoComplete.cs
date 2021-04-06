// <copyright file="MatAutoComplete.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using Allors.Meta;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class MatAutocomplete : SelectorComponent
    {
        public MatAutocomplete(IWebDriver driver, RoleType roleType, params string[] scopes)
        : base(driver) =>
            this.Selector = By.XPath($".//a-mat-autocomplete{this.ByScopesPredicate(scopes)}//*[@data-allors-roletype='{roleType.IdAsString}']");

        public IWebElement Input => this.Driver.FindElement(new ByChained(this.Selector, By.CssSelector("input")));

        public override By Selector { get; }

        public void Select(string value, string selection = null)
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
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MatAutocomplete<T> : MatAutocomplete where T : Component
    {
        public MatAutocomplete(T page, RoleType roleType, params string[] scopes)
            : base(page.Driver, roleType, scopes) =>
            this.Page = page;

        public T Page { get; }

        public new T Select(string value, string selection = null)
        {
            base.Select(value, selection);
            return this.Page;
        }
    }
}
