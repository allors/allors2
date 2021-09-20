// <copyright file="PersonEmploymentEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.PartyRelationshipTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Meta;
    using Components;
    using libs.angular.material.@base.src.export.objects.employment.edit;
    using libs.angular.material.@base.src.export.objects.person.list;
    using libs.angular.material.@base.src.export.objects.person.overview;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class PersonEmploymentEditTest : Test
    {
        private readonly PersonListComponent people;

        private readonly Employment editPartyRelationship;

        private readonly Person employee;

        public PersonEmploymentEditTest(TestFixture fixture)
            : base(fixture)
        {
            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            this.employee = new PersonBuilder(this.Session).WithLastName("employee").Build();

            // Delete all existing for the new one to be in the first page of the list.
            foreach (PartyRelationship partyRelationship in allors.PartyRelationshipsWhereParty)
            {
                partyRelationship.Delete();
            }

            this.Session.Derive();
            this.Session.Commit();

            this.editPartyRelationship = new EmploymentBuilder(this.Session)
                .WithEmployee(this.employee)
                .WithEmployer(allors)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            this.Login();
            this.people = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Create()
        {
            var before = new Employments(this.Session).Extent().ToArray();

            var employer = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");

            this.people.Table.DefaultAction(this.employee);
            var employmentEditComponent = new PersonOverviewComponent(this.people.Driver).PartyrelationshipOverviewPanel.Click().CreateEmployment();

            employmentEditComponent.FromDate
                .Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ThroughDate.Set(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1))
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Employments(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var partyRelationship = after.Except(before).First();

            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, partyRelationship.FromDate.Date.ToUniversalTime().Date);
            // Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1).Date, partyRelationship.ThroughDate.Value.Date.ToUniversalTime().Date);
            Assert.Equal(employer, partyRelationship.Employer);
            Assert.Equal(this.employee, partyRelationship.Employee);
        }

        [Fact]
        public void Edit()
        {
            var before = new Employments(this.Session).Extent().ToArray();

            var employer = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");

            this.people.Table.DefaultAction(this.employee);
            var personOverviewPage = new PersonOverviewComponent(this.people.Driver);

            var partyRelationshipOverview = personOverviewPage.PartyrelationshipOverviewPanel.Click();
            partyRelationshipOverview.Table.DefaultAction(this.editPartyRelationship);

            var employmentEditComponent = new EmploymentEditComponent(this.Driver);
            employmentEditComponent.FromDate.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ThroughDate.Set(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1))
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Employments(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, this.editPartyRelationship.FromDate.Date.ToUniversalTime().Date);
            Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1).Date, this.editPartyRelationship.ThroughDate.Value.Date.ToUniversalTime().Date);
            Assert.Equal(employer, this.editPartyRelationship.Employer);
            Assert.Equal(this.employee, this.editPartyRelationship.Employee);
        }
    }
}
