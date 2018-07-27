namespace Intranet.Tests
{
    using System.Threading;

    using Allors.Meta;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;

    public class MaterialSlider
    : Component
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
}
