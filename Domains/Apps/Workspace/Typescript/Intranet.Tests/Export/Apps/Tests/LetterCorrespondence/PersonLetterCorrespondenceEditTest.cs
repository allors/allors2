namespace Tests.LetterCorrespondenceTests
{
    using System;
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Angular;

    using Pages.PersonTests;

    using Tests.PersonTests;

    using Xunit;

    [Collection("Test collection")]
    public class PersonLetterCorrespondenceEditTest : Test
    {
        private readonly PersonListPage people;

        private readonly LetterCorrespondence editCommunicationEvent;

        public PersonLetterCorrespondenceEditTest(TestFixture fixture)
            : base(fixture)
        {
            var people = new People(this.Session).Extent();
            var person = people.First(v => v.PartyName.Equals("John0 Doe0"));

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = allors.ActiveEmployees.First(v => v.FirstName.Equals("first"));

            var address = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session).WithLocality("city").WithPostalCode("1111").WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE")).Build())
                .Build();

            person.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session).WithContactMechanism(address).Build());

            var employeeAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("home sweet home")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session).WithLocality("suncity").WithPostalCode("0000").WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE")).Build())
                .Build();

            employee.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session).WithContactMechanism(employeeAddress).Build());

            this.editCommunicationEvent = new LetterCorrespondenceBuilder(this.Session)
                .WithSubject("dummy")
                .WithFromParty(employee)
                .WithToParty(person)
                .WithPostalAddress(address)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var dashboard = this.Login();
            this.people = dashboard.Sidenav.NavigateToPersonList();
        }

        [Fact]
        public void Create()
        {
            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = allors.ActiveEmployees.First;
            var postalAddress = (PostalAddress)employee.PartyContactMechanisms.First(v => v.ContactMechanism.GetType().Name == typeof(PostalAddress).Name).ContactMechanism;

            var before = new LetterCorrespondences(this.Session).Extent().ToArray();

            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));

            var personOverview = this.people.Select(person);
            var page = personOverview.NewLetterCorrespondence();

            page.EventState.Value = new CommunicationEventStates(this.Session).Completed.Name;
            page.Purposes.Toggle(new CommunicationEventPurposes(this.Session).Appointment.Name);
            page.FromParty.Value = employee.PartyName;
            page.ToParty.Value = person.PartyName;
            page.PostalAddress.Value = "home sweet home 0000 suncity Belgium";
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
            Assert.Equal(postalAddress, communicationEvent.PostalAddress);
            Assert.Equal(employee, communicationEvent.FromParty);
            Assert.Equal(person, communicationEvent.ToParty);
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
            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var firstEmployee = allors.ActiveEmployees.First(v => v.FirstName.Equals("first"));
            var secondEmployee = allors.ActiveEmployees.First(v => v.FirstName.Equals("second"));

            var before = new LetterCorrespondences(this.Session).Extent().ToArray();

            var postalAddress = (PostalAddress)person.PartyContactMechanisms.First(v => v.ContactMechanism.GetType().Name == typeof(PostalAddress).Name).ContactMechanism;

            var personOverview = this.people.Select(person);

            var page = personOverview.SelectLetterCorrespondence(this.editCommunicationEvent);

            page.EventState.Value = new CommunicationEventStates(this.Session).InProgress.Name;
            page.Purposes.Toggle(new CommunicationEventPurposes(this.Session).Appointment.Name);
            page.FromParty.Value = person.PartyName;
            page.ToParty.Value = firstEmployee.PartyName;
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
            Assert.Equal(person, this.editCommunicationEvent.FromParty);
            Assert.Equal(firstEmployee, this.editCommunicationEvent.ToParty);
            Assert.Equal(postalAddress, this.editCommunicationEvent.PostalAddress);
            Assert.Equal("new subject", this.editCommunicationEvent.Subject);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, communicationEvent.ScheduledStart.Value.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, communicationEvent.ScheduledEnd.Value.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualStart.Value.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualEnd.Value.Date.ToUniversalTime().Date);
            Assert.Equal("new comment", this.editCommunicationEvent.Comment);
        }
    }
}
