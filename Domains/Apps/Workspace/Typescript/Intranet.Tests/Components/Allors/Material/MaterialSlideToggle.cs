namespace Intranet.Tests
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

                var element = this.Driver.FindElement(this.ContainerSelector);
                this.ScrollToElement(element);
                if (element.Selected)
                {
                    if (!value)
                    {
                        element.Click();
                    }
                }
                else
                {
                    if (value)
                    {
                        element.Click();
                    }
                }

                this.Driver.WaitForAngular();
            }
        }
    }
}
