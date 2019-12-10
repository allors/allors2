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

        private UniquelyIdentifiableSticky<Ownership> Cache => this.cache ??= new UniquelyIdentifiableSticky<Ownership>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(OwnId, v =>
            {
                v.Name = "Own";
                localisedName.Set(v, dutchLocale, "Eigen");
                v.IsActive = true;
            });

            merge(TradingId, v =>
            {
                v.Name = "Trading";
                localisedName.Set(v, dutchLocale, "Handel");
                v.IsActive = true;
            });
        }
    }
}
