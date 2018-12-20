namespace Tests.Intranet.PersonTests
{
    using Allors.Meta;

    using OpenQA.Selenium;

    using Tests.Components.Html;
    using Tests.Components.Material;

    public class GoodEditPage : MainPage
    {
        public GoodEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialInput ProductNumber => new MaterialInput(this.Driver, roleType: M.ProductNumber.Identification);

        public MaterialInput Name => new MaterialInput(this.Driver, roleType: M.Good.Name);

        public MaterialTextArea Description => new MaterialTextArea(this.Driver, roleType: M.Good.Description);

        public MaterialDatePicker SalesDiscontinuationDate  => new MaterialDatePicker(this.Driver, roleType: M.Good.SalesDiscontinuationDate);

        public MaterialSelect Part => new MaterialSelect(this.Driver, roleType: M.Good.Part);

        public MaterialSelect Brand => new MaterialSelect(this.Driver, roleType: M.Part.Brand);

        public MaterialSelect Model => new MaterialSelect(this.Driver, roleType: M.Part.Model);

        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Button Cancel => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'CANCEL')]"));

    }
}
