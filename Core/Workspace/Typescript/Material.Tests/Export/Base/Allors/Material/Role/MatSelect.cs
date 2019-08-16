// <copyright file="MatSelect.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using OpenQA.Selenium.Support.PageObjects;

namespace Components
{
    using System.Diagnostics.CodeAnalysis;
    using Allors.Meta;
    using OpenQA.Selenium;

    public class MatSelect : SelectorComponent
    {
        public MatSelect(IWebDriver driver, RoleType roleType, params string[] scopes) : base(driver)
        {
            this.Selector = By.XPath($".//a-mat-select{this.ByScopesPredicate(scopes)}");
            this.ArrowSelector = new ByChained(this.Selector, By.XPath($".//mat-select[@data-allors-roletype='{roleType.IdAsNumberString}']//*[contains(@class,'mat-select-arrow')]"));
            this.ValueTextSelector = new ByChained(this.Selector, By.XPath($".//mat-select[@data-allors-roletype='{roleType.IdAsNumberString}']/*[contains(@class,'mat-select-value-text')]"));
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

        public void Toggle(params string[] values)
        {
            this.Driver.WaitForAngular();
            var arrow = this.Driver.FindElement(this.ArrowSelector);
            this.ScrollToElement(arrow);
            arrow.Click();

            foreach (var value in values)
            {
                this.Driver.WaitForAngular();
                var optionSelector = By.CssSelector($"mat-option[data-allors-option-display='{value}'] span");
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

        public new T Toggle(params string[] values)
        {
            base.Toggle(values);
            return this.Page;
        }

        public new T Set(string value)
        {
            base.Value = value;
            return this.Page;
        }
    }
}
