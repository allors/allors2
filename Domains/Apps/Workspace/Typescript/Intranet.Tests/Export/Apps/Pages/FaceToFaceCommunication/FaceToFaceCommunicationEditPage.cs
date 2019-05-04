namespace Pages.FaceToFaceCommunicationTests
{
    using Allors.Meta;

    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class FaceToFaceCommunicationEditPage : MainPage
    {
        public FaceToFaceCommunicationEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialSelect<FaceToFaceCommunicationEditPage> EventState => this.MaterialSelect(roleType: M.CommunicationEvent.CommunicationEventState);

        public MaterialSelect<FaceToFaceCommunicationEditPage> Purposes => this.MaterialSelect(roleType: M.CommunicationEvent.EventPurposes);

        public MaterialSelect<FaceToFaceCommunicationEditPage> FromParty => this.MaterialSelect(roleType: M.FaceToFaceCommunication.FromParty);

        public MaterialSelect<FaceToFaceCommunicationEditPage> ToParty => this.MaterialSelect(roleType: M.FaceToFaceCommunication.ToParty);

        public MaterialInput<FaceToFaceCommunicationEditPage> Location => this.MaterialInput(roleType: M.FaceToFaceCommunication.Location);

        public MaterialInput<FaceToFaceCommunicationEditPage> Subject => this.MaterialInput(roleType: M.CommunicationEvent.Subject);

        public MaterialDatetimePicker<FaceToFaceCommunicationEditPage> ScheduledStart => this.MaterialDatetimePicker(roleType: M.CommunicationEvent.ScheduledStart);

        public MaterialDatetimePicker<FaceToFaceCommunicationEditPage> ScheduledEnd => this.MaterialDatetimePicker(roleType: M.CommunicationEvent.ScheduledEnd);

        public MaterialDatetimePicker<FaceToFaceCommunicationEditPage> ActualStart => this.MaterialDatetimePicker(roleType: M.CommunicationEvent.ActualStart);

        public MaterialDatetimePicker<FaceToFaceCommunicationEditPage> ActualEnd => this.MaterialDatetimePicker(roleType: M.CommunicationEvent.ActualEnd);

        public Button<FaceToFaceCommunicationEditPage> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor<FaceToFaceCommunicationEditPage> List => this.Anchor(By.CssSelector("a[href='/contacts/people']"));

        public Button<FaceToFaceCommunicationEditPage> NewCommunicationEvent => this.Button(By.CssSelector("div[data-allors-class='communicationevent']"));
    }
}
