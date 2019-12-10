// <copyright file="GenderTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class GenderTypes
    {
        private static readonly Guid MaleId = new Guid("DAB59C10-0D45-4478-A802-3ABE54308CCD");
        private static readonly Guid FemaleId = new Guid("B68704AD-82F1-4d5d-BBAF-A54635B5034F");
        private static readonly Guid OtherId = new Guid("09210D7C-804B-4E76-AD91-0E150D36E86E");

        private UniquelyIdentifiableSticky<GenderType> cache;

        public GenderType Male => this.Cache[MaleId];

        public GenderType Female => this.Cache[FemaleId];

        public GenderType Other => this.Cache[OtherId];

        private UniquelyIdentifiableSticky<GenderType> Cache => this.cache ??= new UniquelyIdentifiableSticky<GenderType>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(MaleId, v =>
            {
                v.Name = "Male";
                localisedName.Set(v, dutchLocale, "Mannelijk");
                v.IsActive = true;
            });

            merge(FemaleId, v =>
            {
                v.Name = "Female";
                localisedName.Set(v, dutchLocale, "Vrouwelijk");
                v.IsActive = true;
            });

            merge(OtherId, v =>
            {
                v.Name = "Other";
                localisedName.Set(v, dutchLocale, "Anders");
                v.IsActive = true;
            });
        }
    }
}
