// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PositionStatuses.cs" company="Allors bvba">
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

    public partial class PositionStatuses
    {
        private static readonly Guid PlannedForId = new Guid("6B5FA0D0-1E9C-4661-977C-A00CD985D614");
        private static readonly Guid ActiveId = new Guid("6001482F-23E1-4f0a-82C7-52F2EA793627");
        private static readonly Guid PositionOpenId = new Guid("B6547476-A5E0-41c4-87B8-08A87B7663C3");
        private static readonly Guid InactiveId = new Guid("C3DF71FC-AE5E-4f72-A088-477A37D4B448");
        private static readonly Guid PositionClosedId = new Guid("49BBC4D9-96E0-444d-971D-56D90AB0F8C1");

        private UniquelyIdentifiableSticky<PositionStatus> cache;

        public PositionStatus PlannedFor => this.Cache[PlannedForId];

        public PositionStatus Active => this.Cache[ActiveId];

        public PositionStatus PositionOpen => this.Cache[PositionOpenId];

        public PositionStatus Inactive => this.Cache[InactiveId];

        public PositionStatus PositionClosed => this.Cache[PositionClosedId];

        private UniquelyIdentifiableSticky<PositionStatus> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<PositionStatus>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new PositionStatusBuilder(this.Session)
                .WithName("Planned For")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Planned For").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Gepland").WithLocale(dutchLocale).Build())
                .WithUniqueId(PlannedForId)
                .Build();
            
            new PositionStatusBuilder(this.Session)
                .WithName("Active")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Active").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Actief").WithLocale(dutchLocale).Build())
                .WithUniqueId(ActiveId)
                .Build();
            
            new PositionStatusBuilder(this.Session)
                .WithName("Open")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Open").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Open").WithLocale(dutchLocale).Build())
                .WithUniqueId(PositionOpenId)
                .Build();
            
            new PositionStatusBuilder(this.Session)
                .WithName("Inactive")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Inactive").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Niet actief").WithLocale(dutchLocale).Build())
                .WithUniqueId(InactiveId)
                .Build();
            
            new PositionStatusBuilder(this.Session)
                .WithName("Closed")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Closed").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Gesloten").WithLocale(dutchLocale).Build())
                .WithUniqueId(PositionClosedId)
                .Build();
        }
    }
}
