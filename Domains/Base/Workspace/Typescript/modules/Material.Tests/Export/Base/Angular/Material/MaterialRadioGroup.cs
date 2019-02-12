namespace Angular.Material
{
    using System.Diagnostics.CodeAnalysis;

    using Allors.Meta;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    using Angular;
    

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

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MaterialRadioGroup<T> : MaterialRadioGroup where T : Page
    {
        public MaterialRadioGroup(T page, RoleType roleType)
            : base(page.Driver, roleType)
        {
            this.Page = page;
        }

        public T Page { get; }

        public new T Select(string value)
        {
            base.Select(value);
            return this.Page;
        }
    }
}
