namespace Tests.OrganisationTests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Angular;

    using Pages.OrganisationTests;

    using Xunit;

    [Collection("Test collection")]
    public class OrganisationEditTest : Test
    {
        private readonly OrganisationListPage organisations;

        private readonly IndustryClassification industryClassification;

        private readonly CustomOrganisationClassification customOrganisationClassification;

        private readonly LegalForm legalForm;

        public OrganisationEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.customOrganisationClassification = new CustomOrganisationClassificationBuilder(this.Session).WithName("Gold").Build();
            this.industryClassification = new IndustryClassificationBuilder(this.Session).WithName("Retail").Build();
            this.legalForm = new LegalForms(this.Session).FindBy(M.LegalForm.Description, "BE - BVBA / SPRL");

            this.Session.Derive();
            this.Session.Commit();

            var dashboard = this.Login();
            this.organisations = dashboard.Sidenav.NavigateToOrganisationList();
        }

        [Fact]
        public void Create()
        {
            this.organisations.AddNew.Click();
            var before = new Organisations(this.Session).Extent().ToArray();

            var page = new OrganisationEditPage(this.Driver);

            page.Name.Value = "new organisation";
            page.TaxNumber.Value = "BE 123 456 789 01";
            page.LegalForm.Value = this.legalForm.Description;
            page.Locale.Value = this.Session.GetSingleton().AdditionalLocales.First.Name;
            page.IndustryClassifications.Toggle(this.industryClassification.Name);
            page.CustomClassifications.Toggle(this.customOrganisationClassification.Name);
            page.IsManufacturer.Value = true;
            page.IsInternalOrganisation.Value = true;
            page.Comment.Value = "comment";

            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Organisations(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var organisation = after.Except(before).First();

            Assert.Equal("new organisation", organisation.Name);
            Assert.Equal("BE 123 456 789 01", organisation.TaxNumber);
            Assert.Equal(this.legalForm, organisation.LegalForm);
            Assert.Equal(this.Session.GetSingleton().AdditionalLocales.First, organisation.Locale);
            Assert.Contains(this.industryClassification, organisation.IndustryClassifications);
            Assert.Contains(this.customOrganisationClassification, organisation.CustomClassifications);
            Assert.True(organisation.IsManufacturer);
            Assert.True(organisation.IsInternalOrganisation);
            Assert.Equal("comment", organisation.Comment);
        }

        [Fact]
        public void Edit()
        {
            var before = new Organisations(this.Session).Extent().ToArray();

            var organisation = before.First(v => v.PartyName.Equals("Acme0"));
            var id = organisation.Id;

            var organisationOverviewPage = this.organisations.Select(organisation);
            var page = organisationOverviewPage.Edit();

            page.Name.Value = "new organisation";
            page.TaxNumber.Value = "BE 123 456 789 01";
            page.LegalForm.Value = this.legalForm.Description;
            page.Locale.Value = this.Session.GetSingleton().AdditionalLocales.First.Name;
            page.IndustryClassifications.Toggle(this.industryClassification.Name);
            page.CustomClassifications.Toggle(this.customOrganisationClassification.Name);
            page.IsManufacturer.Value = true;
            page.IsInternalOrganisation.Value = true;
            page.Comment.Value = "comment";

            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Organisations(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            organisation = after.First(v => v.Id.Equals(id));

            Assert.Equal("new organisation", organisation.Name);
            Assert.Equal("BE 123 456 789 01", organisation.TaxNumber);
            Assert.Equal(this.legalForm, organisation.LegalForm);
            Assert.Equal(this.Session.GetSingleton().AdditionalLocales.First, organisation.Locale);
            Assert.Contains(this.industryClassification, organisation.IndustryClassifications);
            Assert.Contains(this.customOrganisationClassification, organisation.CustomClassifications);
            Assert.True(organisation.IsManufacturer);
            Assert.True(organisation.IsInternalOrganisation);
            Assert.Equal("comment", organisation.Comment);
        }
    }
}
