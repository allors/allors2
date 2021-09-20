// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.TestPopulation
{
    public static partial class PersonBuilderExtensions
    {
        public static PersonBuilder WithDefaults(this PersonBuilder @this)
        {
            var faker = @this.Session.Faker();

            var emailAddress = new EmailAddressBuilder(@this.Session).WithDefaults().Build();
            var email = emailAddress.ElectronicAddressString;

            @this.WithFirstName(faker.Name.FirstName());
            @this.WithLastName(faker.Name.LastName());
            @this.WithSalutation(faker.Random.ListItem(@this.Session.Extent<Salutation>()));
            @this.WithGender(faker.Random.ListItem(@this.Session.Extent<GenderType>()));
            @this.WithTitle(faker.Random.ListItem(@this.Session.Extent<PersonalTitle>()));
            @this.WithLocale(@this.Session.GetSingleton().DefaultLocale);
            @this.WithPicture(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.DataUri(200, 200)).Build());
            @this.WithComment(faker.Lorem.Paragraph());
            @this.WithUserName(email);

            foreach (Locale additionalLocale in @this.Session.GetSingleton().AdditionalLocales)
            {
                @this.WithLocalisedComment(new LocalisedTextBuilder(@this.Session).WithLocale(additionalLocale).WithText(faker.Lorem.Paragraph()).Build());
            }

            @this.WithPartyContactMechanism(new PartyContactMechanismBuilder(@this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(emailAddress)
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).PersonalEmailAddress)
                .Build());

            @this.WithPartyContactMechanism(new PartyContactMechanismBuilder(@this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(new TelecommunicationsNumberBuilder(@this.Session).WithDefaults().Build())
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).GeneralPhoneNumber)
                .Build());

            @this.WithPartyContactMechanism(new PartyContactMechanismBuilder(@this.Session)
                .WithUseAsDefault(true)
                .WithContactMechanism(new PostalAddressBuilder(@this.Session).WithDefaults().Build())
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).GeneralCorrespondence)
                .WithContactPurpose(new ContactMechanismPurposes(@this.Session).ShippingAddress)
                .Build());

            return @this;
        }
    }
}
