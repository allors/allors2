// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostalAddressBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.TestPopulation
{
    using Allors.Meta;

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
