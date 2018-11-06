namespace Tests.Components.Material
{
    using Allors;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class MaterialList : Component
    {
        public MaterialList(IWebDriver driver, By selector = null)
            : base(driver)
        {
            this.Selector = selector;
        }

        public By Selector { get; }

        public MaterialListItem FindListItem(IObject obj)
        {
            this.Driver.WaitForAngular();

            var itemPath = By.CssSelector($"mat-list-item[data-allors-id='{obj.Id}']");
            var path = this.Selector != null ? new ByChained(this.Selector, itemPath) : itemPath; 
            var listItem = this.Driver.FindElement(path);

            return new MaterialListItem(this.Driver, listItem);
        }
    }
}
