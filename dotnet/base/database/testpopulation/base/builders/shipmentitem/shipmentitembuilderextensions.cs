// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShipmentItemBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.TestPopulation
{
    using System.Linq;

    public static partial class ShipmentItemBuilderExtensions
    {
        public static ShipmentItemBuilder WithSerializedUnifiedGoodDefaults(this ShipmentItemBuilder @this, Organisation internalOrganization)
        {
            var faker = @this.Session.Faker();

            var good = internalOrganization.CreateUnifiedWithGoodInventoryAvailableForSale(faker);
            var serializedItem = good.SerialisedItems.First(v => v.AvailableForSale.Equals(true));

            @this.WithGood(good);
            @this.WithSerialisedItem(serializedItem);
            @this.WithNextSerialisedItemAvailability(faker.Random.ListItem(@this.Session.Extent<SerialisedItemAvailability>()));
            @this.WithQuantity(1);

            return @this;
        }
    }
}
