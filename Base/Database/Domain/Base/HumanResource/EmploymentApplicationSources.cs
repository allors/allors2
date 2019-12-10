// <copyright file="EmploymentApplicationSources.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class EmploymentApplicationSources
    {
        private static readonly Guid NewsPaperId = new Guid("206E641B-DAC1-4b2e-9DD4-E4770AF09B9F");
        private static readonly Guid PersonallReferalId = new Guid("C7029F05-6CCD-4639-A497-A9D8320549D7");
        private static readonly Guid InternetId = new Guid("7931D959-4396-492d-90E4-C44632F2E3EA");

        private UniquelyIdentifiableSticky<EmploymentApplicationSource> cache;

        public EmploymentApplicationSource NewsPaper => this.Cache[NewsPaperId];

        public EmploymentApplicationSource PersonallReferal => this.Cache[PersonallReferalId];

        public EmploymentApplicationSource Internet => this.Cache[InternetId];

        private UniquelyIdentifiableSticky<EmploymentApplicationSource> Cache => this.cache ??= new UniquelyIdentifiableSticky<EmploymentApplicationSource>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(NewsPaperId, v =>
            {
                v.Name = "NewsPaper";
                localisedName.Set(v, dutchLocale, "Krant");
                v.IsActive = true;
            });

            merge(PersonallReferalId, v =>
            {
                v.Name = "PersonallReferal";
                localisedName.Set(v, dutchLocale, "Persoonlijk doorverwezen");
                v.IsActive = true;
            });

            merge(InternetId, v =>
            {
                v.Name = "Internet";
                localisedName.Set(v, dutchLocale, "Internet");
                v.IsActive = true;
            });
        }
    }
}
