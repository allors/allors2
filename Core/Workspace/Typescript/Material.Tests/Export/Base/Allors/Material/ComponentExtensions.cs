// <copyright file="ComponentExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Components
{
    using Allors.Meta;
    using OpenQA.Selenium;

    public static partial class ComponentExtensions
    {
        public static MatAutocomplete<T> MatAutocomplete<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component
        {
            return new MatAutocomplete<T>(@this, roleType, scopes);
        }

        public static MatCheckbox<T> MatCheckbox<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component
        {
            return new MatCheckbox<T>(@this, roleType, scopes);
        }

        public static MatChips<T> MatChips<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component
        {
            return new MatChips<T>(@this, roleType, scopes);
        }

        public static MatDatepicker<T> MatDatepicker<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component
        {
            return new MatDatepicker<T>(@this, roleType, scopes);
        }

        public static MatDatetimepicker<T> MatDatetimepicker<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component
        {
            return new MatDatetimepicker<T>(@this, roleType, scopes);
        }

        public static MatFile<T> MatFile<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component
        {
            return new MatFile<T>(@this, roleType, scopes);
        }

        public static MatFiles<T> MatFiles<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component
        {
            return new MatFiles<T>(@this, roleType, scopes);
        }

        public static MatInput<T> MatInput<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component
        {
            return new MatInput<T>(@this, roleType, scopes);
        }

        public static MatList<T> MatList<T>(this T @this, By selector = null) where T : Component
        {
            return new MatList<T>(@this, selector);
        }

        public static MatLocalised<T> MatLocalised<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component
        {
            return new MatLocalised<T>(@this, roleType, scopes);
        }

        public static MatSelect<T> MatSelect<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component
        {
            return new MatSelect<T>(@this, roleType, scopes);
        }

        public static MatRadiogroup<T> MatRadiogroup<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component
        {
            return new MatRadiogroup<T>(@this, roleType, scopes);
        }

        public static MatSlider<T> MatSlider<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component
        {
            return new MatSlider<T>(@this, roleType, scopes);
        }

        public static MatSlidetoggle<T> MatSlidetoggle<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component
        {
            return new MatSlidetoggle<T>(@this, roleType, scopes);
        }

        public static MatStatic<T> MatStatic<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component
        {
            return new MatStatic<T>(@this, roleType, scopes);
        }

        public static MatTable<T> MatTable<T>(this T @this, By selector = null) where T : Component
        {
            return new MatTable<T>(@this, selector);
        }

        public static MatTextarea<T> MatTextarea<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component
        {
            return new MatTextarea<T>(@this, roleType, scopes);
        }
    }
}
