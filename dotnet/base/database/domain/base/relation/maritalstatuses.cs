// <copyright file="MaritalStatuses.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class MaritalStatuses
    {
        private static readonly Guid SingleId = new Guid("83CD5BBD-85A4-417d-A3E0-5DAA509ACE23");
        private static readonly Guid MarriedId = new Guid("487A649F-DF55-43c1-8B66-3964A056DF2A");
        private static readonly Guid DivorcedId = new Guid("B6DE4339-76B0-4675-802D-20AAA482E30F");
        private static readonly Guid WidowedId = new Guid("C3755D4A-FF2E-4c4c-B17E-BCC5BC0599BF");

        private UniquelyIdentifiableSticky<MaritalStatus> cache;

        public MaritalStatus Single => this.Cache[SingleId];

        public MaritalStatus Married => this.Cache[MarriedId];

        public MaritalStatus Divorced => this.Cache[DivorcedId];

        public MaritalStatus Widowed => this.Cache[WidowedId];

        private UniquelyIdentifiableSticky<MaritalStatus> Cache => this.cache ??= new UniquelyIdentifiableSticky<MaritalStatus>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(SingleId, v =>
            {
                v.Name = "Single";
                localisedName.Set(v, dutchLocale, "Alleenstaand");
                v.IsActive = true;
            });

            merge(MarriedId, v =>
            {
                v.Name = "Married";
                localisedName.Set(v, dutchLocale, "Gehuwd");
                v.IsActive = true;
            });

            merge(DivorcedId, v =>
            {
                v.Name = "Divorced";
                localisedName.Set(v, dutchLocale, "Gescheiden");
                v.IsActive = true;
            });

            merge(WidowedId, v =>
            {
                v.Name = "Widowed";
                localisedName.Set(v, dutchLocale, "Weduw(e)(naar)");
                v.IsActive = true;
            });
        }
    }
}
