// <copyright file="InternalOrganisationEmploymentEditTest.cs" company="Allors bvba">
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
    using libs.angular.material.@base.src.export.objects.employment.edit;
    using libs.angular.material.@base.src.export.objects.organisation.list;
    using libs.angular.material.@base.src.export.objects.organisation.overview;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class InternalOrganisationEmploymentEditTest : Test
    {
        private readonly OrganisationListComponent organisationListPage;

        public InternalOrganisationEmploymentEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.organisationListPage = this.Sidenav.NavigateToOrganisations();
        }

        [Fact]
        public void Create()
        {
            var employer = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = new PersonBuilder(this.Session).WithLastName("employee").Build();

            // Delete all existing for the new one to be in the first page of the list.
            foreach (PartyRelationship relationship in employer.PartyRelationshipsWhereParty)
            {
                relationship.Delete();
            }

            this.Session.Derive();
            this.Session.Commit();

            var before = new Employments(this.Session).Extent().ToArray();

            this.organisationListPage.Table.DefaultAction(employer);
            var partyRelationshipEdit = new OrganisationOverviewComponent(this.organisationListPage.Driver).PartyrelationshipOverviewPanel.Click().CreateEmployment();

            partyRelationshipEdit
                .FromDate.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ThroughDate.Set(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1))
                .Employee.Select(employee)
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Employments(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var partyRelationship = after.Except(before).First();

            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, partyRelationship.FromDate.Date.ToUniversalTime().Date);
            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1).Date, partyRelationship.ThroughDate.Value.Date.ToUniversalTime().Date);
            Assert.Equal(employer, partyRelationship.Employer);
            Assert.Equal(employee, partyRelationship.Employee);
        }

        [Fact]
        public void Edit()
        {
            var employer = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = new PersonBuilder(this.Session).WithLastName("employee").Build();

            // Delete all existing for the new one to be in the first page of the list.
            foreach (PartyRelationship partyRelationship in employer.PartyRelationshipsWhereParty)
            {
                partyRelationship.Delete();
            }

            this.Session.Derive();
            this.Session.Commit();

            var editPartyRelationship = new EmploymentBuilder(this.Session)
                .WithEmployee(employee)
                .WithEmployer(employer)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var before = new Employments(this.Session).Extent().ToArray();

            this.organisationListPage.Table.DefaultAction(employer);
            var organisationOverview = new OrganisationOverviewComponent(this.organisationListPage.Driver);

            var partyRelationshipOverview = organisationOverview.PartyrelationshipOverviewPanel.Click();
            partyRelationshipOverview.Table.DefaultAction(editPartyRelationship);

            var partyRelationshipEdit = new EmploymentEditComponent(organisationOverview.Driver);
            partyRelationshipEdit
                .FromDate.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ThroughDate.Set(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1))
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Employments(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, this.editPartyRelationship.FromDate.Date.ToUniversalTime().Date);
            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1).Date, this.editPartyRelationship.ThroughDate.Value.Date.ToUniversalTime().Date);
            Assert.Equal(employer, editPartyRelationship.Employer);
            Assert.Equal(employee, editPartyRelationship.Employee);
        }
    }
}
