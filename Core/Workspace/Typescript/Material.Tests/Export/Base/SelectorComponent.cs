// <copyright file="SelectorComponent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Components
{
    using OpenQA.Selenium;

    public abstract class SelectorComponent : Component
    {
        protected SelectorComponent(IWebDriver driver) : base(driver)
        {
        }

        public abstract By Selector { get; }
    }
}
