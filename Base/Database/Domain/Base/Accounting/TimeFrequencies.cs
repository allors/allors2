// <copyright file="TimeFrequencies.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using Meta;

    public partial class TimeFrequencies
    {
        private static readonly Guid MillisecondId = new Guid("8498CC91-3607-43D0-AFEC-0C981438435C");
        private static readonly Guid SecondId = new Guid("79878001-261C-42B1-BDD8-D80808DE885A");
        private static readonly Guid MinuteId = new Guid("A8DAC2A7-F217-4931-8C70-EB1C5A324FA6");
        private static readonly Guid HourId = new Guid("DB14E5D5-5EAF-4ec8-B149-C558A28D99F5");
        private static readonly Guid DayId = new Guid("DA53F7DB-F911-4183-B281-14440EE108A6");
        private static readonly Guid WeekId = new Guid("806D7DED-CAF0-4d1b-A0FA-0AC2F2773A32");
        private static readonly Guid FortnightId = new Guid("DBE8A817-ED3F-49F0-8BD0-ADAB918AEB7D");
        private static readonly Guid MonthId = new Guid("4BF13734-E648-4204-AB92-CA0C096105FE");
        private static readonly Guid SemesterId = new Guid("1E98D1ED-AEC2-4bd5-996D-EF3A9D372490");
        private static readonly Guid TrimesterId = new Guid("E3E9AB81-D44C-4166-8983-917FAF4CCF6D");
        private static readonly Guid YearId = new Guid("F43E9D63-2BF0-4181-8247-B6BF39BA5313");

        private UniquelyIdentifiableSticky<TimeFrequency> cache;

        public TimeFrequency Millisecond => this.Cache[MillisecondId];

        public TimeFrequency Second => this.Cache[SecondId];

        public TimeFrequency Minute => this.Cache[MinuteId];

        public TimeFrequency Hour => this.Cache[HourId];

        public TimeFrequency Day => this.Cache[DayId];

        public TimeFrequency Week => this.Cache[WeekId];

        public TimeFrequency Fortnight => this.Cache[FortnightId];

        public TimeFrequency Month => this.Cache[MonthId];

        public TimeFrequency Semester => this.Cache[SemesterId];

        public TimeFrequency Trimester => this.Cache[TrimesterId];

        public TimeFrequency Year => this.Cache[YearId];

        private UniquelyIdentifiableSticky<TimeFrequency> Cache => this.cache ??= new UniquelyIdentifiableSticky<TimeFrequency>(this.Session);

        protected override void BasePrepare(Setup setup)
        {
            setup.AddDependency(this.ObjectType, M.Locale);
            setup.AddDependency(this.ObjectType, M.UnitOfMeasure);
        }

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);
            var unitOfMeasureConversion = new UnitOfMeasureConversionAccessor(this.Meta.UnitOfMeasureConversions);

            merge(MillisecondId, v =>
            {
                v.Name = "millisecond";
                v.Abbreviation = "ms";
                localisedName.Set(v, dutchLocale, "milliseconde");
                v.IsActive = true;
            });

            merge(SecondId, v =>
            {
                v.Name = "second";
                v.Abbreviation = "sec";
                localisedName.Set(v, dutchLocale, "seconde");
                v.IsActive = true;
            });

            merge(MinuteId, v =>
            {
                v.Name = "minute";
                v.Abbreviation = "min";
                localisedName.Set(v, dutchLocale, "minuut");
                v.IsActive = true;
            });

            merge(HourId, v =>
            {
                v.Name = "hour";
                v.Abbreviation = "hr";
                localisedName.Set(v, dutchLocale, "uur");
                v.IsActive = true;
            });

            merge(DayId, v =>
            {
                v.Name = "day";
                v.Abbreviation = "dy";
                localisedName.Set(v, dutchLocale, "dag");
                v.IsActive = true;
            });

            merge(WeekId, v =>
            {
                v.Name = "week";
                v.Abbreviation = "wk";
                localisedName.Set(v, dutchLocale, "week");
                v.IsActive = true;
            });

            merge(FortnightId, v =>
            {
                v.Name = "fortnight";
                localisedName.Set(v, dutchLocale, "2 weken");
                v.IsActive = true;
            });

            merge(MonthId, v =>
            {
                v.Name = "month";
                v.Abbreviation = "mth";
                localisedName.Set(v, dutchLocale, "maand");
                v.IsActive = true;
            });

            merge(SemesterId, v =>
            {
                v.Name = "semester";
                localisedName.Set(v, dutchLocale, "half jaar");
                v.IsActive = true;
            });

            merge(TrimesterId, v =>
            {
                v.Name = "trimester";
                localisedName.Set(v, dutchLocale, "kwartaal");
                v.IsActive = true;
            });

            merge(YearId, v =>
            {
                v.Name = "year";
                v.Abbreviation = "yr";
                localisedName.Set(v, dutchLocale, "jaar");
                v.IsActive = true;
            });

            // Conversions
            unitOfMeasureConversion.Set(this.Millisecond, this.Millisecond, 1m);
            unitOfMeasureConversion.Set(this.Millisecond, this.Second, 1m / 1000m);
            unitOfMeasureConversion.Set(this.Millisecond, this.Minute, 1m / 60000m);
            unitOfMeasureConversion.Set(this.Millisecond, this.Hour, 1m / 3600000m);
            unitOfMeasureConversion.Set(this.Millisecond, this.Day, 1m / 86400000m);
            unitOfMeasureConversion.Set(this.Millisecond, this.Week, 1m / 604800000m);

            unitOfMeasureConversion.Set(this.Second, this.Millisecond, 1000m);
            unitOfMeasureConversion.Set(this.Second, this.Second, 1m);
            unitOfMeasureConversion.Set(this.Second, this.Minute, 1m / 60m);
            unitOfMeasureConversion.Set(this.Second, this.Hour, 1m / 3600m);
            unitOfMeasureConversion.Set(this.Second, this.Day, 1m / 86400m);
            unitOfMeasureConversion.Set(this.Second, this.Week, 1m / 604800m);

            unitOfMeasureConversion.Set(this.Minute, this.Millisecond, 60000m);
            unitOfMeasureConversion.Set(this.Minute, this.Second, 60m);
            unitOfMeasureConversion.Set(this.Minute, this.Minute, 1m);
            unitOfMeasureConversion.Set(this.Minute, this.Hour, 1m / 60m);
            unitOfMeasureConversion.Set(this.Minute, this.Day, 1m / 1440m);
            unitOfMeasureConversion.Set(this.Minute, this.Week, 1m / 10080m);

            unitOfMeasureConversion.Set(this.Hour, this.Millisecond, 3600000m);
            unitOfMeasureConversion.Set(this.Hour, this.Second, 3600m);
            unitOfMeasureConversion.Set(this.Hour, this.Minute, 60m);
            unitOfMeasureConversion.Set(this.Hour, this.Hour, 1m);
            unitOfMeasureConversion.Set(this.Hour, this.Day, 1m / 24m);
            unitOfMeasureConversion.Set(this.Hour, this.Week, 1m / 168m);

            unitOfMeasureConversion.Set(this.Day, this.Millisecond, 86400000m);
            unitOfMeasureConversion.Set(this.Day, this.Second, 86400m);
            unitOfMeasureConversion.Set(this.Day, this.Minute, 1440m);
            unitOfMeasureConversion.Set(this.Day, this.Hour, 24m);
            unitOfMeasureConversion.Set(this.Day, this.Day, 1m);
            unitOfMeasureConversion.Set(this.Day, this.Week, 1m / 7m);

            unitOfMeasureConversion.Set(this.Week, this.Millisecond, 604800000m);
            unitOfMeasureConversion.Set(this.Week, this.Second, 604800m);
            unitOfMeasureConversion.Set(this.Week, this.Minute, 10080m);
            unitOfMeasureConversion.Set(this.Week, this.Hour, 168m);
            unitOfMeasureConversion.Set(this.Week, this.Day, 7m);
            unitOfMeasureConversion.Set(this.Week, this.Week, 1m);
        }
    }
}
