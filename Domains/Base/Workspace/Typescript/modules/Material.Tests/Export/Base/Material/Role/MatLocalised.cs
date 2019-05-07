namespace Components
{
    using System.Diagnostics.CodeAnalysis;
    using Allors.Meta;
    using OpenQA.Selenium;

    public class MatLocalised : Component
    {
        public MatLocalised(IWebDriver driver, RoleType roleType, params string[] scopes)
        : base(driver)
        {
            var xpath = $"//a-mat-static{this.ByScopesPredicate(scopes)}//*[@data-allors-roletype='{roleType.IdAsNumberString}']";
            this.Selector = By.XPath(xpath);
        }

        public MatLocalised(IWebDriver driver, By selector)
            : base(driver)
        {
            this.Selector = selector;
        }

        public By Selector { get; }

        // TODO:
        public string Value
        {
            get
            {
                this.Driver.WaitForAngular();
                var element = this.Driver.FindElement(this.Selector);
                return element.Text;
            }
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MatLocalised<T> : MatInput where T : Component
    {
        public MatLocalised(T page, RoleType roleType, params string[] scopes)
            : base(page.Driver, roleType, scopes)
        {
            this.Page = page;
        }

        public T Page { get; }

        public T Set(string value)
        {
            this.Value = value;
            return this.Page;
        }
    }
}
