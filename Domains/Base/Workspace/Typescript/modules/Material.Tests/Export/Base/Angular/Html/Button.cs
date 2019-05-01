using OpenQA.Selenium.Support.PageObjects;

namespace Angular.Html
{
    using System.Diagnostics.CodeAnalysis;

    using OpenQA.Selenium;

    public class Button : Component
    {
        public Button(IWebDriver driver, params By[] selectors)
        : base(driver)
        {
            this.Selector = selectors.Length == 1 ? selectors[0] : new ByChained(selectors);
        }

        public By Selector { get; }

        public void Click()
        {
            this.Driver.WaitForAngular();
            var element = this.Driver.FindElement(this.Selector);
            this.ScrollToElement(element);
            element.Click();
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class Button<T> : Button where T : Component
    {
        public Button(T page, params By[] selectors)
            : base(page.Driver, selectors)
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
