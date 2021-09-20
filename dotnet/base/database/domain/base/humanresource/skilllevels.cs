// <copyright file="SkillLevels.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SkillLevels
    {
        private static readonly Guid BeginnerId = new Guid("F8A94F72-E5F2-49c5-B738-6B0F5EFC7BAF");
        private static readonly Guid IntermediateId = new Guid("BD859561-9488-4db4-82C4-621D0F4AA1B4");
        private static readonly Guid AdvancedId = new Guid("C4FD3054-20F4-40e8-B6A3-E91734D75C13");
        private static readonly Guid ExpertId = new Guid("E204AA8A-C61E-44f6-906B-FE45AB15D4B0");

        private UniquelyIdentifiableSticky<SkillLevel> cache;

        public SkillLevel Beginner => this.Cache[BeginnerId];

        public SkillLevel Intermediate => this.Cache[IntermediateId];

        public SkillLevel Advanced => this.Cache[AdvancedId];

        public SkillLevel Expert => this.Cache[ExpertId];

        private UniquelyIdentifiableSticky<SkillLevel> Cache => this.cache ??= new UniquelyIdentifiableSticky<SkillLevel>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(BeginnerId, v =>
            {
                v.Name = "Beginner";
                localisedName.Set(v, dutchLocale, "Starter");
                v.IsActive = true;
            });

            merge(IntermediateId, v =>
            {
                v.Name = "Intermediate";
                localisedName.Set(v, dutchLocale, "Intermediate");
                v.IsActive = true;
            });

            merge(AdvancedId, v =>
            {
                v.Name = "Advanced";
                localisedName.Set(v, dutchLocale, "Ervaren");
                v.IsActive = true;
            });

            merge(ExpertId, v =>
            {
                v.Name = "Expert";
                localisedName.Set(v, dutchLocale, "Expert");
                v.IsActive = true;
            });
        }
    }
}
