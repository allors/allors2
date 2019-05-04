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

        public MaterialSelect<LetterCorrespondenceEditPage> EventState => this.MaterialSelect(roleType: M.CommunicationEvent.CommunicationEventState);

        public MaterialSelect<LetterCorrespondenceEditPage> Purposes => this.MaterialSelect(roleType: M.CommunicationEvent.EventPurposes);

        public MaterialSelect<LetterCorrespondenceEditPage> PostalAddress => this.MaterialSelect(roleType: M.LetterCorrespondence.PostalAddress);

        public MaterialSelect<LetterCorrespondenceEditPage> FromParty => this.MaterialSelect(roleType: M.LetterCorrespondence.FromParty);

        public MaterialSelect<LetterCorrespondenceEditPage> ToParty => this.MaterialSelect(roleType: M.LetterCorrespondence.ToParty);

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
