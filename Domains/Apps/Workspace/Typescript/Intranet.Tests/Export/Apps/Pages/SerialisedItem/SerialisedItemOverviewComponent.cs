using src.app.main;

namespace Pages.SerialisedItemTests
{
    using Components;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class SerialisedItemOverviewComponent : MainComponent
    {
        public SerialisedItemOverviewComponent(IWebDriver driver)
            : base(driver)
        {
        }

        public Element<SerialisedItemOverviewComponent> DetailPanel => this.Element(By.CssSelector("div[data-allors-panel='detail']"));

        public MatTable<SerialisedItemOverviewComponent> Table => this.MatTable();

        public Anchor<SerialisedItemOverviewComponent> AddNew => this.Anchor(By.CssSelector("[mat-fab]"));

        public SerialisedItemEditComponent Edit()
        {
            this.DetailPanel.Click();
            return new SerialisedItemEditComponent(this.Driver);
        }
    }
}
