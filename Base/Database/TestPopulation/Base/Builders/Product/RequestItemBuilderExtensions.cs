// <copyright file="RequestItemBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain.TestPopulation
{
    public static partial class RequestItemBuilderExtensions
    {
        public static RequestItemBuilder WithSerializedDefaults(this RequestItemBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var finishedGood = new UnifiedGoodBuilder(@this.Session).WithSerialisedDefaults(internalOrganisation).Build();

            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithMaximumAllowedPrice(faker.Random.UInt());
            @this.WithProduct(finishedGood).Build();
            @this.WithRequiredByDate(@this.Session.Now().AddDays(7)).Build();
            @this.WithSerialisedItem(finishedGood.SerialisedItems.First);

            return @this;
        }

        public static RequestItemBuilder WithNonSerializedDefaults(this RequestItemBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var finishedGood = new UnifiedGoodBuilder(@this.Session).WithNonSerialisedDefaults(internalOrganisation).Build();

            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithMaximumAllowedPrice(faker.Random.UInt());
            @this.WithQuantity(faker.Random.UShort());
            @this.WithProduct(finishedGood).Build();
            @this.WithRequiredByDate(@this.Session.Now().AddDays(7)).Build();

            return @this;
        }
    }
}
