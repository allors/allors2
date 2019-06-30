namespace Components
{
    using System.Diagnostics.CodeAnalysis;

    using Allors.Meta;

    using Components;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class MatCheckbox : Component
    {
        public MatCheckbox(IWebDriver driver, RoleType roleType, params string[] scopes)
        : base(driver)
        {
            var xpath = $"//a-mat-checkbox{this.ByScopesPredicate(scopes)}//*[@data-allors-roletype='{roleType.IdAsNumberString}']";
            this.Selector = By.XPath(xpath);
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
    public class MatCheckbox<T> : MatCheckbox where T : Component
    {
        public MatCheckbox(T page, RoleType roleType, params string[] scopes)
            : base(page.Driver, roleType, scopes)
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
