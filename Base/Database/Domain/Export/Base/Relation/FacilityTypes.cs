// <copyright file="FacilityTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class FacilityTypes
    {
        private static readonly Guid WarehouseId = new Guid("56AD0A65-1FC0-40EA-BDA8-DADDFA6CBE63");
        private static readonly Guid StorageLocationId = new Guid("FF66C1AD-3048-48FD-A7D9-FBF97A090EDD");

        private UniquelyIdentifiableSticky<FacilityType> cache;

        public FacilityType Warehouse => this.Cache[WarehouseId];

        public FacilityType StorageLocation => this.Cache[StorageLocationId];

        private UniquelyIdentifiableSticky<FacilityType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<FacilityType>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new FacilityTypeBuilder(this.Session)
                .WithName("Warehouse")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Magazijn").WithLocale(dutchLocale).Build())
                .WithUniqueId(WarehouseId)
                .WithIsActive(true)
                .Build();

            new FacilityTypeBuilder(this.Session)
                .WithName("Storage location")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Opslag plaats").WithLocale(dutchLocale).Build())
                .WithUniqueId(StorageLocationId)
                .WithIsActive(true)
                .Build();
        }
    }
}
