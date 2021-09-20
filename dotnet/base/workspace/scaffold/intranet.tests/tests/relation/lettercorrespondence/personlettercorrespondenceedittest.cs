// <copyright file="PersonLetterCorrespondenceEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.LetterCorrespondenceTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;
    using Components;
    using libs.angular.material.@base.src.export.objects.lettercorrespondence.edit;
    using libs.angular.material.@base.src.export.objects.person.list;
    using libs.angular.material.@base.src.export.objects.person.overview;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class PersonLetterCorrespondenceEditTest : Test
    {
        private readonly PersonListComponent personListPage;

        public PersonLetterCorrespondenceEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.personListPage = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Edit()
        {
            var person = new People(this.Session).Extent().First;

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = allors.ActiveEmployees.First();

            var address = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithLocality("city")
                .WithPostalCode("1111")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            person.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session).WithContactMechanism(address).Build());

            var employeeAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("home sweet home")
                .WithLocality("suncity")
                .WithPostalCode("0000")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
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

            this.personListPage.Table.DefaultAction(person);
            var personOverview = new PersonOverviewComponent(this.personListPage.Driver);

            var communicationEventOverview = personOverview.CommunicationeventOverviewPanel.Click();
            communicationEventOverview.Table.DefaultAction(editCommunicationEvent);

            var letterCorrespondenceEditComponent = new LetterCorrespondenceEditComponent(this.Driver);
            letterCorrespondenceEditComponent
                .CommunicationEventState.Select(new CommunicationEventStates(this.Session).InProgress)
                .EventPurposes.Toggle(new CommunicationEventPurposes(this.Session).Appointment)
                .FromParty.Select(person)
                .ToParty.Select(employee)
                .FromPostalAddress.Select(postalAddress)
                .Subject.Set("new subject")
                .ScheduledStart.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .ScheduledEnd.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .ActualStart.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .ActualEnd.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .Comment.Set("new comment")
                .SAVE.Click();

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
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, editCommunicationEvent.ScheduledStart.Value.ToUniversalTime().Date);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, editCommunicationEvent.ScheduledEnd.Value.Date.ToUniversalTime().Date);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, editCommunicationEvent.ActualStart.Value.Date.ToUniversalTime().Date);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, editCommunicationEvent.ActualEnd.Value.Date.ToUniversalTime().Date);
            Assert.Equal("new comment", editCommunicationEvent.Comment);
        }
    }
}
