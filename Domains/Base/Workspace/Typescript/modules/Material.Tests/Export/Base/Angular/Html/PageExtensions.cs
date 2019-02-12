namespace Angular.Html
{

    using OpenQA.Selenium;

    public static partial class PageExtensions
    {
        public static Anchor<T> Anchor<T>(this T @this, By selector) where T : Page
        {
            return new Anchor<T>(@this, selector);
        }

        public static Button<T> Button<T>(this T @this, By selector) where T : Page
        {
            return new Button<T>(@this, selector);
        }

        public static Element<T> Element<T>(this T @this, By selector) where T : Page
        {
            return new Element<T>(@this, selector);
        }

        public static Input<T> Input<T>(this T @this, By selector = null, string formControlName = null) where T : Page
        {
            return new Input<T>(@this, selector, formControlName);
        }
    }
}
