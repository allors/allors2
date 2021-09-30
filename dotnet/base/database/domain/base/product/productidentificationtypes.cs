// <copyright file="ProductIdentificationTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class ProductIdentificationTypes
    {
        private static readonly Guid SkuId = new Guid("2BFB8F67-B2E1-4730-AC3D-3F2AF39B7EAF");
        private static readonly Guid IsbnId = new Guid("BA6A724F-7E90-4025-BED6-C10884735BAB");
        private static readonly Guid UpcaId = new Guid("27B32B96-0DCA-4414-AC09-1638B7CA4651");
        private static readonly Guid UpceId = new Guid("9AEE7C62-7237-473C-869F-4CB014C90529");
        private static readonly Guid EanId = new Guid("B2F15A78-0728-4041-86B2-6AC4C0FA9C7D");
        private static readonly Guid ManufacturerId = new Guid("3C349265-1794-4403-ADCF-C7D957527607");
        private static readonly Guid PartId = new Guid("5735191A-CDC4-4563-96EF-DDDC7B969CA6");
        private static readonly Guid GoodId = new Guid("B640630D-A556-4526-A2E5-60A84AB0DB3F");

        private UniquelyIdentifiableSticky<ProductIdentificationType> cache;

        public ProductIdentificationType Sku => this.Cache[SkuId];

        public ProductIdentificationType Isbn => this.Cache[IsbnId];

        public ProductIdentificationType Upca => this.Cache[UpcaId];

        public ProductIdentificationType Upce => this.Cache[UpceId];

        public ProductIdentificationType Ean => this.Cache[EanId];

        public ProductIdentificationType Manufacturer => this.Cache[ManufacturerId];

        public ProductIdentificationType Part => this.Cache[PartId];

        public ProductIdentificationType Good => this.Cache[GoodId];

        private UniquelyIdentifiableSticky<ProductIdentificationType> Cache => this.cache ??= new UniquelyIdentifiableSticky<ProductIdentificationType>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(SkuId, v =>
            {
                v.Name = "SKU";
                localisedName.Set(v, dutchLocale, "SKU");
                v.IsActive = true;
            });

            merge(IsbnId, v =>
            {
                v.Name = "ISBN";
                localisedName.Set(v, dutchLocale, "ISBN");
                v.IsActive = true;
            });

            merge(UpcaId, v =>
            {
                v.Name = "UPCA";
                localisedName.Set(v, dutchLocale, "UPCA");
                v.IsActive = true;
            });

            merge(UpceId, v =>
            {
                v.Name = "UPCE";
                localisedName.Set(v, dutchLocale, "UPCE");
                v.IsActive = true;
            });

            merge(EanId, v =>
            {
                v.Name = "EAN";
                localisedName.Set(v, dutchLocale, "EAN");
                v.IsActive = true;
            });

            merge(ManufacturerId, v =>
            {
                v.Name = "Manufacturer Id";
                localisedName.Set(v, dutchLocale, "Fabrikant Id");
                v.IsActive = true;
            });

            merge(GoodId, v =>
            {
                v.Name = "Product Id";
                localisedName.Set(v, dutchLocale, "Product Id");
                v.IsActive = true;
            });

            merge(PartId, v =>
            {
                v.Name = "Part Id";
                localisedName.Set(v, dutchLocale, "Part Id");
                v.IsActive = true;
            });
        }
    }
}
