// <copyright file="PersonPhoneCommunicationEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.PhoneCommunicationTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;
    using Components;
    using src.allors.material.@base.objects.person.list;
    using src.allors.material.@base.objects.person.overview;
    using src.allors.material.@base.objects.phonecommunication.edit;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class PersonPhoneCommunicationEditTest : Test
    {
        private readonly PersonListComponent people;

        private readonly PartyContactMechanism anotherPhoneNumber;

        private readonly PhoneCommunication editCommunicationEvent;

        public PersonPhoneCommunicationEditTest(TestFixture fixture)
            : base(fixture)
        {
            var person = new People(this.Session).Extent().First;

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var firstEmployee = allors.ActiveEmployees.First();

            this.editCommunicationEvent = new PhoneCommunicationBuilder(this.Session)
                .WithSubject("dummy")
                .WithLeftVoiceMail(true)
                .WithFromParty(person)
                .WithToParty(firstEmployee)
                .WithPhoneNumber(person.GeneralPhoneNumber)
                .Build();

            this.anotherPhoneNumber = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(new TelecommunicationsNumberBuilder(this.Session).WithCountryCode("+1").WithAreaCode("111").WithContactNumber("222").Build())
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).SalesOffice)
                .WithUseAsDefault(false)
                .Build();

            person.AddPartyContactMechanism(this.anotherPhoneNumber);

            this.Session.Derive();
            this.Session.Commit();

            this.Login();
            this.people = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Edit()
        {
            var person = new People(this.Session).Extent().First;

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var firstEmployee = allors.ActiveEmployees.First();

            var before = new PhoneCommunications(this.Session).Extent().ToArray();

            this.people.Table.DefaultAction(person);
            var personOverview = new PersonOverviewComponent(this.people.Driver);

            var communicationEventOverview = personOverview.CommunicationeventOverviewPanel.Click();
            communicationEventOverview.Table.DefaultAction(this.editCommunicationEvent);

            var phoneCommunicationEditComponent = new PhoneCommunicationEditComponent(this.Driver);
            phoneCommunicationEditComponent
                .LeftVoiceMail.Set(false)
                .CommunicationEventState.Select(new CommunicationEventStates(this.Session).Completed)
                .EventPurposes.Toggle(new CommunicationEventPurposes(this.Session).Inquiry)
                .FromPhoneNumber.Select(this.anotherPhoneNumber.ContactMechanism)
                .Subject.Set("new subject")
                .FromParty.Select(firstEmployee)
                .ToParty.Select(person)
                .ScheduledStart.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .ScheduledEnd.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .ActualStart.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .ActualEnd.Set(DateTimeFactory.CreateDate(2018, 12, 25))
                .Comment.Set("new comment")
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PhoneCommunications(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.False(this.editCommunicationEvent.LeftVoiceMail);
            Assert.Equal(new CommunicationEventStates(this.Session).Completed, this.editCommunicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Inquiry, this.editCommunicationEvent.EventPurposes);
            Assert.Equal(firstEmployee, this.editCommunicationEvent.FromParty);
            Assert.Equal(person, this.editCommunicationEvent.ToParty);
            Assert.Equal(this.anotherPhoneNumber.ContactMechanism, this.editCommunicationEvent.PhoneNumber);
            Assert.Equal("new subject", this.editCommunicationEvent.Subject);
            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ScheduledStart);
            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ScheduledEnd.Value.Date);
            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualStart.Value.Date);
            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, communicationEvent.ActualEnd.Value.Date);
            Assert.Equal("new comment", this.editCommunicationEvent.Comment);
        }
    }
}
