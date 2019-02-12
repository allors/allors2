namespace Angular.Material
{
    using System.Diagnostics.CodeAnalysis;

    using Allors;

    using Angular;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class MaterialTable : Component
    {
        public MaterialTable(IWebDriver driver, By selector = null)
            : base(driver)
        {
            this.Selector = selector;
        }

        public By Selector { get; }

        public MaterialTableRow FindRow(IObject obj)
        {
            var row = this.TableRowElement(obj);
            return new MaterialTableRow(this.Driver, row);
        }

        protected IWebElement TableRowElement(IObject obj)
        {
            this.Driver.WaitForAngular();

            var rowPath = By.CssSelector($"tr[mat-row][data-allors-id='{obj.Id}']");
            var path = this.Selector != null ? new ByChained(this.Selector, rowPath) : rowPath;
            var row = this.Driver.FindElement(path);
            return row;
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MaterialTable<T> : MaterialTable where T : Page
    {
        public MaterialTable(T page, By selector = null)
            : base(page.Driver, selector)
        {
            this.Page = page;
        }

        public T Page { get; }

        public new MaterialTableRow<T> FindRow(IObject obj)
        {
            var row = this.TableRowElement(obj);
            return new MaterialTableRow<T>(this.Page, row);
        }
    }
}
