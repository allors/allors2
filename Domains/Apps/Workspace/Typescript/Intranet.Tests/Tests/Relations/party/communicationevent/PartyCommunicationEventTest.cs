namespace Intranet.Tests.Relations
{
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;

    using Intranet.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class PartyCommunicationEventTest : Test
    {
        private readonly PeopleOverviewPage people;

        public PartyCommunicationEventTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.people = dashboard.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Title()
        {
            var person = new People(this.Session).FindBy(M.Person.PartyName, "contact1");
            var meeting = (FaceToFaceCommunication)person.CommunicationEventsWhereInvolvedParty.First(v => v is FaceToFaceCommunication);

            var page = this.people.Select(person);

            page.Select(meeting);

            this.Driver.WaitForAngular();

            Assert.Equal("Communication Event overview", this.Driver.Title);
        }
    }
}
