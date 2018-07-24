namespace Intranet.Tests
{
    using System;

    using Allors.Meta;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class MaterialDatetimePicker
    : Component
    {
        public MaterialDatetimePicker(IWebDriver driver, RoleType roleType)
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
                var dateElement = this.Driver.FindElement(new ByChained(this.Selector, By.XPath("//input[1]")));
                var dateValue = dateElement.GetAttribute("value");
                if (!string.IsNullOrEmpty(dateValue))
                {
                    // TODO: UTC
                    var dateTime = DateTime.Parse(dateValue);

                    var hourElement = this.Driver.FindElement(new ByChained(this.Selector, By.XPath("//input[2]")));
                    var hourValue = hourElement.GetAttribute("value");
                    if (int.TryParse(hourValue, out var hours))
                    {
                        dateTime = dateTime.AddHours(hours);
                    }

                    var minuteElement = this.Driver.FindElement(new ByChained(this.Selector, By.XPath("//input[2]")));
                    var minuteValue = minuteElement.GetAttribute("value");
                    if (int.TryParse(minuteValue, out var minutes))
                    {
                        dateTime = dateTime.AddMinutes(minutes);
                    }

                    return dateTime;
                }

                return null;
            }

            set
            {
                this.Driver.WaitForAngular();

                var dateElement = this.Driver.FindElement(this.Selector);
                this.ScrollToElement(dateElement);
                dateElement.Click();

                this.Driver.WaitForAngular();

                dateElement = this.Driver.FindElement(new ByChained(this.Selector, By.XPath("//input[1]")));
                dateElement.Clear();
                if (value != null)
                {
                    dateElement.SendKeys(value.Value.ToString("d"));
                }

                var hourElement = this.Driver.FindElement(new ByChained(this.Selector, By.XPath("//input[2]")));
                this.ScrollToElement(hourElement);
                hourElement.Clear();
                if (value != null)
                {
                    hourElement.SendKeys(value.Value.Hour.ToString());
                }

                var minuteElement = this.Driver.FindElement(new ByChained(this.Selector, By.XPath("//input[3]")));
                this.ScrollToElement(minuteElement);
                minuteElement.Clear();
                if (value != null)
                {
                    minuteElement.SendKeys(value.Value.Minute.ToString());
                }

                minuteElement.SendKeys(Keys.Tab);
            }
        }
    }
}
