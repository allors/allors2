namespace Angular.Material
{
    using System.Diagnostics.CodeAnalysis;

    using Allors.Meta;

    using Angular;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;

    public class MaterialSlider
    : Directive
    {
        public MaterialSlider(IWebDriver driver, RoleType roleType)
        : base(driver)
        {
            this.Selector = By.CssSelector($"a-mat-slider *[data-allors-roletype='{roleType.IdAsNumberString}'] mat-slider");
        }

        public By Selector { get; }

        public void Select(int min, int max, int value)
        {
            this.Driver.WaitForAngular();
            var element = this.Driver.FindElement(this.Selector);
            this.ScrollToElement(element);

            var width = element.Size.Width;
            var height = element.Size.Height;

            var offsetX = (value - 1) * width / (max - min);
            var offsetY = height / 2;
            new Actions(this.Driver).MoveToElement(element, offsetX, offsetY).Click().Build().Perform();
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MaterialSlider<T> : MaterialSlider where T : Component
    {
        public MaterialSlider(T page, RoleType roleType)
            : base(page.Driver, roleType)
        {
            this.Page = page;
        }

        public T Page { get; }

        public new T Select(int min, int max, int value)
        {
            base.Select(min, max, value);
            return this.Page;
        }
    }
}
