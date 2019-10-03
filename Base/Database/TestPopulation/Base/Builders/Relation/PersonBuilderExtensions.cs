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

namespace Allors.Domain.TestPopulation
{
    public static partial class PersonBuilderExtensions
    {
        //public PersonBuilder WithDefaults(ISession session, Config config)
        //{
        //    if (config.End2End)
        //    {

        //        var person = faker.Person;

        //        this.WithName(company.CompanyName());
        //        this.WithLocale(@this.Session.GetSingleton().DefaultLocale);
        //        this.WithVatRegime(new VatRegimes(@this.Session).IntraCommunautair);

        //        this.WithAgreement(new SalesAgreementBuilder(@this.Session)
        //            .WithDescription("PaymentNetDays")
        //            .WithAgreementTerm(new InvoiceTermBuilder(@this.Session)
        //            .WithTermType(new InvoiceTermTypes(@this.Session).PaymentNetDays).WithTermValue("30").Build())
        //            .Build());

        //        this.WithPartyContactMechanism(new PartyContactMechanismBuilder(@this.Session)
        //            .WithUseAsDefault(true)
        //            .WithContactMechanism(new PostalAddressBuilder(@this.Session).WithDefaults(@this.Session, config).Build())
        //            .WithContactPurpose(new ContactMechanismPurposes(@this.Session).GeneralCorrespondence)
        //            .WithContactPurpose(new ContactMechanismPurposes(@this.Session).ShippingAddress)
        //            .WithContactPurpose(new ContactMechanismPurposes(@this.Session).HeadQuarters)
        //            .Build());

        //        this.WithPartyContactMechanism(new PartyContactMechanismBuilder(@this.Session)
        //            .WithUseAsDefault(true)
        //            .WithContactMechanism(new EmailAddressBuilder(@this.Session).WithDefaults(@this.Session, config).Build())
        //            .WithContactPurpose(new ContactMechanismPurposes(@this.Session).GeneralEmail)
        //            .WithContactPurpose(new ContactMechanismPurposes(@this.Session).BillingAddress)
        //            .Build());

        //        this.WithPartyContactMechanism(new PartyContactMechanismBuilder(@this.Session)
        //            .WithUseAsDefault(true)
        //            .WithContactMechanism(new WebAddressBuilder(@this.Session).WithDefaults(@this.Session, config).Build())
        //            .WithContactPurpose(new ContactMechanismPurposes(@this.Session).InternetAddress)
        //            .Build());

        //        this.WithPartyContactMechanism(new PartyContactMechanismBuilder(@this.Session)
        //            .WithUseAsDefault(true)
        //            .WithContactMechanism(new TelecommunicationsNumberBuilder(@this.Session).WithDefaults(@this.Session, config).Build())
        //            .WithContactPurpose(new ContactMechanismPurposes(@this.Session).GeneralPhoneNumber)
        //            .WithContactPurpose(new ContactMechanismPurposes(@this.Session).FinanceAdministration)
        //            .Build());

        //        this.WithPartyContactMechanism(new PartyContactMechanismBuilder(@this.Session)
        //            .WithUseAsDefault(true)
        //            .WithContactMechanism(new TelecommunicationsNumberBuilder(@this.Session).WithDefaults(@this.Session, config).Build())
        //            .WithContactPurpose(new ContactMechanismPurposes(@this.Session).OrderInquiriesPhone)
        //            .WithContactPurpose(new ContactMechanismPurposes(@this.Session).ShippingInquiriesPhone)
        //            .Build());
        //    }

        //    return this;
        //}

        public static PersonBuilder WithEmployeeOrCompanyContactDefaults(this PersonBuilder @this)
        {
            var faker = @this.Session.Faker();

            var person = faker.Person;

            @this.WithFirstName(person.FirstName);
            @this.WithLastName(person.LastName);
            @this.WithSalutation(faker.Random.ListItem(@this.Session.Extent<Salutation>()));
            @this.WithGender(faker.Random.ListItem(@this.Session.Extent<GenderType>()));
            @this.WithTitle(faker.Random.ListItem(@this.Session.Extent<PersonalTitle>()));
            @this.WithLocale(@this.Session.GetSingleton().DefaultLocale);
            @this.WithPicture(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.PicsumUrl()).Build());
            @this.WithComment(faker.Lorem.Paragraph());

            foreach (Locale additionalLocale in @this.Session.GetSingleton().AdditionalLocales)
            {
                @this.WithLocalisedComment(new LocalisedTextBuilder(@this.Session).WithLocale(additionalLocale).WithText(faker.Lorem.Paragraph()).Build());
            }

            @this.WithPartyContactMechanism(new PartyContactMechanismBuilder(@this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(new EmailAddressBuilder(@this.Session).WithDefaults().Build())
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).PersonalEmailAddress)
                .Build());

            @this.WithPartyContactMechanism(new PartyContactMechanismBuilder(@this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(new TelecommunicationsNumberBuilder(@this.Session).WithDefaults().Build())
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).GeneralPhoneNumber)
                .Build());

            return @this;
        }
    }
}
