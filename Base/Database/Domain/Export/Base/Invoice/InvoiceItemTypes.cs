// <copyright file="InvoiceItemTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class InvoiceItemTypes
    {
        internal static readonly Guid FeeId = new Guid("90E9010A-E040-484c-9644-735C750B5A7C");
        internal static readonly Guid DiscountId = new Guid("29AF6097-A7ED-4916-94DC-686E7E55E31E");
        internal static readonly Guid InterestChargeId = new Guid("9077D502-1823-4ac5-B8E9-A6F074186A3F");
        internal static readonly Guid MiscChargeId = new Guid("2175B0D1-E2D3-4a83-8842-DDF02A7DF794");
        internal static readonly Guid PromotionId = new Guid("2D2E81A5-E42F-497e-B446-3D448FF8FF75");
        internal static readonly Guid ShippingAndHandlingId = new Guid("735349ED-14FF-4f57-8754-F053FD358B8A");
        internal static readonly Guid SurchargeId = new Guid("7B5AD1AC-BC9F-46ea-8FD3-01A9624D7E13");
        internal static readonly Guid WarrantyId = new Guid("5F5994E3-AF24-4cab-9F1E-5869556488E3");
        internal static readonly Guid ProductFeatureItemId = new Guid("2C8742AA-B4CD-436b-9350-B4B7AD18E7AC");
        internal static readonly Guid PartItemId = new Guid("FF2B943D-57C9-4311-9C56-9FF37959653B");
        internal static readonly Guid ProductItemId = new Guid("0D07F778-2735-44cb-8354-FB887ADA42AD");
        internal static readonly Guid TimeId = new Guid("DA178F93-234A-41ed-815C-819AF8CA4E6F");
        internal static readonly Guid FreightChargeId = new Guid("199AEA42-FDC1-4C40-AF19-2255EBBD2729");
        internal static readonly Guid WorkDoneId = new Guid("A4D2E6D0-C6C1-46EC-A1CF-3A64822E7A9E");

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

        private UniquelyIdentifiableSticky<InvoiceItemType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<InvoiceItemType>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new InvoiceItemTypeBuilder(this.Session)
                .WithName("Fee")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Honorarium").WithLocale(dutchLocale).Build())
                .WithUniqueId(FeeId)
                .WithIsActive(true)
                .Build();

            new InvoiceItemTypeBuilder(this.Session)
                .WithName("Discount")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Korting").WithLocale(dutchLocale).Build())
                .WithUniqueId(DiscountId)
                .WithIsActive(true)
                .Build();

            new InvoiceItemTypeBuilder(this.Session)
                .WithName("Interest Charge")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Interest toeslag").WithLocale(dutchLocale).Build())
                .WithUniqueId(InterestChargeId)
                .WithIsActive(true)
                .Build();

            new InvoiceItemTypeBuilder(this.Session)
                .WithName("Miscellaneous Charge")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Overige toeslag").WithLocale(dutchLocale).Build())
                .WithUniqueId(MiscChargeId)
                .WithIsActive(true)
                .Build();

            new InvoiceItemTypeBuilder(this.Session)
                .WithName("Promotion")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verkoopbevordering ").WithLocale(dutchLocale).Build())
                .WithUniqueId(PromotionId)
                .WithIsActive(true)
                .Build();

            new InvoiceItemTypeBuilder(this.Session)
                .WithName("Shipping & Handling")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Bezorgkosten").WithLocale(dutchLocale).Build())
                .WithUniqueId(ShippingAndHandlingId)
                .WithIsActive(true)
                .Build();

            new InvoiceItemTypeBuilder(this.Session)
                .WithName("Surcharge")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Toeslag").WithLocale(dutchLocale).Build())
                .WithUniqueId(SurchargeId)
                .WithIsActive(true)
                .Build();

            new InvoiceItemTypeBuilder(this.Session)
                .WithName("Warranty")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Garantie").WithLocale(dutchLocale).Build())
                .WithUniqueId(WarrantyId)
                .WithIsActive(true)
                .Build();

            new InvoiceItemTypeBuilder(this.Session)
                .WithName("Product Feature")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Product onderdeel").WithLocale(dutchLocale).Build())
                .WithUniqueId(ProductFeatureItemId)
                .WithIsActive(true)
                .Build();

            new InvoiceItemTypeBuilder(this.Session)
                .WithName("Part Item")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Onderdeel item").WithLocale(dutchLocale).Build())
                .WithUniqueId(PartItemId)
                .WithIsActive(true)
                .Build();

            new InvoiceItemTypeBuilder(this.Session)
                .WithName("Product")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Product").WithLocale(dutchLocale).Build())
                .WithUniqueId(ProductItemId)
                .WithIsActive(true)
                .Build();

            new InvoiceItemTypeBuilder(this.Session)
                .WithName("Time")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Tijd").WithLocale(dutchLocale).Build())
                .WithUniqueId(TimeId)
                .WithIsActive(true)
                .Build();

            new InvoiceItemTypeBuilder(this.Session)
                .WithName("Freight Charge")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vracht toeslag").WithLocale(dutchLocale).Build())
                .WithUniqueId(FreightChargeId)
                .WithIsActive(true)
                .Build();

            new InvoiceItemTypeBuilder(this.Session)
                .WithName("Work Done")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Uitgevoerde werkzaamheden").WithLocale(dutchLocale).Build())
                .WithUniqueId(WorkDoneId)
                .WithIsActive(true)
                .Build();
        }
    }
}
