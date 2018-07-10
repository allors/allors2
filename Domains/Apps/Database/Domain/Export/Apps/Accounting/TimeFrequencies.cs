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
    using System;

    public partial class TimeFrequencies
    {
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

            new TimeFrequencyBuilder(this.Session)
                .WithName("second")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("seconde").WithLocale(dutchLocale).Build())
                .WithAbbreviation("s")
                .WithUniqueId(SecondId)
                .WithIsActive(true)
                .Build();

            new TimeFrequencyBuilder(this.Session)
                .WithName("minute")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("minuut").WithLocale(dutchLocale).Build())
                .WithAbbreviation("min")
                .WithUniqueId(MinuteId)
                .WithIsActive(true)
                .Build();

            new TimeFrequencyBuilder(this.Session)
                .WithName("hour")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("uur").WithLocale(dutchLocale).Build())
                .WithAbbreviation("hr")
                .WithUniqueId(HourId)
                .WithIsActive(true)
                .Build();

            new TimeFrequencyBuilder(this.Session)
                .WithName("day")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("dag").WithLocale(dutchLocale).Build())
                .WithUniqueId(DayId)
                .WithIsActive(true)
                .Build();
            
            new TimeFrequencyBuilder(this.Session)
                .WithName("week")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("week").WithLocale(dutchLocale).Build())
                .WithUniqueId(WeekId)
                .WithIsActive(true)
                .Build();

            new TimeFrequencyBuilder(this.Session)
                .WithName("fortnight")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("2 weken").WithLocale(dutchLocale).Build())
                .WithUniqueId(FortnightId)
                .WithIsActive(true)
                .Build();
            
            new TimeFrequencyBuilder(this.Session)
                .WithName("month")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("maand").WithLocale(dutchLocale).Build())
                .WithUniqueId(MonthId)
                .WithIsActive(true)
                .Build();
            
            new TimeFrequencyBuilder(this.Session)
                .WithName("semester")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("half jaar").WithLocale(dutchLocale).Build())
                .WithUniqueId(SemesterId)
                .WithIsActive(true)
                .Build();

            new TimeFrequencyBuilder(this.Session)
                .WithName("trimester")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("kwartaal").WithLocale(dutchLocale).Build())
                .WithUniqueId(TrimesterId)
                .WithIsActive(true)
                .Build();
            
            new TimeFrequencyBuilder(this.Session)
                .WithName("year")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("jaar").WithLocale(dutchLocale).Build())
                .WithUniqueId(YearId)
                .WithIsActive(true)
                .Build();
        }
    }
}
