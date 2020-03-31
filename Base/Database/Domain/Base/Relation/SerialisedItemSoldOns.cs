// <copyright file="SerialisedItemSoldOns.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SerialisedItemSoldOns
    {
        public static readonly Guid SalesOrderPostId = new Guid("b4f69621-7eb7-4763-928c-415cf3b4a4b5");
        public static readonly Guid CustomerShipmentShipId = new Guid("9cb9b456-8e10-4bcc-813d-3aa508c060e1");

        private UniquelyIdentifiableSticky<SerialisedItemSoldOn> cache;

        public SerialisedItemSoldOn SalesOrderPost => this.Cache[SalesOrderPostId];

        public SerialisedItemSoldOn CustomerShipmentShip => this.Cache[CustomerShipmentShipId];

        private UniquelyIdentifiableSticky<SerialisedItemSoldOn> Cache => this.cache ??= new UniquelyIdentifiableSticky<SerialisedItemSoldOn>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(SalesOrderPostId, v => v.Name = "SalesOrder Post");
            merge(CustomerShipmentShipId, v => v.Name = "CustomerShipment Ship");
        }
    }
}
