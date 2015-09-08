// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplatePurposes.cs" company="Allors bvba">
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

    public partial class TemplatePurposes
    {
        public static readonly Guid PurchaseInvoiceId = new Guid("2EBC99D1-C8C0-4a3b-971A-66076E51BF2E");
        public static readonly Guid SalesInvoiceId = new Guid("FC4B2E31-CF59-4b00-8B3B-52F1A69589DE");
        public static readonly Guid PurchaseOrderId = new Guid("7400FF89-0CA8-4ebb-8C91-B10A3F35E6C4");
        public static readonly Guid SalesOrderId = new Guid("879AB98A-E01E-4b08-A0CC-26754FAA0D4F");
        public static readonly Guid QuoteId = new Guid("526DBEB1-0EBA-4142-97EB-D1A47B3674D6");
        public static readonly Guid PickListId = new Guid("D46F2D64-637F-4b64-BE1C-81395DE1FAE5");
        public static readonly Guid PackagingSlipId = new Guid("B15C1EE6-588F-4E26-AE10-3B2CF6F5A57A");
        public static readonly Guid PurchaseShipmentId = new Guid("14FF3BCC-862E-440C-B4AC-FBCC538FB9FF");
        public static readonly Guid CustomerShipmentId = new Guid("6961C252-B2A7-4991-82DA-1EA2DE32B4C8");

        private UniquelyIdentifiableCache<TemplatePurpose> cache;

        public TemplatePurpose PurchaseInvoice
        {
            get { return this.Cache.Get(PurchaseInvoiceId); }
        }

        public TemplatePurpose SalesInvoice
        {
            get { return this.Cache.Get(SalesInvoiceId); }
        }

        public TemplatePurpose PurchaseOrder
        {
            get { return this.Cache.Get(PurchaseOrderId); }
        }

        public TemplatePurpose SalesOrder
        {
            get { return this.Cache.Get(SalesOrderId); }
        }

        public TemplatePurpose Quote
        {
            get { return this.Cache.Get(QuoteId); }
        }

        public TemplatePurpose PickList
        {
            get { return this.Cache.Get(PickListId); }
        }

        public TemplatePurpose PackagingSlip
        {
            get { return this.Cache.Get(PackagingSlipId); }
        }

        public TemplatePurpose PurchaseShipment
        {
            get { return this.Cache.Get(PurchaseShipmentId); }
        }

        public TemplatePurpose CustomerShipment
        {
            get { return this.Cache.Get(CustomerShipmentId); }
        }

        private UniquelyIdentifiableCache<TemplatePurpose> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<TemplatePurpose>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new TemplatePurposeBuilder(this.Session)
                .WithName("Purchase invoice")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Purchase invoice").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Inkoopfactuur").WithLocale(dutchLocale).Build())
                .WithUniqueId(PurchaseInvoiceId)
                .Build();

            new TemplatePurposeBuilder(this.Session)
                .WithName("Sales invoice")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Sales invoice").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verkoopfactuur").WithLocale(dutchLocale).Build())
                .WithUniqueId(SalesInvoiceId)
                .Build();
            
            new TemplatePurposeBuilder(this.Session)
                .WithName("Purchase order")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Purchase order").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Inkooporder").WithLocale(dutchLocale).Build())
                .WithUniqueId(PurchaseOrderId)
                .Build();
            
            new TemplatePurposeBuilder(this.Session)
                .WithName("Sales order")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Sales order").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verkooporder").WithLocale(dutchLocale).Build())
                .WithUniqueId(SalesOrderId)
                .Build();
            
            new TemplatePurposeBuilder(this.Session)
                .WithName("Quote")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Quote").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Prijsopgave").WithLocale(dutchLocale).Build())
                .WithUniqueId(QuoteId)
                .Build();
            
            new TemplatePurposeBuilder(this.Session)
                .WithName("Pick List")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Pick List").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verzamellijst").WithLocale(dutchLocale).Build())
                .WithUniqueId(PickListId)
                .Build();

            new TemplatePurposeBuilder(this.Session)
                .WithName("Packing list")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Packing list").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Paklijst").WithLocale(dutchLocale).Build())
                .WithUniqueId(PackagingSlipId)
                .Build();

            new TemplatePurposeBuilder(this.Session)
                .WithName("Customer Shipment")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Customer Shipment").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Klant zending").WithLocale(dutchLocale).Build())
                .WithUniqueId(CustomerShipmentId)
                .Build();

            new TemplatePurposeBuilder(this.Session)
                .WithName("Purchase Shipment")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Purchase Shipment").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Leverancier zending").WithLocale(dutchLocale).Build())
                .WithUniqueId(PurchaseShipmentId)
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
