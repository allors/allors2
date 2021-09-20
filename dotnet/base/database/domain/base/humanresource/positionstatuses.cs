// <copyright file="PositionStatuses.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class PositionStatuses
    {
        private static readonly Guid PlannedForId = new Guid("6B5FA0D0-1E9C-4661-977C-A00CD985D614");
        private static readonly Guid ActiveId = new Guid("6001482F-23E1-4f0a-82C7-52F2EA793627");
        private static readonly Guid PositionOpenId = new Guid("B6547476-A5E0-41c4-87B8-08A87B7663C3");
        private static readonly Guid InactiveId = new Guid("C3DF71FC-AE5E-4f72-A088-477A37D4B448");
        private static readonly Guid PositionClosedId = new Guid("49BBC4D9-96E0-444d-971D-56D90AB0F8C1");

        private UniquelyIdentifiableSticky<PositionStatus> cache;

        public PositionStatus PlannedFor => this.Cache[PlannedForId];

        public PositionStatus Active => this.Cache[ActiveId];

        public PositionStatus PositionOpen => this.Cache[PositionOpenId];

        public PositionStatus Inactive => this.Cache[InactiveId];

        public PositionStatus PositionClosed => this.Cache[PositionClosedId];

        private UniquelyIdentifiableSticky<PositionStatus> Cache => this.cache ??= new UniquelyIdentifiableSticky<PositionStatus>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(PlannedForId, v =>
            {
                v.Name = "Planned For";
                localisedName.Set(v, dutchLocale, "Gepland");
                v.IsActive = true;
            });

            merge(ActiveId, v =>
            {
                v.Name = "Active";
                localisedName.Set(v, dutchLocale, "Actief");
                v.IsActive = true;
            });

            merge(PositionOpenId, v =>
            {
                v.Name = "Open";
                localisedName.Set(v, dutchLocale, "Open");
                v.IsActive = true;
            });

            merge(InactiveId, v =>
            {
                v.Name = "Inactivef";
                localisedName.Set(v, dutchLocale, "Niet actief");
                v.IsActive = true;
            });

            merge(PositionClosedId, v =>
            {
                v.Name = "Closed";
                localisedName.Set(v, dutchLocale, "Gesloten");
                v.IsActive = true;
            });
        }
    }
}
