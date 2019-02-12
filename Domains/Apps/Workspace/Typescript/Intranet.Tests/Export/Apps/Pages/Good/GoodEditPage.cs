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

        public MaterialInput<GoodEditPage> ProductNumber => this.MaterialInput(roleType: M.ProductNumber.Identification);

        public MaterialInput<GoodEditPage> Name => this.MaterialInput(roleType: M.Good.Name);

        public MaterialTextArea<GoodEditPage> Description => this.MaterialTextArea(roleType: M.Good.Description);

        public MaterialDatePicker<GoodEditPage> SalesDiscontinuationDate => this.MaterialDatePicker(roleType: M.Good.SalesDiscontinuationDate);

        public MaterialSingleSelect<GoodEditPage> Part => this.MaterialSingleSelect(roleType: M.Good.Part);

        public MaterialSingleSelect<GoodEditPage> Brand => this.MaterialSingleSelect(roleType: M.Part.Brand);

        public MaterialSingleSelect<GoodEditPage> Model => this.MaterialSingleSelect(roleType: M.Part.Model);

        public Button<GoodEditPage> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Button<GoodEditPage> Cancel => this.Button(By.XPath("//button/span[contains(text(), 'CANCEL')]"));
    }
}
