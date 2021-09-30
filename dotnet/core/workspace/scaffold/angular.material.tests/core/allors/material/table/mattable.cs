// <copyright file="MatTable.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Allors;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class MatTable : SelectorComponent
    {
        public MatTable(IWebDriver driver, By selector = null)
            : base(driver) =>
            this.Selector = selector;

        public override By Selector { get; }

        public string[] ObjectIds
        {
            get
            {
                var rowPath = By.CssSelector($"tr[mat-row][data-allors-id]");
                var path = this.Selector != null ? new ByChained(this.Selector, rowPath) : rowPath;
                var rows = this.Driver.FindElements(path);
                return rows.Select(v => v.GetAttribute("data-allors-id")).ToArray();
            }
        }

        public string[] Actions
        {
            get
            {
                this.Driver.WaitForAngular();

                var tablePath = By.CssSelector($"table");
                var path = this.Selector != null ? new ByChained(this.Selector, tablePath) : tablePath;
                var table = this.Driver.FindElement(path);
                var attribute = table.GetAttribute("data-allors-actions");
                return !string.IsNullOrWhiteSpace(attribute) ? attribute.Split(",") : new string[0];
            }
        }

        public MatTableRow FindRow(IObject obj)
        {
            var row = this.TableRowElement(obj);
            return new MatTableRow(this.Driver, row);
        }

        public void DefaultAction(IObject obj)
        {
            var row = this.FindRow(obj);
            var cell = row.Cells[1];
            cell.Click();
        }

        public void Action(IObject obj, string action)
        {
            var row = this.FindRow(obj);
            var cell = row.FindCell("menu");
            cell.Click();

            var menu = new MatMenu(this.Driver);
            menu.Select(action);
        }

        protected IWebElement TableRowElement(IObject obj)
        {
            this.Driver.WaitForAngular();

            var rowPath = By.CssSelector($"tr[mat-row][data-allors-id='{obj.Id}']");
            var path = this.Selector != null ? new ByChained(this.Selector, rowPath) : rowPath;
            return this.Driver.FindElement(path);
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MatTable<T> : MatTable where T : Component
    {
        public MatTable(T page, By selector = null)
            : base(page.Driver, selector) =>
            this.Page = page;

        public T Page { get; }

        public new MatTableRow<T> FindRow(IObject obj)
        {
            var row = this.TableRowElement(obj);
            return new MatTableRow<T>(this.Page, row);
        }
    }
}
