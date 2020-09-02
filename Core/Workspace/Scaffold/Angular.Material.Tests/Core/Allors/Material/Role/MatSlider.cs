// <copyright file="MatSlider.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System.Diagnostics.CodeAnalysis;
    using Allors.Meta;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;

    public class MatSlider
    : SelectorComponent
    {
        public MatSlider(IWebDriver driver, RoleType roleType, params string[] scopes)
        : base(driver) =>
            this.Selector = By.XPath($".//a-mat-slider{this.ByScopesPredicate(scopes)}//*[@data-allors-roletype='{roleType.IdAsString}']//mat-slider");

        public override By Selector { get; }

        public void Select(int min, int max, int value)
        {
            this.Driver.WaitForAngular();
            var element = this.Driver.FindElement(this.Selector);
            this.ScrollToElement(element);

            var width = element.Size.Width;
            var height = element.Size.Height;

            var offsetX = (value - 1) * width / (max - min);
            var offsetY = height / 2;
            new Actions(this.Driver).MoveToElement(element, offsetX, offsetY).Click().Build().Perform();
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MatSlider<T> : MatSlider where T : Component
    {
        public MatSlider(T page, RoleType roleType, params string[] scopes)
            : base(page.Driver, roleType, scopes) =>
            this.Page = page;

        public T Page { get; }

        public new T Select(int min, int max, int value)
        {
            base.Select(min, max, value);
            return this.Page;
        }
    }
}
