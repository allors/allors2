// <copyright file="DeductionTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class DeductionTypes
    {
        private static readonly Guid RetirementId = new Guid("2D0F0788-EB2B-4ef3-A09A-A7285DAD72CF");
        private static readonly Guid InsuranceId = new Guid("D82A5A9F-068F-4e30-88F5-5E6C81D03BAF");

        private UniquelyIdentifiableSticky<DeductionType> cache;

        public DeductionType Retirement => this.Cache[RetirementId];

        public DeductionType Insurance => this.Cache[InsuranceId];

        private UniquelyIdentifiableSticky<DeductionType> Cache => this.cache ??= new UniquelyIdentifiableSticky<DeductionType>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(RetirementId, v =>
            {
                v.Name = "Retirement";
                localisedName.Set(v, dutchLocale, "Pensioen");
                v.IsActive = true;
            });

            merge(InsuranceId, v =>
            {
                v.Name = "Insurance";
                localisedName.Set(v, dutchLocale, "Verzekering");
                v.IsActive = true;
            });
        }
    }
}
