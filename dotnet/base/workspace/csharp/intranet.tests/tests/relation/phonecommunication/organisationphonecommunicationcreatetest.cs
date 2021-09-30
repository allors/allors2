// <copyright file="OrganisationPhoneCommunicationCreateTest.cs" company="Allors bvba">
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
    using src.allors.material.@base.objects.organisation.list;
    using src.allors.material.@base.objects.organisation.overview;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class OrganisationPhoneCommunicationCreateTest : Test
    {
        private readonly OrganisationListComponent organisations;

        private readonly PartyContactMechanism organisationPhoneNumber;

        private readonly PhoneCommunication editCommunicationEvent;

        public OrganisationPhoneCommunicationCreateTest(TestFixture fixture)
            : base(fixture)
        {
            var organisation = new Organisations(this.Session).Extent().First;

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var firstEmployee = allors.ActiveEmployees.First();

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

            this.Login();
            this.organisations = this.Sidenav.NavigateToOrganisations();
        }

        [Fact]
        public void Create()
        {
            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = allors.ActiveEmployees.First;

            var before = new PhoneCommunications(this.Session).Extent().ToArray();

            var organisation = allors.ActiveCustomers.First(v => v.GetType().Name == typeof(Organisation).Name);
            var contact = organisation.CurrentContacts.First;

            this.organisations.Table.DefaultAction(organisation);
            var phoneCommunication = new OrganisationOverviewComponent(this.organisations.Driver).CommunicationeventOverviewPanel.Click().CreatePhoneCommunication();

            phoneCommunication
                .LeftVoiceMail.Set(true)
                .CommunicationEventState.Select(new CommunicationEventStates(this.Session).Completed)
                .EventPurposes.Toggle(new CommunicationEventPurposes(this.Session).Inquiry)
                .Subject.Set("subject")
                .FromParty.Select(contact)
                .ToParty.Select(employee)
                .FromPhoneNumber.Select(contact.GeneralPhoneNumber)
                .ScheduledStart.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ScheduledEnd.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ActualStart.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .ActualEnd.Set(DateTimeFactory.CreateDate(2018, 12, 23))
                .Comment.Set("comment")
            .SAVE.Click();

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
            Assert.Equal("comment", communicationEvent.Comment);
        }
    }
}
