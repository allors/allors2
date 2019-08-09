namespace Components
{
    using OpenQA.Selenium;

    public abstract class SelectorComponent : Component
    {
        protected SelectorComponent(IWebDriver driver) : base(driver)
        {
        }

        public abstract By Selector { get; }
    }
}
