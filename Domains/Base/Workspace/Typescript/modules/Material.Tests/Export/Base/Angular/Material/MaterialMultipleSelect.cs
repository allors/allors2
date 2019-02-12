namespace Angular.Material
{
    using System.Diagnostics.CodeAnalysis;

    using Allors.Meta;

    using Angular;

    using OpenQA.Selenium;

    public class MaterialMultipleSelect : Component
    {
        public MaterialMultipleSelect(IWebDriver driver, RoleType roleType) : base(driver)
        {
            this.ArrowSelector = By.CssSelector($"mat-select[data-allors-roletype='{roleType.IdAsNumberString}'] .mat-select-arrow");
            this.ValueTextSelector = By.CssSelector($"mat-select[data-allors-roletype='{roleType.IdAsNumberString}'] .mat-select-value-text");
        }

        public string Value
        {
            get
            {
                this.Driver.WaitForAngular();
                var element = this.Driver.FindElement(this.ValueTextSelector);
                var property = element.Text;
                return property;
            }
        }

        public void Toggle(params string[] values)
        {
            this.Driver.WaitForAngular();
            var arrow = this.Driver.FindElement(this.ArrowSelector);
            this.ScrollToElement(arrow);
            arrow.Click();

            foreach (var value in values)
            {
                this.Driver.WaitForAngular();
                var optionSelector = By.CssSelector($"mat-option[data-allors-option-display='{value}'] span");
                var option = this.Driver.FindElement(optionSelector);
                option.Click();
            }

            this.Driver.WaitForAngular();
            this.Driver.FindElement(By.TagName("body")).SendKeys(Keys.Escape);
        }

        private By ArrowSelector { get; }

        private By ValueTextSelector { get; }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MaterialMultipleSelect<T> : MaterialMultipleSelect where T : Page
    {
        public MaterialMultipleSelect(T page, RoleType roleType)
            : base(page.Driver, roleType)
        {
            this.Page = page;
        }

        public T Page { get; }
        
        public new T Toggle(params string[] values)
        {
            base.Toggle(values);
            return this.Page;
        }
    }
}
