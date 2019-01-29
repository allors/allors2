namespace Tests.Components.Html
{
    using OpenQA.Selenium;

    public class Input : Component
    {
        public Input(IWebDriver driver, By selector = null, string formControlName = null)
            : base(driver)
        {
            if (selector != null)
            {
                this.Selector = selector;
            }
            else if (formControlName != null)
            {
                this.Selector = By.CssSelector($"input[formcontrolname='{formControlName}']");
            }
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