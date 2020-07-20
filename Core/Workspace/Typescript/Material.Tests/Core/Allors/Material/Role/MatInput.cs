// <copyright file="MatInput.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Xml;
    using Allors.Meta;
    using OpenQA.Selenium;

    public class MatInput : SelectorComponent
    {
        public MatInput(IWebDriver driver, RoleType roleType, params string[] scopes)
        : base(driver)
        {
            this.Selector = By.XPath($".//a-mat-input{this.ByScopesPredicate(scopes)}//*[@data-allors-roletype='{roleType.IdAsString}']//input");
            this.RoleType = roleType;
        }

        public MatInput(IWebDriver driver, By selector, RoleType roleType)
            : base(driver)
        {
            this.Selector = selector;
            this.RoleType = roleType;
        }

        public override By Selector { get; }

        public RoleType RoleType { get; }

        public string Text
        {
            get
            {
                this.Driver.WaitForAngular();
                var element = this.Driver.FindElement(this.Selector);
                return element.GetAttribute("value");
            }

            set
            {
                this.Driver.WaitForAngular();
                var element = this.Driver.FindElement(this.Selector);
                this.ScrollToElement(element);
                element.Clear();
                element.SendKeys(value);
                element.SendKeys(Keys.Tab);
            }
        }

        public object Value
        {
            get
            {
                var locale = this.Driver.Locale();
                var cultureInfo = new CultureInfo(locale);

                var text = this.Text;
                if (text != null)
                {
                    var unit = (Unit)this.RoleType.ObjectType;
                    switch (unit.UnitTag)
                    {
                        case UnitTags.String:
                            return text;
                        case UnitTags.Integer:
                            return Convert.ToInt32(text);
                        case UnitTags.Decimal:
                            return Convert.ToDecimal(text, cultureInfo);
                        case UnitTags.Float:
                            return Convert.ToDouble(text, cultureInfo);
                        case UnitTags.Boolean:
                            return Convert.ToBoolean(text);
                        case UnitTags.DateTime:
                            return XmlConvert.ToDateTime(text, XmlDateTimeSerializationMode.Utc);
                        case UnitTags.Unique:
                            return Guid.Parse(text);
                        case UnitTags.Binary:
                            return Convert.FromBase64String(text);
                        default:
                            throw new ArgumentException("Unknown Unit ObjectType: " + unit);
                    }
                }

                return null;
            }

            set
            {
                if(value is string stringValue)
                {
                    this.Text = stringValue;
                    return;
                }

                var locale = this.Driver.Locale();
                var cultureInfo = new CultureInfo(locale);

                var unit = (Unit)this.RoleType.ObjectType;
                switch (unit.UnitTag)
                {
                    case UnitTags.String:
                        this.Text = (string)value;
                        break;
                    case UnitTags.Integer:
                        this.Text = Convert.ToString((int)value, cultureInfo);
                        break;
                    case UnitTags.Decimal:
                        this.Text = Convert.ToString((decimal)value, cultureInfo);
                        break;
                    case UnitTags.Float:
                        this.Text = Convert.ToString((double)value, cultureInfo);
                        break;
                    case UnitTags.Boolean:
                        this.Text = Convert.ToString((bool)value, cultureInfo);
                        break;
                    case UnitTags.DateTime:
                        this.Text = XmlConvert.ToString((DateTime)value, XmlDateTimeSerializationMode.Utc);
                        break;
                    case UnitTags.Unique:
                        this.Text = Convert.ToString((Guid)value, cultureInfo);
                        break;
                    case UnitTags.Binary:
                        this.Text = Convert.ToBase64String((byte[])value);
                        break;
                    default:
                        throw new ArgumentException("Unknown Unit ObjectType: " + unit);
                }
            }
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MatInput<T> : MatInput where T : Component
    {
        public MatInput(T page, RoleType roleType, params string[] scopes)
            : base(page.Driver, roleType, scopes) =>
            this.Page = page;

        public T Page { get; }

        public T Set(string value)
        {
            this.Value = value;
            return this.Page;
        }
    }
}
