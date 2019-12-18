// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TelecommunicationsNumberBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.TestPopulation
{
    public static partial class TelecommunicationsNumberBuilderExtensions
    {
        public static TelecommunicationsNumberBuilder WithDefaults(this TelecommunicationsNumberBuilder @this)
        {
            var faker = @this.Session.Faker();

            @this.WithCountryCode(faker.Phone.PhoneNumber("####"));
            @this.WithContactNumber(faker.Phone.PhoneNumber("## ## ##"));
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithContactMechanismType(faker.Random.ListItem(@this.Session.Extent<ContactMechanismType>()));

            return @this;
        }
    }
}
