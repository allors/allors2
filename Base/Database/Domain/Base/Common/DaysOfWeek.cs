// <copyright file="DaysOfWeek.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class DaysOfWeek
    {
        private static readonly Guid SundayId = new Guid("FD1FEED7-0041-433E-99CB-62A4903BD494");
        private static readonly Guid MondayId = new Guid("1E5AE60A-AECE-4CF8-B707-F9A49219C6CD");
        private static readonly Guid TuesdayId = new Guid("D73DFC6D-CB49-4352-8617-B4FC4875A491");
        private static readonly Guid WednesdayId = new Guid("CE5A9D43-A9AB-42D2-B915-015AEB12AA18");
        private static readonly Guid ThursdayId = new Guid("F0B0B22E-03AA-4BEB-806F-5369C384D441");
        private static readonly Guid FridayId = new Guid("F2164C35-0330-4875-90D9-7225FBE7ED32");
        private static readonly Guid SaturdayId = new Guid("57773FB1-317E-48E0-9D68-40C4D626A020");

        private UniquelyIdentifiableSticky<DayOfWeek> cache;

        public DayOfWeek Sunday => this.Cache[SundayId];

        public DayOfWeek Monday => this.Cache[MondayId];

        public DayOfWeek Tuesday => this.Cache[TuesdayId];

        public DayOfWeek Wednesday => this.Cache[WednesdayId];

        public DayOfWeek Thursday => this.Cache[ThursdayId];

        public DayOfWeek Friday => this.Cache[FridayId];

        public DayOfWeek Saturday => this.Cache[SaturdayId];

        private UniquelyIdentifiableSticky<DayOfWeek> Cache => this.cache ??= new UniquelyIdentifiableSticky<DayOfWeek>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(SundayId, v =>
            {
                v.Name = "Sunday";
                localisedName.Set(v, dutchLocale, "Zondag");
                v.IsActive = true;
            });

            merge(MondayId, v =>
            {
                v.Name = "Monday";
                localisedName.Set(v, dutchLocale, "Maandag");
                v.IsActive = true;
            });

            merge(TuesdayId, v =>
            {
                v.Name = "Tuesday";
                localisedName.Set(v, dutchLocale, "Dinsdag");
                v.IsActive = true;
            });

            merge(WednesdayId, v =>
            {
                v.Name = "Wednesday";
                localisedName.Set(v, dutchLocale, "Woensdag");
                v.IsActive = true;
            });

            merge(ThursdayId, v =>
            {
                v.Name = "Thursday";
                localisedName.Set(v, dutchLocale, "Donderdag");
                v.IsActive = true;
            });

            merge(FridayId, v =>
            {
                v.Name = "Friday";
                localisedName.Set(v, dutchLocale, "Vrijdag");
                v.IsActive = true;
            });

            merge(SaturdayId, v =>
            {
                v.Name = "Saturday";
                localisedName.Set(v, dutchLocale, "Zaterdag");
                v.IsActive = true;
            });
        }
    }
}
