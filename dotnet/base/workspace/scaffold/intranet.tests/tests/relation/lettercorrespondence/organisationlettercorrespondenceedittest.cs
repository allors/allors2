// <copyright file="OrganisationLetterCorrespondenceEditTest.cs" company="Allors bvba">
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
    using libs.angular.material.@base.src.export.objects.organisation.list;
    using libs.angular.material.@base.src.export.objects.organisation.overview;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class OrganisationLetterCorrespondenceEditTest : Test
    {
        private readonly OrganisationListComponent organisationListPage;

        public OrganisationLetterCorrespondenceEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.organisationListPage = this.Sidenav.NavigateToOrganisations();
        }

        [Fact]
        public void Edit()
        {
            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = allors.ActiveEmployees.First();

            var organisation = allors.ActiveCustomers.First(v => v.GetType().Name == typeof(Organisation).Name);

            var organisationAddress = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithLocality("city")
                .WithPostalCode("1111")
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            organisation.AddPartyContactMechanism(new PartyContactMechanismBuilder(this.Session).WithContactMechanism(organisationAddress).Build());

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
                .WithToParty(organisation)
                .WithPostalAddress(organisationAddress)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var before = new LetterCorrespondences(this.Session).Extent().ToArray();

            this.organisationListPage.Table.DefaultAction(organisation);
            var organisationOverviewPage = new OrganisationOverviewComponent(this.organisationListPage.Driver);

            var communicationEventOverview = organisationOverviewPage.CommunicationeventOverviewPanel.Click();
            communicationEventOverview.Table.DefaultAction(editCommunicationEvent);

            var letterCorrespondenceEdit = new LetterCorrespondenceEditComponent(organisationOverviewPage.Driver);
            letterCorrespondenceEdit
                .CommunicationEventState.Select(new CommunicationEventStates(this.Session).InProgress)
                .EventPurposes.Toggle(new CommunicationEventPurposes(this.Session).Appointment)
                .FromParty.Select(organisation)
                .ToParty.Select(employee)
                .FromPostalAddress.Select(organisationAddress)
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
            Assert.Equal(organisation, editCommunicationEvent.FromParty);
            Assert.Equal(employee, editCommunicationEvent.ToParty);
            Assert.Equal(organisationAddress, editCommunicationEvent.PostalAddress);
            Assert.Equal("new subject", editCommunicationEvent.Subject);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, editCommunicationEvent.ScheduledStart.Value.ToUniversalTime().Date);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, editCommunicationEvent.ScheduledEnd.Value.Date.ToUniversalTime().Date);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, editCommunicationEvent.ActualStart.Value.Date.ToUniversalTime().Date);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, editCommunicationEvent.ActualEnd.Value.Date.ToUniversalTime().Date);
            Assert.Equal("new comment", editCommunicationEvent.Comment);
        }
    }
}
