// <copyright file="SerialisedItemStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SerialisedItemStates
    {
        private static readonly Guid NaId = new Guid("E5AD6F2D-2EDF-4563-8AD4-59EF1211273F");
        private static readonly Guid SoldId = new Guid("FECCF869-98D7-4E9C-8979-5611A43918BC");
        private static readonly Guid InRentId = new Guid("9ACC6C05-60B5-4085-8B43-EB730939DB47");
        private static readonly Guid GoodId = new Guid("81C04143-6336-479B-B48E-40FE1925F29B");
        private static readonly Guid BeingRepairedId = new Guid("FC0ACDBC-A89F-45AD-AC86-8199C18471DA");
        private static readonly Guid SlightlyDamagedId = new Guid("BD2741E8-9EDB-47DF-AEFB-ED258AB4F7B9");
        private static readonly Guid DefectiveId = new Guid("41067E72-5833-461C-8087-308F91C205E4");
        private static readonly Guid ScrapId = new Guid("8FD0680D-28E4-4F85-8BC9-E935B6B64FAE");
        private static readonly Guid AvailableId = new Guid("2FE8050B-C970-4A14-8156-ABEAA9FF0140");
        private static readonly Guid AssignedId = new Guid("20338765-67A6-4ED0-83C0-A9B0831E6B40");

        private UniquelyIdentifiableSticky<SerialisedItemState> cache;

        public SerialisedItemState NA => this.Cache[NaId];

        public SerialisedItemState Sold => this.Cache[SoldId];

        public SerialisedItemState InRent => this.Cache[InRentId];

        public SerialisedItemState Good => this.Cache[GoodId];

        public SerialisedItemState BeingRepaired => this.Cache[BeingRepairedId];

        public SerialisedItemState SlightlyDamaged => this.Cache[SlightlyDamagedId];

        public SerialisedItemState Defective => this.Cache[DefectiveId];

        public SerialisedItemState Scrap => this.Cache[ScrapId];

        public SerialisedItemState Available => this.Cache[AvailableId];

        public SerialisedItemState Assigned => this.Cache[AssignedId];

        private UniquelyIdentifiableSticky<SerialisedItemState> Cache => this.cache ??= new UniquelyIdentifiableSticky<SerialisedItemState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(NaId, v => v.Name = "N/A");
            merge(SoldId, v => v.Name = "Sold");
            merge(InRentId, v => v.Name = "InRent");
            merge(GoodId, v => v.Name = "In good order");
            merge(BeingRepairedId, v => v.Name = "Being Repaired");
            merge(SlightlyDamagedId, v => v.Name = "Slightly Damaged");
            merge(DefectiveId, v => v.Name = "Defective");
            merge(ScrapId, v => v.Name = "Scrap");
            merge(AvailableId, v => v.Name = "Available");
            merge(AssignedId, v => v.Name = "Assigned");
        }
    }
}
