// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SkuIdentificationExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.TestPopulation
{
    public static partial class SkuIdentificationExtensions
    {
        public static SkuIdentificationBuilder WithDefaults(this SkuIdentificationBuilder @this)
        {
            var faker = @this.Session.Faker();
            @this.WithIdentification(faker.Random.AlphaNumeric(7));
            @this.WithProductIdentificationType(new ProductIdentificationTypes(@this.Session).Sku);
            return @this;
        }
    }
}
