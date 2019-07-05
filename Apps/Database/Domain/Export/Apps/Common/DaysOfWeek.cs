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

        private UniquelyIdentifiableSticky<DayOfWeek> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<DayOfWeek>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            

            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new DayOfWeekBuilder(this.Session)
                .WithName("Sunday")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Zondag").WithLocale(dutchLocale).Build())
                .WithUniqueId(SundayId)
                .WithIsActive(true)
                .Build();

            new DayOfWeekBuilder(this.Session)
                .WithName("Monday")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Maandag").WithLocale(dutchLocale).Build())
                .WithUniqueId(MondayId)
                .WithIsActive(true)
                .Build();

            new DayOfWeekBuilder(this.Session)
                .WithName("Tuesday")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("dinsdag").WithLocale(dutchLocale).Build())
                .WithUniqueId(TuesdayId)
                .WithIsActive(true)
                .Build();

            new DayOfWeekBuilder(this.Session)
                .WithName("Wednesday")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Woensdag").WithLocale(dutchLocale).Build())
                .WithUniqueId(WednesdayId)
                .WithIsActive(true)
                .Build();

            new DayOfWeekBuilder(this.Session)
                .WithName("Thursday")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Donderdag").WithLocale(dutchLocale).Build())
                .WithUniqueId(ThursdayId)
                .WithIsActive(true)
                .Build();

            new DayOfWeekBuilder(this.Session)
                .WithName("Friday")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vrijdag").WithLocale(dutchLocale).Build())
                .WithUniqueId(FridayId)
                .WithIsActive(true)
                .Build();

            new DayOfWeekBuilder(this.Session)
                .WithName("Saturday")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Zaterdag").WithLocale(dutchLocale).Build())
                .WithUniqueId(SaturdayId)
                .WithIsActive(true)
                .Build();
        }
    }
}
