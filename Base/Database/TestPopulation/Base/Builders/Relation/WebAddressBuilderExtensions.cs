// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebAddressBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.TestPopulation
{
    public static partial class WebAddressBuilderExtensions
    {
        public static WebAddressBuilder WithDefaults(this WebAddressBuilder @this)
        {
            var faker = @this.Session.Faker();

            @this.WithElectronicAddressString(faker.Internet.Url());
            @this.WithDescription(faker.Lorem.Sentence());

            return @this;
        }
    }
}
