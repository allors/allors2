namespace Intranet.Tests
{
    using OpenQA.Selenium;

    public class Anchor : Component
    {
        public Anchor(IWebDriver driver, By selector)
        : base(driver)
        {
            this.Selector = selector;
        }

        public By Selector { get; }

        public bool IsVisble => this.SelectorIsVisible(this.Selector);

        public void Click()
        {
            this.Driver.WaitForAngular();
            var element = this.Driver.FindElement(this.Selector);
            this.ScrollToElement(element);
            element.Click();
        }
    }
}
