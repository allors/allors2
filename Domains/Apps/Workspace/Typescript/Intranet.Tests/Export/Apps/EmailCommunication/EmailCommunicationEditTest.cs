namespace Tests.Intranet.EmailCommunicationTests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;

    using Tests.Components;
    using Tests.Intranet.PersonTests;

    using Xunit;

    [Collection("Test collection")]
    public class EmailCommunicationEditTest : Test
    {
        private readonly PersonListPage people;

        public EmailAddress originatorEmailAddress { get; set; }

        public EmailAddress addressee1EmailAddress { get; set; }

        public EmailAddress addressee2EmailAddress { get; set; }

        public EmailAddress addressee3EmailAddress { get; set; }

        public EmailAddress addressee4EmailAddress { get; set; }

        public EmailCommunicationEditTest(TestFixture fixture)
            : base(fixture)
        {
            var people = new People(this.Session).Extent();
            var person = people.First(v => v.PartyName.Equals("John0 Doe0"));

            this.originatorEmailAddress = new EmailAddressBuilder(this.Session).WithElectronicAddressString("originator@allors.com").Build();
            this.addressee1EmailAddress = new EmailAddressBuilder(this.Session).WithElectronicAddressString("addressee1@xxx.com").Build();
            this.addressee2EmailAddress = new EmailAddressBuilder(this.Session).WithElectronicAddressString("addressee2@xxx.com").Build();
            this.addressee3EmailAddress = new EmailAddressBuilder(this.Session).WithElectronicAddressString("addressee3@xxx.com").Build();
            this.addressee4EmailAddress = new EmailAddressBuilder(this.Session).WithElectronicAddressString("addressee4@xxx.com").Build();

            person.AddCommunicationEvent(new EmailCommunicationBuilder(this.Session)
                .WithSubject("dummy")
                .WithOriginator(this.originatorEmailAddress)
                .WithAddressee(this.addressee1EmailAddress)
                .WithEmailTemplate(new EmailTemplateBuilder(this.Session).Build())
                .Build());

            this.Session.Derive();
            this.Session.Commit();

            var dashboard = this.Login();
            this.people = dashboard.Sidenav.NavigateToPersonList();
        }

        [Fact]
        public void AddToPerson()
        {
            var before = new EmailCommunications(this.Session).Extent().ToArray();

            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));

            var personOverview = this.people.Select(person);
            var page = personOverview.NewEmailCommunication();

            page.IncomingMail.Value = false;
            page.EventState.Value = new CommunicationEventStates(this.Session).Completed.Name;
            page.Purposes.Toggle(new CommunicationEventPurposes(this.Session).Appointment.Name);
            page.Originator.Value = this.originatorEmailAddress.ElectronicAddressString;
            page.Addressees.Add(this.addressee1EmailAddress.ElectronicAddressString);
            page.Addressees.Add(this.addressee2EmailAddress.ElectronicAddressString);
            page.CarbonCopies.Add(this.addressee3EmailAddress.ElectronicAddressString);
            page.BlindCopies.Add(this.addressee4EmailAddress.ElectronicAddressString);
            page.Subject.Value = "subject";
            page.Body.Value = "body";
            page.ScheduledStart.Value = DateTimeFactory.CreateDate(2018, 12, 22);
            page.ScheduledEnd.Value = DateTimeFactory.CreateDate(2018, 12, 22);
            page.ActualStart.Value = DateTimeFactory.CreateDate(2018, 12, 23);
            page.ActualEnd.Value = DateTimeFactory.CreateDate(2018, 12, 23);

            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new EmailCommunications(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var communicationEvent = after.Except(before).First();

            Assert.False(communicationEvent.IncomingMail);
            Assert.Equal(new CommunicationEventStates(this.Session).Completed, communicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Appointment, communicationEvent.EventPurposes);
            Assert.Equal(this.originatorEmailAddress, communicationEvent.Originator);
            Assert.Equal(2, communicationEvent.Addressees.Count);
            Assert.Contains(this.addressee1EmailAddress, communicationEvent.Addressees);
            Assert.Contains(this.addressee2EmailAddress, communicationEvent.Addressees);
            Assert.Single(communicationEvent.CarbonCopies);
            Assert.Contains(this.addressee3EmailAddress, communicationEvent.CarbonCopies);
            Assert.Single(communicationEvent.BlindCopies);
            Assert.Contains(this.addressee4EmailAddress, communicationEvent.BlindCopies);
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
            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));

            var before = new EmailCommunications(this.Session).Extent().ToArray();

            var communicationEvent = (EmailCommunication)person.CommunicationEvents.First(v => v.GetType().Name == typeof(EmailCommunication).Name);
            var id = communicationEvent.Id;

            var personOverview = this.people.Select(person);

            var page = personOverview.SelectEmailCommunication(communicationEvent);

            page.IncomingMail.Value = true;
            page.EventState.Value = new CommunicationEventStates(this.Session).Completed.Name;
            page.Purposes.Toggle(new CommunicationEventPurposes(this.Session).Inquiry.Name);
            page.Originator.Value = this.addressee1EmailAddress.ElectronicAddressString;
            page.Addressees.Remove(this.addressee1EmailAddress.ElectronicAddressString);
            page.Addressees.Add(this.originatorEmailAddress.ElectronicAddressString);
            page.Subject.Value = "new subject";
            page.Body.Value = "new body";
            page.ScheduledStart.Value = DateTimeFactory.CreateDate(2018, 12, 24);
            page.ScheduledEnd.Value = DateTimeFactory.CreateDate(2018, 12, 24);
            page.ActualStart.Value = DateTimeFactory.CreateDate(2018, 12, 24);
            page.ActualEnd.Value = DateTimeFactory.CreateDate(2018, 12, 24);

            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new EmailCommunications(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            communicationEvent = after.First(v => v.Id.Equals(id));

            Assert.Equal(new CommunicationEventStates(this.Session).Completed, communicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Inquiry, communicationEvent.EventPurposes);
            Assert.Equal(this.addressee1EmailAddress, communicationEvent.Originator);
            Assert.Single(communicationEvent.Addressees);
            Assert.Contains(this.originatorEmailAddress, communicationEvent.Addressees);
            Assert.Empty(communicationEvent.CarbonCopies);
            Assert.Empty(communicationEvent.BlindCopies);
            Assert.Equal("new subject", communicationEvent.EmailTemplate.SubjectTemplate);
            Assert.Equal("new body", communicationEvent.EmailTemplate.BodyTemplate);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ScheduledStart);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ScheduledEnd.Value.Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualStart.Value.Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualEnd.Value.Date);
        }
    }
}
