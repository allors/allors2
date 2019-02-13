namespace Pages.NonUnifiedGood
{
    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    public class NonUnifiedGoodOverviewPage : MainPage
    {
        public NonUnifiedGoodOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Button<NonUnifiedGoodOverviewPage> EditButton => this.Button(By.XPath("//button/span[contains(text(), 'Edit')]"));

        public MaterialList<NonUnifiedGoodOverviewPage> List => this.MaterialList();

        public NonUnifiedGoodEditPage Edit()
        {
            this.EditButton.Click();
            return new NonUnifiedGoodEditPage(this.Driver);
        }
    }
}
