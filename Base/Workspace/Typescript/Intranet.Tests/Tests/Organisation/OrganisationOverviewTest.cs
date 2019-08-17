// <copyright file="OrganisationOverviewTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using src.allors.material.@base.objects.organisation.list;
using src.allors.material.@base.objects.organisation.overview;

namespace Tests.OrganisationTests
{
    using Allors.Domain;
    using Allors.Meta;
    using Xunit;

    [Collection("Test collection")]
    public class OrganisationOverviewTest : Test
    {
        private readonly OrganisationListComponent organisationListPage;

        public OrganisationOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.organisationListPage = this.Sidenav.NavigateToOrganisations();
        }

        [Fact]
        public void Title()
        {
            var organisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Acme");

            this.organisationListPage.Table.DefaultAction(organisation);
            new OrganisationOverviewComponent(this.organisationListPage.Driver);

            Assert.Equal("Organisation", this.Driver.Title);
        }

        [Fact]
        public void NavigateToList()
        {
            var organisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Acme");
            this.organisationListPage.Table.DefaultAction(organisation);
            var organisationOverview = new OrganisationOverviewComponent(this.organisationListPage.Driver);

            organisationOverview.Organisations.Click();

            Assert.Equal("Organisations", this.Driver.Title);
        }
    }
}
