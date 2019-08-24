// <copyright file="Ownerships.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class Ownerships
    {
        private static readonly Guid OwnId = new Guid("74AA16DE-5719-4AE1-9547-0570E1111EDC");
        private static readonly Guid TradingId = new Guid("1E1BABFA-2F4F-45EF-BFC0-848E0199F4DF");

        private UniquelyIdentifiableSticky<Ownership> cache;

        public Ownership Own => this.Cache[OwnId];

        public Ownership Trading => this.Cache[TradingId];

        private UniquelyIdentifiableSticky<Ownership> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<Ownership>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new OwnershipBuilder(this.Session)
                .WithName("Own")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Eigen").WithLocale(dutchLocale).Build())
                .WithUniqueId(OwnId)
                .WithIsActive(true)
                .Build();

            new OwnershipBuilder(this.Session)
                .WithName("Trading")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Handel").WithLocale(dutchLocale).Build())
                .WithUniqueId(TradingId)
                .WithIsActive(true)
                .Build();
        }
    }
}
