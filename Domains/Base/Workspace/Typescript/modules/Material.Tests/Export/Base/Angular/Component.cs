namespace Angular
{
    using OpenQA.Selenium;

    public abstract class Component : Directive
    {
        protected Component(IWebDriver driver) : base(driver)
        {
        }
    }
}
