namespace Pages.ProductTest
{
    using Angular.Html;

    using OpenQA.Selenium;

    using Pages.NonUnifiedGood;

    public class GoodListPage : MainPage
    {
        public GoodListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input<GoodListPage> Name => this.Input(formControlName: "name");

        public Anchor<GoodListPage> AddNew => this.Anchor(By.CssSelector("[mat-fab]"));

        public Button<GoodListPage> NewNonUnifiedGoodButton => this.Button(By.CssSelector("button[data-allors-class='NonUnifiedGood']"));

        public NonUnifiedGoodEditPage NewNonUnifiedGood()
        {
            this.AddNew.Click();

            this.NewNonUnifiedGoodButton.Click();

            return new NonUnifiedGoodEditPage(this.Driver);
        }
    }
}
