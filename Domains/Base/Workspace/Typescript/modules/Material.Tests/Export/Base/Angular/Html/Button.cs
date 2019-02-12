namespace Angular.Html
{
    using System.Diagnostics.CodeAnalysis;

    using OpenQA.Selenium;

    

    public class Button : Component
    {
        public Button(IWebDriver driver, By selector)
        : base(driver)
        {
            this.Selector = selector;
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
    public class Button<T> : Button where T : Page
    {
        public Button(T page, By selector)
            : base(page.Driver, selector)
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
