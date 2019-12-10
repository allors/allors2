// <copyright file="PickListStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class PickListStates
    {
        private static readonly Guid CreatedId = new Guid("E65872C0-AD3C-4802-A253-CF99F8209011");
        private static readonly Guid PickedId = new Guid("93C6B430-91BD-4e6c-AC2C-C287F2A8021D");
        private static readonly Guid CancelledId = new Guid("CD552AF5-E695-4329-BF87-5644C2EA98F3");
        private static readonly Guid OnHoldId = new Guid("1733E2B0-48CA-4731-8F3C-93C6CF3A9543");

        private UniquelyIdentifiableSticky<PickListState> cache;

        public PickListState Created => this.Cache[CreatedId];

        public PickListState Picked => this.Cache[PickedId];

        public PickListState Cancelled => this.Cache[CancelledId];

        public PickListState OnHold => this.Cache[OnHoldId];

        private UniquelyIdentifiableSticky<PickListState> Cache => this.cache ??= new UniquelyIdentifiableSticky<PickListState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(CreatedId, v => v.Name = "Created");
            merge(PickedId, v => v.Name = "Picked");
            merge(CancelledId, v => v.Name = "Cancelled");
            merge(OnHoldId, v => v.Name = "On Hold");
        }
    }
}
