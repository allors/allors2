// <copyright file="InvoiceItemTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class InvoiceItemTypes
    {
        public static readonly Guid ProductFeatureItemId = new Guid("2C8742AA-B4CD-436b-9350-B4B7AD18E7AC");
        public static readonly Guid PartItemId = new Guid("FF2B943D-57C9-4311-9C56-9FF37959653B");
        public static readonly Guid ProductItemId = new Guid("0D07F778-2735-44cb-8354-FB887ADA42AD");
        public static readonly Guid ServiceId = new Guid("A4D2E6D0-C6C1-46EC-A1CF-3A64822E7A9E");
        public static readonly Guid TimeId = new Guid("DA178F93-234A-41ed-815C-819AF8CA4E6F");

        private UniquelyIdentifiableSticky<InvoiceItemType> cache;

        public InvoiceItemType ProductFeatureItem => this.Cache[ProductFeatureItemId];

        public InvoiceItemType PartItem => this.Cache[PartItemId];

        public InvoiceItemType ProductItem => this.Cache[ProductItemId];

        public InvoiceItemType Service => this.Cache[ServiceId];

        public InvoiceItemType Time => this.Cache[TimeId];

        private UniquelyIdentifiableSticky<InvoiceItemType> Cache => this.cache ??= new UniquelyIdentifiableSticky<InvoiceItemType>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(ProductFeatureItemId, v =>
            {
                v.Name = "Product Feature";
                localisedName.Set(v, dutchLocale, "Product onderdeel");
                v.IsActive = true;
            });

            merge(PartItemId, v =>
            {
                v.Name = "Part Item";
                localisedName.Set(v, dutchLocale, "Onderdeel");
                v.IsActive = true;
            });

            merge(ProductItemId, v =>
            {
                v.Name = "Product";
                localisedName.Set(v, dutchLocale, "Product");
                v.IsActive = true;
            });

            merge(ServiceId, v =>
            {
                v.Name = "Service";
                localisedName.Set(v, dutchLocale, "Service");
                v.IsActive = true;
                v.MaxQuantity = 1;
            });

            merge(TimeId, v =>
            {
                v.Name = "Time";
                localisedName.Set(v, dutchLocale, "Tijd");
                v.IsActive = true;
            });
        }
    }
}
