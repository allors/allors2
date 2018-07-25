namespace Intranet.Pages.Relations
{
    using Allors.Meta;

    using Intranet.Tests;

    using OpenQA.Selenium;

    public class FormPage : MainPage
    {
        public FormPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialInput String => new MaterialInput(this.Driver, roleType: M.Data.String);

        public MaterialDatePicker Date => new MaterialDatePicker(this.Driver, roleType: M.Data.Date);

        public MaterialDatetimePicker Datetime => new MaterialDatetimePicker(this.Driver, roleType: M.Data.DateTime);


        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));
    }
}
