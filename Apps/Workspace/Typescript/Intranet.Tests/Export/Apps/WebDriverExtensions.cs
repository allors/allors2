namespace Components
{
    using System;
    using System.Linq;
    using System.Reflection;
    using OpenQA.Selenium;

    public static partial class WebDriverExtensions
    {
        public static Component GetDialog(this IWebDriver @this)
        {
            @this.WaitForAngular();
            var dialogElement = @this.FindElement(By.CssSelector("mat-dialog-container ng-component[data-test-scope]"));
            var testScope = dialogElement.GetAttribute("data-test-scope");
            var type = Assembly.GetExecutingAssembly().GetTypes().First(v => v.Name.Equals(testScope));
            var dialog = (Component)Activator.CreateInstance(type, @this);
            return dialog;
        }
    }
}
