namespace Angular.Material
{
    using System.Diagnostics.CodeAnalysis;
    using Allors.Meta;
    using Angular;
    using OpenQA.Selenium;

    public class MaterialInput : Component
    {
        public MaterialInput(IWebDriver driver, RoleType roleType, params string[] scopes)
        : base(driver)
        {
            var xpath = $"//a-mat-input{this.ByScopePredicate(scopes)}//*[@data-allors-roletype='{roleType.IdAsNumberString}']//input";
            this.Selector = By.XPath(xpath);
        }

        public MaterialInput(IWebDriver driver, By selector)
            : base(driver)
        {
            this.Selector = selector;
        }

        public By Selector { get; }

        public string Value
        {
            get
            {
                this.Driver.WaitForAngular();
                var element = this.Driver.FindElement(this.Selector);
                return element.GetAttribute("value");
            }

            set
            {
                this.Driver.WaitForAngular();
                var element = this.Driver.FindElement(this.Selector);
                this.ScrollToElement(element);
                element.Clear();
                element.SendKeys(value);
                element.SendKeys(Keys.Tab);
            }
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MaterialInput<T> : MaterialInput where T : Component
    {
        public MaterialInput(T page, RoleType roleType, params string[] scopes)
            : base(page.Driver, roleType, scopes)
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
