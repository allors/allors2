namespace Intranet.Tests.RelationsCommunicationEvent
{
    using Intranet.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class CommunicationsOverviewTest : Test
    {
        public CommunicationsOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToCommunications();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Communications", this.Driver.Title);
        }

        [Fact]
        public void Search()
        {
            var page = new CommunicationsOverviewPage(this.Driver);

            page.Subject.Text = "acme";
        }
    }
}
