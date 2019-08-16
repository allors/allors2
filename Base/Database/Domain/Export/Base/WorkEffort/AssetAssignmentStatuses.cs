
// <copyright file="AssetAssignmentStatuses.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class AssetAssignmentStatuses
    {
        private static readonly Guid RequestedId = new Guid("9CF35CC2-9E16-4c8a-A2F5-2D2DDD056AED");
        private static readonly Guid AssignedId = new Guid("7CA979A0-8CBF-426f-AFD2-F5C519FB206D");

        private UniquelyIdentifiableSticky<AssetAssignmentStatus> cache;

        public AssetAssignmentStatus Requested => this.Cache[RequestedId];

        public AssetAssignmentStatus Assigned => this.Cache[AssignedId];

        private UniquelyIdentifiableSticky<AssetAssignmentStatus> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<AssetAssignmentStatus>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new AssetAssignmentStatusBuilder(this.Session)
                .WithName("Requested")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Aangevraagd").WithLocale(dutchLocale).Build())
                .WithUniqueId(RequestedId)
                .WithIsActive(true)
                .Build();

            new AssetAssignmentStatusBuilder(this.Session)
                .WithName("Assigned")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Toegekend").WithLocale(dutchLocale).Build())
                .WithUniqueId(AssignedId)
                .WithIsActive(true)
                .Build();
        }
    }
}
