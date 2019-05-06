
namespace src.allors.material.apps.objects.good.list
{
    using Angular.Html;

    using OpenQA.Selenium;

    using Pages.NonUnifiedGood;

    public partial class GoodListComponent
    {
        public Anchor<GoodListComponent> AddNew => this.Anchor(By.CssSelector("[mat-fab]"));

        public Button<GoodListComponent> NewNonUnifiedGoodButton => this.Button(By.CssSelector("button[data-allors-class='NonUnifiedGood']"));

        public NonUnifiedGoodEditPage NewNonUnifiedGood()
        {
            this.AddNew.Click();

            this.NewNonUnifiedGoodButton.Click();

            return new NonUnifiedGoodEditPage(this.Driver);
        }
    }
}
