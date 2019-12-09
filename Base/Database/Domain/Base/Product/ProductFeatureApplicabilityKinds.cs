// <copyright file="ProductFeatureApplicabilityKinds.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class ProductFeatureApplicabilityKinds
    {
        private static readonly Guid RequiredId = new Guid("7FBDCDDF-5983-4653-A0FC-768AECA686D0");
        private static readonly Guid StandardId = new Guid("76E0AC21-842F-4D84-A69B-E2AA5E8DD6CF");
        private static readonly Guid OptionalId = new Guid("4210498F-7252-454C-A1D1-E61F5139D8DB");
        private static readonly Guid SelectableId = new Guid("FAD59954-725E-4E40-BCD5-66F1E30990BF");

        private UniquelyIdentifiableSticky<ProductFeatureApplicabilityKind> cache;

        public ProductFeatureApplicabilityKind Required => this.Cache[RequiredId];

        public ProductFeatureApplicabilityKind Standard => this.Cache[StandardId];

        public ProductFeatureApplicabilityKind Optional => this.Cache[OptionalId];

        public ProductFeatureApplicabilityKind Selectable => this.Cache[SelectableId];

        private UniquelyIdentifiableSticky<ProductFeatureApplicabilityKind> Cache => this.cache ??= new UniquelyIdentifiableSticky<ProductFeatureApplicabilityKind>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(RequiredId, v =>
            {
                v.Name = "Required";
                localisedName.Set(v, dutchLocale, "Verplicht");
                v.IsActive = true;
            });

            merge(RequiredId, v =>
            {
                v.Name = "Standard";
                localisedName.Set(v, dutchLocale, "Standaard");
                v.IsActive = true;
            });

            merge(OptionalId, v =>
            {
                v.Name = "Optional";
                localisedName.Set(v, dutchLocale, "Optioneel");
                v.IsActive = true;
            });

            merge(OptionalId, v =>
            {
                v.Name = "Selectable";
                localisedName.Set(v, dutchLocale, "Selecteerbaar");
                v.IsActive = true;
            });
        }
    }
}
