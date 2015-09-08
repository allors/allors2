// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PositionStatuses.cs" company="Allors bvba">
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

    public partial class PositionStatuses
    {
        public static readonly Guid PlannedForId = new Guid("6B5FA0D0-1E9C-4661-977C-A00CD985D614");
        public static readonly Guid ActiveId = new Guid("6001482F-23E1-4f0a-82C7-52F2EA793627");
        public static readonly Guid PositionOpenId = new Guid("B6547476-A5E0-41c4-87B8-08A87B7663C3");
        public static readonly Guid InactiveId = new Guid("C3DF71FC-AE5E-4f72-A088-477A37D4B448");
        public static readonly Guid PositionClosedId = new Guid("49BBC4D9-96E0-444d-971D-56D90AB0F8C1");

        private UniquelyIdentifiableCache<PositionStatus> cache;

        public PositionStatus PlannedFor
        {
            get { return this.Cache.Get(PlannedForId); }
        }

        public PositionStatus Active
        {
            get { return this.Cache.Get(ActiveId); }
        }

        public PositionStatus PositionOpen
        {
            get { return this.Cache.Get(PositionOpenId); }
        }

        public PositionStatus Inactive
        {
            get { return this.Cache.Get(InactiveId); }
        }

        public PositionStatus PositionClosed
        {
            get { return this.Cache.Get(PositionClosedId); }
        }

        private UniquelyIdentifiableCache<PositionStatus> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<PositionStatus>(this.Session)); }
        }

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

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}
