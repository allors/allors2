namespace Angular.Material
{
    using System.Diagnostics.CodeAnalysis;

    using Allors.Meta;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    using Angular;
    

    public class MaterialCheckbox
    : Component
    {
        public MaterialCheckbox(IWebDriver driver, RoleType roleType)
        : base(driver)
        {
            this.Selector = By.CssSelector($"a-mat-checkbox *[data-allors-roletype='{roleType.IdAsNumberString}']");
        }

        public By Selector { get; }

        public IWebElement Label => this.Driver.FindElement(new ByChained(this.Selector, By.CssSelector("label")));

        public IWebElement Input => this.Driver.FindElement(new ByChained(this.Selector, By.CssSelector("input")));

        public bool Value
        {
            get
            {
                this.Driver.WaitForAngular();
                return this.Input.Selected;
            }

            set
            {
                this.Driver.WaitForAngular();
                this.ScrollToElement(this.Input);

                if (this.Input.Selected)
                {
                    if (!value)
                    {
                        this.Label.Click();
                    }
                }
                else
                {
                    if (value)
                    {
                        this.Label.Click();
                    }
                }
            }
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MaterialCheckbox<T> : MaterialCheckbox where T : Page
    {
        public MaterialCheckbox(T page, RoleType roleType)
            : base(page.Driver, roleType)
        {
            this.Page = page;
        }

        public T Page { get; }

        public T Set(bool value)
        {
            this.Value = value;
            return this.Page;
        }
    }
}
