// <copyright file="PersonLetterCorrespondenceCreateTest.cs" company="Allors bvba">
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
    using libs.angular.material.@base.src.export.objects.person.list;
    using libs.angular.material.@base.src.export.objects.person.overview;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class PersonLetterCorrespondenceCreateTest : Test
    {
        private readonly PersonListComponent personListPage;

        public PersonLetterCorrespondenceCreateTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.personListPage = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Create()
        {
            var person = new People(this.Session).Extent().First;

            var address = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithLocality("city")
                .WithPostalCode("1111")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            person.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session).WithContactMechanism(address).Build());

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = allors.ActiveEmployees.First;

            var employeeAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("home sweet home")
                .WithLocality("suncity")
                .WithPostalCode("0000")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            employee.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session).WithContactMechanism(employeeAddress).Build());

            this.Session.Derive();
            this.Session.Commit();

            var before = new LetterCorrespondences(this.Session).Extent().ToArray();

            this.personListPage.Table.DefaultAction(person);
            var letterCorrespondenceEdit = new PersonOverviewComponent(this.personListPage.Driver).CommunicationeventOverviewPanel.Click().CreateLetterCorrespondence();

            letterCorrespondenceEdit
                .CommunicationEventState.Select(new CommunicationEventStates(this.Session).Completed)
                .EventPurposes.Toggle(new CommunicationEventPurposes(this.Session).Appointment)
                .FromParty.Select(employee)
                .ToParty.Select(person)
                .FromPostalAddress.Select(employeeAddress)
                .Subject.Set("subject")
                .ScheduledStart.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ScheduledEnd.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ActualStart.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .ActualEnd.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .Comment.Set("comment")
                .SAVE.Click();

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
            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, communicationEvent.ScheduledStart.Value.ToUniversalTime().Date);
            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, communicationEvent.ScheduledEnd.Value.Date.ToUniversalTime().Date);
            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, communicationEvent.ActualStart.Value.Date.ToUniversalTime().Date);
            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, communicationEvent.ActualEnd.Value.Date.ToUniversalTime().Date);
            Assert.Equal("comment", communicationEvent.Comment);
        }
    }
}
