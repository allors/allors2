// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManufacturerIdentificationExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.TestPopulation
{
    public static partial class ManufacturerIdentificationExtensions
    {
        public static ManufacturerIdentificationBuilder WithDefaults(this ManufacturerIdentificationBuilder @this)
        {
            var faker = @this.Session.Faker();
            @this.WithIdentification(faker.Random.AlphaNumeric(9));
            @this.WithProductIdentificationType(new ProductIdentificationTypes(@this.Session).Manufacturer);
            return @this;
        }
    }
}
