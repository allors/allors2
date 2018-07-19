namespace Intranet.Tests
{
    using OpenQA.Selenium;

    public class MaterialListItem : Component
    {
        public MaterialListItem(IWebDriver driver, IWebElement element)
        : base(driver)
        {
            this.Element = element;
        }

        public IWebElement Element { get; }

        public void Click()
        {
            this.Driver.WaitForAngular();
            this.Element.Click();
        }
    }
}
