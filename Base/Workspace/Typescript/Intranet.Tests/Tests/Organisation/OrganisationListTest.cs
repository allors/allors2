// <copyright file="OrganisationListTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using src.allors.material.@base.objects.organisation.list;

namespace Tests.OrganisationTests
{
    using Allors.Domain;
    using Allors.Meta;
    using Xunit;

    [Collection("Test collection")]
    public class OrganisationListTest : Test
    {
        private readonly OrganisationListComponent page;

        public OrganisationListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.page = this.Sidenav.NavigateToOrganisations();
        }

        [Fact]
        public void Title() => Assert.Equal("Organisations", this.Driver.Title);

        [Fact]
        public void Table()
        {
            var organisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Acme");
            var row = this.page.Table.FindRow(organisation);
            var cell = row.FindCell("name");

            Assert.Equal("Acme", cell.Element.Text);
        }
    }
}
