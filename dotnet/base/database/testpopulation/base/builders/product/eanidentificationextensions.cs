// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EanIdentificationExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.TestPopulation
{
    public static partial class EanIdentificationExtensions
    {
        public static EanIdentificationBuilder WithDefaults(this EanIdentificationBuilder @this)
        {
            var faker = @this.Session.Faker();
            @this.WithIdentification(faker.Commerce.Ean13());
            @this.WithProductIdentificationType(new ProductIdentificationTypes(@this.Session).Ean);
            return @this;
        }
    }
}
