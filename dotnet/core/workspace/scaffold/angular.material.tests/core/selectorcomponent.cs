// <copyright file="SelectorComponent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using System.Collections.Generic;
    using System.Linq;
    using OpenQA.Selenium;

    public abstract class SelectorComponent : Component
    {
        private static readonly char[] CssEscapeCharacters = new char[] { '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', ':', ';', '<', '=', '>', '?', '@', '[', '\\', ']', '^', '`', '{', '|', '}', '~' };
        private static readonly IDictionary<char, string> CssReplacements = CssEscapeCharacters.ToDictionary(v => v, v => $"\\{v}");

        protected SelectorComponent(IWebDriver driver) : base(driver)
        {
        }

        public abstract By Selector { get; }


        public string CssEscape(string value) => string.Join("", value.Select(v => CssReplacements.TryGetValue(v, out var replacement) ? replacement : v.ToString()));
    }
}
