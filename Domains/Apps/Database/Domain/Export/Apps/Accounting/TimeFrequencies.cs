// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeFrequencies.cs" company="Allors bvba">
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
    using Allors.Meta;
    using System;

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

        private UniquelyIdentifiableSticky<TimeFrequency> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<TimeFrequency>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            this.NewTimeFrequency("millisecond", "milliseconde", dutchLocale, "ms", MillisecondId, true);
            this.NewTimeFrequency("second", "seconde", dutchLocale, "sec", SecondId, true);
            this.NewTimeFrequency("minute", "minuut", dutchLocale, "min", MinuteId, true);
            this.NewTimeFrequency("hour", "uur", dutchLocale, "hr", HourId, true);
            this.NewTimeFrequency("day", "dag", dutchLocale, "dy", DayId, true);
            this.NewTimeFrequency("week", "week", dutchLocale, "wk", WeekId, true);
            this.NewTimeFrequency("fortnight", "2 weken", dutchLocale, FortnightId, true);
            this.NewTimeFrequency("month", "maand", dutchLocale, "mth", MonthId, true);
            this.NewTimeFrequency("semester", "half jar", dutchLocale, SemesterId, true);
            this.NewTimeFrequency("trimester", "kwartall", dutchLocale, TrimesterId, true);
            this.NewTimeFrequency("year", "jaar", dutchLocale, "yr", YearId, true);

            // Millisecond Conversions
            this.Millisecond.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Millisecond, 1m));
            this.Millisecond.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Second, 1000m));
            this.Millisecond.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Minute, 60000m));
            this.Millisecond.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Hour, 3600000m));
            this.Millisecond.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Day, 86400000m));
            this.Millisecond.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Week, 604800000m));

            // Second Conversions
            this.Second.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Millisecond, 1m / 1000m));
            this.Second.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Second, 1m));
            this.Second.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Minute, 60m));
            this.Second.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Hour, 3600m));
            this.Second.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Day, 86400m));
            this.Second.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Week, 604800m));

            // Minute Conversions
            this.Minute.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Millisecond, 1m / 60000m));
            this.Minute.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Second, 1m / 60m));
            this.Minute.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Minute, 1m));
            this.Minute.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Hour, 60m));
            this.Minute.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Day, 1440m));
            this.Minute.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Week, 10080m));

            // Hour Conversions
            this.Hour.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Millisecond, 1m / 3600000m));
            this.Hour.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Second, 1m / 3600m));
            this.Hour.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Minute, 1m / 60m));
            this.Hour.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Hour, 1m));
            this.Hour.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Day, 24m));
            this.Hour.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Week, 168m));

            // Day Conversions
            this.Day.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Millisecond, 1m / 86400000m));
            this.Day.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Second, 1m / 86400m));
            this.Day.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Minute, 1m / 1440m));
            this.Day.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Hour, 1m / 24m));
            this.Day.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Day, 1m));
            this.Day.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Week, 7m));

            // Week Conversions
            this.Week.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Millisecond, 1m / 604800000m));
            this.Week.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Second, 1m / 604800m));
            this.Week.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Minute, 1m / 10080m));
            this.Week.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Hour, 1m / 168m));
            this.Week.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Day, 1m / 7m));
            this.Week.AddUnitOfMeasureConversion(this.NewFrequencyConversion(this.Week, 1m));
        }

        private TimeFrequency NewTimeFrequency(string name, string localName, Locale locale, Guid uniqueId, bool isActive)
            => new TimeFrequencyBuilder(this.Session)
                .WithName(name)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText(localName).WithLocale(locale).Build())
                .WithUniqueId(uniqueId)
                .WithIsActive(isActive)
                .Build();

        private TimeFrequency NewTimeFrequency(string name, string localName, Locale locale, string abbrev, Guid uniqueId, bool isActive)
            => new TimeFrequencyBuilder(this.Session)
                .WithName(name)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText(localName).WithLocale(locale).Build())
                .WithAbbreviation(abbrev)
                .WithUniqueId(uniqueId)
                .WithIsActive(isActive)
                .Build();

        private UnitOfMeasureConversion NewFrequencyConversion(TimeFrequency toFrequency, decimal conversionFactor)
            => new UnitOfMeasureConversionBuilder(this.Session)
            .WithToUnitOfMeasure(toFrequency)
            .WithConversionFactor(conversionFactor)
            .Build();
    }
}
