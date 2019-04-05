namespace Angular.Material
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Allors.Meta;

    using Angular;

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
                    var dateTime = DateTime.Parse(dateValue).ToLocalTime().ToUniversalTime();

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

                dateElement.SendKeys(Keys.Tab);

                this.Driver.WaitForAngular();

                var hourElement = this.Driver.FindElements(this.Selector)[1];
                this.ScrollToElement(hourElement);
                hourElement.Clear();
                if (value != null)
                {
                    hourElement.SendKeys(value.Value.Hour.ToString());
                }

                hourElement.SendKeys(Keys.Tab);

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

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MaterialDatetimePicker<T> : MaterialDatetimePicker where T : Page
    {
        public MaterialDatetimePicker(T page, RoleType roleType)
            : base(page.Driver, roleType)
        {
            this.Page = page;
        }

        public T Page { get; }

        public T Set(DateTime value)
        {
            this.Value = value;
            return this.Page;
        }
    }
}
