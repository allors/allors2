// <copyright file="MatListItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System.Diagnostics.CodeAnalysis;
    using OpenQA.Selenium;

    public class MatListItem
    {
        public MatListItem(IWebDriver driver, IWebElement element)
        {
            this.Driver = driver;
            this.Element = element;
        }

        public IWebDriver Driver { get; }

        public IWebElement Element { get; }

        public void Click()
        {
            this.Driver.WaitForAngular();
            this.Element.Click();
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MatListItem<T> : MatListItem where T : Component
    {
        public MatListItem(T page, IWebElement element)
            : base(page.Driver, element) =>
            this.Page = page;

        public T Page { get; }

        public new T Click()
        {
            base.Click();
            return this.Page;
        }
    }
}
