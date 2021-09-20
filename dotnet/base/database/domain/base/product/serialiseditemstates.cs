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
        private static readonly Guid GoodId = new Guid("81C04143-6336-479B-B48E-40FE1925F29B");
        private static readonly Guid SlightlyDamagedId = new Guid("BD2741E8-9EDB-47DF-AEFB-ED258AB4F7B9");
        private static readonly Guid DefectiveId = new Guid("41067E72-5833-461C-8087-308F91C205E4");
        private static readonly Guid ScrapId = new Guid("8FD0680D-28E4-4F85-8BC9-E935B6B64FAE");

        private UniquelyIdentifiableSticky<SerialisedItemState> cache;

        public SerialisedItemState NA => this.Cache[NaId];

        public SerialisedItemState Good => this.Cache[GoodId];

        public SerialisedItemState SlightlyDamaged => this.Cache[SlightlyDamagedId];

        public SerialisedItemState Defective => this.Cache[DefectiveId];

        public SerialisedItemState Scrap => this.Cache[ScrapId];

        private UniquelyIdentifiableSticky<SerialisedItemState> Cache =>
            this.cache ??= new UniquelyIdentifiableSticky<SerialisedItemState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(NaId, v =>
            {
                v.Name = "N/A";
                v.IsActive = true;
            });
            merge(GoodId, v =>
            {
                v.Name = "In good order";
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
        }
    }
}
