// <copyright file="SerialisedInventoryItemStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SerialisedInventoryItemStates
    {
        private static readonly Guid GoodId = new Guid("CD80FC9B-BF25-4587-8D52-57E491E74104");
        private static readonly Guid BeingRepairedId = new Guid("4584A95F-60BA-478c-8E09-E6AFC88CF683");
        private static readonly Guid SlightlyDamagedId = new Guid("9CA506D4-ACB1-40ac-BEEC-83F080E6029E");
        private static readonly Guid DefectiveId = new Guid("36F94BB5-D93E-44cc-8F10-37A046002E5B");
        private static readonly Guid ScrapId = new Guid("9D02749B-A30E-4bb4-B016-E1CF96A5F99B");
        private static readonly Guid AvailableId = new Guid("E5AD6F2D-2EDF-4563-8AD4-59EF1211273F");
        private static readonly Guid NotAvailableId = new Guid("E553F107-FFB1-4F80-9EA0-4C436FD1F736");
        private static readonly Guid AssignedId = new Guid("3AD2DEC0-65AB-4E31-BDE0-3227727D9329");

        private UniquelyIdentifiableSticky<SerialisedInventoryItemState> cache;

        public SerialisedInventoryItemState Good => this.Cache[GoodId];

        public SerialisedInventoryItemState BeingRepaired => this.Cache[BeingRepairedId];

        public SerialisedInventoryItemState SlightlyDamaged => this.Cache[SlightlyDamagedId];

        public SerialisedInventoryItemState Defective => this.Cache[DefectiveId];

        public SerialisedInventoryItemState Scrap => this.Cache[ScrapId];

        public SerialisedInventoryItemState Available => this.Cache[AvailableId];

        public SerialisedInventoryItemState NotAvailable => this.Cache[NotAvailableId];

        public SerialisedInventoryItemState Assigned => this.Cache[AssignedId];

        private UniquelyIdentifiableSticky<SerialisedInventoryItemState> Cache => this.cache ??= new UniquelyIdentifiableSticky<SerialisedInventoryItemState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(GoodId, v =>
            {
                v.Name = "In good order";
                v.IsActive = true;
            });

            merge(BeingRepairedId, v =>
            {
                v.Name = "Being Repaired";
                v.IsActive = true;
            });

            merge(SlightlyDamagedId, v =>
            {
                v.Name = "Slightly Damaged";
                v.IsActive = true;
            });

            merge(DefectiveId, v =>
            {
                v.Name = "Defective";
                v.IsActive = true;
            });

            merge(ScrapId, v =>
            {
                v.Name = "Scrap";
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

            merge(AssignedId, v =>
            {
                v.Name = "Assigned";
                v.IsActive = true;
            });
        }
    }
}
