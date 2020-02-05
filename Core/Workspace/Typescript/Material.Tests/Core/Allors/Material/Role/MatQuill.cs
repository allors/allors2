// <copyright file="MatTextarea.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Allors.Meta;
    using OpenQA.Selenium;

    public class MatQuill : SelectorComponent
    {
        public MatQuill(IWebDriver driver, RoleType roleType, params string[] scopes)
        : base(driver) =>
            this.Selector = By.XPath($".//a-mat-quill{this.ByScopesPredicate(scopes)}//*[@data-allors-roletype='{roleType.IdAsString}']");

        public override By Selector { get; }

        public string Value
        {
            get
            {
                throw new NotImplementedException();
                //this.Driver.WaitForAngular();
                //var element = this.Driver.FindElement(this.Selector);
                //return element.GetAttribute("value");
            }

            set
            {
                try
                {
                    throw new NotImplementedException();
                    //this.Driver.WaitForAngular();
                    //var element = this.Driver.FindElement(this.Selector);
                    //this.ScrollToElement(element);
                    //element.Clear();
                    //element.SendKeys(value);
                    //element.SendKeys(Keys.Tab);
                }
                catch
                {
                    throw;
                }
            }
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MatQuill<T> : MatTextarea where T : Component
    {
        public MatQuill(T page, RoleType roleType, params string[] scopes)
            : base(page.Driver, roleType, scopes) =>
            this.Page = page;

        public T Page { get; }

        public T Set(string value)
        {
            this.Value = value;
            return this.Page;
        }
    }
}
