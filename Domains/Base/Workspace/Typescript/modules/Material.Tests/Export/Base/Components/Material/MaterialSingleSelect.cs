namespace Tests.Components.Material
{
    using Allors.Meta;

    using OpenQA.Selenium;

    public class MaterialSingleSelect : Component
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
}
