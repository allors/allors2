namespace Pages.EmailCommunicationTests
{
    using Allors.Meta;

    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class EmailCommunicationEditPage : MainPage
    {
        public EmailCommunicationEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialSelect<EmailCommunicationEditPage> EventState => this.MaterialSelect(roleType: M.CommunicationEvent.CommunicationEventState);

        public MaterialSelect<EmailCommunicationEditPage> Purposes => this.MaterialSelect(roleType: M.CommunicationEvent.EventPurposes);

        public MaterialSelect<EmailCommunicationEditPage> FromParty => this.MaterialSelect(roleType: M.EmailCommunication.FromParty);

        public MaterialSelect<EmailCommunicationEditPage> ToParty => this.MaterialSelect(roleType: M.EmailCommunication.ToParty);

        public MaterialSelect<EmailCommunicationEditPage> FromEmail => this.MaterialSelect(roleType: M.EmailCommunication.FromEmail);

        public MaterialSelect<EmailCommunicationEditPage> ToEmail => this.MaterialSelect(roleType: M.EmailCommunication.ToEmail);

        public MaterialInput<EmailCommunicationEditPage> Subject => this.MaterialInput(roleType: M.EmailTemplate.SubjectTemplate);

        public MaterialTextArea<EmailCommunicationEditPage> Body => this.MaterialTextArea(roleType: M.EmailTemplate.BodyTemplate);

        public MaterialDatetimePicker<EmailCommunicationEditPage> ScheduledStart => this.MaterialDatetimePicker(roleType: M.CommunicationEvent.ScheduledStart);

        public MaterialDatetimePicker<EmailCommunicationEditPage> ScheduledEnd => this.MaterialDatetimePicker(roleType: M.CommunicationEvent.ScheduledEnd);

        public MaterialDatetimePicker<EmailCommunicationEditPage> ActualStart => this.MaterialDatetimePicker(roleType: M.CommunicationEvent.ActualStart);

        public MaterialDatetimePicker<EmailCommunicationEditPage> ActualEnd => this.MaterialDatetimePicker(roleType: M.CommunicationEvent.ActualEnd);

        public Button<EmailCommunicationEditPage> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor<EmailCommunicationEditPage> List => this.Anchor(By.CssSelector("a[href='/contacts/people']"));

        public Button<EmailCommunicationEditPage> NewCommunicationEvent => this.Button(By.CssSelector("div[data-allors-class='communicationevent']"));
    }
}
