// <copyright file="CostOfGoodsSoldMethods.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class CostOfGoodsSoldMethods
    {
        private static readonly Guid FiFoId = new Guid("CC6A3F78-9063-4E4A-BBB1-08A529017696");
        private static readonly Guid LiFoId = new Guid("A4E05005-FEC2-4F81-AD67-4F9C958BF94A");
        private static readonly Guid AverageId = new Guid("857D84FF-18AE-4139-8A4F-8A4BAD78C79E");

        private UniquelyIdentifiableSticky<CostOfGoodsSoldMethod> cache;

        public CostOfGoodsSoldMethod FiFo => this.Cache[FiFoId];

        public CostOfGoodsSoldMethod LiFo => this.Cache[LiFoId];

        public CostOfGoodsSoldMethod Average => this.Cache[AverageId];

        private UniquelyIdentifiableSticky<CostOfGoodsSoldMethod> Cache => this.cache ??= new UniquelyIdentifiableSticky<CostOfGoodsSoldMethod>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(FiFoId, v =>
            {
                v.Name = "FiFo";
                localisedName.Set(v, dutchLocale, "FiFo");
                v.IsActive = true;
            });

            merge(LiFoId, v =>
            {
                v.Name = "LiFo";
                localisedName.Set(v, dutchLocale, "LiFo");
                v.IsActive = true;
            });

            merge(AverageId, v =>
            {
                v.Name = "Average price";
                localisedName.Set(v, dutchLocale, "Gemiddelde prijs");
                v.IsActive = true;
            });
        }
    }
}
