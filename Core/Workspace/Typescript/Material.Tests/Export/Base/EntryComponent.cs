namespace Components
{
    using OpenQA.Selenium;

    public abstract class EntryComponent : Component
    {
        protected EntryComponent(IWebDriver driver) : base(driver)
        {
        }
    }
}
