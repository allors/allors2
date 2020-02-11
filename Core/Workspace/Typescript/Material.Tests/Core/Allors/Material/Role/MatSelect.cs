// <copyright file="MatSelect.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System.Diagnostics.CodeAnalysis;
    using Allors;
    using Allors.Domain;
    using Allors.Meta;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class MatSelect : SelectorComponent
    {
        public MatSelect(IWebDriver driver, RoleType roleType, params string[] scopes) : base(driver)
        {
            this.Selector = By.XPath($".//a-mat-select{this.ByScopesPredicate(scopes)}");
            this.ArrowSelector = new ByChained(this.Selector, By.XPath($".//mat-select[@data-allors-roletype='{roleType.IdAsString}']//*[contains(@class,'mat-select-arrow')]"));
            this.ValueTextSelector = new ByChained(this.Selector, By.XPath($".//mat-select[@data-allors-roletype='{roleType.IdAsString}']/*[contains(@class,'mat-select-value-text')]"));
        }

        public override By Selector { get; }

        public string Value
        {
            get
            {
                this.Driver.WaitForAngular();
                var element = this.Driver.FindElement(this.ValueTextSelector);
                var property = element.Text;
                return property;
            }

            set
            {
                this.Driver.WaitForAngular();
                var arrow = this.Driver.FindElement(this.ArrowSelector);
                this.ScrollToElement(arrow);
                arrow.Click();

                this.Driver.WaitForAngular();
                var optionSelector = By.CssSelector($"mat-option[data-allors-option-display='{value}'] span");
                var option = this.Driver.FindElement(optionSelector);
                option.Click();
            }
        }

        public void Select(IObject @object)
        {
            this.Driver.WaitForAngular();
            var arrow = this.Driver.FindElement(this.ArrowSelector);
            this.ScrollToElement(arrow);
            arrow.Click();

            this.Driver.WaitForAngular();
            var optionSelector = By.CssSelector($"mat-option[data-allors-option-id='{@object.Id}'] span");
            var option = this.Driver.FindElement(optionSelector);
            option.Click();
        }

        public void Toggle(params IObject[] objects)
        {
            this.Driver.WaitForAngular();
            var arrow = this.Driver.FindElement(this.ArrowSelector);
            this.ScrollToElement(arrow);
            arrow.Click();

            foreach (var @object in objects)
            {
                this.Driver.WaitForAngular();
                var optionSelector = By.CssSelector($"mat-option[data-allors-option-id='{@object.Id}'] span");
                var option = this.Driver.FindElement(optionSelector);
                option.Click();
            }

            this.Driver.WaitForAngular();
            this.Driver.FindElement(By.TagName("body")).SendKeys(Keys.Escape);
        }

        private By ArrowSelector { get; }

        private By ValueTextSelector { get; }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MatSelect<T> : MatSelect where T : Component
    {
        public MatSelect(T page, RoleType roleType, params string[] scopes)
            : base(page.Driver, roleType, scopes) =>
            this.Page = page;

        public T Page { get; }

        public new T Toggle(params IObject[] objects)
        {
            base.Toggle(objects);
            return this.Page;
        }

        public new T Select(IObject @object)
        {
            base.Select(@object);
            return this.Page;
        }
    }
}
