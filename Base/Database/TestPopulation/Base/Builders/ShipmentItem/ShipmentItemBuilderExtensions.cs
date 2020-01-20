// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShipmentItemBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using Allors.Meta;

namespace Allors.Domain.TestPopulation
{
    public static partial class ShipmentItemBuilderExtensions
    {
        public static ShipmentItemBuilder WithSerialisedUnifiedGoodDefaults(this ShipmentItemBuilder @this, Organisation internalOrganization)
        {
            var faker = @this.Session.Faker();

            var goods = new UnifiedGoods(@this.Session).Extent();
            goods.Filter.AddEquals(M.UnifiedGood.InventoryItemKind.RoleType, new InventoryItemKinds(@this.Session).Serialised);
            var serializedGood = goods.First;

            @this.WithGood(serializedGood);
            @this.WithQuantity(1);
            @this.WithSerialisedItem(serializedGood.SerialisedItems.First);

            return @this;
        }
    }
}
