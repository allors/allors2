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
        public static PersonBuilder WithEmployeeOrCompanyContactDefaults(this PersonBuilder @this)
        {
            var faker = @this.Session.Faker();

            var person = faker.Person;
            var emailAddress = new EmailAddressBuilder(@this.Session).WithDefaults().Build();
            var email = emailAddress.ElectronicAddressString;

            @this.WithFirstName(person.FirstName);
            @this.WithLastName(person.LastName);
            @this.WithSalutation(faker.Random.ListItem(@this.Session.Extent<Salutation>()));
            @this.WithGender(faker.Random.ListItem(@this.Session.Extent<GenderType>()));
            @this.WithTitle(faker.Random.ListItem(@this.Session.Extent<PersonalTitle>()));
            @this.WithLocale(@this.Session.GetSingleton().DefaultLocale);
            @this.WithPicture(new MediaBuilder(@this.Session).WithInDataUri(faker.Image.PicsumUrl()).Build());
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

            return @this;
        }
    }
}
