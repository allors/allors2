// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BillingProcesses.cs" company="Allors bvba">
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

    public partial class BillingProcesses
    {
        private static readonly Guid BillingForOrderItemsId = new Guid("AB01CCC2-6480-4FC0-B20E-265AFD41FAE2");
        private static readonly Guid BillingForShipmentItemsId = new Guid("E242D221-7DD6-4BD8-9A4A-E0582EEBECB0");
        private static readonly Guid BillingForWorkEffortsId = new Guid("580A72F2-E428-43C1-8F3E-2A05B7C61712");
        private static readonly Guid BillingForTimeEntriesId = new Guid("0E480590-CA53-40C6-86A8-92871E18FB38");

        private UniquelyIdentifiableSticky<BillingProcess> cache;

        public BillingProcess BillingForOrderItems => this.Cache[BillingForOrderItemsId];

        public BillingProcess BillingForShipmentItems => this.Cache[BillingForShipmentItemsId];

        public BillingProcess BillingForWorkEfforts => this.Cache[BillingForWorkEffortsId];

        public BillingProcess BillingForTimeEntries => this.Cache[BillingForTimeEntriesId];

        private UniquelyIdentifiableSticky<BillingProcess> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<BillingProcess>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new BillingProcessBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Billing for Order Items").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Billing for Order Items").WithLocale(dutchLocale).Build())
                .WithUniqueId(BillingForOrderItemsId)
                .Build();

            new BillingProcessBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Billing for Shipment Items").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Billing for Shipment Items").WithLocale(dutchLocale).Build())
                .WithUniqueId(BillingForShipmentItemsId)
                .Build();

            new BillingProcessBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Billing for Work Efforts").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Billing for Shipment Items").WithLocale(dutchLocale).Build())
                .WithUniqueId(BillingForWorkEffortsId)
                .Build();

            new BillingProcessBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Billing for Time Entries").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Billing for Shipment Items").WithLocale(dutchLocale).Build())
                .WithUniqueId(BillingForTimeEntriesId)
                .Build();
        }
    }
}
