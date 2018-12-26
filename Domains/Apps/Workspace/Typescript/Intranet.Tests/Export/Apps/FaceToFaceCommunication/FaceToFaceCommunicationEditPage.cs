namespace Tests.Intranet.FaceToFaceCommunicationTests
{
    using Allors.Meta;

    using OpenQA.Selenium;

    using Tests.Components.Html;
    using Tests.Components.Material;

    public class FaceToFaceCommunicationEditPage : MainPage
    {
        public FaceToFaceCommunicationEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialSingleSelect EventState => new MaterialSingleSelect(this.Driver, roleType: M.CommunicationEvent.CommunicationEventState);

        public MaterialMultipleSelect Purposes => new MaterialMultipleSelect(this.Driver, roleType: M.CommunicationEvent.EventPurposes);

        public MaterialSingleSelect FromParty => new MaterialSingleSelect(this.Driver, roleType: M.FaceToFaceCommunication.FromParty);

        public MaterialSingleSelect ToParty => new MaterialSingleSelect(this.Driver, roleType: M.FaceToFaceCommunication.ToParty);

        public MaterialInput Location => new MaterialInput(this.Driver, roleType: M.FaceToFaceCommunication.Location);

        public MaterialInput Subject => new MaterialInput(this.Driver, roleType: M.CommunicationEvent.Subject);

        public MaterialDatetimePicker ScheduledStart => new MaterialDatetimePicker(this.Driver, roleType: M.CommunicationEvent.ScheduledStart);

        public MaterialDatetimePicker ScheduledEnd => new MaterialDatetimePicker(this.Driver, roleType: M.CommunicationEvent.ScheduledEnd);

        public MaterialDatetimePicker ActualStart => new MaterialDatetimePicker(this.Driver, roleType: M.CommunicationEvent.ActualStart);

        public MaterialDatetimePicker ActualEnd => new MaterialDatetimePicker(this.Driver, roleType: M.CommunicationEvent.ActualEnd);

        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));

        public Anchor List => new Anchor(this.Driver, By.CssSelector("a[href='/contacts/people']"));

        public Button NewCommunicationEvent => new Button(this.Driver, By.CssSelector("div[data-allors-class='communicationevent']"));
    }
}
