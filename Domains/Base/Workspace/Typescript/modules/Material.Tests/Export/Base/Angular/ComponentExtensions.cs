namespace Components
{
    using OpenQA.Selenium;

    public static partial class ComponentExtensions
    {
        public static Anchor<T> Anchor<T>(this T @this, By selector) where T : Component
        {
            return new Anchor<T>(@this, selector);
        }

        public static Button<T> Button<T>(this T @this, By selector) where T : Component
        {
            return new Button<T>(@this, selector);
        }

        public static Element<T> Element<T>(this T @this, By selector) where T : Component
        {
            return new Element<T>(@this, selector);
        }

        public static Input<T> Input<T>(this T @this, params By[] selectors) where T : Component
        {
            return new Input<T>(@this, selectors);
        }

        // TODO: Remove
        public static Input<T> Input<T>(this T @this, string formControlName) where T : Component
        {
            return new Input<T>(@this, "formControlName", formControlName);
        }
    }
}
