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

        private UniquelyIdentifiableSticky<SerialisedItemState> stateCache;

        public SerialisedItemState NA => this.StateCache[NaId];

        public SerialisedItemState Sold => this.StateCache[SoldId];

        public SerialisedItemState InRent => this.StateCache[InRentId];

        public SerialisedItemState Good => this.StateCache[GoodId];

        public SerialisedItemState BeingRepaired => this.StateCache[BeingRepairedId];

        public SerialisedItemState SlightlyDamaged => this.StateCache[SlightlyDamagedId];

        public SerialisedItemState Defective => this.StateCache[DefectiveId];

        public SerialisedItemState Scrap => this.StateCache[ScrapId];

        public SerialisedItemState Available => this.StateCache[AvailableId];

        public SerialisedItemState Assigned => this.StateCache[AssignedId];

        private UniquelyIdentifiableSticky<SerialisedItemState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<SerialisedItemState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            new SerialisedItemStateBuilder(this.Session)
                .WithUniqueId(NaId)
                .WithName("N/A")
                .WithIsActive(true)
                .Build();

            new SerialisedItemStateBuilder(this.Session)
                .WithUniqueId(SoldId)
                .WithName("Sold")
                .WithIsActive(true)
                .Build();

            new SerialisedItemStateBuilder(this.Session)
                .WithUniqueId(InRentId)
                .WithName("InRent")
                .WithIsActive(true)
                .Build();

            new SerialisedItemStateBuilder(this.Session)
                .WithUniqueId(GoodId)
                .WithName("In good order")
                .WithIsActive(true)
                .Build();

            new SerialisedItemStateBuilder(this.Session)
                .WithUniqueId(BeingRepairedId)
                .WithName("Being Repaired")
                .WithIsActive(true)
                .Build();

            new SerialisedItemStateBuilder(this.Session)
                .WithUniqueId(SlightlyDamagedId)
                .WithName("Slightly Damaged")
                .WithIsActive(true)
                .Build();

            new SerialisedItemStateBuilder(this.Session)
                .WithUniqueId(DefectiveId)
                .WithName("Defective")
                .WithIsActive(true)
                .Build();

            new SerialisedItemStateBuilder(this.Session)
                .WithUniqueId(ScrapId)
                .WithName("Scrap")
                .WithIsActive(true)
                .Build();

            new SerialisedItemStateBuilder(this.Session)
                .WithUniqueId(AvailableId)
                .WithName("Available")
                .WithIsActive(true)
                .Build();

            new SerialisedItemStateBuilder(this.Session)
                .WithUniqueId(AssignedId)
                .WithName("Assigned")
                .WithIsActive(true)
                .Build();
        }
    }
}
