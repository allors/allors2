// <copyright file="ComponentExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Components
{
    using Allors.Meta;
    using OpenQA.Selenium;

    public static partial class ComponentExtensions
    {
        public static MatAutocomplete<T> MatAutocomplete<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatAutocomplete<T>(@this, roleType, scopes);

        public static MatCheckbox<T> MatCheckbox<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatCheckbox<T>(@this, roleType, scopes);

        public static MatChips<T> MatChips<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatChips<T>(@this, roleType, scopes);

        public static MatDatepicker<T> MatDatepicker<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatDatepicker<T>(@this, roleType, scopes);

        public static MatDatetimepicker<T> MatDatetimepicker<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatDatetimepicker<T>(@this, roleType, scopes);

        public static MatFile<T> MatFile<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatFile<T>(@this, roleType, scopes);

        public static MatFiles<T> MatFiles<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatFiles<T>(@this, roleType, scopes);

        public static MatInput<T> MatInput<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatInput<T>(@this, roleType, scopes);

        public static MatList<T> MatList<T>(this T @this, By selector = null) where T : Component => new MatList<T>(@this, selector);

        public static MatLocalised<T> MatLocalised<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatLocalised<T>(@this, roleType, scopes);

        public static MatLocalisedMarkdown<T> MatLocalisedMarkdown<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatLocalisedMarkdown<T>(@this, roleType, scopes);

        public static MatLocalisedText<T> MatLocalisedText<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatLocalisedText<T>(@this, roleType, scopes);

        public static MatMonthpicker<T> MatMonthpicker<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatMonthpicker<T>(@this, roleType, scopes);

        public static MatSelect<T> MatSelect<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatSelect<T>(@this, roleType, scopes);

        public static MatRadiogroup<T> MatRadiogroup<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatRadiogroup<T>(@this, roleType, scopes);

        public static MatSlider<T> MatSlider<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatSlider<T>(@this, roleType, scopes);

        public static MatSlidetoggle<T> MatSlidetoggle<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatSlidetoggle<T>(@this, roleType, scopes);

        public static MatStatic<T> MatStatic<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatStatic<T>(@this, roleType, scopes);

        public static MatTable<T> MatTable<T>(this T @this, By selector = null) where T : Component => new MatTable<T>(@this, selector);

        public static MatTextarea<T> MatTextarea<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatTextarea<T>(@this, roleType, scopes);

        public static MatMarkdown<T> MatMarkdown<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatMarkdown<T>(@this, roleType, scopes);

        public static MatQuill<T> MatQuill<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component => new MatQuill<T>(@this, roleType, scopes);
    }
}
