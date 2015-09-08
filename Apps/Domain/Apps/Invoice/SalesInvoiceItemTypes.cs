// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoiceItemTypes.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    public partial class SalesInvoiceItemTypes
    {
        public static readonly Guid FeeId = new Guid("90E9010A-E040-484c-9644-735C750B5A7C");
        public static readonly Guid DiscountId = new Guid("29AF6097-A7ED-4916-94DC-686E7E55E31E");
        public static readonly Guid InterestChargeId = new Guid("9077D502-1823-4ac5-B8E9-A6F074186A3F");
        public static readonly Guid MiscChargeId = new Guid("2175B0D1-E2D3-4a83-8842-DDF02A7DF794");
        public static readonly Guid PromotionId = new Guid("2D2E81A5-E42F-497e-B446-3D448FF8FF75");
        public static readonly Guid VatId = new Guid("2DB43A22-6FED-4e85-843F-377157E9D72B");
        public static readonly Guid ShippingAndHandlingId = new Guid("735349ED-14FF-4f57-8754-F053FD358B8A");
        public static readonly Guid SurchargeId = new Guid("7B5AD1AC-BC9F-46ea-8FD3-01A9624D7E13");
        public static readonly Guid WarrantyId = new Guid("5F5994E3-AF24-4cab-9F1E-5869556488E3");
        public static readonly Guid ProductFeatureItemId = new Guid("2C8742AA-B4CD-436b-9350-B4B7AD18E7AC");
        public static readonly Guid ProductItemId = new Guid("0D07F778-2735-44cb-8354-FB887ADA42AD");
        public static readonly Guid ServiceProductItemId = new Guid("DA178F93-234A-41ed-815C-819AF8CA4E6F");

        private UniquelyIdentifiableCache<SalesInvoiceItemType> cache;

        public SalesInvoiceItemType Fee
        {
            get { return this.Cache.Get(FeeId); }
        }

        public SalesInvoiceItemType Discount
        {
            get { return this.Cache.Get(DiscountId); }
        }

        public SalesInvoiceItemType InterestCharge
        {
            get { return this.Cache.Get(InterestChargeId); }
        }

        public SalesInvoiceItemType MiscCharge
        {
            get { return this.Cache.Get(MiscChargeId); }
        }

        public SalesInvoiceItemType Promotion
        {
            get { return this.Cache.Get(PromotionId); }
        }

        public SalesInvoiceItemType SalesTax
        {
            get { return this.Cache.Get(VatId); }
        }

        public SalesInvoiceItemType ShippingAndHandling
        {
            get { return this.Cache.Get(ShippingAndHandlingId); }
        }

        public SalesInvoiceItemType Surcharge
        {
            get { return this.Cache.Get(SurchargeId); }
        }

        public SalesInvoiceItemType Warranty
        {
            get { return this.Cache.Get(WarrantyId); }
        }

        public SalesInvoiceItemType ProductFeatureItem
        {
            get { return this.Cache.Get(ProductFeatureItemId); }
        }

        public SalesInvoiceItemType ProductItem
        {
            get { return this.Cache.Get(ProductItemId); }
        }

        public SalesInvoiceItemType ServiceProductItem
        {
            get { return this.Cache.Get(ServiceProductItemId); }
        }

        private UniquelyIdentifiableCache<SalesInvoiceItemType> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<SalesInvoiceItemType>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithName("Fee")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Fee").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Honorarium").WithLocale(dutchLocale).Build())
                .WithUniqueId(FeeId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithName("Discount")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Discount").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Korting").WithLocale(dutchLocale).Build())
                .WithUniqueId(DiscountId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithName("Interest Charge")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Interest Charge").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Interest toeslag").WithLocale(dutchLocale).Build())
                .WithUniqueId(InterestChargeId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithName("Miscellaneous Charge")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Miscellaneous Charge").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Overige toeslag").WithLocale(dutchLocale).Build())
                .WithUniqueId(MiscChargeId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithName("Promotion")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Promotion").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verkoopbevordering ").WithLocale(dutchLocale).Build())
                .WithUniqueId(PromotionId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithName("VAT")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("VAT").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("BTW").WithLocale(dutchLocale).Build())
                .WithUniqueId(VatId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithName("Shipping & Handling")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Shipping & Handling").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Bezorgkosten").WithLocale(dutchLocale).Build())
                .WithUniqueId(ShippingAndHandlingId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithName("Surcharge")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Surcharge").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Toeslag").WithLocale(dutchLocale).Build())
                .WithUniqueId(SurchargeId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithName("Warranty")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Warranty").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Garantie").WithLocale(dutchLocale).Build())
                .WithUniqueId(WarrantyId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithName("Productfeature Item")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Product Item").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Product onderdeel").WithLocale(dutchLocale).Build())
                .WithUniqueId(ProductFeatureItemId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithName("Product Feature Item")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Product Feature Item").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Product kenmerk onderdeel").WithLocale(dutchLocale).Build())
                .WithUniqueId(ProductItemId)
                .Build();
            
            new SalesInvoiceItemTypeBuilder(this.Session)
                .WithName("Service Product Item")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Service Product Item").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Dienst onderdeel").WithLocale(dutchLocale).Build())
                .WithUniqueId(ServiceProductItemId)
                .Build();
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}
