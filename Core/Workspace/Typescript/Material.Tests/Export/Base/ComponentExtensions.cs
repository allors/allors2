// <copyright file="ComponentExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Components
{
    using OpenQA.Selenium;

    public static partial class ComponentExtensions
    {
        public static Anchor<T> Anchor<T>(this T @this, By selector) where T : Component => new Anchor<T>(@this, selector);

        public static Button<T> Button<T>(this T @this, By selector) where T : Component => new Button<T>(@this, selector);

        public static Element<T> Element<T>(this T @this, By selector) where T : Component => new Element<T>(@this, selector);

        public static Input<T> Input<T>(this T @this, params By[] selectors) where T : Component => new Input<T>(@this, selectors);

        // TODO: Remove
        public static Input<T> Input<T>(this T @this, string formControlName) where T : Component => new Input<T>(@this, "formControlName", formControlName);
    }
}
