// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseInvoiceItemTypes.cs" company="Allors bvba">
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

    public partial class PurchaseInvoiceItemTypes
    {
        private static readonly Guid FeeId = new Guid("AE5BE38C-86DA-44f1-A236-8AF976956A12");
        private static readonly Guid DiscountId = new Guid("3F689042-E6BE-4f7c-9AA7-511DD8CEE9CB");
        private static readonly Guid InterestChargeId = new Guid("3C79AC7E-A982-429d-AA63-82A639119532");
        private static readonly Guid MiscChargeId = new Guid("CC5C8729-96C0-44ec-A15C-A3A9182A5A99");
        private static readonly Guid PromotionId = new Guid("C17EB73B-EA29-4cea-9DBD-B52E50BBEF3D");
        private static readonly Guid VatId = new Guid("1C72D31E-4C97-4fab-9A8A-42068B00F986");
        private static readonly Guid ShippingAndHandlingId = new Guid("8235C63F-42DE-4eaa-ACE5-2AF0FB3AB973");
        private static readonly Guid SurchargeId = new Guid("674742D1-A9A4-46e6-93CE-D8AEA9089C8B");
        private static readonly Guid WarrantyId = new Guid("CC0B0A30-5E01-4146-B80D-6BB5F91574BE");
        private static readonly Guid PartItemId = new Guid("314FA35E-C015-4084-A54F-644EB8738E31");

        private UniquelyIdentifiableSticky<PurchaseInvoiceItemType> cache;

        public PurchaseInvoiceItemType Fee => this.Cache[FeeId];

        public PurchaseInvoiceItemType Discount => this.Cache[DiscountId];

        public PurchaseInvoiceItemType InterestCharge => this.Cache[InterestChargeId];

        public PurchaseInvoiceItemType MiscCharge => this.Cache[MiscChargeId];

        public PurchaseInvoiceItemType Promotion => this.Cache[PromotionId];

        public PurchaseInvoiceItemType SalesTax => this.Cache[VatId];

        public PurchaseInvoiceItemType ShippingAndHandling => this.Cache[ShippingAndHandlingId];

        public PurchaseInvoiceItemType Surcharge => this.Cache[SurchargeId];

        public PurchaseInvoiceItemType Warranty => this.Cache[WarrantyId];

        public PurchaseInvoiceItemType PartItem => this.Cache[PartItemId];

        private UniquelyIdentifiableSticky<PurchaseInvoiceItemType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<PurchaseInvoiceItemType>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new PurchaseInvoiceItemTypeBuilder(this.Session)
                .WithName("Fee")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Fee").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Honorarium").WithLocale(dutchLocale).Build())
                .WithUniqueId(FeeId)
                .Build();
            
            new PurchaseInvoiceItemTypeBuilder(this.Session)
                .WithName("Discount")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Discount").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Korting").WithLocale(dutchLocale).Build())
                .WithUniqueId(DiscountId)
                .Build();
            
            new PurchaseInvoiceItemTypeBuilder(this.Session)
                .WithName("Interest Charge")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Interest Charge").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Interest toeslag").WithLocale(dutchLocale).Build())
                .WithUniqueId(InterestChargeId)
                .Build();
            
            new PurchaseInvoiceItemTypeBuilder(this.Session)
                .WithName("Miscellaneous Charge")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Miscellaneous Charge").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Overige toeslag").WithLocale(dutchLocale).Build())
                .WithUniqueId(MiscChargeId)
                .Build();
            
            new PurchaseInvoiceItemTypeBuilder(this.Session)
                .WithName("Promotion")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Promotion").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verkoopbevordering ").WithLocale(dutchLocale).Build())
                .WithUniqueId(PromotionId)
                .Build();
            
            new PurchaseInvoiceItemTypeBuilder(this.Session)
                .WithName("VAT")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("VAT").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("BTW").WithLocale(dutchLocale).Build())
                .WithUniqueId(VatId)
                .Build();
            
            new PurchaseInvoiceItemTypeBuilder(this.Session)
                .WithName("Shipping & Handling")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Shipping & Handling").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Bezorgkosten").WithLocale(dutchLocale).Build())
                .WithUniqueId(ShippingAndHandlingId)
                .Build();
            
            new PurchaseInvoiceItemTypeBuilder(this.Session)
                .WithName("Surcharge")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Surcharge").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Toeslag").WithLocale(dutchLocale).Build())
                .WithUniqueId(SurchargeId)
                .Build();
            
            new PurchaseInvoiceItemTypeBuilder(this.Session)
                .WithName("Warranty")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Warranty").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Garantie").WithLocale(dutchLocale).Build())
                .WithUniqueId(WarrantyId)
                .Build();
            
            new PurchaseInvoiceItemTypeBuilder(this.Session)
                .WithName("Part Item")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Part Item").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Onderdeel item").WithLocale(dutchLocale).Build())
                .WithUniqueId(PartItemId)
                .Build();
        }
    }
}
