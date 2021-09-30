namespace libs.angular.material.@base.src.export.objects.organisation.create
{
    using Allors.Domain;
    using Xunit;

    public static partial class OrganisationCreateComponentExtensions
    {
        public static OrganisationCreateComponent Build(this OrganisationCreateComponent @this, Organisation organisation, bool minimal = false)
        {
            @this.Name.Set(organisation.Name);

            if (!minimal)
            {
                @this.TaxNumber.Set(organisation.TaxNumber);
                @this.LegalForm.Select(organisation.LegalForm);
                @this.Locale.Select(organisation.Locale);

                foreach (IndustryClassification industryClassification in organisation.IndustryClassifications)
                {
                    @this.IndustryClassifications.Toggle(industryClassification);
                }

                foreach (CustomOrganisationClassification customOrganisationClassification in organisation.CustomClassifications)
                {
                    @this.CustomClassifications.Toggle(customOrganisationClassification);
                }

                @this.IsManufacturer.Set(organisation.IsManufacturer);
                @this.IsInternalOrganisation.Set(organisation.IsInternalOrganisation);
                @this.Comment.Set(organisation.Comment);
            }

            return @this;
        }

        public static void AssertFull(this OrganisationCreateComponent @this, Organisation organisation)
        {
            Assert.True(organisation.ExistName);
            Assert.True(organisation.ExistTaxNumber);
            Assert.True(organisation.ExistLegalForm);
            Assert.True(organisation.ExistLocale);
            Assert.True(organisation.ExistIsInternalOrganisation);
            Assert.True(organisation.ExistComment);
        }
    }
}
