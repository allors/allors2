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

        public Element<SerialisedItemOverviewPage> DetailPanel => this.Element(By.CssSelector("div[data-allors-panel='detail']"));

        public MaterialTable<SerialisedItemOverviewPage> Table => this.MaterialTable();

        public Anchor<SerialisedItemOverviewPage> AddNew => this.Anchor(By.CssSelector("[mat-fab]"));

        public SerialisedItemEditPage Edit()
        {
            this.DetailPanel.Click();
            return new SerialisedItemEditPage(this.Driver);
        }
    }
}
