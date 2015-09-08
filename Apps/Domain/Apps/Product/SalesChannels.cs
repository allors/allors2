// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesChannels.cs" company="Allors bvba">
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

    public partial class SalesChannels
    {
        public static readonly Guid NoChannelId = new Guid("C2F8E220-722E-4b6d-94A9-C02B2ABB2ABE");
        public static readonly Guid WebChannelId = new Guid("AE821BB6-7F54-4d98-930C-9D2B144FB662");
        public static readonly Guid PosChannelId = new Guid("6E35BE16-C0C7-479d-9E87-D382C0E34FDC");
        public static readonly Guid FaxChannelId = new Guid("06B1A63D-AC2C-434d-947E-465058E6C2BB");
        public static readonly Guid PhoneChannelId = new Guid("04F7E8BE-CAB5-481a-8250-1C126B06D5E0");
        public static readonly Guid EmailChannelId = new Guid("88E3D10B-CF0E-439a-8D75-6B68909E6D8E");
        public static readonly Guid SnailMailChannelId = new Guid("7EFBA4C0-A853-4c61-8862-B606EF5C1B63");
        public static readonly Guid AffiliateChannelId = new Guid("0FC9C19F-2005-4d7b-80B3-C7C862048CFA");
        public static readonly Guid EbayChannelId = new Guid("93FFD696-F11F-4cc1-A461-367AFCFD0579");

        private UniquelyIdentifiableCache<SalesChannel> cache;

        public SalesChannel NoChannel
        {
            get { return this.Cache.Get(NoChannelId); }
        }

        public SalesChannel WebChannel
        {
            get { return this.Cache.Get(WebChannelId); }
        }

        public SalesChannel PosChannel
        {
            get { return this.Cache.Get(PosChannelId); }
        }

        public SalesChannel FaxChannel
        {
            get { return this.Cache.Get(FaxChannelId); }
        }

        public SalesChannel PhoneChannel
        {
            get { return this.Cache.Get(PhoneChannelId); }
        }

        public SalesChannel EmailChannel
        {
            get { return this.Cache.Get(EmailChannelId); }
        }

        public SalesChannel SnailMailChannel
        {
            get { return this.Cache.Get(SnailMailChannelId); }
        }

        public SalesChannel AffiliateChannel
        {
            get { return this.Cache.Get(AffiliateChannelId); }
        }

        public SalesChannel EbayChannel
        {
            get { return this.Cache.Get(EbayChannelId); }
        }

        private UniquelyIdentifiableCache<SalesChannel> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<SalesChannel>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new SalesChannelBuilder(this.Session)
                .WithName("No Channel")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("No Channel").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Geen verkoopkanaal").WithLocale(dutchLocale).Build())
                .WithUniqueId(NoChannelId)
                .Build();
            
            new SalesChannelBuilder(this.Session)
                .WithName("via Web")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("via Web").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("via Web").WithLocale(dutchLocale).Build())
                .WithUniqueId(WebChannelId)
                .Build();
            
            new SalesChannelBuilder(this.Session)
                .WithName("POS Channel")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("POS Channel").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Kassa Kanaal").WithLocale(dutchLocale).Build())
                .WithUniqueId(PosChannelId)
                .Build();
            
            new SalesChannelBuilder(this.Session)
                .WithName("by Fax")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("by Fax").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("via Fax").WithLocale(dutchLocale).Build())
                .WithUniqueId(FaxChannelId)
                .Build();
            
            new SalesChannelBuilder(this.Session)
                .WithName("by Phone")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("by Phone").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Telefonisch").WithLocale(dutchLocale).Build())
                .WithUniqueId(PhoneChannelId)
                .Build();
            
            new SalesChannelBuilder(this.Session)
                .WithName("via E-Mail")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("via E-Mail").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("via E-Mail").WithLocale(dutchLocale).Build())
                .WithUniqueId(EmailChannelId)
                .Build();
            
            new SalesChannelBuilder(this.Session)
                .WithName("Snail mail Channel")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Snail mail Channel").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("via Post").WithLocale(dutchLocale).Build())
                .WithUniqueId(SnailMailChannelId)
                .Build();
            
            new SalesChannelBuilder(this.Session)
                .WithName("Affiliate Channel")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Affiliate Channel").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Affiliate Kanaal").WithLocale(dutchLocale).Build())
                .WithUniqueId(AffiliateChannelId)
                .Build();
            
            new SalesChannelBuilder(this.Session)
                .WithName("via eBay")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("via eBay").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("via eBay").WithLocale(dutchLocale).Build())
                .WithUniqueId(EbayChannelId)
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
