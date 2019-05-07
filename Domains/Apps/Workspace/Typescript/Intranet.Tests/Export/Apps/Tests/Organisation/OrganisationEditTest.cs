using src.allors.material.apps.objects.organisation.create;
using src.allors.material.apps.objects.organisation.list;

namespace Tests.OrganisationTests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Components;
    using Xunit;

    [Collection("Test collection")]
    public class OrganisationEditTest : Test
    {
        private readonly OrganisationListComponent organisationListPage;

        public OrganisationEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.organisationListPage = this.Sidenav.NavigateToOrganisations();
        }

        [Fact]
        public void Create()
        {
            var customOrganisationClassification = new CustomOrganisationClassificationBuilder(this.Session).WithName("Gold").Build();
            var industryClassification = new IndustryClassificationBuilder(this.Session).WithName("Retail").Build();
            var legalForm = new LegalForms(this.Session).FindBy(M.LegalForm.Description, "BE - BVBA / SPRL");

            this.Session.Derive();
            this.Session.Commit();

            this.organisationListPage.AddNew.Click();
            var before = new Organisations(this.Session).Extent().ToArray();

            var page = new OrganisationCreateComponent(this.Driver);

            page.Name.Set("new organisation")
                .TaxNumber.Set("BE 123 456 789 01")
                .LegalForm.Set(legalForm.Description)
                .Locale.Set(this.Session.GetSingleton().AdditionalLocales.First.Name)
                .IndustryClassifications.Toggle(industryClassification.Name)
                .CustomClassifications.Toggle(customOrganisationClassification.Name)
                .IsManufacturer.Set(true)
                .IsInternalOrganisation.Set(true)
                .Comment.Set("comment")
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Organisations(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var organisation = after.Except(before).First();

            Assert.Equal("new organisation", organisation.Name);
            Assert.Equal("BE 123 456 789 01", organisation.TaxNumber);
            Assert.Equal(legalForm, organisation.LegalForm);
            Assert.Equal(Session.GetSingleton().AdditionalLocales.First, organisation.Locale);
            Assert.Contains(industryClassification, organisation.IndustryClassifications);
            Assert.Contains(customOrganisationClassification, organisation.CustomClassifications);
            Assert.True(organisation.IsManufacturer);
            Assert.True(organisation.IsInternalOrganisation);
            Assert.Equal("comment", organisation.Comment);
        }

        [Fact]
        public void Edit()
        {
            var customOrganisationClassification = new CustomOrganisationClassificationBuilder(this.Session).WithName("Gold").Build();
            var industryClassification = new IndustryClassificationBuilder(this.Session).WithName("Retail").Build();
            var legalForm = new LegalForms(this.Session).FindBy(M.LegalForm.Description, "BE - BVBA / SPRL");

            this.Session.Derive();
            this.Session.Commit();

            var before = new Organisations(this.Session).Extent().ToArray();

            var organisation = before.First(v => v.PartyName.Equals("Acme0"));
            var id = organisation.Id;

            var organisationOverviewPage = this.organisationListPage.Select(organisation);
            var page = organisationOverviewPage.Edit();

            page.Name.Set("new organisation")
                .TaxNumber.Set("BE 123 456 789 01")
                .LegalForm.Set(legalForm.Description)
                .Locale.Set(this.Session.GetSingleton().AdditionalLocales.First.Name)
                .IndustryClassifications.Toggle(industryClassification.Name)
                .CustomClassifications.Toggle(customOrganisationClassification.Name)
                .IsManufacturer.Set(true)
                .IsInternalOrganisation.Set(true)
                .Comment.Set("comment")
                .SAVE.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Organisations(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            organisation = after.First(v => v.Id.Equals(id));

            Assert.Equal("new organisation", organisation.Name);
            Assert.Equal("BE 123 456 789 01", organisation.TaxNumber);
            Assert.Equal(legalForm, organisation.LegalForm);
            Assert.Equal(this.Session.GetSingleton().AdditionalLocales.First, organisation.Locale);
            Assert.Contains(industryClassification, organisation.IndustryClassifications);
            Assert.Contains(customOrganisationClassification, organisation.CustomClassifications);
            Assert.True(organisation.IsManufacturer);
            Assert.True(organisation.IsInternalOrganisation);
            Assert.Equal("comment", organisation.Comment);
        }
    }
}
