namespace Pages.SerialisedItemTests
{
    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class SerialisedItemOverviewPage : MainPage
    {
        public SerialisedItemOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Element DetailPanel => new Element(this.Driver, By.CssSelector("div[data-allors-panel='detail']"));

        public MaterialTable Table => new MaterialTable(this.Driver);

        public Anchor AddNew => new Anchor(this.Driver, By.CssSelector("[mat-fab]"));

        public SerialisedItemEditPage Edit()
        {
            this.DetailPanel.Click();
            return new SerialisedItemEditPage(this.Driver);
        }
    }
}
