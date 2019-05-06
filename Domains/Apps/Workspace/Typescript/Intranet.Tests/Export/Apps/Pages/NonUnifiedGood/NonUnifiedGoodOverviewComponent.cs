using Pages;
using src.allors.material.apps.objects.nonunifiedgood.create;

namespace src.allors.material.apps.objects.nonunifiedgood.overview
{
    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    public partial class NonUnifiedGoodOverviewComponent 
    {

        public Button<NonUnifiedGoodOverviewComponent> EditButton => this.Button(By.XPath("//button/span[contains(text(), 'Edit')]"));

        public MaterialList<NonUnifiedGoodOverviewComponent> List => this.MaterialList();

        public NonUnifiedGoodCreateComponent Edit()
        {
            this.EditButton.Click();
            return new NonUnifiedGoodCreateComponent(this.Driver);
        }
    }
}
