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

        public Button EditButton => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'Edit')]"));

        public MaterialList List => new MaterialList(this.Driver);

        public GoodEditPage Edit()
        {
            this.EditButton.Click();
            return new GoodEditPage(this.Driver);
        }
    }
}
