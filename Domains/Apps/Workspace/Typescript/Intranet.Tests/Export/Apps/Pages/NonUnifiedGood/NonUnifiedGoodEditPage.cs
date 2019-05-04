namespace Pages.NonUnifiedGood
{
    using Allors.Meta;

    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    public class NonUnifiedGoodEditPage : MainPage
    {
        public NonUnifiedGoodEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialInput<NonUnifiedGoodEditPage> ProductNumber => this.MaterialInput(roleType: M.ProductNumber.Identification);

        public MaterialInput<NonUnifiedGoodEditPage> Name => this.MaterialInput(roleType: M.Good.Name);

        public MaterialTextArea<NonUnifiedGoodEditPage> Description => this.MaterialTextArea(roleType: M.Good.Description);

        public MaterialDatePicker<NonUnifiedGoodEditPage> SalesDiscontinuationDate  => this.MaterialDatePicker(roleType: M.Good.SalesDiscontinuationDate);

        public MaterialSelect<NonUnifiedGoodEditPage> Part => this.MaterialSelect(roleType: M.NonUnifiedGood.Part);

        public MaterialSelect<NonUnifiedGoodEditPage> Brand => this.MaterialSelect(roleType: M.Part.Brand);

        public MaterialSelect<NonUnifiedGoodEditPage> Model => this.MaterialSelect(roleType: M.Part.Model);

        public Button<NonUnifiedGoodEditPage> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Button<NonUnifiedGoodEditPage> Cancel => this.Button(By.XPath("//button/span[contains(text(), 'CANCEL')]"));
    }
}
