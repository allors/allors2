namespace Components
{
    using System.Diagnostics.CodeAnalysis;
    using Allors.Meta;
    using OpenQA.Selenium;

    public class MatMenu : Component
    {
        public MatMenu(IWebDriver driver, params string[] scopes) : base(driver)
        {
        }

        public void Select(string value)
        {
            this.Driver.WaitForAngular();
            var arrow = this.Driver.FindElement(By.CssSelector($"button[data-allors-action='{value}']"));
            this.ScrollToElement(arrow);
            arrow.Click();
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MatMenu<T> : MatSelect where T : Component
    {
        public MatMenu(T page, RoleType roleType, params string[] scopes)
            : base(page.Driver, roleType, scopes)
        {
            this.Page = page;
        }

        public T Page { get; }

        public new T Toggle(params string[] values)
        {
            base.Toggle(values);
            return this.Page;
        }

        public new T Set(string value)
        {
            base.Value = value;
            return this.Page;
        }
    }
}
