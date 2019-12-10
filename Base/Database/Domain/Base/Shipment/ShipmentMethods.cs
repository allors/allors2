// <copyright file="ShipmentMethods.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class ShipmentMethods
    {
        private static readonly Guid GroundId = new Guid("8E534609-12C4-4701-A868-D7B3B3D4D6D9");
        private static readonly Guid RailId = new Guid("5F13CAFD-022F-46aa-BA89-7992DC52F69B");
        private static readonly Guid FirstClassAirId = new Guid("D2A20D3A-7790-485c-910A-A09854E37FED");
        private static readonly Guid BoatId = new Guid("CD6A439A-445A-4f8c-8DE2-654A0C504F48");
        private static readonly Guid ExWorksId = new Guid("5E628193-2E06-4C9B-BE86-D9BEB91B7D64");

        private UniquelyIdentifiableSticky<ShipmentMethod> cache;

        public ShipmentMethod Ground => this.Cache[GroundId];

        public ShipmentMethod Rail => this.Cache[RailId];

        public ShipmentMethod FirstClassAir => this.Cache[FirstClassAirId];

        public ShipmentMethod Boat => this.Cache[BoatId];

        public ShipmentMethod ExWorks => this.Cache[ExWorksId];

        private UniquelyIdentifiableSticky<ShipmentMethod> Cache => this.cache ??= new UniquelyIdentifiableSticky<ShipmentMethod>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(GroundId, v =>
            {
                v.Name = "Ground";
                localisedName.Set(v, dutchLocale, "Weg");
                v.IsActive = true;
            });

            merge(RailId, v =>
            {
                v.Name = "Rail";
                localisedName.Set(v, dutchLocale, "Spoor");
                v.IsActive = true;
            });

            merge(FirstClassAirId, v =>
            {
                v.Name = "Air";
                localisedName.Set(v, dutchLocale, "Vliegtuig");
                v.IsActive = true;
            });

            merge(BoatId, v =>
            {
                v.Name = "Boat";
                localisedName.Set(v, dutchLocale, "Boot");
                v.IsActive = true;
            });

            merge(ExWorksId, v =>
            {
                v.Name = "Ex works";
                localisedName.Set(v, dutchLocale, "Af fabriek");
                v.IsActive = true;
            });
        }
    }
}
