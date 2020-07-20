// <copyright file="OrganisationFaceToFaceCommunicationEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.FaceToFaceCommunicationTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;
    using Components;
    using src.allors.material.@base.objects.facetofacecommunication.edit;
    using src.allors.material.@base.objects.organisation.list;
    using src.allors.material.@base.objects.organisation.overview;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class OrganisationFaceToFaceCommunicationEditTest : Test
    {
        private readonly OrganisationListComponent organisationListPage;

        public OrganisationFaceToFaceCommunicationEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.organisationListPage = this.Sidenav.NavigateToOrganisations();
        }

        [Fact]
        public void Edit()
        {
            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var firstEmployee = allors.ActiveEmployees.First();

            var secondEmployee = allors.CreateEmployee("letmein", this.Session.Faker()); //second employee
            this.Session.Derive();

            var organisation = allors.ActiveCustomers.First(v => v.GetType().Name == typeof(Organisation).Name);
            var contact = organisation.CurrentContacts.First;

            var editCommunicationEvent = new FaceToFaceCommunicationBuilder(this.Session)
                .WithSubject("dummy")
                .WithFromParty(organisation.CurrentContacts.First)
                .WithToParty(firstEmployee)
                .WithLocation("old location")
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var before = new FaceToFaceCommunications(this.Session).Extent().ToArray();

            this.organisationListPage.Table.DefaultAction(organisation);
            var organisationOverview = new OrganisationOverviewComponent(this.organisationListPage.Driver);

            var communicationEventOverview = organisationOverview.CommunicationeventOverviewPanel.Click();
            communicationEventOverview.Table.DefaultAction(editCommunicationEvent);

            var faceToFaceCommunicationEdit = new FaceToFaceCommunicationEditComponent(organisationOverview.Driver);
            faceToFaceCommunicationEdit
                .CommunicationEventState.Select(new CommunicationEventStates(this.Session).Completed)
                .EventPurposes.Toggle(new CommunicationEventPurposes(this.Session).Conference)
                .Location.Set("new location")
                .Subject.Set("new subject")
                .FromParty.Select(secondEmployee)
                .ToParty.Select(contact)
                .ScheduledStart.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .ScheduledEnd.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .ActualStart.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .ActualEnd.Set(DateTimeFactory.CreateDate(2018, 12, 24))
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new FaceToFaceCommunications(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(new CommunicationEventStates(this.Session).Completed, editCommunicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Conference, editCommunicationEvent.EventPurposes);
            Assert.Equal(secondEmployee, editCommunicationEvent.FromParty);
            Assert.Equal(contact, editCommunicationEvent.ToParty);
            Assert.Equal("new location", editCommunicationEvent.Location);
            Assert.Equal("new subject", editCommunicationEvent.Subject);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, editCommunicationEvent.ScheduledStart);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, editCommunicationEvent.ScheduledEnd.Value.Date);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, editCommunicationEvent.ActualStart.Value.Date);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 24).Date, editCommunicationEvent.ActualEnd.Value.Date);
        }
    }
}
