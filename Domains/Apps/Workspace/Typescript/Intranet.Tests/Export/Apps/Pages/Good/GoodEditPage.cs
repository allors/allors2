namespace Pages.PersonTests
{
    using Allors.Meta;

    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

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

        public MaterialSingleSelect Part => new MaterialSingleSelect(this.Driver, roleType: M.Good.Part);

        public MaterialSingleSelect Brand => new MaterialSingleSelect(this.Driver, roleType: M.Part.Brand);

        public MaterialSingleSelect Model => new MaterialSingleSelect(this.Driver, roleType: M.Part.Model);

        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Button Cancel => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'CANCEL')]"));

    }
}
