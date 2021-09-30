// <copyright file="MatDatetimePicker.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Allors.Meta;
    using OpenQA.Selenium;

    public class MatDatetimepicker
    : SelectorComponent
    {
        public MatDatetimepicker(IWebDriver driver, RoleType roleType, params string[] scopes)
        : base(driver) =>
            this.Selector = By.XPath($".//a-mat-datetimepicker{this.ByScopesPredicate(scopes)}//*[@data-allors-roletype='{roleType.IdAsString}']//input");

        public override By Selector { get; }

        public DateTime? Value
        {
            get
            {
                this.Driver.WaitForAngular();
                var dateElement = this.Driver.FindElements(this.Selector)[0];
                var dateValue = dateElement.GetAttribute("value");
                if (!string.IsNullOrEmpty(dateValue))
                {
                    var hourElement = this.Driver.FindElements(this.Selector)[1];
                    var hourValue = hourElement.GetAttribute("value");
                    int.TryParse(hourValue, out var hours);

                    var minuteElement = this.Driver.FindElements(this.Selector)[2];
                    var minuteValue = minuteElement.GetAttribute("value");
                    int.TryParse(minuteValue, out var minutes);

                    if (DateTime.TryParse(dateValue, out var date))
                    {
                        var dateTime = new DateTime(date.Year, date.Month, date.Day, hours, minutes, 0);
                        return dateTime.ToUniversalTime();
                    }
                }

                return null;
            }

            set
            {
                value = value?.ToLocalTime();

                this.Driver.WaitForAngular();

                var dateElement = this.Driver.FindElement(this.Selector);
                this.ScrollToElement(dateElement);
                dateElement.Click();

                this.Driver.WaitForAngular();
                dateElement.Clear();

                this.Driver.WaitForAngular();
                dateElement.SendKeys(Keys.Control + "a");
                this.Driver.WaitForAngular();
                dateElement.SendKeys(Keys.Delete);

                if (value != null)
                {
                    this.Driver.WaitForAngular();
                    var text = value.Value.ToString("d");
                    dateElement.SendKeys(text);
                }

                this.Driver.WaitForAngular();
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
    public class MatDatetimepicker<T> : MatDatetimepicker where T : Component
    {
        public MatDatetimepicker(T page, RoleType roleType, params string[] scopes)
            : base(page.Driver, roleType, scopes) =>
            this.Page = page;

        public T Page { get; }

        public T Set(DateTime value)
        {
            this.Value = value;
            return this.Page;
        }
    }
}
