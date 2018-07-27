namespace Intranet.Pages.Relations
{
    using Allors.Meta;

    using Intranet.Tests;
    using Intranet.Tests.Relations;

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

        public MaterialAutocomplete AutoCompleteFilter => new MaterialAutocomplete(this.Driver, roleType: M.Data.AutocompleteFilter);

        public MaterialAutocomplete AutoCompleteOptions => new MaterialAutocomplete(this.Driver, roleType: M.Data.AutocompleteOptions);

        public MaterialCheckbox Checkbox => new MaterialCheckbox(this.Driver, roleType: M.Data.Checkbox);

        public MaterialChips Chips => new MaterialChips(this.Driver, roleType: M.Data.Chips);

        public MaterialFile File => new MaterialFile(this.Driver, roleType: M.Data.File);

        public MaterialFiles Files => new MaterialFiles(this.Driver, roleType: M.Data.MultipleFiles);

        public MaterialRadioGroup RadioGroup => new MaterialRadioGroup(this.Driver, roleType: M.Data.RadioGroup);

        public MaterialSlider Slider => new MaterialSlider(this.Driver, roleType: M.Data.Slider);

        public MaterialSlideToggle SlideToggle => new MaterialSlideToggle(this.Driver, roleType: M.Data.SlideToggle);

        public MaterialTextArea TextArea => new MaterialTextArea(this.Driver, roleType: M.Data.TextArea);

        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));
    }
}
