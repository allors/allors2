// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VatRateUsages.cs" company="Allors bvba">
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

    public partial class VatRateUsages
    {
        private static readonly Guid PurchaseId = new Guid("50C1F5FE-C569-4DA7-8451-F06F1C2ADDDE");
        private static readonly Guid SalesId = new Guid("719E799C-A890-459E-AB5C-407530EEC964");
        private static readonly Guid PurchaseAndSalesId = new Guid("1A28BE06-56E0-4ABA-81E9-04232DF4C909");

        private UniquelyIdentifiableSticky<VatRateUsage> cache;

        public VatRateUsage Purchase => this.Cache[PurchaseId];

        public VatRateUsage Sales => this.Cache[SalesId];

        public VatRateUsage PurchaseAndSales => this.Cache[PurchaseAndSalesId];

        private UniquelyIdentifiableSticky<VatRateUsage> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<VatRateUsage>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new VatRateUsageBuilder(this.Session)
                .WithName("Purchase")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Purchase").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("inkoop").WithLocale(dutchLocale).Build())
                .WithUniqueId(PurchaseId).Build();

            new VatRateUsageBuilder(this.Session)
                .WithName("Sales")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Sales").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verkoop").WithLocale(dutchLocale).Build())
                .WithUniqueId(SalesId).Build();

            new VatRateUsageBuilder(this.Session)
                .WithName("Purchase & sales")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Purchase & sales").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Inkoop & verkoop").WithLocale(dutchLocale).Build())
                .WithUniqueId(PurchaseAndSalesId).Build();
        }
    }
}