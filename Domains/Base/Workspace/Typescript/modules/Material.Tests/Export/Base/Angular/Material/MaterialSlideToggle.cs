namespace Angular.Material
{
    using System.Diagnostics.CodeAnalysis;

    using Allors.Meta;

    using Angular;

    using OpenQA.Selenium;

    public class MaterialSlideToggle
    : Directive
    {
        public MaterialSlideToggle(IWebDriver driver, RoleType roleType)
        : base(driver)
        {
            this.InputSelector = By.CssSelector($"mat-slide-toggle[data-allors-roletype='{roleType.IdAsNumberString}'] input");
            this.ContainerSelector = By.CssSelector($"mat-slide-toggle[data-allors-roletype='{roleType.IdAsNumberString}']");
        }

        public By InputSelector { get; }

        public By ContainerSelector { get; }

        public bool Value
        {
            get
            {
                this.Driver.WaitForAngular();
                var element = this.Driver.FindElement(this.InputSelector);
                return element.Selected;
            }

            set
            {
                this.Driver.WaitForAngular();
                var container = this.Driver.FindElement(this.ContainerSelector);
                var element = this.Driver.FindElement(this.InputSelector);
                this.ScrollToElement(container);
                if (element.Selected)
                {
                    if (!value)
                    {
                        container.Click();
                    }
                }
                else
                {
                    if (value)
                    {
                        container.Click();
                    }
                }
            }
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MaterialSlideToggle<T> : MaterialSlideToggle where T : Component
    {
        public MaterialSlideToggle(T page, RoleType roleType)
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
