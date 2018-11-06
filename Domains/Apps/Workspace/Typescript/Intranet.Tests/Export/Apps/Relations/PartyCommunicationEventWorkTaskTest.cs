namespace Tests.Intranet.Relations
{
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using global::Tests.Intranet.Relations;

  

    using Tests.Components;
    using Tests.Intranet;

    using Xunit;

    [Collection("Test collection")]
    public class CommunicationEventWorkTaskTest : Test
    {
        private readonly Sidenav sideNav;

        public CommunicationEventWorkTaskTest(TestFixture fixture)
            : base(fixture)
        {
            this.sideNav = this.Login().Sidenav;
        }

        [Fact]
        public void Title()
        {
            var person = new People(this.Session).FindBy(M.Person.PartyName, "contact1");
            var email = (EmailCommunication)person.CommunicationEventsWhereInvolvedParty.First(v => v is EmailCommunication);

            this.sideNav.NavigateToPersonList().Select(person).Select(email).AddNew.Click();

            Assert.Equal("Work Task", this.Driver.Title);
        }

        [Fact]
        public void Add()
        {
            var person = new People(this.Session).FindBy(M.Person.PartyName, "contact1");
            var email = (EmailCommunication)person.CommunicationEventsWhereInvolvedParty.First(v => v is EmailCommunication);

            this.sideNav.NavigateToPersonList().Select(person).Select(email).AddNew.Click();

            var page = new CommunicationEventWorkTaskPage(this.Driver);

            var scheduledStart = this.Session.Now();

            page.Name.Value = "Do it!";
            page.ScheduledStart.Value = scheduledStart;
            
            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            Assert.Single(email.WorkEfforts);

            var task = email.WorkEfforts.First;

            Assert.Equal("Do it!", task.Name);
            Assert.True(task.ExistScheduledStart);
        }
    }
}
