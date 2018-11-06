namespace Tests.Components.Material
{
    using System.IO;

    using Allors.Meta;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    using Tests.Components;
    using Tests.Intranet;

    public class MaterialRadioGroup : Component
    {
        public MaterialRadioGroup(IWebDriver driver, RoleType roleType)
            : base(driver)
        {
            this.Selector = By.CssSelector($"a-mat-radiogroup *[data-allors-roletype='{roleType.IdAsNumberString}']");
        }

        public By Selector { get; }


        public void Select(string value)
        {
            this.Driver.WaitForAngular();
            var radioSelector = new ByChained(this.Selector, By.CssSelector($"mat-radio-button[data-allors-radio-display='{value}'] label"));
            var radio = this.Driver.FindElement(radioSelector);
            radio.Click();
        }
    }
}
