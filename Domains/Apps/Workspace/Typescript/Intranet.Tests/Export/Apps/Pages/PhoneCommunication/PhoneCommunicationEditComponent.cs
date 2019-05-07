using src.app.main;

namespace Pages.PhoneCommunicationTests
{
    using Allors.Meta;

    using OpenQA.Selenium;
    using Components;

    using Pages.ApplicationTests;

    public class PhoneCommunicationEditComponent : MainComponent
    {
        public PhoneCommunicationEditComponent(IWebDriver driver)
            : base(driver)
        {
        }

        public MatSelect<PhoneCommunicationEditComponent> EventState => this.MatSelect(roleType: M.CommunicationEvent.CommunicationEventState);

        public MatSelect<PhoneCommunicationEditComponent> Purposes => this.MatSelect(roleType: M.CommunicationEvent.EventPurposes);

        public MatSelect<PhoneCommunicationEditComponent> FromParty => this.MatSelect(roleType: M.PhoneCommunication.FromParty);

        public MatSelect<PhoneCommunicationEditComponent> ToParty => this.MatSelect(roleType: M.PhoneCommunication.ToParty);

        public MatSelect<PhoneCommunicationEditComponent> PhoneNumber => this.MatSelect(roleType: M.PhoneCommunication.PhoneNumber);

        public MatSlidetoggle<PhoneCommunicationEditComponent> LeftVoiceMail => this.MatSlidetoggle(roleType: M.PhoneCommunication.LeftVoiceMail);

        public MatInput<PhoneCommunicationEditComponent> Subject => this.MatInput(roleType: M.CommunicationEvent.Subject);

        public MatDatetimepicker<PhoneCommunicationEditComponent> ScheduledStart => this.MatDatetimepicker(roleType: M.CommunicationEvent.ScheduledStart);

        public MatDatetimepicker<PhoneCommunicationEditComponent> ScheduledEnd => this.MatDatetimepicker(roleType: M.CommunicationEvent.ScheduledEnd);

        public MatDatetimepicker<PhoneCommunicationEditComponent> ActualStart => this.MatDatetimepicker(roleType: M.CommunicationEvent.ActualStart);

        public MatDatetimepicker<PhoneCommunicationEditComponent> ActualEnd => this.MatDatetimepicker(roleType: M.CommunicationEvent.ActualEnd);

        public MatTextarea<PhoneCommunicationEditComponent> Comment => this.MatTextarea(roleType: M.CommunicationEvent.Comment);

        public Button<PhoneCommunicationEditComponent> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor<PhoneCommunicationEditComponent> List => this.Anchor(By.CssSelector("a[href='/contacts/people']"));

        public Button<PhoneCommunicationEditComponent> NewCommunicationEvent => this.Button(By.CssSelector("div[data-allors-class='communicationevent']"));
    }
}
