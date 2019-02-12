namespace Pages.PersonTests
{
    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class GoodOverviewPage : MainPage
    {
        public GoodOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Button<GoodOverviewPage> EditButton => this.Button(By.XPath("//button/span[contains(text(), 'Edit')]"));

        public MaterialList<GoodOverviewPage> List => this.MaterialList();

        public GoodEditPage Edit()
        {
            this.EditButton.Click();
            return new GoodEditPage(this.Driver);
        }
    }
}
