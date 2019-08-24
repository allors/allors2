// <copyright file="VatRatePurchaseKinds.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class VatRatePurchaseKinds
    {
        private static readonly Guid GoodsId = new Guid("6C38B839-DD14-4C79-BD18-6293EB5E87A3");
        private static readonly Guid ServicesId = new Guid("6B7250E4-F1DA-4BA2-ACB6-CAB351839E9F");
        private static readonly Guid InvestmentsId = new Guid("F317A718-6197-47C6-B231-138EF9C26814");

        private UniquelyIdentifiableSticky<VatRatePurchaseKind> cache;

        public VatRatePurchaseKind Goods => this.Cache[GoodsId];

        public VatRatePurchaseKind Services => this.Cache[ServicesId];

        public VatRatePurchaseKind Investments => this.Cache[InvestmentsId];

        private UniquelyIdentifiableSticky<VatRatePurchaseKind> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<VatRatePurchaseKind>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new VatRatePurchaseKindBuilder(this.Session)
                .WithName("Goods")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Goederen").WithLocale(dutchLocale).Build())
                .WithUniqueId(GoodsId)
                .WithIsActive(true)
                .Build();

            new VatRatePurchaseKindBuilder(this.Session)
                .WithName("Services")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Diensten").WithLocale(dutchLocale).Build())
                .WithUniqueId(ServicesId)
                .WithIsActive(true)
                .Build();

            new VatRatePurchaseKindBuilder(this.Session)
                .WithName("Investments")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Investeringen").WithLocale(dutchLocale).Build())
                .WithUniqueId(InvestmentsId).WithIsActive(true)
                .Build();
        }
    }
}
