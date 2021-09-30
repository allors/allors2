// <copyright file="MatMenu.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System.Diagnostics.CodeAnalysis;
    using Allors.Meta;
    using OpenQA.Selenium;

    public class MatMenu : SelectorComponent
    {
        public MatMenu(IWebDriver driver, params string[] scopes) : base(driver) => this.Selector = By.CssSelector($"button[data-allors-action]");

        public override By Selector { get; }

        public void Select(string value)
        {
            this.Driver.WaitForAngular();
            var arrow = this.Driver.FindElement(By.CssSelector($"button[data-allors-action='{value}']"));
            this.ScrollToElement(arrow);
            arrow.Click();
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MatMenu<T> : MatSelect where T : Component
    {
        public MatMenu(T page, RoleType roleType, params string[] scopes)
            : base(page.Driver, roleType, scopes) =>
            this.Page = page;

        public T Page { get; }

        public new T Set(string value)
        {
            base.Value = value;
            return this.Page;
        }
    }
}
