namespace Components
{
    using OpenQA.Selenium;

    public abstract class RoutedComponent : Component
    {
        protected RoutedComponent(IWebDriver driver) : base(driver)
        {
        }
    }
}
