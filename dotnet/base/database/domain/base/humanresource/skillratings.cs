// <copyright file="SkillRatings.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SkillRatings
    {
        private static readonly Guid PoorId = new Guid("5D2D23C7-95AA-49ed-8B2A-9A3E4D91BC3D");
        private static readonly Guid FairId = new Guid("583BCA0A-2A5E-40c1-936C-D8F16A4DAAC5");
        private static readonly Guid GoodId = new Guid("374DEE3A-82FA-4bee-B66B-F48CA1B0CBD7");
        private static readonly Guid ExcellentId = new Guid("52029ECD-1752-4b40-A39D-54B0C1CB8297");

        private UniquelyIdentifiableSticky<SkillRating> cache;

        public SkillRating Poor => this.Cache[PoorId];

        public SkillRating Fair => this.Cache[FairId];

        public SkillRating Good => this.Cache[GoodId];

        public SkillRating Excellent => this.Cache[ExcellentId];

        private UniquelyIdentifiableSticky<SkillRating> Cache => this.cache ??= new UniquelyIdentifiableSticky<SkillRating>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(PoorId, v =>
            {
                v.Name = "Poor";
                localisedName.Set(v, dutchLocale, "Slecht");
                v.IsActive = true;
            });

            merge(FairId, v =>
            {
                v.Name = "Fair";
                localisedName.Set(v, dutchLocale, "Matig");
                v.IsActive = true;
            });

            merge(GoodId, v =>
            {
                v.Name = "Good";
                localisedName.Set(v, dutchLocale, "Goed");
                v.IsActive = true;
            });

            merge(ExcellentId, v =>
            {
                v.Name = "Excellent";
                localisedName.Set(v, dutchLocale, "Uitstekend");
                v.IsActive = true;
            });
        }
    }
}
