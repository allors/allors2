// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RatingTypes.cs" company="Allors bvba">
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

    public partial class RatingTypes
    {
        private static readonly Guid PoorId = new Guid("1DC90B94-1CDA-428e-B5D6-C2970C57D142");
        private static readonly Guid FairId = new Guid("90E42936-FAC5-4673-BE94-01D16A8ADC3A");
        private static readonly Guid GoodId = new Guid("5F5C4F82-22A3-44a3-B175-0CFAE502574C");
        private static readonly Guid OutstandingId = new Guid("44E7BC25-5AEC-44ab-905E-E5BAA5415F72");

        private UniquelyIdentifiableSticky<RatingType> cache;

        public RatingType Poor => this.Cache[PoorId];

        public RatingType Fair => this.Cache[FairId];

        public RatingType Good => this.Cache[GoodId];

        public RatingType Outstanding => this.Cache[OutstandingId];

        private UniquelyIdentifiableSticky<RatingType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<RatingType>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new RatingTypeBuilder(this.Session)
                .WithName("Poor")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Laag").WithLocale(dutchLocale).Build())
                .WithUniqueId(PoorId)
                .Build();
            
            new RatingTypeBuilder(this.Session)
                .WithName("Fair")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Redelijk").WithLocale(dutchLocale).Build())
                .WithUniqueId(FairId)
                .Build();
            
            new RatingTypeBuilder(this.Session)
                .WithName("Good")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Goed").WithLocale(dutchLocale).Build())
                .WithUniqueId(GoodId)
                .Build();
            
            new RatingTypeBuilder(this.Session)
                .WithName("Outstanding")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Uitstekend").WithLocale(dutchLocale).Build())
                .WithUniqueId(OutstandingId)
                .Build();
        }
    }
}
