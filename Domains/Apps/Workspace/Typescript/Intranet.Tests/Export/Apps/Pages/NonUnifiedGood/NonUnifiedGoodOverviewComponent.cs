using Pages;
using src.allors.material.apps.objects.nonunifiedgood.create;

namespace src.allors.material.apps.objects.nonunifiedgood.overview
{
    using Components;

    using OpenQA.Selenium;

    public partial class NonUnifiedGoodOverviewComponent 
    {

        public Button<NonUnifiedGoodOverviewComponent> EditButton => this.Button(By.XPath("//button/span[contains(text(), 'Edit')]"));

        public MatList<NonUnifiedGoodOverviewComponent> List => this.MatList();

        public NonUnifiedGoodCreateComponent Edit()
        {
            this.EditButton.Click();
            return new NonUnifiedGoodCreateComponent(this.Driver);
        }
    }
}
