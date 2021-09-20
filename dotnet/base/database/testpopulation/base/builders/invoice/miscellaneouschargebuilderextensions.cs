// <copyright file="MiscellaneousChargeBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain.TestPopulation
{
    public static partial class MiscellaneousChargeBuilderExtensions
    {
        public static MiscellaneousChargeBuilder WithAmountDefaults(this MiscellaneousChargeBuilder @this)
        {
            var faker = @this.Session.Faker();

            @this.WithAmount(decimal.Round(faker.Random.Decimal(10, 100), 2));
            @this.WithDescription(faker.Lorem.Sentence());

            return @this;
        }

        public static MiscellaneousChargeBuilder WithPercentageDefaults(this MiscellaneousChargeBuilder @this)
        {
            var faker = @this.Session.Faker();

            @this.WithPercentage(decimal.Round(faker.Random.Decimal(1, 5), 2));
            @this.WithDescription(faker.Lorem.Sentence());

            return @this;
        }
    }
}
