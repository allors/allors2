// <copyright file="ShipmentStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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

        private UniquelyIdentifiableSticky<ShipmentState> sticky;

        public Sticky<Guid, ShipmentState> Sticky => this.sticky ?? (this.sticky = new UniquelyIdentifiableSticky<ShipmentState>(this.Session));

        public ShipmentState NotShipped => this.Sticky[NotShippedId];

        public ShipmentState PartiallyShipped => this.Sticky[PartiallyShippedId];

        public ShipmentState Shipped => this.Sticky[ShippedId];

        protected override void CoreSetup(Setup setup)
        {
            new ShipmentStateBuilder(this.Session).WithUniqueId(NotShippedId).WithName("NotShipped").Build();
            new ShipmentStateBuilder(this.Session).WithUniqueId(PartiallyShippedId).WithName("PartiallyShipped").Build();
            new ShipmentStateBuilder(this.Session).WithUniqueId(ShippedId).WithName("Shipped").Build();
        }
    }
}
