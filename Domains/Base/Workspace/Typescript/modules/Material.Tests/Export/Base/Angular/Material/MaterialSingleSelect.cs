namespace Angular.Material
{
    using System.Diagnostics.CodeAnalysis;

    using Allors.Meta;

    using Angular;

    using OpenQA.Selenium;

    public class MaterialSingleSelect : Directive
    {
        public MaterialSingleSelect(IWebDriver driver, RoleType roleType) : base(driver)
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

            set
            {
                this.Driver.WaitForAngular();
                var arrow = this.Driver.FindElement(this.ArrowSelector);
                this.ScrollToElement(arrow);
                arrow.Click();

                this.Driver.WaitForAngular();
                var optionSelector = By.CssSelector($"mat-option[data-allors-option-display='{value}'] span");
                var option = this.Driver.FindElement(optionSelector);
                option.Click();
            }
        }

        private By ArrowSelector { get; }

        private By ValueTextSelector { get; }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MaterialSingleSelect<T> : MaterialSingleSelect where T : Component
    {
        public MaterialSingleSelect(T page, RoleType roleType)
            : base(page.Driver, roleType)
        {
            this.Page = page;
        }

        public T Page { get; }

        public T Set(string value)
        {
            this.Value = value;
            return this.Page;
        }
    }
}
