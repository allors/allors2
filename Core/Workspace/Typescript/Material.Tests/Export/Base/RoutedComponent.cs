// <copyright file="RoutedComponent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Components
{
    using OpenQA.Selenium;

    public abstract class RoutedComponent : Component
    {
        protected RoutedComponent(IWebDriver driver) : base(driver)
        {
        }
    }
}
