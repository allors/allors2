// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationBuilderExtensions.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.TestPopulation
{
    using Allors.Meta;
    using Bogus;

    public static partial class OrganisationBuilderExtensions
    {
        public static OrganisationBuilder WithDefaults(this OrganisationBuilder @this)
        {
            var faker = @this.Session.Faker();

            var company = faker.Company;
            var euCountry = new Countries(@this.Session).FindBy(M.Country.IsoCode, faker.PickRandom(Countries.EuMemberStates));

            @this.WithName(company.CompanyName());
            @this.WithEuListingState(euCountry);
            @this.WithVatRegime(new VatRegimes(@this.Session).IntraCommunautair);
            @this.WithLegalForm(faker.Random.ListItem(@this.Session.Extent<LegalForm>()));
            @this.WithTaxNumber($"{euCountry.IsoCode}{faker.Random.Number(99999999)}");

            @this.WithAgreement(new SalesAgreementBuilder(@this.Session)
                .WithDescription("PaymentNetDays")
                .WithAgreementTerm(new InvoiceTermBuilder(@this.Session)
                .WithTermType(new InvoiceTermTypes(@this.Session).PaymentNetDays).WithTermValue("30").Build())
                .Build());

            @this.WithPartyContactMechanism(new PartyContactMechanismBuilder(@this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(new PostalAddressBuilder(@this.Session).WithDefaults().Build())
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).GeneralCorrespondence)
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).ShippingAddress)
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).HeadQuarters)
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
    }
}
