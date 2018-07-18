namespace Intranet.Tests
{
    using Allors;
    using Allors.Meta;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class MaterialList<T> 
    : Component where T : IObject
    {
        private readonly RoleType roleType;

        public MaterialList(IWebDriver driver, RoleType roleType, By selector = null)
        : base(driver)
        {
            this.roleType = roleType;
            this.Selector = selector ?? By.CssSelector("mat-list");
        }

        public By Selector { get; }

        public void Select(T obj)
        {
            this.Driver.WaitForAngular();

            var path = new ByChained(this.Selector, By.XPath($"//mat-list-item[.//*[text()[contains(.,'{obj.Strategy.GetUnitRole(this.roleType.RelationType)}')]]]"));
            var listItem = this.Driver.FindElement(path);
            listItem.Click();

            this.Driver.WaitForAngular();
        }
    }
}
