// <copyright file="OrganisationLetterCorrespondenceCreateTest.cs" company="Allors bvba">
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
    using libs.angular.material.@base.src.export.objects.organisation.list;
    using libs.angular.material.@base.src.export.objects.organisation.overview;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class OrganisationLetterCorrespondenceCreateTest : Test
    {
        private readonly OrganisationListComponent organisationListPage;

        public OrganisationLetterCorrespondenceCreateTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.organisationListPage = this.Sidenav.NavigateToOrganisations();
        }

        [Fact]
        public void Create()
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

            this.Session.Derive();
            this.Session.Commit();

            var before = new LetterCorrespondences(this.Session).Extent().ToArray();

            this.organisationListPage.Table.DefaultAction(organisation);
            var letterCorrespondenceEdit = new OrganisationOverviewComponent(this.organisationListPage.Driver).CommunicationeventOverviewPanel.Click().CreateLetterCorrespondence();

            letterCorrespondenceEdit
                .CommunicationEventState.Select(new CommunicationEventStates(this.Session).Completed)
                .EventPurposes.Toggle(new CommunicationEventPurposes(this.Session).Appointment)
                .FromParty.Select(organisation)
                .ToParty.Select(employee)
                .FromPostalAddress.Select(organisationAddress)
                .Subject.Set("subject");
            letterCorrespondenceEdit.ScheduledStart.Set(DateTimeFactory.CreateDate(2018, 12, 22));
            letterCorrespondenceEdit.ScheduledEnd.Set(DateTimeFactory.CreateDate(2018, 12, 22));
            letterCorrespondenceEdit.ActualStart.Set(DateTimeFactory.CreateDate(2018, 12, 23));
            letterCorrespondenceEdit.ActualEnd.Set(DateTimeFactory.CreateDate(2018, 12, 23));
            letterCorrespondenceEdit.Comment.Set("comment");
                            letterCorrespondenceEdit.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new LetterCorrespondences(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var communicationEvent = after.Except(before).First();

            Assert.Equal(new CommunicationEventStates(this.Session).Completed, communicationEvent.CommunicationEventState);
            Assert.Contains(new CommunicationEventPurposes(this.Session).Appointment, communicationEvent.EventPurposes);
            Assert.Equal(organisationAddress, communicationEvent.PostalAddress);
            Assert.Equal(organisation, communicationEvent.FromParty);
            Assert.Equal(employee, communicationEvent.ToParty);
            Assert.Equal("subject", communicationEvent.Subject);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, communicationEvent.ScheduledStart.Value.ToUniversalTime().Date);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, communicationEvent.ScheduledEnd.Value.Date.ToUniversalTime().Date);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, communicationEvent.ActualStart.Value.Date.ToUniversalTime().Date);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 23).Date, communicationEvent.ActualEnd.Value.Date.ToUniversalTime().Date);
            Assert.Equal("comment", communicationEvent.Comment);
        }
    }
}
