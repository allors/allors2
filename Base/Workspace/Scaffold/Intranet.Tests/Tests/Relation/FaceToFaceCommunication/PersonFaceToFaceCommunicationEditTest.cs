// <copyright file="PersonFaceToFaceCommunicationEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.FaceToFaceCommunicationTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Meta;
    using Components;
    using libs.angular.material.@base.src.export.objects.facetofacecommunication.edit;
    using libs.angular.material.@base.src.export.objects.person.list;
    using libs.angular.material.@base.src.export.objects.person.overview;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class PersonFaceToFaceCommunicationEditTest : Test
    {
        private readonly PersonListComponent personListPage;

        public PersonFaceToFaceCommunicationEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.personListPage = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Edit()
        {
            var faker = this.Session.Faker();
            var subject = faker.Lorem.Sentence();
            var location = faker.Address.FullAddress();

            var person = new People(this.Session).Extent().First;

            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var firstEmployee = allors.ActiveEmployees.First();
            var secondEmployee = allors.ActiveEmployees.Last();

            var editCommunicationEvent = new FaceToFaceCommunicationBuilder(this.Session)
                .WithSubject(subject)
                .WithFromParty(person)
                .WithToParty(firstEmployee)
                .WithLocation(location)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var before = new FaceToFaceCommunications(this.Session).Extent().ToArray();

            this.personListPage.Table.DefaultAction(person);
            var personOverview = new PersonOverviewComponent(this.personListPage.Driver);

            var communicationEventOverview = personOverview.CommunicationeventOverviewPanel.Click();
            communicationEventOverview.Table.DefaultAction(editCommunicationEvent);

            var faceToFaceCommunicationEditComponent = new FaceToFaceCommunicationEditComponent(this.Driver);

            var scheduleStartDate = DateTimeFactory.CreateDate(2018, 12, 24);
            var scheduleEndDate = DateTimeFactory.CreateDate(2018, 12, 24);
            var actualStartDate = DateTimeFactory.CreateDate(2018, 12, 24);
            var actualEndDate = DateTimeFactory.CreateDate(2018, 12, 24);

            faceToFaceCommunicationEditComponent
                .CommunicationEventState.Select(new CommunicationEventStates(this.Session).Completed)
                .EventPurposes.Toggle(new CommunicationEventPurposes(this.Session).Conference)
                .Subject.Set(subject)
                .Location.Set(location)
                .FromParty.Select(secondEmployee)
                .ToParty.Select(person)
                .ScheduledStart.Set(scheduleStartDate)
                .ScheduledEnd.Set(scheduleEndDate)
                .ActualStart.Set(actualStartDate)
                .ActualEnd.Set(actualEndDate)
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new FaceToFaceCommunications(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(new CommunicationEventStates(this.Session).Completed, editCommunicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Conference, editCommunicationEvent.EventPurposes);
            Assert.Equal(secondEmployee, editCommunicationEvent.FromParty);
            Assert.Equal(person, editCommunicationEvent.ToParty);
            Assert.Equal(subject, editCommunicationEvent.Subject);
            Assert.Equal(location, editCommunicationEvent.Location);
            Assert.Equal(scheduleStartDate.Date, editCommunicationEvent.ScheduledStart.Value.Date);
            Assert.Equal(scheduleEndDate.Date, editCommunicationEvent.ScheduledEnd.Value.Date);
            Assert.Equal(actualStartDate.Date, editCommunicationEvent.ActualStart.Value.Date);
            Assert.Equal(actualEndDate.Date, editCommunicationEvent.ActualEnd.Value.Date);
        }
    }
}
