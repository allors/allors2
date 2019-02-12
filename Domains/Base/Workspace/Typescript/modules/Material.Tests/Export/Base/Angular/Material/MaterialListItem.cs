namespace Angular.Material
{
    using System.Diagnostics.CodeAnalysis;

    using Angular;

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

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MaterialListItem<T> : MaterialListItem where T : Page
    {
        public MaterialListItem(T page, IWebElement element)
            : base(page.Driver, element)
        {
            this.Page = page;
        }

        public T Page { get; }

        public new T Click()
        {
            base.Click();
            return this.Page;
        }
    }
}
