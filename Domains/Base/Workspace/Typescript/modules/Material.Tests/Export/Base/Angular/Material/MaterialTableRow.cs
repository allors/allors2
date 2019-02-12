namespace Angular.Material
{
    using System.Diagnostics.CodeAnalysis;

    using OpenQA.Selenium;

    using Angular;
    

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
            var cell = this.TableCell(name);
            return new MaterialTableCell(this.Driver, cell);
        }

        protected IWebElement TableCell(string name)
        {
            this.Driver.WaitForAngular();

            var cellPath = By.CssSelector($"td.mat-column-{name}");
            var cell = this.Element.FindElement(cellPath);
            return cell;
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MaterialTableRow<T> : MaterialTableRow where T : Page
    {
        public MaterialTableRow(T page, IWebElement element)
            : base(page.Driver, element)
        {
            this.Page = page;
        }

        public T Page { get; }

        public new MaterialTableCell<T> FindCell(string name)
        {
            var cell = this.TableCell(name);
            return new MaterialTableCell<T>(this.Page, cell);
        }
    }
}
