
// <copyright file="CatScopes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class CatScopes
    {
        private static readonly Guid PrivateId = new Guid("E312891F-7744-43ba-A69F-13878B1FC66B");
        private static readonly Guid PublicId = new Guid("6593FE82-A00F-4de6-9516-D652FE28A3EA");

        private UniquelyIdentifiableSticky<CatScope> cache;

        public CatScope Private => this.Cache[PrivateId];

        public CatScope Public => this.Cache[PublicId];

        private UniquelyIdentifiableSticky<CatScope> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<CatScope>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new CatScopeBuilder(this.Session)
                .WithName("Private")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Prive").WithLocale(dutchLocale).Build())
                .WithUniqueId(PrivateId)
                .WithIsActive(true)
                .Build();

            new CatScopeBuilder(this.Session)
                .WithName("Public")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Publiek").WithLocale(dutchLocale).Build())
                .WithUniqueId(PublicId)
                .WithIsActive(true)
                .Build();
        }
    }
}
