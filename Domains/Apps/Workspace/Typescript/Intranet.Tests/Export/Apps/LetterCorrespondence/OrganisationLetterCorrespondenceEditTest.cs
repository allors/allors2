namespace Tests.Intranet.LetterCorrespondenceTests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Tests.Components;
    using Tests.Intranet.OrganisationTests;

    using Xunit;

    [Collection("Test collection")]
    public class OrganisationLetterCorrespondenceEditTest : Test
    {
        private readonly OrganisationListPage organisations;

        private readonly LetterCorrespondence editCommunicationEvent;

        private readonly PostalAddress organisationAddress;

        private readonly PostalAddress employeeAddress;

        public OrganisationLetterCorrespondenceEditTest(TestFixture fixture)
            : base(fixture)
        {
            var organisations = new Organisations(this.Session).Extent();
            var organisation = organisations.First(v => v.PartyName.Equals("Acme0"));

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = allors.ActiveEmployees.First(v => v.FirstName.Equals("first"));

            this.organisationAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session).WithLocality("city").WithPostalCode("1111").WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE")).Build())
                .Build();

            organisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session).WithContactMechanism(this.organisationAddress).Build());

            this.employeeAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("home sweet home")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session).WithLocality("suncity").WithPostalCode("0000").WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE")).Build())
                .Build();

            employee.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session).WithContactMechanism(this.employeeAddress).Build());

            this.editCommunicationEvent = new LetterCorrespondenceBuilder(this.Session)
                .WithSubject("dummy")
                .WithFromParty(employee)
                .WithToParty(organisation)
                .WithPostalAddress(this.organisationAddress)
                .Build();

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

            var before = new LetterCorrespondences(this.Session).Extent().ToArray();

            var extent = new Organisations(this.Session).Extent();
            var organisation = extent.First(v => v.PartyName.Equals("Acme0"));

            var organisationOverviewPage = this.organisations.Select(organisation);
            var page = organisationOverviewPage.NewLetterCorrespondence();

            page.EventState.Value = new CommunicationEventStates(this.Session).Completed.Name;
            page.Purposes.Toggle(new CommunicationEventPurposes(this.Session).Appointment.Name);
            page.FromParty.Value = organisation.PartyName;
            page.ToParty.Value = employee.PartyName;
            page.PostalAddress.Value = "Haverwerf 15 1111 city Belgium";
            page.Subject.Value = "subject";
            page.ScheduledStart.Value = DateTimeFactory.CreateDate(2018, 12, 22);
            page.ScheduledEnd.Value = DateTimeFactory.CreateDate(2018, 12, 22);
            page.ActualStart.Value = DateTimeFactory.CreateDate(2018, 12, 23);
            page.ActualEnd.Value = DateTimeFactory.CreateDate(2018, 12, 23);
            page.Comment.Value = "comment";

            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new LetterCorrespondences(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var communicationEvent = after.Except(before).First();

            Assert.Equal(new CommunicationEventStates(this.Session).Completed, communicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Appointment, communicationEvent.EventPurposes);
            Assert.Equal(this.organisationAddress, communicationEvent.PostalAddress);
            Assert.Equal(organisation, communicationEvent.FromParty);
            Assert.Equal(employee, communicationEvent.ToParty);
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
            var employee = allors.ActiveEmployees.First(v => v.FirstName.Equals("first"));

            var before = new LetterCorrespondences(this.Session).Extent().ToArray();

            var organisationOverviewPage = this.organisations.Select(organisation);

            var page = organisationOverviewPage.SelectLetterCorrespondence(this.editCommunicationEvent);

            page.EventState.Value = new CommunicationEventStates(this.Session).InProgress.Name;
            page.Purposes.Toggle(new CommunicationEventPurposes(this.Session).Appointment.Name);
            page.FromParty.Value = organisation.PartyName;
            page.ToParty.Value = employee.PartyName;
            page.PostalAddress.Value = "Haverwerf 15 1111 city Belgium";
            page.Subject.Value = "new subject";
            page.ScheduledStart.Value = DateTimeFactory.CreateDate(2018, 12, 23);
            page.ScheduledEnd.Value = DateTimeFactory.CreateDate(2018, 12, 23);
            page.ActualStart.Value = DateTimeFactory.CreateDate(2018, 12, 24);
            page.ActualEnd.Value = DateTimeFactory.CreateDate(2018, 12, 24);
            page.Comment.Value = "new comment";

            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new LetterCorrespondences(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(new CommunicationEventStates(this.Session).InProgress, this.editCommunicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Appointment, this.editCommunicationEvent.EventPurposes);
            Assert.Equal(organisation, this.editCommunicationEvent.FromParty);
            Assert.Equal(employee, this.editCommunicationEvent.ToParty);
            Assert.Equal(this.organisationAddress, this.editCommunicationEvent.PostalAddress);
            Assert.Equal("new subject", this.editCommunicationEvent.Subject);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, communicationEvent.ScheduledStart.Value.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, communicationEvent.ScheduledEnd.Value.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualStart.Value.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualEnd.Value.Date.ToUniversalTime().Date);
            Assert.Equal("new comment", this.editCommunicationEvent.Comment);
        }
    }
}
