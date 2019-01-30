namespace Tests.Components.Material
{
    using Allors.Meta;

    using OpenQA.Selenium;

    public class MaterialTextArea : Component
    {
        public MaterialTextArea(IWebDriver driver, RoleType roleType)
        : base(driver)
        {
            this.Selector = By.CssSelector($"textarea[data-allors-roletype='{roleType.IdAsNumberString}']");
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
}
