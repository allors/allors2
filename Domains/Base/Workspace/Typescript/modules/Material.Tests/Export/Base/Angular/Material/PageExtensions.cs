namespace Angular.Material
{
    using Allors.Meta;

    using OpenQA.Selenium;

    using Angular;
    

    public static partial class PageExtensions
    {
        public static MaterialAutocomplete<T> MaterialAutocomplete<T>(this T @this, RoleType roleType) where T : Page
        {
            return new MaterialAutocomplete<T>(@this, roleType);
        }

        public static MaterialCheckbox<T> MaterialCheckbox<T>(this T @this, RoleType roleType) where T : Page
        {
            return new MaterialCheckbox<T>(@this, roleType);
        }

        public static MaterialChips<T> MaterialChips<T>(this T @this, RoleType roleType) where T : Page
        {
            return new MaterialChips<T>(@this, roleType);
        }

        public static MaterialDatePicker<T> MaterialDatePicker<T>(this T @this, RoleType roleType) where T : Page
        {
            return new MaterialDatePicker<T>(@this, roleType);
        }

        public static MaterialDatetimePicker<T> MaterialDatetimePicker<T>(this T @this, RoleType roleType) where T : Page
        {
            return new MaterialDatetimePicker<T>(@this, roleType);
        }

        public static MaterialFile<T> MaterialFile<T>(this T @this, RoleType roleType) where T : Page
        {
            return new MaterialFile<T>(@this, roleType);
        }

        public static MaterialFiles<T> MaterialFiles<T>(this T @this, RoleType roleType) where T : Page
        {
            return new MaterialFiles<T>(@this, roleType);
        }

        public static MaterialInput<T> MaterialInput<T>(this T @this, RoleType roleType) where T : Page
        {
            return new MaterialInput<T>(@this, roleType);
        }

        public static MaterialList<T> MaterialList<T>(this T @this, By selector = null) where T : Page
        {
            return new MaterialList<T>(@this, selector);
        }

        public static MaterialMultipleSelect<T> MaterialMultipleSelect<T>(this T @this, RoleType roleType) where T : Page
        {
            return new MaterialMultipleSelect<T>(@this, roleType);
        }

        public static MaterialRadioGroup<T> MaterialRadioGroup<T>(this T @this, RoleType roleType) where T : Page
        {
            return new MaterialRadioGroup<T>(@this, roleType);
        }

        public static MaterialSingleSelect<T> MaterialSingleSelect<T>(this T @this, RoleType roleType) where T : Page
        {
            return new MaterialSingleSelect<T>(@this, roleType);
        }

        public static MaterialSlider<T> MaterialSlider<T>(this T @this, RoleType roleType) where T : Page
        {
            return new MaterialSlider<T>(@this, roleType);
        }

        public static MaterialTable<T> MaterialTable<T>(this T @this, By selector = null) where T : Page
        {
            return new MaterialTable<T>(@this, selector);
        }

        public static MaterialTextArea<T> MaterialTextArea<T>(this T @this, RoleType roleType) where T : Page
        {
            return new MaterialTextArea<T>(@this, roleType);
        }
    }
}
