namespace Tests.Components.Material
{
    using Allors;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    using Tests.Components;
    using Tests.Intranet;

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
            this.Driver.WaitForAngular();

            var rowPath = By.CssSelector($"tr[mat-row][data-allors-id='{obj.Id}']");
            var path = this.Selector != null ? new ByChained(this.Selector, rowPath) : rowPath; 
            var row = this.Driver.FindElement(path);

            return new MaterialTableRow(this.Driver, row);
        }
    }
}
