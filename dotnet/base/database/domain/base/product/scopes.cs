// <copyright file="CatScopes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class Scopes
    {
        private static readonly Guid PrivateId = new Guid("E312891F-7744-43ba-A69F-13878B1FC66B");
        private static readonly Guid PublicId = new Guid("6593FE82-A00F-4de6-9516-D652FE28A3EA");

        private UniquelyIdentifiableSticky<Scope> cache;

        public Scope Private => this.Cache[PrivateId];

        public Scope Public => this.Cache[PublicId];

        private UniquelyIdentifiableSticky<Scope> Cache => this.cache ??= new UniquelyIdentifiableSticky<Scope>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(PrivateId, v =>
            {
                v.Name = "Private";
                localisedName.Set(v, dutchLocale, "Prive");
                v.IsActive = true;
            });

            merge(PublicId, v =>
            {
                v.Name = "Public";
                localisedName.Set(v, dutchLocale, "Publiek");
                v.IsActive = true;
            });
        }
    }
}
