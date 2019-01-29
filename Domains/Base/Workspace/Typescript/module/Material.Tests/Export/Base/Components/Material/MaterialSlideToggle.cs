namespace Tests.Components.Material
{
    using Allors.Meta;

    using OpenQA.Selenium;

    public class MaterialSlideToggle
    : Component
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
}
