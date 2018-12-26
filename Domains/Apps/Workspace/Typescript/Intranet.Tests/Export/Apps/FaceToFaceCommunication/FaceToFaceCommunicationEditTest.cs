namespace Tests.Intranet.FaceToFaceCommunicationTests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Tests.Components;
    using Tests.Intranet.PersonTests;

    using Xunit;

    [Collection("Test collection")]
    public class FaceToFaceCommunicationEditTest : Test
    {
        private readonly PersonListPage people;

        public FaceToFaceCommunicationEditTest(TestFixture fixture)
            : base(fixture)
        {
            var people = new People(this.Session).Extent();
            var person = people.First(v => v.PartyName.Equals("John0 Doe0"));

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var firstEmployee = allors.ActiveEmployees.First(v => v.FirstName.Equals("first"));

            person.AddCommunicationEvent(new FaceToFaceCommunicationBuilder(this.Session)
                .WithSubject("dummy")
                .WithFromParty(person)
                .WithToParty(firstEmployee)
                .WithLocation("old location")
                .Build());

            this.Session.Derive();
            this.Session.Commit();

            var dashboard = this.Login();
            this.people = dashboard.Sidenav.NavigateToPersonList();
        }

        [Fact]
        public void AddToPerson()
        {
            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = allors.ActiveEmployees.First;

            var before = new FaceToFaceCommunications(this.Session).Extent().ToArray();

            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));

            var personOverview = this.people.Select(person);
            var page = personOverview.NewFaceToFaceCommunication();

            page.EventState.Value = new CommunicationEventStates(this.Session).Completed.Name;
            page.Purposes.Toggle(new CommunicationEventPurposes(this.Session).Appointment.Name);
            page.Location.Value = "location";
            page.Subject.Value = "subject";
            page.FromParty.Value = employee.PartyName;
            page.ToParty.Value = person.PartyName;
            page.ScheduledStart.Value = DateTimeFactory.CreateDate(2018, 12, 22);
            page.ScheduledEnd.Value = DateTimeFactory.CreateDate(2018, 12, 22);
            page.ActualStart.Value = DateTimeFactory.CreateDate(2018, 12, 23);
            page.ActualEnd.Value = DateTimeFactory.CreateDate(2018, 12, 23);

            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new FaceToFaceCommunications(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var communicationEvent = after.Except(before).First();

            Assert.Equal(new CommunicationEventStates(this.Session).Completed, communicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Appointment, communicationEvent.EventPurposes);
            Assert.Equal(employee, communicationEvent.FromParty);
            Assert.Equal(person, communicationEvent.ToParty);
            Assert.Equal("location", communicationEvent.Location);
            Assert.Equal("subject", communicationEvent.Subject);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, communicationEvent.ScheduledStart.Value.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, communicationEvent.ScheduledEnd.Value.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, communicationEvent.ActualStart.Value.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, communicationEvent.ActualEnd.Value.Date.ToUniversalTime().Date);
        }

        [Fact]
        public void Edit()
        {
            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var firstEmployee = allors.ActiveEmployees.First(v => v.FirstName.Equals("first"));
            var secondEmployee = allors.ActiveEmployees.First(v => v.FirstName.Equals("second"));

            var before = new FaceToFaceCommunications(this.Session).Extent().ToArray();

            var communicationEvent = (FaceToFaceCommunication)person.CommunicationEvents.First(v => v.GetType().Name == typeof(FaceToFaceCommunication).Name);
            var id = communicationEvent.Id;

            var personOverview = this.people.Select(person);

            var page = personOverview.SelectFaceToFaceCommunication(communicationEvent);

            page.EventState.Value = new CommunicationEventStates(this.Session).Completed.Name;
            page.Purposes.Toggle(new CommunicationEventPurposes(this.Session).Conference.Name);
            page.Location.Value = "new location";
            page.Subject.Value = "new subject";
            page.FromParty.Value = secondEmployee.PartyName;
            page.ToParty.Value = person.PartyName;
            page.ScheduledStart.Value = DateTimeFactory.CreateDate(2018, 12, 24);
            page.ScheduledEnd.Value = DateTimeFactory.CreateDate(2018, 12, 24);
            page.ActualStart.Value = DateTimeFactory.CreateDate(2018, 12, 24);
            page.ActualEnd.Value = DateTimeFactory.CreateDate(2018, 12, 24);

            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new FaceToFaceCommunications(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            communicationEvent = after.First(v => v.Id.Equals(id));

            Assert.Equal(new CommunicationEventStates(this.Session).Completed, communicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Conference, communicationEvent.EventPurposes);
            Assert.Equal(secondEmployee, communicationEvent.FromParty);
            Assert.Equal(person, communicationEvent.ToParty);
            Assert.Equal("new location", communicationEvent.Location);
            Assert.Equal("new subject", communicationEvent.Subject);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ScheduledStart);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ScheduledEnd.Value.Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualStart.Value.Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualEnd.Value.Date);
        }
    }
}
