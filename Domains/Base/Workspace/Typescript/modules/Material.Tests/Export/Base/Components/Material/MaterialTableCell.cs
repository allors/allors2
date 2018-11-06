namespace Tests.Components.Material
{
    using OpenQA.Selenium;

    public class MaterialTableCell : Component
    {
        public MaterialTableCell(IWebDriver driver, IWebElement element)
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
