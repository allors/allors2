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

using Allors.Meta;

namespace Allors.Domain.End2End
{
    public static partial class OrganisationBuilderExtensions
    {
        public static OrganisationBuilder WithDefaults(this OrganisationBuilder @this, ISession session, Config config)
        {
            if (config.End2End)
            {
                var company = config.faker.Company;
                var euCountry = new Countries(session).FindBy(M.Country.IsoCode, config.faker.PickRandom(Countries.EuMemberStates));

                @this.WithName(company.CompanyName());
                @this.WithEuListingState(euCountry);
                @this.WithVatRegime(new VatRegimes(session).IntraCommunautair);
                @this.WithLegalForm(config.faker.Random.ArrayElement(session.Extent<LegalForm>().ToArray()));
                @this.WithTaxNumber($"{euCountry.IsoCode}{config.faker.Random.Number(99999999)}");

                @this.WithAgreement(new SalesAgreementBuilder(session)
                    .WithDescription("PaymentNetDays")
                    .WithAgreementTerm(new InvoiceTermBuilder(session)
                    .WithTermType(new InvoiceTermTypes(session).PaymentNetDays).WithTermValue("30").Build())
                    .Build());

                @this.WithPartyContactMechanism(new PartyContactMechanismBuilder(session)
                    .WithUseAsDefault(true)
                    .WithContactMechanism(new PostalAddressBuilder(session).WithDefaults(session, config).Build())
                    .WithContactPurpose(new ContactMechanismPurposes(session).GeneralCorrespondence)
                    .WithContactPurpose(new ContactMechanismPurposes(session).ShippingAddress)
                    .WithContactPurpose(new ContactMechanismPurposes(session).HeadQuarters)
                    .Build());

                @this.WithPartyContactMechanism(new PartyContactMechanismBuilder(session)
                    .WithUseAsDefault(true)
                    .WithContactMechanism(new EmailAddressBuilder(session).WithDefaults(session, config).Build())
                    .WithContactPurpose(new ContactMechanismPurposes(session).GeneralEmail)
                    .WithContactPurpose(new ContactMechanismPurposes(session).BillingAddress)
                    .Build());

                @this.WithPartyContactMechanism(new PartyContactMechanismBuilder(session)
                    .WithUseAsDefault(true)
                    .WithContactMechanism(new WebAddressBuilder(session).WithDefaults(session, config).Build())
                    .WithContactPurpose(new ContactMechanismPurposes(session).InternetAddress)
                    .Build());

                @this.WithPartyContactMechanism(new PartyContactMechanismBuilder(session)
                    .WithUseAsDefault(true)
                    .WithContactMechanism(new TelecommunicationsNumberBuilder(session).WithDefaults(session, config).Build())
                    .WithContactPurpose(new ContactMechanismPurposes(session).GeneralPhoneNumber)
                    .WithContactPurpose(new ContactMechanismPurposes(session).BillingInquiriesPhone)
                    .Build());

                @this.WithPartyContactMechanism(new PartyContactMechanismBuilder(session)
                    .WithUseAsDefault(true)
                    .WithContactMechanism(new TelecommunicationsNumberBuilder(session).WithDefaults(session, config).Build())
                    .WithContactPurpose(new ContactMechanismPurposes(session).OrderInquiriesPhone)
                    .WithContactPurpose(new ContactMechanismPurposes(session).ShippingInquiriesPhone)
                    .Build());
            }

            return @this;
        }
    }
}
