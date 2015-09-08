// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeFrequencies.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    public partial class TimeFrequencies
    {
        public static readonly Guid HourId = new Guid("DB14E5D5-5EAF-4ec8-B149-C558A28D99F5");
        public static readonly Guid DayId = new Guid("DA53F7DB-F911-4183-B281-14440EE108A6");
        public static readonly Guid WeekId = new Guid("806D7DED-CAF0-4d1b-A0FA-0AC2F2773A32");
        public static readonly Guid FortnightId = new Guid("DBE8A817-ED3F-49F0-8BD0-ADAB918AEB7D");
        public static readonly Guid MonthId = new Guid("4BF13734-E648-4204-AB92-CA0C096105FE");
        public static readonly Guid SemesterId = new Guid("1E98D1ED-AEC2-4bd5-996D-EF3A9D372490");
        public static readonly Guid TrimesterId = new Guid("E3E9AB81-D44C-4166-8983-917FAF4CCF6D");
        public static readonly Guid YearId = new Guid("F43E9D63-2BF0-4181-8247-B6BF39BA5313");

        private UniquelyIdentifiableCache<TimeFrequency> cache;

        public TimeFrequency Hour
        {
            get { return this.Cache.Get(HourId); }
        }

        public TimeFrequency Day
        {
            get { return this.Cache.Get(DayId); }
        }

        public TimeFrequency Week
        {
            get { return this.Cache.Get(WeekId); }
        }

        public TimeFrequency Fortnight
        {
            get { return this.Cache.Get(FortnightId); }
        }

        public TimeFrequency Month
        {
            get { return this.Cache.Get(MonthId); }
        }

        public TimeFrequency Semester
        {
            get { return this.Cache.Get(SemesterId); }
        }

        public TimeFrequency Trimester
        {
            get { return this.Cache.Get(TrimesterId); }
        }

        public TimeFrequency Year
        {
            get { return this.Cache.Get(YearId); }
        }

        private UniquelyIdentifiableCache<TimeFrequency> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<TimeFrequency>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new TimeFrequencyBuilder(this.Session)
                .WithName("Hour")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Hour").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Uur").WithLocale(dutchLocale).Build())
                .WithUniqueId(HourId)
                .Build();

            new TimeFrequencyBuilder(this.Session)
                .WithName("Day")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Day").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Dag").WithLocale(dutchLocale).Build())
                .WithUniqueId(DayId)
                .Build();
            
            new TimeFrequencyBuilder(this.Session)
                .WithName("Week")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Week").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Week").WithLocale(dutchLocale).Build())
                .WithUniqueId(WeekId)
                .Build();

            new TimeFrequencyBuilder(this.Session)
                .WithName("Fortnight")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Fortnight").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("2 Weken").WithLocale(dutchLocale).Build())
                .WithUniqueId(FortnightId)
                .Build();
            
            new TimeFrequencyBuilder(this.Session)
                .WithName("Month")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Month").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Maand").WithLocale(dutchLocale).Build())
                .WithUniqueId(MonthId)
                .Build();
            
            new TimeFrequencyBuilder(this.Session)
                .WithName("Semester")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Semester").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Half jaar").WithLocale(dutchLocale).Build())
                .WithUniqueId(SemesterId)
                .Build();

            new TimeFrequencyBuilder(this.Session)
                .WithName("Trimester")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Trimester").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Kwartaal").WithLocale(dutchLocale).Build())
                .WithUniqueId(TrimesterId)
                .Build();
            
            new TimeFrequencyBuilder(this.Session)
                .WithName("Year")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Year").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Jaar").WithLocale(dutchLocale).Build())
                .WithUniqueId(YearId)
                .Build();
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}
