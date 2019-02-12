namespace Tests.Intranet.PersonTests
{
    using Allors.Meta;

    using OpenQA.Selenium;

    using Tests.Components.Html;
    using Tests.Components.Material;

    public class NonUnifiedGoodEditPage : MainPage
    {
        public NonUnifiedGoodEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialInput ProductNumber => new MaterialInput(this.Driver, roleType: M.ProductNumber.Identification);

        public MaterialInput Name => new MaterialInput(this.Driver, roleType: M.Good.Name);

        public MaterialTextArea Description => new MaterialTextArea(this.Driver, roleType: M.Good.Description);

        public MaterialDatePicker SalesDiscontinuationDate  => new MaterialDatePicker(this.Driver, roleType: M.Good.SalesDiscontinuationDate);

        public MaterialSingleSelect Part => new MaterialSingleSelect(this.Driver, roleType: M.NonUnifiedGood.Part);

        public MaterialSingleSelect Brand => new MaterialSingleSelect(this.Driver, roleType: M.Part.Brand);

        public MaterialSingleSelect Model => new MaterialSingleSelect(this.Driver, roleType: M.Part.Model);

        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Button Cancel => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'CANCEL')]"));

    }
}
