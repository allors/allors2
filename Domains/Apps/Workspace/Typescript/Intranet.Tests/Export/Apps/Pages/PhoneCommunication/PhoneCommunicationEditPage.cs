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

        public MaterialSingleSelect<PhoneCommunicationEditPage> EventState => this.MaterialSingleSelect(roleType: M.CommunicationEvent.CommunicationEventState);

        public MaterialMultipleSelect<PhoneCommunicationEditPage> Purposes => this.MaterialMultipleSelect(roleType: M.CommunicationEvent.EventPurposes);

        public MaterialSingleSelect<PhoneCommunicationEditPage> FromParty => this.MaterialSingleSelect(roleType: M.PhoneCommunication.FromParty);

        public MaterialSingleSelect<PhoneCommunicationEditPage> ToParty => this.MaterialSingleSelect(roleType: M.PhoneCommunication.ToParty);

        public MaterialSingleSelect<PhoneCommunicationEditPage> PhoneNumber => this.MaterialSingleSelect(roleType: M.PhoneCommunication.PhoneNumber);

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
