namespace Pages.PhoneCommunicationTests
{
    using Allors.Meta;

    using OpenQA.Selenium;

    using Angular.Html;
    using Angular.Material;

    using Pages.ApplicationTests;

    public class PhoneCommunicationEditPage : MainPage
    {
        public PhoneCommunicationEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialSelect<PhoneCommunicationEditPage> EventState => this.MaterialSelect(roleType: M.CommunicationEvent.CommunicationEventState);

        public MaterialSelect<PhoneCommunicationEditPage> Purposes => this.MaterialSelect(roleType: M.CommunicationEvent.EventPurposes);

        public MaterialSelect<PhoneCommunicationEditPage> FromParty => this.MaterialSelect(roleType: M.PhoneCommunication.FromParty);

        public MaterialSelect<PhoneCommunicationEditPage> ToParty => this.MaterialSelect(roleType: M.PhoneCommunication.ToParty);

        public MaterialSelect<PhoneCommunicationEditPage> PhoneNumber => this.MaterialSelect(roleType: M.PhoneCommunication.PhoneNumber);

        public MaterialSlideToggle<PhoneCommunicationEditPage> LeftVoiceMail => this.MaterialSlideToggle(roleType: M.PhoneCommunication.LeftVoiceMail);

        public MaterialInput<PhoneCommunicationEditPage> Subject => this.MaterialInput(roleType: M.CommunicationEvent.Subject);

        public MaterialDatetimePicker<PhoneCommunicationEditPage> ScheduledStart => this.MaterialDatetimePicker(roleType: M.CommunicationEvent.ScheduledStart);

        public MaterialDatetimePicker<PhoneCommunicationEditPage> ScheduledEnd => this.MaterialDatetimePicker(roleType: M.CommunicationEvent.ScheduledEnd);

        public MaterialDatetimePicker<PhoneCommunicationEditPage> ActualStart => this.MaterialDatetimePicker(roleType: M.CommunicationEvent.ActualStart);

        public MaterialDatetimePicker<PhoneCommunicationEditPage> ActualEnd => this.MaterialDatetimePicker(roleType: M.CommunicationEvent.ActualEnd);

        public MaterialTextArea<PhoneCommunicationEditPage> Comment => this.MaterialTextArea(roleType: M.CommunicationEvent.Comment);

        public Button<PhoneCommunicationEditPage> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor<PhoneCommunicationEditPage> List => this.Anchor(By.CssSelector("a[href='/contacts/people']"));

        public Button<PhoneCommunicationEditPage> NewCommunicationEvent => this.Button(By.CssSelector("div[data-allors-class='communicationevent']"));
    }
}
