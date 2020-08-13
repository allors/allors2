// <copyright file="SupplierRelationshipEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.PartyRelationshipTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;
    using Components;
    using libs.angular.material.@base.src.export.objects.organisation.list;
    using libs.angular.material.@base.src.export.objects.organisation.overview;
    using libs.angular.material.@base.src.export.objects.supplierrelationship.edit;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class SupplierRelationshipEditTest : Test
    {
        private readonly OrganisationListComponent organisations;

        private readonly PartyRelationship editPartyRelationship;

        public SupplierRelationshipEditTest(TestFixture fixture)
            : base(fixture)
        {
            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();

            // Delete all existing for the new one to be in the first page of the list.
            foreach (PartyRelationship partyRelationship in allors.PartyRelationshipsWhereParty)
            {
                partyRelationship.Delete();
            }

            this.editPartyRelationship = new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(supplier)
                .WithInternalOrganisation(allors)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.Login();
            this.organisations = this.Sidenav.NavigateToOrganisations();
        }

        [Fact]
        public void Create()
        {
            var before = new PartyRelationships(this.Session).Extent().ToArray();

            var extent = new Organisations(this.Session).Extent();
            var internalOrganisation = extent.First(v => v.DisplayName().Equals("Allors BVBA"));

            this.organisations.Table.DefaultAction(internalOrganisation);
            var partyRelationshipEdit = new OrganisationOverviewComponent(this.organisations.Driver).PartyrelationshipOverviewPanel.Click().CreateCustomerRelationship();

            partyRelationshipEdit
                .FromDate.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ThroughDate.Set(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1))
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PartyRelationships(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var partyRelationship = after.Except(before).First();

            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, partyRelationship.FromDate.Date.ToUniversalTime().Date);
            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1).Date, partyRelationship.ThroughDate.Value.Date.ToUniversalTime().Date);
        }

        [Fact]
        public void Edit()
        {
            var before = new PartyRelationships(this.Session).Extent().ToArray();

            var extent = new Organisations(this.Session).Extent();
            var internalOrganisation = extent.First(v => v.DisplayName().Equals("Allors BVBA"));

            this.organisations.Table.DefaultAction(internalOrganisation);
            var organisationOverviewPage = new OrganisationOverviewComponent(this.organisations.Driver);

            var partyRelationshipOverview = organisationOverviewPage.PartyrelationshipOverviewPanel.Click();
            partyRelationshipOverview.Table.DefaultAction(this.editPartyRelationship);

            var partyRelationshipEdit = new SupplierRelationshipEditComponent(organisationOverviewPage.Driver);
            partyRelationshipEdit
                .FromDate.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ThroughDate.Set(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1))
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PartyRelationships(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, this.editPartyRelationship.FromDate.Date.ToUniversalTime().Date);
            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1).Date, this.editPartyRelationship.ThroughDate.Value.Date.ToUniversalTime().Date);
        }
    }
}
