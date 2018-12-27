namespace Tests.Intranet.EmailCommunicationTests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Tests.Components;
    using Tests.Intranet.OrganisationTests;

    using Xunit;

    [Collection("Test collection")]
    public class OrganisationEmailCommunicationEditTest : Test
    {
        private readonly OrganisationListPage organisations;

        public EmailAddress employeeEmailAddress { get; set; }

        public EmailAddress personEmailAddress { get; set; }

        private readonly EmailCommunication editCommunicationEvent;

        public OrganisationEmailCommunicationEditTest(TestFixture fixture)
            : base(fixture)
        {
            var organisations = new Organisations(this.Session).Extent();
            var organisation = organisations.First(v => v.PartyName.Equals("Acme0"));

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var firstEmployee = allors.ActiveEmployees.First(v => v.FirstName.Equals("first"));

            this.employeeEmailAddress = firstEmployee.PersonalEmailAddress;
            this.personEmailAddress = organisation.CurrentContacts.First.PersonalEmailAddress;

            this.editCommunicationEvent = new EmailCommunicationBuilder(this.Session)
                .WithSubject("dummy")
                .WithFromParty(firstEmployee)
                .WithFromEmail(this.employeeEmailAddress)
                .WithToParty(organisation.CurrentContacts.First)
                .WithToEmail(this.personEmailAddress)
                .WithEmailTemplate(new EmailTemplateBuilder(this.Session).Build())
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var dashboard = this.Login();
            this.organisations = dashboard.Sidenav.NavigateToOrganisationList();
        }

        [Fact]
        public void Add()
        {
            var before = new EmailCommunications(this.Session).Extent().ToArray();

            var extent = new Organisations(this.Session).Extent();
            var organisation = extent.First(v => v.PartyName.Equals("Acme0"));
            var contact = organisation.CurrentContacts.First;

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = allors.ActiveEmployees.First(v => v.FirstName.Equals("first"));

            var organisationOverviewPage = this.organisations.Select(organisation);
            var page = organisationOverviewPage.NewEmailCommunication();

            page.EventState.Value = new CommunicationEventStates(this.Session).Completed.Name;
            page.Purposes.Toggle(new CommunicationEventPurposes(this.Session).Appointment.Name);
            page.FromParty.Value = employee.PartyName;
            page.FromEmail.Value = this.employeeEmailAddress.ElectronicAddressString;
            page.ToParty.Value = contact.PartyName;
            page.ToEmail.Value = this.personEmailAddress.ElectronicAddressString;
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

            Assert.Equal(new CommunicationEventStates(this.Session).Completed, communicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Appointment, communicationEvent.EventPurposes);
            Assert.Equal(employee, communicationEvent.FromParty);
            Assert.Equal(this.employeeEmailAddress, communicationEvent.FromEmail);
            Assert.Equal(contact, communicationEvent.ToParty);
            Assert.Equal(this.personEmailAddress, communicationEvent.ToEmail);
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
            var extent = new Organisations(this.Session).Extent();
            var organisation = extent.First(v => v.PartyName.Equals("Acme0"));
            var contact = organisation.CurrentContacts.First;

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = allors.ActiveEmployees.First(v => v.FirstName.Equals("first"));

            var before = new EmailCommunications(this.Session).Extent().ToArray();

            var organisationOverviewPage = this.organisations.Select(organisation);

            var page = organisationOverviewPage.SelectEmailCommunication(this.editCommunicationEvent);

            page.EventState.Value = new CommunicationEventStates(this.Session).Completed.Name;
            page.Purposes.Toggle(new CommunicationEventPurposes(this.Session).Inquiry.Name);
            page.FromParty.Value = contact.PartyName;
            page.FromEmail.Value = this.personEmailAddress.ElectronicAddressString;
            page.ToParty.Value = employee.PartyName;
            page.ToEmail.Value = this.employeeEmailAddress.ElectronicAddressString;
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

            Assert.Equal(new CommunicationEventStates(this.Session).Completed, this.editCommunicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Inquiry, this.editCommunicationEvent.EventPurposes);
            Assert.Equal(contact, this.editCommunicationEvent.FromParty);
            Assert.Equal(this.personEmailAddress, this.editCommunicationEvent.FromEmail);
            Assert.Equal(employee, this.editCommunicationEvent.ToParty);
            Assert.Equal(this.employeeEmailAddress, this.editCommunicationEvent.ToEmail);
            Assert.Equal("new subject", this.editCommunicationEvent.EmailTemplate.SubjectTemplate);
            Assert.Equal("new body", this.editCommunicationEvent.EmailTemplate.BodyTemplate);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ScheduledStart);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ScheduledEnd.Value.Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualStart.Value.Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualEnd.Value.Date);
        }
    }
}
