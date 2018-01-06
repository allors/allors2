// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoiceItemTypes.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using System;

    public partial class SalesInvoiceItemTypes
    {
        private static readonly Guid FeeId = new Guid("90E9010A-E040-484c-9644-735C750B5A7C");
        private static readonly Guid DiscountId = new Guid("29AF6097-A7ED-4916-94DC-686E7E55E31E");
        private static readonly Guid InterestChargeId = new Guid("9077D502-1823-4ac5-B8E9-A6F074186A3F");
        private static readonly Guid MiscChargeId = new Guid("2175B0D1-E2D3-4a83-8842-DDF02A7DF794");
        private static readonly Guid PromotionId = new Guid("2D2E81A5-E42F-497e-B446-3D448FF8FF75");
        private static readonly Guid VatId = new Guid("2DB43A22-6FED-4e85-843F-377157E9D72B");
        private static readonly Guid ShippingAndHandlingId = new Guid("735349ED-14FF-4f57-8754-F053FD358B8A");
        private static readonly Guid SurchargeId = new Guid("7B5AD1AC-BC9F-46ea-8FD3-01A9624D7E13");
        private static readonly Guid WarrantyId = new Guid("5F5994E3-AF24-4cab-9F1E-5869556488E3");
        private static readonly Guid ProductFeatureItemId = new Guid("2C8742AA-B4CD-436b-9350-B4B7AD18E7AC");
        private static readonly Guid ProductItemId = new Guid("0D07F778-2735-44cb-8354-FB887ADA42AD");
        private static readonly Guid ServiceProductItemId = new Guid("DA178F93-234A-41ed-815C-819AF8CA4E6F");

        private UniquelyIdentifiableSticky<SalesInvoiceItemType> cache;

        public SalesInvoiceItemType Fee => this.Cache[FeeId];

        public SalesInvoiceItemType Discount => this.Cache[DiscountId];

        public SalesInvoiceItemType InterestCharge => this.Cache[InterestChargeId];

        public SalesInvoiceItemType MiscCharge => this.Cache[MiscChargeId];

        public SalesInvoiceItemType Promotion => this.Cache[PromotionId];

        public SalesInvoiceItemType SalesTax => this.Cache[VatId];

        public SalesInvoiceItemType ShippingAndHandling => this.Cache[ShippingAndHandlingId];

        public SalesInvoiceItemType Surcharge => this.Cache[SurchargeId];

        public SalesInvoiceItemType Warranty => this.Cache[WarrantyId];

        public SalesInvoiceItemType ProductFeatureItem => this.Cache[ProductFeatureItemId];

        public SalesInvoiceItemType ProductItem => this.Cache[ProductItemId];

        public SalesInvoiceItemType ServiceProductItem => this.Cache[ServiceProductItemId];

        private UniquelyIdentifiableSticky<SalesInvoiceItemType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<SalesInvoiceItemType>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Fee").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Honorarium").WithLocale(dutchLocale).Build())
                .WithUniqueId(FeeId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Discount").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Korting").WithLocale(dutchLocale).Build())
                .WithUniqueId(DiscountId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Interest Charge").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Interest toeslag").WithLocale(dutchLocale).Build())
                .WithUniqueId(InterestChargeId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Miscellaneous Charge").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Overige toeslag").WithLocale(dutchLocale).Build())
                .WithUniqueId(MiscChargeId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Promotion").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verkoopbevordering ").WithLocale(dutchLocale).Build())
                .WithUniqueId(PromotionId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("VAT").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("BTW").WithLocale(dutchLocale).Build())
                .WithUniqueId(VatId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Shipping & Handling").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Bezorgkosten").WithLocale(dutchLocale).Build())
                .WithUniqueId(ShippingAndHandlingId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Surcharge").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Toeslag").WithLocale(dutchLocale).Build())
                .WithUniqueId(SurchargeId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Warranty").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Garantie").WithLocale(dutchLocale).Build())
                .WithUniqueId(WarrantyId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Product Feature").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Product onderdeel").WithLocale(dutchLocale).Build())
                .WithUniqueId(ProductFeatureItemId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Product").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Product").WithLocale(dutchLocale).Build())
                .WithUniqueId(ProductItemId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Service Product Item").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Dienst onderdeel").WithLocale(dutchLocale).Build())
                .WithUniqueId(ServiceProductItemId)
                .Build();
        }
    }
}
