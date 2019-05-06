
using src.allors.material.apps.objects.nonunifiedgood.create;

namespace src.allors.material.apps.objects.good.list
{
    using Angular.Html;

    using OpenQA.Selenium;

    public partial class GoodListComponent
    {
        public Anchor<GoodListComponent> AddNew => this.Anchor(By.CssSelector("[mat-fab]"));

        public Button<GoodListComponent> NewNonUnifiedGoodButton => this.Button(By.CssSelector("button[data-allors-class='NonUnifiedGood']"));

        public NonUnifiedGoodCreateComponent NewNonUnifiedGood()
        {
            this.AddNew.Click();

            this.NewNonUnifiedGoodButton.Click();

            return new NonUnifiedGoodCreateComponent(this.Driver);
        }
    }
}
