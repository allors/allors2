// <copyright file="SerialisedItemAvailabilities.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SerialisedItemAvailabilities
    {
        private static readonly Guid SoldId = new Guid("9bdc0a55-4e3c-4604-b054-2441a551aa1c");
        private static readonly Guid InRentId = new Guid("ec87f723-2284-4f5c-ba57-fcf328a0b738");
        private static readonly Guid AvailableId = new Guid("c60f5741-a93f-48cc-b416-445aeb3fb166");
        private static readonly Guid NotAvailableId = new Guid("74499ac5-cac9-4276-8b9e-e47f977104fd");

        private UniquelyIdentifiableSticky<SerialisedItemAvailability> cache;

        public SerialisedItemAvailability Sold => this.Cache[SoldId];

        public SerialisedItemAvailability InRent => this.Cache[InRentId];

        public SerialisedItemAvailability Available => this.Cache[AvailableId];

        public SerialisedItemAvailability NotAvailable => this.Cache[NotAvailableId];

        private UniquelyIdentifiableSticky<SerialisedItemAvailability> Cache =>
            this.cache ??= new UniquelyIdentifiableSticky<SerialisedItemAvailability>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(SoldId, v =>
            {
                v.Name = "Sold";
                v.IsActive = true;
            });
            merge(InRentId, v =>
            {
                v.Name = "InRent";
                v.IsActive = true;
            });
            merge(AvailableId, v =>
            {
                v.Name = "Available";
                v.IsActive = true;
            });
            merge(NotAvailableId, v =>
            {
                v.Name = "Not Available";
                v.IsActive = true;
            });
        }
    }
}
