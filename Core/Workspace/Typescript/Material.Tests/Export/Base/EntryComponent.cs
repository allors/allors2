// <copyright file="EntryComponent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Components
{
    using OpenQA.Selenium;

    public abstract class EntryComponent : Component
    {
        protected EntryComponent(IWebDriver driver) : base(driver)
        {
        }
    }
}
