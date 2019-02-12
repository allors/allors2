namespace Angular.Material
{
    using System.Diagnostics.CodeAnalysis;

    using Allors;
    using Allors.Meta;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    using Angular;
    

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
            var listItem = this.ListItemElement(obj);
            return new MaterialListItem(this.Driver, listItem);
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
    public class MaterialList<T> : MaterialList where T : Page
    {
        public MaterialList(T page, By selector = null)
            : base(page.Driver, selector)
        {
            this.Page = page;
        }

        public T Page { get; }

        public new MaterialListItem<T> FindListItem(IObject obj)
        {
            var listItem = this.ListItemElement(obj);
            return new MaterialListItem<T>(this.Page, listItem);
        }
    }
}
