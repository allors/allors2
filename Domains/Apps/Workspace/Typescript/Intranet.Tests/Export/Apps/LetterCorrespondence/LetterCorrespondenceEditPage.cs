namespace Tests.Intranet.LetterCorrespondenceTests
{
    using Allors.Meta;

    using OpenQA.Selenium;

    using Tests.Components.Html;
    using Tests.Components.Material;

    public class LetterCorrespondenceEditPage : MainPage
    {
        public LetterCorrespondenceEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialSingleSelect EventState => new MaterialSingleSelect(this.Driver, roleType: M.CommunicationEvent.CommunicationEventState);

        public MaterialMultipleSelect Purposes => new MaterialMultipleSelect(this.Driver, roleType: M.CommunicationEvent.EventPurposes);

        public MaterialSlideToggle IncomingLetter => new MaterialSlideToggle(this.Driver, roleType: M.LetterCorrespondence.IncomingLetter);

        public MaterialSingleSelect PostalAddress => new MaterialSingleSelect(this.Driver, roleType: M.LetterCorrespondence.PostalAddress);

        public MaterialChips Originators => new MaterialChips(this.Driver, roleType: M.LetterCorrespondence.Originators);

        public MaterialChips Receivers => new MaterialChips(this.Driver, roleType: M.LetterCorrespondence.Receivers);

        public MaterialInput Subject => new MaterialInput(this.Driver, roleType: M.CommunicationEvent.Subject);

        public MaterialDatetimePicker ScheduledStart => new MaterialDatetimePicker(this.Driver, roleType: M.CommunicationEvent.ScheduledStart);

        public MaterialDatetimePicker ScheduledEnd => new MaterialDatetimePicker(this.Driver, roleType: M.CommunicationEvent.ScheduledEnd);

        public MaterialDatetimePicker ActualStart => new MaterialDatetimePicker(this.Driver, roleType: M.CommunicationEvent.ActualStart);

        public MaterialDatetimePicker ActualEnd => new MaterialDatetimePicker(this.Driver, roleType: M.CommunicationEvent.ActualEnd);

        public MaterialTextArea Comment => new MaterialTextArea(this.Driver, roleType: M.CommunicationEvent.Comment);

        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor List => new Anchor(this.Driver, By.CssSelector("a[href='/contacts/people']"));

        public Button NewCommunicationEvent => new Button(this.Driver, By.CssSelector("div[data-allors-class='communicationevent']"));
    }
}
