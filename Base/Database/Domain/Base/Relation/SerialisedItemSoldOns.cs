// <copyright file="SerialisedItemSoldOns.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SerialisedItemSoldOns
    {
        public static readonly Guid SalesOrderId = new Guid("b4f69621-7eb7-4763-928c-415cf3b4a4b5");
        public static readonly Guid CustomerShipmentId = new Guid("9cb9b456-8e10-4bcc-813d-3aa508c060e1");

        private UniquelyIdentifiableSticky<SerialisedItemSoldOn> cache;

        public SerialisedItemSoldOn SalesOrder => this.Cache[SalesOrderId];

        public SerialisedItemSoldOn CustomerShipment => this.Cache[CustomerShipmentId];

        private UniquelyIdentifiableSticky<SerialisedItemSoldOn> Cache => this.cache ??= new UniquelyIdentifiableSticky<SerialisedItemSoldOn>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(SalesOrderId, v => v.Name = "SalesOrder");
            merge(CustomerShipmentId, v => v.Name = "CustomerShipment");
        }
    }
}
