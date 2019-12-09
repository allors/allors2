// <copyright file="DeductionTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class DeductionTypes
    {
        private static readonly Guid RetirementId = new Guid("2D0F0788-EB2B-4ef3-A09A-A7285DAD72CF");
        private static readonly Guid InsuranceId = new Guid("D82A5A9F-068F-4e30-88F5-5E6C81D03BAF");

        private UniquelyIdentifiableSticky<DeductionType> cache;

        public DeductionType Retirement => this.Cache[RetirementId];

        public DeductionType Insurance => this.Cache[InsuranceId];

        private UniquelyIdentifiableSticky<DeductionType> Cache => this.cache ??= new UniquelyIdentifiableSticky<DeductionType>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new DeductionTypeBuilder(this.Session)
                .WithName("Retirement")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Pensioen").WithLocale(dutchLocale).Build())
                .WithUniqueId(RetirementId)
                .WithIsActive(true)
                .Build();

            new DeductionTypeBuilder(this.Session)
                .WithName("Insurance")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verzekering").WithLocale(dutchLocale).Build())
                .WithUniqueId(InsuranceId)
                .WithIsActive(true)
                .Build();
        }
    }
}
