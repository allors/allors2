// <copyright file="Priorities.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class Priorities
    {
        private static readonly Guid VeryHighId = new Guid("AE6AB101-481C-4ff1-8BE5-6CD6311903D7");
        private static readonly Guid HighId = new Guid("23248D29-A6B5-4081-A1B9-101A28460366");
        private static readonly Guid MediumId = new Guid("3B6A4A9A-1124-47fd-B812-DD034BE193E4");
        private static readonly Guid LowId = new Guid("ED1E1A54-343D-42d4-A1C3-884C7D925372");
        private static readonly Guid FirstId = new Guid("9638E638-1DCE-4f51-B6AF-598CE968313C");
        private static readonly Guid SecondId = new Guid("1BE83C5B-72C4-4d08-900B-79D2EF36BF1A");
        private static readonly Guid ThirdId = new Guid("1078C4C8-37B4-4f5b-B650-04DEA2C337C8");

        private UniquelyIdentifiableSticky<Priority> cache;

        public Priority VeryHigh => this.Cache[VeryHighId];

        public Priority High => this.Cache[HighId];

        public Priority Medium => this.Cache[MediumId];

        public Priority Low => this.Cache[LowId];

        public Priority First => this.Cache[FirstId];

        public Priority Second => this.Cache[SecondId];

        public Priority Third => this.Cache[ThirdId];

        private UniquelyIdentifiableSticky<Priority> Cache => this.cache ??= new UniquelyIdentifiableSticky<Priority>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(VeryHighId, v =>
            {
                v.Name = "Very High";
                localisedName.Set(v, dutchLocale, "Zeer hoog");
                v.IsActive = true;
            });

            merge(HighId, v =>
            {
                v.Name = "High";
                localisedName.Set(v, dutchLocale, "Hoog");
                v.IsActive = true;
            });

            merge(MediumId, v =>
            {
                v.Name = "Medium";
                localisedName.Set(v, dutchLocale, "Gemiddeld");
                v.IsActive = true;
            });

            merge(LowId, v =>
            {
                v.Name = "Low";
                localisedName.Set(v, dutchLocale, "Laag");
                v.IsActive = true;
            });

            merge(FirstId, v =>
            {
                v.Name = "First";
                localisedName.Set(v, dutchLocale, "Eerste");
                v.IsActive = true;
            });

            merge(SecondId, v =>
            {
                v.Name = "Second";
                localisedName.Set(v, dutchLocale, "Tweede");
                v.IsActive = true;
            });

            merge(ThirdId, v =>
            {
                v.Name = "Third";
                localisedName.Set(v, dutchLocale, "Derde");
                v.IsActive = true;
            });
        }
    }
}
