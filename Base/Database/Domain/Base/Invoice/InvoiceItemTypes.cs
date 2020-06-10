// <copyright file="InvoiceItemTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class InvoiceItemTypes
    {
        public static readonly Guid FeeId = new Guid("90E9010A-E040-484c-9644-735C750B5A7C");
        public static readonly Guid DiscountId = new Guid("29AF6097-A7ED-4916-94DC-686E7E55E31E");
        public static readonly Guid InterestChargeId = new Guid("9077D502-1823-4ac5-B8E9-A6F074186A3F");
        public static readonly Guid MiscChargeId = new Guid("2175B0D1-E2D3-4a83-8842-DDF02A7DF794");
        public static readonly Guid PromotionId = new Guid("2D2E81A5-E42F-497e-B446-3D448FF8FF75");
        public static readonly Guid ShippingAndHandlingId = new Guid("735349ED-14FF-4f57-8754-F053FD358B8A");
        public static readonly Guid SurchargeId = new Guid("7B5AD1AC-BC9F-46ea-8FD3-01A9624D7E13");
        public static readonly Guid WarrantyId = new Guid("5F5994E3-AF24-4cab-9F1E-5869556488E3");
        public static readonly Guid ProductFeatureItemId = new Guid("2C8742AA-B4CD-436b-9350-B4B7AD18E7AC");
        public static readonly Guid PartItemId = new Guid("FF2B943D-57C9-4311-9C56-9FF37959653B");
        public static readonly Guid ProductItemId = new Guid("0D07F778-2735-44cb-8354-FB887ADA42AD");
        public static readonly Guid TimeId = new Guid("DA178F93-234A-41ed-815C-819AF8CA4E6F");
        public static readonly Guid FreightChargeId = new Guid("199AEA42-FDC1-4C40-AF19-2255EBBD2729");
        public static readonly Guid WorkDoneId = new Guid("A4D2E6D0-C6C1-46EC-A1CF-3A64822E7A9E");
        public static readonly Guid OtherId = new Guid("8AB1F56A-B07E-4552-83A7-CA2DA2043740");

        private UniquelyIdentifiableSticky<InvoiceItemType> cache;

        public InvoiceItemType Fee => this.Cache[FeeId];

        public InvoiceItemType Discount => this.Cache[DiscountId];

        public InvoiceItemType InterestCharge => this.Cache[InterestChargeId];

        public InvoiceItemType MiscCharge => this.Cache[MiscChargeId];

        public InvoiceItemType Promotion => this.Cache[PromotionId];

        public InvoiceItemType ShippingAndHandling => this.Cache[ShippingAndHandlingId];

        public InvoiceItemType Surcharge => this.Cache[SurchargeId];

        public InvoiceItemType Warranty => this.Cache[WarrantyId];

        public InvoiceItemType ProductFeatureItem => this.Cache[ProductFeatureItemId];

        public InvoiceItemType PartItem => this.Cache[PartItemId];

        public InvoiceItemType ProductItem => this.Cache[ProductItemId];

        public InvoiceItemType Time => this.Cache[TimeId];

        public InvoiceItemType FreightCharge => this.Cache[FreightChargeId];

        public InvoiceItemType WorkDone => this.Cache[WorkDoneId];

        public InvoiceItemType Other => this.Cache[OtherId];

        private UniquelyIdentifiableSticky<InvoiceItemType> Cache => this.cache ??= new UniquelyIdentifiableSticky<InvoiceItemType>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(FeeId, v =>
            {
                v.Name = "Fee";
                localisedName.Set(v, dutchLocale, "Honorarium");
                v.IsActive = true;
                v.MaxQuantity = 1;
            });

            merge(DiscountId, v =>
            {
                v.Name = "Discount";
                localisedName.Set(v, dutchLocale, "Korting");
                v.IsActive = true;
                v.MaxQuantity = 1;
            });

            merge(InterestChargeId, v =>
            {
                v.Name = "Interest Charge";
                localisedName.Set(v, dutchLocale, "Interest toeslag");
                v.IsActive = true;
                v.MaxQuantity = 1;
            });

            merge(MiscChargeId, v =>
            {
                v.Name = "Miscellaneous Charge";
                localisedName.Set(v, dutchLocale, "Overige toeslag");
                v.IsActive = true;
                v.MaxQuantity = 1;
            });

            merge(PromotionId, v =>
            {
                v.Name = "Promotion";
                localisedName.Set(v, dutchLocale, "Verkoopbevordering");
                v.IsActive = true;
                v.MaxQuantity = 1;
            });

            merge(ShippingAndHandlingId, v =>
            {
                v.Name = "Shipping & Handling";
                localisedName.Set(v, dutchLocale, "Bezorgkosten");
                v.IsActive = true;
                v.MaxQuantity = 1;
            });

            merge(SurchargeId, v =>
            {
                v.Name = "Surcharge";
                localisedName.Set(v, dutchLocale, "Toeslag");
                v.IsActive = true;
                v.MaxQuantity = 1;
            });

            merge(WarrantyId, v =>
            {
                v.Name = "Warranty";
                localisedName.Set(v, dutchLocale, "Garantie");
                v.IsActive = true;
                v.MaxQuantity = 1;
            });

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

            merge(TimeId, v =>
            {
                v.Name = "Time";
                localisedName.Set(v, dutchLocale, "Tijd");
                v.IsActive = true;
                v.MaxQuantity = 1;
            });

            merge(FreightChargeId, v =>
            {
                v.Name = "Freight Charge";
                localisedName.Set(v, dutchLocale, "Vracht toeslag");
                v.IsActive = true;
                v.MaxQuantity = 1;
            });

            merge(WorkDoneId, v =>
            {
                v.Name = "Work Done";
                localisedName.Set(v, dutchLocale, "Uitgevoerde werkzaamheden");
                v.IsActive = true;
                v.MaxQuantity = 1;
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
