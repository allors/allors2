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

        public MaterialSingleSelect<FaceToFaceCommunicationEditPage> EventState => this.MaterialSingleSelect(roleType: M.CommunicationEvent.CommunicationEventState);

        public MaterialMultipleSelect<FaceToFaceCommunicationEditPage> Purposes => this.MaterialMultipleSelect(roleType: M.CommunicationEvent.EventPurposes);

        public MaterialSingleSelect<FaceToFaceCommunicationEditPage> FromParty => this.MaterialSingleSelect(roleType: M.FaceToFaceCommunication.FromParty);

        public MaterialSingleSelect<FaceToFaceCommunicationEditPage> ToParty => this.MaterialSingleSelect(roleType: M.FaceToFaceCommunication.ToParty);

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
