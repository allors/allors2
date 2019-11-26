// <copyright file="OrganisationEditTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Domain.TestPopulation;
using src.allors.material.@base.objects.organisation.create;

namespace Tests.OrganisationTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Meta;
    using Components;
    using src.allors.material.@base.objects.organisation.list;
    using src.allors.material.@base.objects.organisation.overview;
    using Xunit;

    [Collection("Test collection")]
    public class OrganisationCreateTest : Test
    {
        private readonly OrganisationListComponent organisationListPage;

        public OrganisationCreateTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.organisationListPage = this.Sidenav.NavigateToOrganisations();
        }

        [Fact]
        public void CreateFull()
        {
            var before = new Organisations(this.Session).Extent().ToArray();

            var expected = new OrganisationBuilder(this.MemorySession).WithDefaults().Build();

            var organisationCreate = this.organisationListPage
                .CreateOrganisation()
                .Build(expected);

            organisationCreate.AssertFull(expected);

            organisationCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Organisations(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.TaxNumber, actual.TaxNumber);
            Assert.Equal(expected.LegalForm.Description, actual.LegalForm.Description);
            Assert.Equal(expected.Locale.Name, actual.Locale.Name);
            Assert.Equal(expected.IsManufacturer, actual.IsManufacturer);
            Assert.Equal(expected.Comment, actual.Comment);
        }

        [Fact]
        public void CreateMinimal()
        {
            var before = new Organisations(this.Session).Extent().ToArray();

            var expected = new OrganisationBuilder(this.MemorySession).WithDefaults().Build();

            var organisationCreate = this.organisationListPage
                .CreateOrganisation()
                .Build(expected, true);

            organisationCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Organisations(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expected.Name, actual.Name);
            Assert.False(actual.ExistTaxNumber);
            Assert.False(actual.ExistLegalForm);
            Assert.False(actual.ExistLocale);
            Assert.False(actual.ExistIndustryClassifications);
            Assert.False(actual.ExistIndustryClassifications);
            Assert.False(actual.IsManufacturer);
            Assert.False(actual.ExistComment);
        }
    }
}
