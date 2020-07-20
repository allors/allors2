// <copyright file="OrganisationPhoneCommunicationEditTest.cs" company="Allors bvba">
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
    using src.allors.material.@base.objects.phonecommunication.edit;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class OrganisationPhoneCommunicationEditTest : Test
    {
        private readonly OrganisationListComponent organisations;

        private readonly PartyContactMechanism organisationPhoneNumber;

        private readonly PhoneCommunication editCommunicationEvent;

        public OrganisationPhoneCommunicationEditTest(TestFixture fixture)
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
                .WithContactMechanism(new TelecommunicationsNumberBuilder(this.Session)
                    .WithCountryCode("+1")
                    .WithAreaCode("111")
                    .WithContactNumber("222")
                    .Build())
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
        public void Edit()
        {
            var organisation = new Organisations(this.Session).Extent().First;

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var firstEmployee = allors.ActiveEmployees.First();

            var before = new PhoneCommunications(this.Session).Extent().ToArray();

            this.organisations.Table.DefaultAction(organisation);
            var personOverview = new OrganisationOverviewComponent(this.organisations.Driver);

            var communicationEventOverview = personOverview.CommunicationeventOverviewPanel.Click();
            communicationEventOverview.Table.DefaultAction(this.editCommunicationEvent);

            var phoneCommunicationEdit = new PhoneCommunicationEditComponent(personOverview.Driver);
            phoneCommunicationEdit
                .LeftVoiceMail.Set(false)
                .CommunicationEventState.Select(new CommunicationEventStates(this.Session).Completed)
                .EventPurposes.Toggle(new CommunicationEventPurposes(this.Session).Inquiry)
                .FromParty.Select(organisation)
                .ToParty.Select(firstEmployee)
                .FromPhoneNumber.Select(this.organisationPhoneNumber.ContactMechanism)
                .Subject.Set("new subject")
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
            Assert.Equal(organisation, this.editCommunicationEvent.FromParty);
            Assert.Equal(firstEmployee, this.editCommunicationEvent.ToParty);
            Assert.Equal(this.organisationPhoneNumber.ContactMechanism, this.editCommunicationEvent.PhoneNumber);
            Assert.Equal("new subject", this.editCommunicationEvent.Subject);
            Assert.Equal("new comment", this.editCommunicationEvent.Comment);
        }
    }
}
