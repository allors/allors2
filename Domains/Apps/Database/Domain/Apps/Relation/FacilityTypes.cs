// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FacilityTypes.cs" company="Allors bvba">
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

    public partial class FacilityTypes
    {
        private static readonly Guid WarehouseId = new Guid("56AD0A65-1FC0-40EA-BDA8-DADDFA6CBE63");

        private UniquelyIdentifiableCache<FacilityType> cache;

        public FacilityType Warehouse => this.Cache.Get(WarehouseId);

        private UniquelyIdentifiableCache<FacilityType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableCache<FacilityType>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new FacilityTypeBuilder(this.Session)
                .WithName("Employee")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Warehouse").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Magazijn").WithLocale(dutchLocale).Build())
                .WithUniqueId(WarehouseId)
                .Build();
        }
    }
}
