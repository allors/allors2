// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommunicationEventObjectStates.cs" company="Allors bvba">
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

    public partial class CommunicationEventObjectStates
    {
        public static readonly Guid ScheduledId = new Guid("199131EB-18FD-4b8a-9FEC-23789C169FF5");
        public static readonly Guid CompletedId = new Guid("35612611-62C5-4de5-B138-9C8D874D8916");
        public static readonly Guid CancelledId = new Guid("F236E865-E2CA-43d7-8F17-56C3DC54C191");
        public static readonly Guid InProgressId = new Guid("D1232CEB-1530-451e-BAED-DB1356BC1EB2");

        private UniquelyIdentifiableCache<CommunicationEventObjectState> cache;

        public CommunicationEventObjectState Scheduled
        {
            get { return this.Cache.Get(ScheduledId); }
        }

        public CommunicationEventObjectState InProgress
        {
            get { return this.Cache.Get(InProgressId); }
        }

        public CommunicationEventObjectState Completed
        {
            get { return this.Cache.Get(CompletedId); }
        }

        public CommunicationEventObjectState Cancelled
        {
            get { return this.Cache.Get(CancelledId); }
        }

        private UniquelyIdentifiableCache<CommunicationEventObjectState> Cache
        {
            get
            {
                return this.cache ?? (this.cache = new UniquelyIdentifiableCache<CommunicationEventObjectState>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var durchLocale = new Locales(this.Session).DutchNetherlands;

            new CommunicationEventObjectStateBuilder(this.Session)
                .WithName("Scheduled")
                .WithUniqueId(ScheduledId)
                .Build();

            new CommunicationEventObjectStateBuilder(this.Session)
                .WithName("In Progress")
                .WithUniqueId(InProgressId)
                .Build();

            new CommunicationEventObjectStateBuilder(this.Session)
                .WithName("Completed")
                .WithUniqueId(CompletedId)
                .Build();

            new CommunicationEventObjectStateBuilder(this.Session)
                .WithName("Cancelled")
                .WithUniqueId(CancelledId)
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