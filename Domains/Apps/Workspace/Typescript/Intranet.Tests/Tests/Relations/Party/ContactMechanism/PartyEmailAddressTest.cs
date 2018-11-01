namespace Intranet.Tests.RelationsPartyCommunicationEvent
{
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Intranet.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class PartyEmailAddressTest : Test
    {
        private readonly Sidenav sideNav;

        public PartyEmailAddressTest(TestFixture fixture)
            : base(fixture)
        {
            this.sideNav = this.Login().Sidenav;
        }

        [Fact]
        public void Title()
        {
            var person = new People(this.Session).FindBy(M.Person.PartyName, "contact1");
            var email = (EmailCommunication)person.CommunicationEventsWhereInvolvedParty.First(v => v is EmailCommunication);

            this.sideNav.NavigateToPersonList().Select(person).Select(email).Edit.Click();

            Assert.Equal("Communication Event overview", this.Driver.Title);
        }

        [Fact]
        public void Edit()
        {
            var person = new People(this.Session).FindBy(M.Person.PartyName, "contact1");
            var email = (EmailCommunication)person.CommunicationEventsWhereInvolvedParty.First(v => v is EmailCommunication);

            Assert.False(email.ExistScheduledStart);

            this.sideNav.NavigateToPersonList().Select(person).Select(email).Edit.Click();

            var page = new PartyEmailCommunicationPage(this.Driver);

            var scheduledStart = this.Session.Now();
            page.ScheduledStart.Value = scheduledStart;
            
            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            Assert.True(email.ExistScheduledStart);
        }
    }
}
