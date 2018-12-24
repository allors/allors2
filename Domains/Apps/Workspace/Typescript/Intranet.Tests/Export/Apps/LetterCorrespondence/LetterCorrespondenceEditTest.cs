namespace Tests.Intranet.LetterCorrespondenceTests
{
    using System;
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Tests.Components;
    using Tests.Intranet.PersonTests;

    using Xunit;

    [Collection("Test collection")]
    public class LetterCorrespondenceEditTest : Test
    {
        private readonly PersonListPage people;

        public LetterCorrespondenceEditTest(TestFixture fixture)
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

            person.AddCommunicationEvent(new LetterCorrespondenceBuilder(this.Session)
                .WithSubject("dummy")
                .WithOriginator(employee)
                .WithReceiver(person)
                .WithPostalAddress(address)
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

            var before = new LetterCorrespondences(this.Session).Extent().ToArray();

            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));
            var postalAddress = (PostalAddress)person.PartyContactMechanisms.First(v => v.ContactMechanism.GetType().Name == typeof(PostalAddress).Name).ContactMechanism;

            var personOverview = this.people.Select(person);
            var page = personOverview.NewLetterCorrespondence();

            page.IncomingLetter.Value = false;
            page.EventState.Value = new CommunicationEventStates(this.Session).Completed.Name;
            page.Purposes.Toggle(new CommunicationEventPurposes(this.Session).Appointment.Name);
            page.Originators.Add(employee.PartyName);
            page.Receivers.Add(person.PartyName);
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

            Assert.False(communicationEvent.IncomingLetter);
            Assert.Equal(new CommunicationEventStates(this.Session).Completed, communicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Appointment, communicationEvent.EventPurposes);
            Assert.Equal(postalAddress, communicationEvent.PostalAddress);
            Assert.Single(communicationEvent.Originators);
            Assert.Contains(employee, communicationEvent.Originators);
            Assert.Single(communicationEvent.Receivers);
            Assert.Contains(person, communicationEvent.Receivers);
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

            var postalAddress = (PostalAddress)firstEmployee.PartyContactMechanisms.First(v => v.ContactMechanism.GetType().Name == typeof(PostalAddress).Name).ContactMechanism;

            var communicationEvent = (LetterCorrespondence)person.CommunicationEvents.First(v => v.GetType().Name == typeof(LetterCorrespondence).Name);
            var id = communicationEvent.Id;

            var personOverview = this.people.Select(person);

            var page = personOverview.SelectLetterCorrespondence(communicationEvent);

            page.IncomingLetter.Value = false;
            page.EventState.Value = new CommunicationEventStates(this.Session).InProgress.Name;
            page.Purposes.Toggle(new CommunicationEventPurposes(this.Session).Appointment.Name);
            page.Originators.Remove(firstEmployee.PartyName);
            page.Originators.Add(secondEmployee.PartyName);
            page.Receivers.Remove(person.PartyName);
            page.Receivers.Add(firstEmployee.PartyName);
            page.PostalAddress.Value = "home sweet home 0000 suncity Belgium";
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

            communicationEvent = after.First(v => v.Id.Equals(id));

            Assert.False(communicationEvent.IncomingLetter);
            Assert.Equal(new CommunicationEventStates(this.Session).InProgress, communicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Appointment, communicationEvent.EventPurposes);
            Assert.Single(communicationEvent.Originators);
            Assert.Contains(secondEmployee, communicationEvent.Originators);
            Assert.Single(communicationEvent.Receivers);
            Assert.Contains(firstEmployee, communicationEvent.Receivers);
            Assert.Equal(postalAddress, communicationEvent.PostalAddress);
            Assert.Equal("new subject", communicationEvent.Subject);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, communicationEvent.ScheduledStart.Value.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, communicationEvent.ScheduledEnd.Value.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualStart.Value.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualEnd.Value.Date.ToUniversalTime().Date);
            Assert.Equal("new comment", communicationEvent.Comment);
        }
    }
}
