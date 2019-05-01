namespace Angular.Material
{
    using System.Diagnostics.CodeAnalysis;

    using Allors.Meta;

    using Angular;

    using OpenQA.Selenium;

    public class MaterialSlideToggle
    : Directive
    {
        public MaterialSlideToggle(IWebDriver driver, RoleType roleType, params string[] scopes)
        : base(driver)
        {
            var inputXPath = $"//a-mat-slider{this.ByScopePredicate(scopes)}//mat-slide-toggle[@data-allors-roletype='{roleType.IdAsNumberString}']//input";
            this.InputSelector = By.XPath(inputXPath);

            var containerXPath = $"//a-mat-slider{this.ByScopePredicate(scopes)}//mat-slide-toggle[@data-allors-roletype='{roleType.IdAsNumberString}']//input";
            this.ContainerSelector = By.XPath(containerXPath);
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
        public MaterialSlideToggle(T page, RoleType roleType, params string[] scopes)
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
