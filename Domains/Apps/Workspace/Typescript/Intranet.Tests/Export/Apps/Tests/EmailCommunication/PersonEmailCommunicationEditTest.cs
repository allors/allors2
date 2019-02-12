namespace Tests.EmailCommunicationTests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Angular;

    using Pages.PersonTests;

    using Xunit;

    [Collection("Test collection")]
    public class PersonEmailCommunicationEditTest : Test
    {
        private PersonListPage personListPage;

        public PersonEmailCommunicationEditTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.personListPage = dashboard.Sidenav.NavigateToPersonList();
        }

        [Fact]
        public void Create()
        {
            var people = new People(this.Session).Extent();
            var person = people.First(v => v.PartyName.Equals("John0 Doe0"));

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = allors.ActiveEmployees.First(v => v.FirstName.Equals("first"));

            var employeeEmailAddress = employee.PersonalEmailAddress;
            var personEmailAddress = person.PersonalEmailAddress;

            this.Session.Derive();
            this.Session.Commit();

            var before = new EmailCommunications(this.Session).Extent().ToArray();

            var personOverview = this.personListPage.Select(person);
            var page = personOverview.NewEmailCommunication();

            page.EventState.Set(new CommunicationEventStates(this.Session).Completed.Name)
                .Purposes.Toggle(new CommunicationEventPurposes(this.Session).Appointment.Name)
                .FromParty.Set(employee.PartyName)
                .FromEmail.Set(employeeEmailAddress.ElectronicAddressString)
                .ToParty.Set(person.PartyName)
                .ToEmail.Set(personEmailAddress.ElectronicAddressString)
                .Subject.Set("subject")
                .Body.Set("body")
                .ScheduledStart.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ScheduledEnd.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ActualStart.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .ActualEnd.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new EmailCommunications(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var communicationEvent = after.Except(before).First();

            Assert.Equal(new CommunicationEventStates(this.Session).Completed, communicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Appointment, communicationEvent.EventPurposes);
            Assert.Equal(employee, communicationEvent.FromParty);
            Assert.Equal(employeeEmailAddress, communicationEvent.FromEmail);
            Assert.Equal(person, communicationEvent.ToParty);
            Assert.Equal(personEmailAddress, communicationEvent.ToEmail);
            Assert.Equal("subject", communicationEvent.EmailTemplate.SubjectTemplate);
            Assert.Equal("body", communicationEvent.EmailTemplate.BodyTemplate);
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
            var employee = allors.ActiveEmployees.First(v => v.FirstName.Equals("first"));

            var employeeEmailAddress = employee.PersonalEmailAddress;
            var personEmailAddress = person.PersonalEmailAddress;

            var editCommunicationEvent = new EmailCommunicationBuilder(this.Session)
                .WithSubject("dummy")
                .WithFromEmail(employeeEmailAddress)
                .WithToEmail(personEmailAddress)
                .WithEmailTemplate(new EmailTemplateBuilder(this.Session).Build())
                .Build();

            this.Session.Derive();
            this.Session.Commit();
            
            var before = new EmailCommunications(this.Session).Extent().ToArray();

            var personOverview = this.personListPage.Select(person);

            var page = personOverview.SelectEmailCommunication(editCommunicationEvent);

            page.EventState.Set(new CommunicationEventStates(this.Session).Completed.Name)
                .Purposes.Toggle(new CommunicationEventPurposes(this.Session).Inquiry.Name)
                .FromParty.Set(person.PartyName)
                .FromEmail.Set(personEmailAddress.ElectronicAddressString)
                .ToParty.Set(employee.PartyName)
                .ToEmail.Set(employeeEmailAddress.ElectronicAddressString)
                .Subject.Set("new subject")
                .Body.Set("new body")
                .ScheduledStart.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .ScheduledEnd.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .ActualStart.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .ActualEnd.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new EmailCommunications(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(new CommunicationEventStates(this.Session).Completed, editCommunicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Inquiry, editCommunicationEvent.EventPurposes);
            Assert.Equal(person, editCommunicationEvent.FromParty);
            Assert.Equal(personEmailAddress, editCommunicationEvent.FromEmail);
            Assert.Equal(employee, editCommunicationEvent.ToParty);
            Assert.Equal(employeeEmailAddress, editCommunicationEvent.ToEmail);
            Assert.Equal("new subject", editCommunicationEvent.EmailTemplate.SubjectTemplate);
            Assert.Equal("new body", editCommunicationEvent.EmailTemplate.BodyTemplate);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ScheduledStart);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ScheduledEnd.Value.Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualStart.Value.Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualEnd.Value.Date);
        }
    }
}
