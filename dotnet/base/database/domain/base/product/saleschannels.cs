// <copyright file="SalesChannels.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SalesChannels
    {
        private static readonly Guid NoChannelId = new Guid("C2F8E220-722E-4b6d-94A9-C02B2ABB2ABE");
        private static readonly Guid WebChannelId = new Guid("AE821BB6-7F54-4d98-930C-9D2B144FB662");
        private static readonly Guid PosChannelId = new Guid("6E35BE16-C0C7-479d-9E87-D382C0E34FDC");
        private static readonly Guid FaxChannelId = new Guid("06B1A63D-AC2C-434d-947E-465058E6C2BB");
        private static readonly Guid PhoneChannelId = new Guid("04F7E8BE-CAB5-481a-8250-1C126B06D5E0");
        private static readonly Guid EmailChannelId = new Guid("88E3D10B-CF0E-439a-8D75-6B68909E6D8E");
        private static readonly Guid MailChannelId = new Guid("7EFBA4C0-A853-4c61-8862-B606EF5C1B63");
        private static readonly Guid AffiliateChannelId = new Guid("0FC9C19F-2005-4d7b-80B3-C7C862048CFA");
        private static readonly Guid EbayChannelId = new Guid("93FFD696-F11F-4cc1-A461-367AFCFD0579");

        private UniquelyIdentifiableSticky<SalesChannel> cache;

        public SalesChannel NoChannel => this.Cache[NoChannelId];

        public SalesChannel WebChannel => this.Cache[WebChannelId];

        public SalesChannel PosChannel => this.Cache[PosChannelId];

        public SalesChannel FaxChannel => this.Cache[FaxChannelId];

        public SalesChannel PhoneChannel => this.Cache[PhoneChannelId];

        public SalesChannel EmailChannel => this.Cache[EmailChannelId];

        public SalesChannel MailChannel => this.Cache[MailChannelId];

        public SalesChannel AffiliateChannel => this.Cache[AffiliateChannelId];

        public SalesChannel EbayChannel => this.Cache[EbayChannelId];

        private UniquelyIdentifiableSticky<SalesChannel> Cache => this.cache ??= new UniquelyIdentifiableSticky<SalesChannel>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(NoChannelId, v =>
            {
                v.Name = "No Channel";
                localisedName.Set(v, dutchLocale, "Geen verkoopkanaal");
                v.IsActive = true;
            });

            merge(WebChannelId, v =>
            {
                v.Name = "via Web";
                localisedName.Set(v, dutchLocale, "via Web");
                v.IsActive = true;
            });

            merge(PosChannelId, v =>
            {
                v.Name = "POS Channel";
                localisedName.Set(v, dutchLocale, "Kassa Kanaal");
                v.IsActive = true;
            });

            merge(PosChannelId, v =>
            {
                v.Name = "POS Channel";
                localisedName.Set(v, dutchLocale, "Kassa Kanaal");
                v.IsActive = true;
            });

            merge(FaxChannelId, v =>
            {
                v.Name = "by Fax";
                localisedName.Set(v, dutchLocale, "Kassa Kanaal");
                v.IsActive = true;
            });

            merge(PhoneChannelId, v =>
            {
                v.Name = "by Phone";
                localisedName.Set(v, dutchLocale, "Telefonisch");
                v.IsActive = true;
            });

            merge(EmailChannelId, v =>
            {
                v.Name = "via E-Mail";
                localisedName.Set(v, dutchLocale, "via E-Mail");
                v.IsActive = true;
            });

            merge(MailChannelId, v =>
            {
                v.Name = "by Mail";
                localisedName.Set(v, dutchLocale, "via Post");
                v.IsActive = true;
            });

            merge(AffiliateChannelId, v =>
            {
                v.Name = "Affiliate Channel";
                localisedName.Set(v, dutchLocale, "Affiliate Kanaal");
                v.IsActive = true;
            });

            merge(EbayChannelId, v =>
            {
                v.Name = "via eBay";
                localisedName.Set(v, dutchLocale, "via eBay");
                v.IsActive = true;
            });
        }
    }
}
