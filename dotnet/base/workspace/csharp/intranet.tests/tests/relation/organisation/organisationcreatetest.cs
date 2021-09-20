// <copyright file="OrganisationCreateTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.OrganisationTests
{
    using System.Linq;
    using Allors;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Components;
    using src.allors.material.@base.objects.organisation.create;
    using src.allors.material.@base.objects.organisation.list;
    using Xunit;

    [Collection("Test collection")]
    [Trait("Category", "Relation")]
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

            var expected = new OrganisationBuilder(this.Session).WithDefaults().Build();

            this.Session.Derive();

            var expectedName = expected.Name;
            var expectedTaxNumber = expected.TaxNumber;
            var expectedLegalFormDescription = expected.LegalForm.Description;
            var expectedLocaleName = expected.Locale.Name;
            var expectedIsManufacturer = expected.IsManufacturer;
            var expectedComment = expected.Comment;

            var organisationCreate = this.organisationListPage
                .CreateOrganisation()
                .Build(expected);

            organisationCreate.AssertFull(expected);

            this.Session.Rollback();
            organisationCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Organisations(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedName, actual.Name);
            Assert.Equal(expectedTaxNumber, actual.TaxNumber);
            Assert.Equal(expectedLegalFormDescription, actual.LegalForm.Description);
            Assert.Equal(expectedLocaleName, actual.Locale.Name);
            Assert.Equal(expectedIsManufacturer, actual.IsManufacturer);
            Assert.Equal(expectedComment, actual.Comment);
        }

        [Fact]
        public void CreateMinimal()
        {
            var before = new Organisations(this.Session).Extent().ToArray();

            var expected = new OrganisationBuilder(this.Session).WithDefaults().Build();

            this.Session.Derive();

            var expectedName = expected.Name;

            var organisationCreate = this.organisationListPage
                .CreateOrganisation()
                .Build(expected, true);

            this.Session.Rollback();
            organisationCreate.SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Organisations(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var actual = after.Except(before).First();

            Assert.Equal(expectedName, actual.Name);
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
