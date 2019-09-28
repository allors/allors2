// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonBuilderExtensions.cs" company="Allors bvba">
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

namespace Allors.Domain.End2End
{
    public static partial class PersonBuilderExtensions
    {
        //public PersonBuilder WithDefaults(ISession session, Config config)
        //{
        //    if (config.End2End)
        //    {

        //        var person = config.faker.Person;

        //        this.WithName(company.CompanyName());
        //        this.WithLocale(session.GetSingleton().DefaultLocale);
        //        this.WithVatRegime(new VatRegimes(session).IntraCommunautair);

        //        this.WithAgreement(new SalesAgreementBuilder(session)
        //            .WithDescription("PaymentNetDays")
        //            .WithAgreementTerm(new InvoiceTermBuilder(session)
        //            .WithTermType(new InvoiceTermTypes(session).PaymentNetDays).WithTermValue("30").Build())
        //            .Build());

        //        this.WithPartyContactMechanism(new PartyContactMechanismBuilder(session)
        //            .WithUseAsDefault(true)
        //            .WithContactMechanism(new PostalAddressBuilder(session).WithDefaults(session, config).Build())
        //            .WithContactPurpose(new ContactMechanismPurposes(session).GeneralCorrespondence)
        //            .WithContactPurpose(new ContactMechanismPurposes(session).ShippingAddress)
        //            .WithContactPurpose(new ContactMechanismPurposes(session).HeadQuarters)
        //            .Build());

        //        this.WithPartyContactMechanism(new PartyContactMechanismBuilder(session)
        //            .WithUseAsDefault(true)
        //            .WithContactMechanism(new EmailAddressBuilder(session).WithDefaults(session, config).Build())
        //            .WithContactPurpose(new ContactMechanismPurposes(session).GeneralEmail)
        //            .WithContactPurpose(new ContactMechanismPurposes(session).BillingAddress)
        //            .Build());

        //        this.WithPartyContactMechanism(new PartyContactMechanismBuilder(session)
        //            .WithUseAsDefault(true)
        //            .WithContactMechanism(new WebAddressBuilder(session).WithDefaults(session, config).Build())
        //            .WithContactPurpose(new ContactMechanismPurposes(session).InternetAddress)
        //            .Build());

        //        this.WithPartyContactMechanism(new PartyContactMechanismBuilder(session)
        //            .WithUseAsDefault(true)
        //            .WithContactMechanism(new TelecommunicationsNumberBuilder(session).WithDefaults(session, config).Build())
        //            .WithContactPurpose(new ContactMechanismPurposes(session).GeneralPhoneNumber)
        //            .WithContactPurpose(new ContactMechanismPurposes(session).FinanceAdministration)
        //            .Build());

        //        this.WithPartyContactMechanism(new PartyContactMechanismBuilder(session)
        //            .WithUseAsDefault(true)
        //            .WithContactMechanism(new TelecommunicationsNumberBuilder(session).WithDefaults(session, config).Build())
        //            .WithContactPurpose(new ContactMechanismPurposes(session).OrderInquiriesPhone)
        //            .WithContactPurpose(new ContactMechanismPurposes(session).ShippingInquiriesPhone)
        //            .Build());
        //    }

        //    return this;
        //}

        public static PersonBuilder WithEmployeeOrCompanyContactDefaults(this PersonBuilder @this, ISession session, Config config)
        {
            if (config.End2End)
            {
                var person = config.faker.Person;

                @this.WithFirstName(person.FirstName);
                @this.WithLastName(person.LastName);
                @this.WithSalutation(config.faker.Random.ListItem(session.Extent<Salutation>()));
                @this.WithGender(config.faker.Random.ListItem(session.Extent<GenderType>()));
                @this.WithTitle(config.faker.Random.ListItem(session.Extent<PersonalTitle>()));
                @this.WithLocale(session.GetSingleton().DefaultLocale);
                @this.WithPicture(new MediaBuilder(session).WithInDataUri(config.faker.Image.PicsumUrl()).Build());
                @this.WithComment(config.faker.Lorem.Paragraph());

                foreach (Locale additionalLocale in session.GetSingleton().AdditionalLocales)
                {
                    @this.WithLocalisedComment(new LocalisedTextBuilder(session).WithLocale(additionalLocale).WithText(config.faker.Lorem.Paragraph()).Build());
                }

                @this.WithPartyContactMechanism(new PartyContactMechanismBuilder(session)
                    .WithUseAsDefault(true)
                    .WithContactMechanism(new EmailAddressBuilder(session).WithDefaults(session, config).Build())
                    .WithContactPurpose(new ContactMechanismPurposes(session).PersonalEmailAddress)
                    .Build());

                @this.WithPartyContactMechanism(new PartyContactMechanismBuilder(session)
                    .WithUseAsDefault(true)
                    .WithContactMechanism(new TelecommunicationsNumberBuilder(session).WithDefaults(session, config).Build())
                    .WithContactPurpose(new ContactMechanismPurposes(session).GeneralPhoneNumber)
                    .Build());
            }

            return @this;
        }
    }
}
