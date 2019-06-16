using src.allors.material.apps.objects.communicationevent.overview.panel;
using src.allors.material.apps.objects.facetofacecommunication.edit;
using src.allors.material.apps.objects.person.list;
using src.allors.material.apps.objects.person.overview;

namespace Tests.FaceToFaceCommunicationTests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Components;
    using Xunit;

    [Collection("Test collection")]
    public class PersonFaceToFaceCommunicationEditTest : Test
    {
        private readonly PersonListComponent personListPage;

        public PersonFaceToFaceCommunicationEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.personListPage = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Create()
        {
            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = allors.ActiveEmployees.First;

            var before = new FaceToFaceCommunications(this.Session).Extent().ToArray();

            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));

            this.personListPage.Table.DefaultAction(person);
            var faceToFaceCommunicationEdit = new PersonOverviewComponent(this.personListPage.Driver).CommunicationeventOverviewPanel.Click().CreateFaceToFaceCommunication();

            faceToFaceCommunicationEdit
                .CommunicationEventState.Set(new CommunicationEventStates(this.Session).Completed.Name)
                .EventPurposes.Toggle(new CommunicationEventPurposes(this.Session).Appointment.Name)
                .Location.Set("location")
                .Subject.Set("subject")
                .FromParty.Set(employee.PartyName)
                .ToParty.Set(person.PartyName)
                .ScheduledStart.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ScheduledEnd.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ActualStart.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .ActualEnd.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .SAVE.Click();

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
            var people = new People(this.Session).Extent();
            var person = people.First(v => v.PartyName.Equals("John0 Doe0"));

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var firstEmployee = allors.ActiveEmployees.First(v => v.FirstName.Equals("first"));
            var secondEmployee = allors.ActiveEmployees.First(v => v.FirstName.Equals("second"));

            var editCommunicationEvent = new FaceToFaceCommunicationBuilder(this.Session)
                .WithSubject("dummy")
                .WithFromParty(person)
                .WithToParty(firstEmployee)
                .WithLocation("old location")
                .Build();

            this.Session.Derive();
            this.Session.Commit();
            
            var before = new FaceToFaceCommunications(this.Session).Extent().ToArray();

            this.personListPage.Table.DefaultAction(person);
            var personOverview = new PersonOverviewComponent(this.personListPage.Driver);

            var communicationEventOverview = personOverview.CommunicationeventOverviewPanel.Click();
            var row = communicationEventOverview.Table.FindRow(editCommunicationEvent);
            var cell = row.FindCell("description");
            cell.Click();

            var faceToFaceCommunicationEditComponent = new FaceToFaceCommunicationEditComponent(this.Driver); ;
            faceToFaceCommunicationEditComponent.CommunicationEventState
                .Set(new CommunicationEventStates(this.Session).Completed.Name)
                .EventPurposes.Toggle(new CommunicationEventPurposes(this.Session).Conference.Name)
                .Location.Set("new location")
                .Subject.Set("new subject")
                .FromParty.Set(secondEmployee.PartyName)
                .ToParty.Set(person.PartyName)
                .ScheduledStart.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .ScheduledEnd.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .ActualStart.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .ActualEnd.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new FaceToFaceCommunications(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(new CommunicationEventStates(this.Session).Completed, editCommunicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Conference, editCommunicationEvent.EventPurposes);
            Assert.Equal(secondEmployee, editCommunicationEvent.FromParty);
            Assert.Equal(person, editCommunicationEvent.ToParty);
            Assert.Equal("new location", editCommunicationEvent.Location);
            Assert.Equal("new subject", editCommunicationEvent.Subject);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ScheduledStart);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ScheduledEnd.Value.Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualStart.Value.Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualEnd.Value.Date);
        }
    }
}
