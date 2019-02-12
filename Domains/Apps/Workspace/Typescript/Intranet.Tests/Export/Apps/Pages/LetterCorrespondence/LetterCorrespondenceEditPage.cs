namespace Pages.LetterCorrespondenceTests
{
    using Allors.Meta;

    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class LetterCorrespondenceEditPage : MainPage
    {
        public LetterCorrespondenceEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialSingleSelect<LetterCorrespondenceEditPage> EventState => this.MaterialSingleSelect(roleType: M.CommunicationEvent.CommunicationEventState);

        public MaterialMultipleSelect<LetterCorrespondenceEditPage> Purposes => this.MaterialMultipleSelect(roleType: M.CommunicationEvent.EventPurposes);

        public MaterialSingleSelect<LetterCorrespondenceEditPage> PostalAddress => this.MaterialSingleSelect(roleType: M.LetterCorrespondence.PostalAddress);

        public MaterialSingleSelect<LetterCorrespondenceEditPage> FromParty => this.MaterialSingleSelect(roleType: M.LetterCorrespondence.FromParty);

        public MaterialSingleSelect<LetterCorrespondenceEditPage> ToParty => this.MaterialSingleSelect(roleType: M.LetterCorrespondence.ToParty);

        public MaterialInput<LetterCorrespondenceEditPage> Subject => this.MaterialInput(roleType: M.CommunicationEvent.Subject);

        public MaterialDatetimePicker<LetterCorrespondenceEditPage> ScheduledStart => this.MaterialDatetimePicker(roleType: M.CommunicationEvent.ScheduledStart);

        public MaterialDatetimePicker<LetterCorrespondenceEditPage> ScheduledEnd => this.MaterialDatetimePicker(roleType: M.CommunicationEvent.ScheduledEnd);

        public MaterialDatetimePicker<LetterCorrespondenceEditPage> ActualStart => this.MaterialDatetimePicker(roleType: M.CommunicationEvent.ActualStart);

        public MaterialDatetimePicker<LetterCorrespondenceEditPage> ActualEnd => this.MaterialDatetimePicker(roleType: M.CommunicationEvent.ActualEnd);

        public MaterialTextArea<LetterCorrespondenceEditPage> Comment => this.MaterialTextArea(roleType: M.CommunicationEvent.Comment);

        public Button<LetterCorrespondenceEditPage> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor<LetterCorrespondenceEditPage> List => this.Anchor(By.CssSelector("a[href='/contacts/people']"));

        public Button<LetterCorrespondenceEditPage> NewCommunicationEvent => this.Button(By.CssSelector("div[data-allors-class='communicationevent']"));
    }
}
