// <copyright file="ShipmentStates.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the HomeAddress type.</summary>

namespace Allors.Domain
{
    using System;

    public partial class ShipmentStates
    {
        private static readonly Guid NotShippedId = new Guid("FC38E48D-C8C4-4F26-A8F1-5D4E962B6F93");
        private static readonly Guid PartiallyShippedId = new Guid("1801737F-2760-4600-9243-7E6BDD8A224D");
        private static readonly Guid ShippedId = new Guid("04FAD96A-2B0F-4F07-ABB7-57657A34E422");

        private UniquelyIdentifiableSticky<ShipmentState> cache;

        public Sticky<Guid, ShipmentState> Cache => this.cache ??= new UniquelyIdentifiableSticky<ShipmentState>(this.Session);

        public ShipmentState NotShipped => this.Cache[NotShippedId];

        public ShipmentState PartiallyShipped => this.Cache[PartiallyShippedId];

        public ShipmentState Shipped => this.Cache[ShippedId];

        protected override void CoreSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(NotShippedId, v => v.Name = "NotShipped");
            merge(PartiallyShippedId, v => v.Name = "PartiallyShipped");
            merge(ShippedId, v => v.Name = "Shipped");
        }
    }
}
