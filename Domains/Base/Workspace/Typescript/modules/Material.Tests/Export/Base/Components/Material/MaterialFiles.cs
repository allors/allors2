namespace Tests.Components.Material
{
    using System.IO;

    using Allors.Meta;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class MaterialFiles : Component
    {
        public MaterialFiles(IWebDriver driver, RoleType roleType)
            : base(driver)
        {
            this.Selector = By.CssSelector($"a-mat-files *[data-allors-roletype='{roleType.IdAsNumberString}']");
        }

        public By Selector { get; }

        public IWebElement Input => this.Driver.FindElement(new ByChained(this.Selector, By.CssSelector("input[type='file']")));

        public IWebElement Delete => this.Driver.FindElement(new ByChained(this.Selector, By.LinkText("DELETE")));

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
}
