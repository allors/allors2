using src.allors.material.apps.objects.person.list;

namespace Tests.LetterCorrespondenceTests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Angular;

    using Pages.PersonTests;

    using Xunit;

    [Collection("Test collection")]
    public class PersonLetterCorrespondenceEditTest : Test
    {
        private readonly PersonListComponent personListPage;

        public PersonLetterCorrespondenceEditTest(TestFixture fixture)
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

            var address = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session).WithLocality("city").WithPostalCode("1111").WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE")).Build())
                .Build();

            person.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session).WithContactMechanism(address).Build());

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = allors.ActiveEmployees.First;

            var employeeAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("home sweet home")
                .WithPostalBoundary(new PostalBoundaryBuilder(this.Session).WithLocality("suncity").WithPostalCode("0000").WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE")).Build())
                .Build();

            employee.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session).WithContactMechanism(employeeAddress).Build());

            this.Session.Derive();
            this.Session.Commit();

            var before = new LetterCorrespondences(this.Session).Extent().ToArray();

            var page = this.personListPage.Select(person).NewLetterCorrespondence();

            page.EventState.Set(new CommunicationEventStates(this.Session).Completed.Name)
                .Purposes.Toggle(new CommunicationEventPurposes(this.Session).Appointment.Name)
                .FromParty.Set(employee.PartyName)
                .ToParty.Set(person.PartyName)
                .PostalAddress.Set("home sweet home 0000 suncity Belgium")
                .Subject.Set("subject")
                .ScheduledStart.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ScheduledEnd.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ActualStart.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .ActualEnd.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .Comment.Set("comment")
                .Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new LetterCorrespondences(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var communicationEvent = after.Except(before).First();

            Assert.Equal(new CommunicationEventStates(this.Session).Completed, communicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Appointment, communicationEvent.EventPurposes);
            Assert.Equal(employeeAddress, communicationEvent.PostalAddress);
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

            var editCommunicationEvent = new LetterCorrespondenceBuilder(this.Session)
                .WithSubject("dummy")
                .WithFromParty(employee)
                .WithToParty(person)
                .WithPostalAddress(address)
                .Build();

            this.Session.Derive();
            this.Session.Commit();
            
            var before = new LetterCorrespondences(this.Session).Extent().ToArray();

            var postalAddress = (PostalAddress)person.PartyContactMechanisms.First(v => v.ContactMechanism.GetType().Name == typeof(PostalAddress).Name).ContactMechanism;

            var personOverview = this.personListPage.Select(person);

            var page = personOverview.SelectLetterCorrespondence(editCommunicationEvent);

            page.EventState.Set(new CommunicationEventStates(this.Session).InProgress.Name)
                .Purposes.Toggle(new CommunicationEventPurposes(this.Session).Appointment.Name)
                .FromParty.Set(person.PartyName)
                .ToParty.Set(employee.PartyName)
                .PostalAddress.Set("Haverwerf 15 1111 city Belgium")
                .Subject.Set("new subject")
                .ScheduledStart.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .ScheduledEnd.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .ActualStart.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .ActualEnd.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .Comment.Set("new comment")
                .Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new LetterCorrespondences(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(new CommunicationEventStates(this.Session).InProgress, editCommunicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Appointment, editCommunicationEvent.EventPurposes);
            Assert.Equal(person, editCommunicationEvent.FromParty);
            Assert.Equal(employee, editCommunicationEvent.ToParty);
            Assert.Equal(postalAddress, editCommunicationEvent.PostalAddress);
            Assert.Equal("new subject", editCommunicationEvent.Subject);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, communicationEvent.ScheduledStart.Value.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, communicationEvent.ScheduledEnd.Value.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualStart.Value.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualEnd.Value.Date.ToUniversalTime().Date);
            Assert.Equal("new comment", editCommunicationEvent.Comment);
        }
    }
}
