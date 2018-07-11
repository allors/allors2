namespace Intranet.Tests
{
    using Allors.Meta;

    using OpenQA.Selenium;

    public class MaterialInput
    : Component
    {
        public MaterialInput(IWebDriver driver, RoleType roleType)
        : base(driver)
        {
            this.Selector = By.CssSelector($"input[data-allors-roletype='{roleType.IdAsNumberString}']");
        }

        public By Selector { get; }

        public string Text
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
}
