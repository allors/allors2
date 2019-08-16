
// <copyright file="RatingTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new RatingTypeBuilder(this.Session)
                .WithName("Poor")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Laag").WithLocale(dutchLocale).Build())
                .WithUniqueId(PoorId)
                .WithIsActive(true)
                .Build();

            new RatingTypeBuilder(this.Session)
                .WithName("Fair")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Redelijk").WithLocale(dutchLocale).Build())
                .WithUniqueId(FairId)
                .WithIsActive(true)
                .Build();

            new RatingTypeBuilder(this.Session)
                .WithName("Good")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Goed").WithLocale(dutchLocale).Build())
                .WithUniqueId(GoodId)
                .WithIsActive(true)
                .Build();

            new RatingTypeBuilder(this.Session)
                .WithName("Outstanding")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Uitstekend").WithLocale(dutchLocale).Build())
                .WithUniqueId(OutstandingId)
                .WithIsActive(true)
                .Build();
        }
    }
}
