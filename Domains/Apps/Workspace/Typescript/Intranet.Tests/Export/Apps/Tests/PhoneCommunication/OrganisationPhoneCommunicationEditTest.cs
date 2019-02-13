namespace Tests.PhoneCommunicationTests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Angular;

    using Pages.OrganisationTests;

    using Tests.OrganisationTests;

    using Xunit;

    [Collection("Test collection")]
    public class OrganisationPhoneCommunicationEditTest : Test
    {
        private readonly OrganisationListPage organisations;

        private readonly PartyContactMechanism organisationPhoneNumber;

        private readonly PhoneCommunication editCommunicationEvent;

        public OrganisationPhoneCommunicationEditTest(TestFixture fixture)
            : base(fixture)
        {
            var people = new Organisations(this.Session).Extent();
            var organisation = people.First(v => v.PartyName.Equals("Acme0"));

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var firstEmployee = allors.ActiveEmployees.First(v => v.FirstName.Equals("first"));

            this.editCommunicationEvent = new PhoneCommunicationBuilder(this.Session)
                .WithSubject("dummy")
                .WithLeftVoiceMail(true)
                .WithFromParty(firstEmployee)
                .WithToParty(organisation.CurrentContacts.First)
                .WithPhoneNumber(organisation.GeneralPhoneNumber)
                .Build();

            this.organisationPhoneNumber = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(new TelecommunicationsNumberBuilder(this.Session).WithCountryCode("+1").WithAreaCode("111").WithContactNumber("222").Build())
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).SalesOffice)
                .WithUseAsDefault(false)
                .Build();

            organisation.AddPartyContactMechanism(this.organisationPhoneNumber);

            this.Session.Derive();
            this.Session.Commit();

            var dashboard = this.Login();
            this.organisations = dashboard.Sidenav.NavigateToOrganisationList();
        }

        [Fact]
        public void Create()
        {
            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = allors.ActiveEmployees.First;

            var before = new PhoneCommunications(this.Session).Extent().ToArray();

            var extent = new Organisations(this.Session).Extent();
            var organisation = extent.First(v => v.PartyName.Equals("Acme0"));
            var contact = organisation.CurrentContacts.First(v => v.FirstName.Equals("Jane0"));

            var organisationOverviewPage = this.organisations.Select(organisation);
            var page = organisationOverviewPage.NewPhoneCommunication();

            page.LeftVoiceMail.Set(true)
                .EventState.Set(new CommunicationEventStates(this.Session).Completed.Name)
                .Purposes.Toggle(new CommunicationEventPurposes(this.Session).Inquiry.Name)
                .Subject.Set("subject")
                .FromParty.Set(contact.PartyName)
                .ToParty.Set(employee.PartyName)
                .PhoneNumber.Set("+1 123 456")
                .ScheduledStart.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ScheduledEnd.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ActualStart.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .ActualEnd.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .Comment.Set("comment")
                .Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PhoneCommunications(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var communicationEvent = after.Except(before).First();

            Assert.True(communicationEvent.LeftVoiceMail);
            Assert.Equal(new CommunicationEventStates(this.Session).Completed, communicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Inquiry, communicationEvent.EventPurposes);
            Assert.Equal(contact, communicationEvent.FromParty);
            Assert.Equal(employee, communicationEvent.ToParty);
            Assert.Equal(contact.GeneralPhoneNumber, communicationEvent.PhoneNumber);
            Assert.Equal("subject", communicationEvent.Subject);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, communicationEvent.ScheduledStart.Value.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, communicationEvent.ScheduledEnd.Value.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, communicationEvent.ActualStart.Value.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, communicationEvent.ActualEnd.Value.Date.ToUniversalTime().Date);
            Assert.Equal("comment", communicationEvent.Comment);
        }

        [Fact]
        public void Edit()
        {
            var extent = new Organisations(this.Session).Extent();
            var organisation = extent.First(v => v.PartyName.Equals("Acme0"));

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var firstEmployee = allors.ActiveEmployees.First(v => v.FirstName.Equals("first"));

            var before = new PhoneCommunications(this.Session).Extent().ToArray();

            var personOverview = this.organisations.Select(organisation);

            var page = personOverview.SelectPhoneCommunication(this.editCommunicationEvent);

            page.LeftVoiceMail.Set(false)
                .EventState.Set(new CommunicationEventStates(this.Session).Completed.Name)
                .Purposes.Toggle(new CommunicationEventPurposes(this.Session).Inquiry.Name)
                .FromParty.Set(organisation.PartyName)
                .ToParty.Set(firstEmployee.PartyName)
                .PhoneNumber.Set("+1 111 222")
                .Subject.Set("new subject")
                .ScheduledStart.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .ScheduledEnd.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .ActualStart.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .ActualEnd.Set(DateTimeFactory.CreateDate(2018, 12, 25))
                .Comment.Set("new comment")
                .Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PhoneCommunications(this.Session).Extent().ToArray();
            Assert.Equal(after.Length, before.Length);

            Assert.False(this.editCommunicationEvent.LeftVoiceMail);
            Assert.Equal(new CommunicationEventStates(this.Session).Completed, this.editCommunicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Inquiry, this.editCommunicationEvent.EventPurposes);
            Assert.Equal(organisation, this.editCommunicationEvent.FromParty);
            Assert.Equal(firstEmployee, this.editCommunicationEvent.ToParty);
            Assert.Equal(this.organisationPhoneNumber.ContactMechanism, this.editCommunicationEvent.PhoneNumber);
            Assert.Equal("new subject", this.editCommunicationEvent.Subject);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ScheduledStart);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ScheduledEnd.Value.Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualStart.Value.Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualEnd.Value.Date);
            Assert.Equal("new comment", this.editCommunicationEvent.Comment);
        }
    }
}
