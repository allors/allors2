// <copyright file="MatFactoryFab.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System.Linq;
    using Allors.Meta;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class MatFactoryFab : SelectorComponent
    {
        public MatFactoryFab(IWebDriver driver, Composite composite, By selector)
            : base(driver)
        {
            this.Composite = composite;
            this.Selector = selector;
        }

        public Composite Composite { get; set; }

        public override By Selector { get; }

        public Class[] Classes
        {
            get
            {
                this.Driver.WaitForAngular();

                var classes = this.Composite.Classes;

                var fabPath = By.CssSelector($"a-mat-factory-fab mat-menu");
                var path = this.Selector != null ? new ByChained(this.Selector, fabPath) : fabPath;
                var fab = this.Driver.FindElement(path);
                var attribute = fab.GetAttribute("data-allors-actions");
                var actions = !string.IsNullOrWhiteSpace(attribute) ? attribute.Split(",") : new string[0];

                return classes.Where(v => actions.Contains(v.Name)).ToArray();
            }
        }

        public void Create(Class @class = null)
        {
            this.Anchor(By.CssSelector("[mat-fab]")).Click();

            if ((@class != null) && (this.Classes.Count() > 1))
            {
                this.Button(By.CssSelector($"button[data-allors-class='{@class.Name}']")).Click();
            }
        }
    }
}
