// <copyright file="OrganisationBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.TestPopulation
{
    using Allors.Meta;
    using Bogus;

    public static partial class OrganisationBuilderExtensions
    {
        public static OrganisationBuilder WithDefaults(this OrganisationBuilder @this)
        {
            var faker = @this.Session.Faker();

            var euCountry = new Countries(@this.Session).FindBy(M.Country.IsoCode, faker.PickRandom(Countries.EuMemberStates));

            @this.WithName(faker.Company.CompanyName());
            @this.WithEuListingState(euCountry);
            @this.WithLegalForm(faker.Random.ListItem(@this.Session.Extent<LegalForm>()));
            @this.WithLocale(faker.Random.ListItem(@this.Session.GetSingleton().Locales));
            @this.WithTaxNumber($"{euCountry.IsoCode}{faker.Random.Number(99999999)}");
            @this.WithComment(faker.Lorem.Paragraph());

            @this.WithPartyContactMechanism(new PartyContactMechanismBuilder(@this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(new PostalAddressBuilder(@this.Session).WithDefaults().Build())
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).GeneralCorrespondence)
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).ShippingAddress)
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).HeadQuarters)
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).OrderAddress)
                .Build());

            @this.WithPartyContactMechanism(new PartyContactMechanismBuilder(@this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(new EmailAddressBuilder(@this.Session).WithDefaults().Build())
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).GeneralEmail)
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).BillingAddress)
                .Build());

            @this.WithPartyContactMechanism(new PartyContactMechanismBuilder(@this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(new WebAddressBuilder(@this.Session).WithDefaults().Build())
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).InternetAddress)
                .Build());

            @this.WithPartyContactMechanism(new PartyContactMechanismBuilder(@this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(new TelecommunicationsNumberBuilder(@this.Session).WithDefaults().Build())
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).GeneralPhoneNumber)
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).BillingInquiriesPhone)
                .Build());

            @this.WithPartyContactMechanism(new PartyContactMechanismBuilder(@this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(new TelecommunicationsNumberBuilder(@this.Session).WithDefaults().Build())
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).OrderInquiriesPhone)
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).ShippingInquiriesPhone)
                .Build());

            return @this;
        }

        public static OrganisationBuilder WithManufacturerDefaults(this OrganisationBuilder @this, Faker faker)
        {
            var company = faker.Company;

            @this.WithName(company.CompanyName());
            @this.WithIsManufacturer(true);

            return @this;
        }

        public static OrganisationBuilder WithInternalOrganisationDefaults(this OrganisationBuilder @this)
        {
            var faker = @this.Session.Faker();

            var company = faker.Company;
            var euCountry = new Countries(@this.Session).FindBy(M.Country.IsoCode, faker.PickRandom(Countries.EuMemberStates));

            @this.WithName(company.CompanyName());
            @this.WithEuListingState(euCountry);
            @this.WithLegalForm(faker.Random.ListItem(@this.Session.Extent<LegalForm>()));
            @this.WithLocale(faker.Random.ListItem(@this.Session.GetSingleton().Locales));
            @this.WithTaxNumber($"{euCountry.IsoCode}{faker.Random.Number(99999999)}");
            @this.WithComment(faker.Lorem.Paragraph());
            @this.WithIsInternalOrganisation(true);

            return @this;
        }
    }
}
