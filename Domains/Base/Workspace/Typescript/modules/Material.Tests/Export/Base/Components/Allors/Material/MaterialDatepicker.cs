namespace Intranet.Tests
{
    using System;

    using Allors.Meta;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class MaterialDatePicker
    : Component
    {
        public MaterialDatePicker(IWebDriver driver, RoleType roleType)
        : base(driver)
        {
            this.Selector = By.CssSelector($"div[data-allors-roletype='{roleType.IdAsNumberString}']");
        }

        public By Selector { get; }
        
        public DateTime? Date
        {
            get
            {
                this.Driver.WaitForAngular();
                var element = this.Driver.FindElement(new ByChained(this.Selector, By.XPath("//input")));
                var value = element.GetAttribute("value");
                if (!string.IsNullOrEmpty(value))
                {
                    // TODO: UTC
                    return DateTime.Parse(value);
                }

                return null;
            }

            set
            {
                this.Driver.WaitForAngular();
                var element = this.Driver.FindElement(new ByChained(this.Selector, By.XPath("//input")));
                this.ScrollToElement(element);
                element.Clear();
                if (value != null)
                {
                    element.SendKeys(value.Value.ToString("d"));
                }
                element.SendKeys(Keys.Tab);
            }
        }
    }
}
