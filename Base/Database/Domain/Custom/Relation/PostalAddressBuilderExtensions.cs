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

namespace Allors.Domain.End2End
{
    public static partial class PostalAddressBuilderExtensions
    {
        public static PostalAddressBuilder WithDefaults(this PostalAddressBuilder @this, ISession session, Config config)
        {
            if (config.End2End)
            {
                @this.WithAddress1(config.faker.Address.StreetAddress());
                @this.WithAddress2(config.faker.Address.SecondaryAddress());
                @this.WithAddress3(config.faker.Address.BuildingNumber());
                @this.WithPostalCode(config.faker.Address.ZipCode());
                @this.WithLocality(config.faker.Address.City());
                @this.WithCountry(new Countries(session).FindBy(M.Country.IsoCode, config.faker.Address.CountryCode()));
                @this.WithLatitude(config.faker.Address.Latitude());
                @this.WithLongitude(config.faker.Address.Longitude());
            }

            return @this;
        }
    }
}
