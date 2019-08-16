// <copyright file="MatList.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System.Diagnostics.CodeAnalysis;
    using Allors;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class MatList : SelectorComponent
    {
        public MatList(IWebDriver driver, By selector = null)
            : base(driver) =>
            this.Selector = selector;

        public override By Selector { get; }

        public MatListItem FindListItem(IObject obj)
        {
            var listItem = this.ListItemElement(obj);
            return new MatListItem(this.Driver, listItem);
        }

        protected IWebElement ListItemElement(IObject obj)
        {
            this.Driver.WaitForAngular();

            var itemPath = By.CssSelector($"mat-list-item[data-allors-id='{obj.Id}']");
            var path = this.Selector != null ? new ByChained(this.Selector, itemPath) : itemPath;
            var listItem = this.Driver.FindElement(path);

            return listItem;
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MatList<T> : MatList where T : Component
    {
        public MatList(T page, By selector = null)
            : base(page.Driver, selector) =>
            this.Page = page;

        public T Page { get; }

        public new MatListItem<T> FindListItem(IObject obj)
        {
            var listItem = this.ListItemElement(obj);
            return new MatListItem<T>(this.Page, listItem);
        }
    }
}
