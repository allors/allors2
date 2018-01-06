// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Ownerships.cs" company="Allors bvba">
//   Copyright 2002-201Scopes2 Allors bvba.
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

    public partial class Ownerships
    {
        private static readonly Guid OwnId = new Guid("74AA16DE-5719-4AE1-9547-0570E1111EDC");
        private static readonly Guid TradingId = new Guid("1E1BABFA-2F4F-45EF-BFC0-848E0199F4DF");

        private UniquelyIdentifiableSticky<Ownership> cache;

        public Ownership Own => this.Cache[OwnId];

        public Ownership Trading => this.Cache[TradingId];

        private UniquelyIdentifiableSticky<Ownership> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<Ownership>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new OwnershipBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Own").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Eigen").WithLocale(dutchLocale).Build())
                .WithUniqueId(OwnId)
                .Build();
            
            new OwnershipBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Trading").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Handel").WithLocale(dutchLocale).Build())
                .WithUniqueId(TradingId)
                .Build();
        }
    }
}
