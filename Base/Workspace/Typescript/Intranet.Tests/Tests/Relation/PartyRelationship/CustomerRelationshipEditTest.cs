// <copyright file="CustomerRelationshipEditTest.cs" company="Allors bvba">
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
    using src.allors.material.@base.objects.customerrelationship.edit;
    using src.allors.material.@base.objects.person.list;
    using src.allors.material.@base.objects.person.overview;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
    public class CustomerRelationshipEditTest : Test
    {
        private readonly PersonListComponent personListPage;

        public CustomerRelationshipEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.personListPage = this.Sidenav.NavigateToPeople();
        }

        [Fact]
        public void Create()
        {
            var before = new PartyRelationships(this.Session).Extent().ToArray();

            var person = new People(this.Session).Extent().First;

            this.personListPage.Table.DefaultAction(person);
            var customerRelationshipEdit = new PersonOverviewComponent(this.personListPage.Driver).PartyrelationshipOverviewPanel.Click().CreateCustomerRelationship();

            customerRelationshipEdit
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
            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");

            var person = new People(this.Session).Extent().First;

            var editPartyRelationship = new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(person)
                .WithInternalOrganisation(allors)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var before = new PartyRelationships(this.Session).Extent().ToArray();

            this.personListPage.Table.DefaultAction(person);
            var personOverview = new PersonOverviewComponent(this.personListPage.Driver);

            var partyRelationshipOverview = personOverview.PartyrelationshipOverviewPanel.Click();
            partyRelationshipOverview.Table.DefaultAction(editPartyRelationship);

            var customerRelationshipEditComponent = new CustomerRelationshipEditComponent(this.Driver);
            customerRelationshipEditComponent.FromDate.Set(DateTimeFactory.CreateDate(2018, 12, 22))
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
