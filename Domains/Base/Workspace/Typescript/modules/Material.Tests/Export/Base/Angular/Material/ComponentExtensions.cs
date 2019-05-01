namespace Angular.Material
{
    using Allors.Meta;

    using Angular;

    using OpenQA.Selenium;

    public static partial class ComponentExtensions
    {
        public static MaterialAutocomplete<T> MaterialAutocomplete<T>(this T @this, RoleType roleType) where T : Component
        {
            return new MaterialAutocomplete<T>(@this, roleType);
        }

        public static MaterialCheckbox<T> MaterialCheckbox<T>(this T @this, RoleType roleType) where T : Component
        {
            return new MaterialCheckbox<T>(@this, roleType);
        }

        public static MaterialChips<T> MaterialChips<T>(this T @this, RoleType roleType) where T : Component
        {
            return new MaterialChips<T>(@this, roleType);
        }

        public static MaterialDatePicker<T> MaterialDatePicker<T>(this T @this, RoleType roleType) where T : Component
        {
            return new MaterialDatePicker<T>(@this, roleType);
        }

        public static MaterialDatetimePicker<T> MaterialDatetimePicker<T>(this T @this, RoleType roleType) where T : Component
        {
            return new MaterialDatetimePicker<T>(@this, roleType);
        }

        public static MaterialFile<T> MaterialFile<T>(this T @this, RoleType roleType) where T : Component
        {
            return new MaterialFile<T>(@this, roleType);
        }

        public static MaterialFiles<T> MaterialFiles<T>(this T @this, RoleType roleType) where T : Component
        {
            return new MaterialFiles<T>(@this, roleType);
        }

        public static MaterialInput<T> MaterialInput<T>(this T @this, RoleType roleType, params string[] scopes) where T : Component
        {
            return new MaterialInput<T>(@this, roleType, scopes);
        }

        public static MaterialList<T> MaterialList<T>(this T @this, By selector = null) where T : Component
        {
            return new MaterialList<T>(@this, selector);
        }

        public static MaterialMultipleSelect<T> MaterialMultipleSelect<T>(this T @this, RoleType roleType) where T : Component
        {
            return new MaterialMultipleSelect<T>(@this, roleType);
        }

        public static MaterialRadioGroup<T> MaterialRadioGroup<T>(this T @this, RoleType roleType) where T : Component
        {
            return new MaterialRadioGroup<T>(@this, roleType);
        }

        public static MaterialSingleSelect<T> MaterialSingleSelect<T>(this T @this, RoleType roleType) where T : Component
        {
            return new MaterialSingleSelect<T>(@this, roleType);
        }

        public static MaterialSlider<T> MaterialSlider<T>(this T @this, RoleType roleType) where T : Component
        {
            return new MaterialSlider<T>(@this, roleType);
        }

        public static MaterialSlideToggle<T> MaterialSlideToggle<T>(this T @this, RoleType roleType) where T : Component
        {
            return new MaterialSlideToggle<T>(@this, roleType);
        }

        public static MaterialTable<T> MaterialTable<T>(this T @this, By selector = null) where T : Component
        {
            return new MaterialTable<T>(@this, selector);
        }

        public static MaterialTextArea<T> MaterialTextArea<T>(this T @this, RoleType roleType) where T : Component
        {
            return new MaterialTextArea<T>(@this, roleType);
        }
    }
}
