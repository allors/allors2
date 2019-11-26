// <copyright file="MatFiles.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Allors.Meta;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class MatFiles : SelectorComponent
    {
        public MatFiles(IWebDriver driver, RoleType roleType, params string[] scopes)
            : base(driver) =>
            this.Selector = By.XPath($".//a-mat-files{this.ByScopesPredicate(scopes)}//*[@data-allors-roletype='{roleType.IdAsString}']");

        public override By Selector { get; }

        public IWebElement Input => this.Driver.FindElement(new ByChained(this.Selector, By.CssSelector("input[type='file']")));

        public IWebElement Delete => this.Driver.FindElement(new ByChained(this.Selector, By.XPath($".//mat-icon[contains(text(), 'delete')]")));

        public void Upload(string fileName)
        {
            var file = new FileInfo(fileName);

            this.Driver.WaitForAngular();
            this.ScrollToElement(this.Input);
            this.Input.SendKeys(file.FullName);
        }

        public void Remove()
        {
            this.Driver.WaitForAngular();
            this.ScrollToElement(this.Delete);
            this.Delete.Click();
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FilesMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MatFiles<T> : MatFiles where T : Component
    {
        public MatFiles(T page, RoleType roleType, params string[] scopes)
            : base(page.Driver, roleType, scopes) =>
            this.Page = page;

        public T Page { get; }

        public new T Upload(string fileName)
        {
            base.Upload(fileName);
            return this.Page;
        }

        public new T Remove()
        {
            base.Remove();
            return this.Page;
        }
    }
}
