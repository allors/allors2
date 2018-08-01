namespace Intranet.Tests.RelationsPartyCommunicationEvent
{
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Intranet.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class PartyLetterCorrespondenceTest : Test
    {
        private readonly Sidenav sideNav;

        public PartyLetterCorrespondenceTest(TestFixture fixture)
            : base(fixture)
        {
            this.sideNav = this.Login().Sidenav;
        }

        [Fact]
        public void Title()
        {
            var person = new People(this.Session).FindBy(M.Person.PartyName, "contact1");
            var email = (LetterCorrespondence)person.CommunicationEventsWhereInvolvedParty.First(v => v is LetterCorrespondence);

            this.sideNav.NavigateToPeople().Select(person).Select(email).Edit.Click();

            Assert.Equal("Communication Event overview", this.Driver.Title);
        }

        [Fact]
        public void Edit()
        {
            var person = new People(this.Session).FindBy(M.Person.PartyName, "contact1");
            var email = (LetterCorrespondence)person.CommunicationEventsWhereInvolvedParty.First(v => v is LetterCorrespondence);

            Assert.False(email.ExistScheduledStart);

            this.sideNav.NavigateToPeople().Select(person).Select(email).Edit.Click();

            var page = new PartyLetterCorrespondencePage(this.Driver);

            var scheduledStart = this.Session.Now();
            page.ScheduledStart.Value = scheduledStart;
            
            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            Assert.True(email.ExistScheduledStart);
        }
    }
}
