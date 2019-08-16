// <copyright file="Element.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Components
{
    using System.Diagnostics.CodeAnalysis;

    using OpenQA.Selenium;

    public class Element : SelectorComponent
    {
        public Element(IWebDriver driver, By selector)
        : base(driver) =>
            this.Selector = selector;

        public override By Selector { get; }

        public bool IsVisible => this.Driver.SelectorIsVisible(this.Selector);

        public void Click()
        {
            this.Driver.WaitForAngular();
            var element = this.Driver.FindElement(this.Selector);
            this.ScrollToElement(element);
            element.Click();
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class Element<T> : Element where T : Component
    {
        public Element(T page, By selector)
            : base(page.Driver, selector) =>
            this.Page = page;

        public T Page { get; }

        public new T Click()
        {
            base.Click();
            return this.Page;
        }
    }
}
