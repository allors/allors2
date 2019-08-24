// <copyright file="SelectorComponent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
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
