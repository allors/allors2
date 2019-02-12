namespace Tests.Intranet.PersonTests
{
    using Allors.Domain;

    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;
    using Tests.Components.Material;

    public class NonUnifiedGoodOverviewPage : MainPage
    {
        public NonUnifiedGoodOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Button EditButton => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'Edit')]"));

        public MaterialList List => new MaterialList(this.Driver);

        public NonUnifiedGoodEditPage Edit()
        {
            this.EditButton.Click();
            return new NonUnifiedGoodEditPage(this.Driver);
        }
    }
}
