namespace Angular.Material
{
    using System.Diagnostics.CodeAnalysis;

    using Allors.Meta;

    using Angular;

    using OpenQA.Selenium;

    public class MaterialSelect : Component
    {
        public MaterialSelect(IWebDriver driver, RoleType roleType, params string[] scopes) : base(driver)
        {
            var arrowXPath = $"//a-mat-select{this.ByScopesPredicate(scopes)}//mat-select[@data-allors-roletype='{roleType.IdAsNumberString}']//*[contains(@class,'mat-select-arrow')]";
            this.ArrowSelector = By.XPath(arrowXPath);

            var valueTextXPath = $"//a-mat-select{this.ByScopesPredicate(scopes)}//mat-select[@data-allors-roletype='{roleType.IdAsNumberString}']/*[contains(@class,'mat-select-value-text')]";
            this.ValueTextSelector = By.XPath(valueTextXPath);
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
    public class MaterialSelect<T> : MaterialSelect where T : Component
    {
        public MaterialSelect(T page, RoleType roleType, params string[] scopes)
            : base(page.Driver, roleType, scopes)
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
