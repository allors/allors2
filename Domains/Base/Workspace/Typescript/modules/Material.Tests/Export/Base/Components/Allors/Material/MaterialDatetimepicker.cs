namespace Intranet.Tests
{
    using System;

    using Allors.Meta;

    using OpenQA.Selenium;

    public class MaterialDatetimePicker
    : Component
    {
        public MaterialDatetimePicker(IWebDriver driver, RoleType roleType)
        : base(driver)
        {
            this.Selector = By.CssSelector($"div[data-allors-roletype='{roleType.IdAsNumberString}'] input");
        }

        public By Selector { get; }

         public DateTime? Value
        {
            get
            {
                this.Driver.WaitForAngular();
                var dateElement = this.Driver.FindElements(this.Selector)[0];
                var dateValue = dateElement.GetAttribute("value");
                if (!string.IsNullOrEmpty(dateValue))
                {
                    // TODO: UTC
                    var dateTime = DateTime.Parse(dateValue);

                    var hourElement = this.Driver.FindElements(this.Selector)[1];
                    var hourValue = hourElement.GetAttribute("value");
                    if (int.TryParse(hourValue, out var hours))
                    {
                        dateTime = dateTime.AddHours(hours);
                    }

                    var minuteElement = this.Driver.FindElements(this.Selector)[2];
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

                dateElement = this.Driver.FindElements(this.Selector)[0];
                dateElement.Clear();
                if (value != null)
                {
                    dateElement.SendKeys(value.Value.ToString("d"));
                }

                this.Driver.WaitForAngular();

                var hourElement = this.Driver.FindElements(this.Selector)[1];
                this.ScrollToElement(hourElement);
                hourElement.Clear();
                if (value != null)
                {
                    hourElement.SendKeys(value.Value.Hour.ToString());
                }

                this.Driver.WaitForAngular();

                var minuteElement = this.Driver.FindElements(this.Selector)[2];
                this.ScrollToElement(minuteElement);
                minuteElement.Clear();
                if (value != null)
                {
                    minuteElement.SendKeys(value.Value.Minute.ToString());
                }

                this.Driver.WaitForAngular();

                minuteElement.SendKeys(Keys.Tab);
            }
        }
    }
}
