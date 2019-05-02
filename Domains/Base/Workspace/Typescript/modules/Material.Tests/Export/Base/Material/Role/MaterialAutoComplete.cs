namespace Angular.Material
{
    using System.Diagnostics.CodeAnalysis;
    using Allors.Meta;
    using Angular;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class MaterialAutocomplete
    : Component
    {
        public MaterialAutocomplete(IWebDriver driver, RoleType roleType, params string[] scopes)
        : base(driver)
        {
            var xpath = $"//a-mat-autocomplete{this.ByScopesPredicate(scopes)}//*[@data-allors-roletype='{roleType.IdAsNumberString}']";
            this.Selector = By.XPath(xpath);
        }

        public IWebElement Input => this.Driver.FindElement(new ByChained(this.Selector, By.CssSelector("input")));

        public By Selector { get; }

        public void Select(string value, string selection = null)
        {
            this.Driver.WaitForAngular();
            this.ScrollToElement(this.Input);
            this.Input.Clear();
            this.Input.SendKeys(value);

            this.Driver.WaitForAngular();
            var optionSelector = By.CssSelector($"mat-option[data-allors-option-display='{selection ?? value}'] span");
            var option = this.Driver.FindElement(optionSelector);
            option.Click();
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MaterialAutocomplete<T> : MaterialAutocomplete where T : Component
    {
        public MaterialAutocomplete(T page, RoleType roleType, params string[] scopes)
            : base(page.Driver, roleType, scopes)
        {
            this.Page = page;
        }

        public T Page { get; }

        public new T Select(string value, string selection = null)
        {
            base.Select(value, selection);
            return this.Page;
        }
    }
}