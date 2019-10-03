// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostalAddressBuilderExtensions.cs" company="Allors bvba">
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

namespace Allors.Domain.TestPopulation
{
    public static partial class PostalAddressBuilderExtensions
    {
        public static PostalAddressBuilder WithDefaults(this PostalAddressBuilder @this)
        {
            var faker = @this.Session.Faker();

            @this.WithAddress1(faker.Address.StreetAddress());
            @this.WithAddress2(faker.Address.SecondaryAddress());
            @this.WithAddress3(faker.Address.BuildingNumber());
            @this.WithPostalCode(faker.Address.ZipCode());
            @this.WithLocality(faker.Address.City());
            @this.WithCountry(new Countries(@this.Session).FindBy(M.Country.IsoCode, faker.Address.CountryCode()));
            @this.WithLatitude(faker.Address.Latitude());
            @this.WithLongitude(faker.Address.Longitude());

            return @this;
        }
    }
}
