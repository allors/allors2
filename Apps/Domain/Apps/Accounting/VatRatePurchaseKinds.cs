// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VatRatePurchaseKinds.cs" company="Allors bvba">
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

    public partial class VatRatePurchaseKinds
    {
        private static readonly Guid GoodsId = new Guid("6C38B839-DD14-4C79-BD18-6293EB5E87A3");
        private static readonly Guid ServicesId = new Guid("6B7250E4-F1DA-4BA2-ACB6-CAB351839E9F");
        private static readonly Guid InvestmentsId = new Guid("F317A718-6197-47C6-B231-138EF9C26814");

        private UniquelyIdentifiableCache<VatRatePurchaseKind> cache;

        public VatRatePurchaseKind Goods => this.Cache.Get(GoodsId);

        public VatRatePurchaseKind Services => this.Cache.Get(ServicesId);

        public VatRatePurchaseKind Investments => this.Cache.Get(InvestmentsId);

        private UniquelyIdentifiableCache<VatRatePurchaseKind> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableCache<VatRatePurchaseKind>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new VatRatePurchaseKindBuilder(this.Session)
                .WithName("Goods")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Goods").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Goederen").WithLocale(dutchLocale).Build())
                .WithUniqueId(GoodsId).Build();

            new VatRatePurchaseKindBuilder(this.Session)
                .WithName("Services")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Services").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Diensten").WithLocale(dutchLocale).Build())
                .WithUniqueId(ServicesId).Build();

            new VatRatePurchaseKindBuilder(this.Session)
                .WithName("Investments")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Investments").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Investeringen").WithLocale(dutchLocale).Build())
                .WithUniqueId(InvestmentsId).Build();
        }
    }
}