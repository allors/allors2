namespace Intranet.Tests
{
    using OpenQA.Selenium;

    public class MaterialTableRow : Component
    {
        public MaterialTableRow(IWebDriver driver, IWebElement element)
        : base(driver)
        {
            this.Element = element;
        }

        public IWebElement Element { get; }

        public MaterialTableCell FindCell(string name)
        {
            this.Driver.WaitForAngular();

            var cellPath = By.CssSelector($"td.mat-column-{name}");
            var cell = this.Element.FindElement(cellPath);

            return new MaterialTableCell(this.Driver, cell);
        }

    }
}
