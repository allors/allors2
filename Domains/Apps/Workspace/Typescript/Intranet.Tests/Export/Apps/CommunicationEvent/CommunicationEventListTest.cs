namespace Tests.Intranet.CommunicationEventTests
{
    using Allors.Domain;
    using Allors.Meta;

    using Xunit;

    [Collection("Test collection")]
    public class CommunicationEventListTest : Test
    {
        private CommunicationEventListPage page;

        public CommunicationEventListTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.page = dashboard.Sidenav.NavigateToCommunicationEventList();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Communications", this.Driver.Title);
        }

        [Fact]
        public void Table()
        {
            var communicationEvent = new CommunicationEvents(this.Session).FindBy(M.CommunicationEvent.Subject, "meeting 0");

            var row = this.page.Table.FindRow(communicationEvent);
            var cell = row.FindCell("subject");

            Assert.Equal("meeting 0", cell.Element.Text);
        }
    }
}
