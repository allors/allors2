// <copyright file="Salutations.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class Salutations
    {
        public static readonly Guid MrId = new Guid("970D27DD-CE6F-4943-8BAF-7BE48FC7EE23");
        public static readonly Guid MrsId = new Guid("0CEEB74D-62C5-4166-9823-EA65BDA5A46F");
        public static readonly Guid DrId = new Guid("5827A6B6-375A-4781-9400-FAD8D62064A1");
        public static readonly Guid MsId = new Guid("BE1E6992-EFB6-4445-BDB6-B7AAE849EEEA");
        public static readonly Guid MmeId = new Guid("DF2FC141-D035-47EB-8135-A880A4EBC93C");

        private UniquelyIdentifiableSticky<Salutation> cache;

        public Salutation Mr => this.Cache[MrId];

        public Salutation Mrs => this.Cache[MrsId];

        public Salutation Dr => this.Cache[DrId];

        public Salutation Ms => this.Cache[MsId];

        public Salutation Mme => this.Cache[MmeId];

        private UniquelyIdentifiableSticky<Salutation> Cache => this.cache ??= new UniquelyIdentifiableSticky<Salutation>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(MrId, v =>
            {
                v.Name = "Mr.";
                localisedName.Set(v, dutchLocale, "Mr.");
                v.IsActive = true;
            });

            merge(MrsId, v =>
            {
                v.Name = "Mrs.";
                localisedName.Set(v, dutchLocale, "Mvr.");
                v.IsActive = true;
            });

            merge(DrId, v =>
            {
                v.Name = "Dr.";
                localisedName.Set(v, dutchLocale, "Dr.");
                v.IsActive = true;
            });

            merge(MsId, v =>
            {
                v.Name = "Ms.";
                localisedName.Set(v, dutchLocale, "Juff.");
                v.IsActive = true;
            });

            merge(MmeId, v =>
            {
                v.Name = "Mme.";
                localisedName.Set(v, dutchLocale, "Mvr.");
                v.IsActive = true;
            });
        }
    }
}
