// <copyright file="Ordinals.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class Ordinals
    {
        private static readonly Guid FirstId = new Guid("E312891F-7744-43ba-A69F-13878B1FC66B");
        private static readonly Guid SecondId = new Guid("6593FE82-A00F-4de6-9516-D652FE28A3EA");
        private static readonly Guid ThirdId = new Guid("C207121C-B534-4764-9724-3E829E9C9F21");

        private UniquelyIdentifiableSticky<Ordinal> cache;

        public Ordinal First => this.Cache[FirstId];

        public Ordinal Second => this.Cache[SecondId];

        public Ordinal Third => this.Cache[ThirdId];

        private UniquelyIdentifiableSticky<Ordinal> Cache => this.cache ??= new UniquelyIdentifiableSticky<Ordinal>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new OrdinalBuilder(this.Session)
                .WithName("First")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Eerste").WithLocale(dutchLocale).Build())
                .WithUniqueId(FirstId)
                .WithIsActive(true)
                .Build();

            new OrdinalBuilder(this.Session)
                .WithName("Second")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Tweede").WithLocale(dutchLocale).Build())
                .WithUniqueId(SecondId)
                .WithIsActive(true)
                .Build();

            new OrdinalBuilder(this.Session)
                .WithName("Third")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Derde").WithLocale(dutchLocale).Build())
                .WithUniqueId(ThirdId)
                .WithIsActive(true)
                .Build();
        }
    }
}
