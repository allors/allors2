// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VatTariffs.cs" company="Allors bvba">
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

    public partial class VatTariffs
    {
        private static readonly Guid StandardId = new Guid("2942BF39-0B73-4015-8815-E6279ED0381F");
        private static readonly Guid ReducedRateId = new Guid("7F5A5FE2-BB18-4644-B1CA-81B391AB82D6");
        private static readonly Guid ZeroRateId = new Guid("70C228CA-AE50-4DA2-B209-D115BEE7F4FF");

        private UniquelyIdentifiableSticky<VatTariff> cache;

        public VatTariff Standard => this.Cache[StandardId];

        public VatTariff ReducedRate => this.Cache[ReducedRateId];

        public VatTariff ZeroRate => this.Cache[ZeroRateId];

        private UniquelyIdentifiableSticky<VatTariff> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<VatTariff>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new VatTariffBuilder(this.Session)
                .WithName("Standard")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Hoog").WithLocale(dutchLocale).Build())
                .WithUniqueId(StandardId).Build();

            new VatTariffBuilder(this.Session)
                .WithName("Reduced rate")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Laag").WithLocale(dutchLocale).Build())
                .WithUniqueId(ReducedRateId).Build();

            new VatTariffBuilder(this.Session)
                .WithName("Zero rate")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("nul tarief").WithLocale(dutchLocale).Build())
                .WithUniqueId(ZeroRateId).Build();
        }
    }
}