// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommunicationEventStates.cs" company="Allors bvba">
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

    public partial class CommunicationEventStates
    {
        private static readonly Guid ScheduledId = new Guid("199131EB-18FD-4b8a-9FEC-23789C169FF5");
        private static readonly Guid CompletedId = new Guid("35612611-62C5-4de5-B138-9C8D874D8916");
        private static readonly Guid CancelledId = new Guid("F236E865-E2CA-43d7-8F17-56C3DC54C191");
        private static readonly Guid InProgressId = new Guid("D1232CEB-1530-451e-BAED-DB1356BC1EB2");

        private UniquelyIdentifiableSticky<CommunicationEventState> cache;

        public CommunicationEventState Scheduled => this.Cache[ScheduledId];

        public CommunicationEventState InProgress => this.Cache[InProgressId];

        public CommunicationEventState Completed => this.Cache[CompletedId];

        public CommunicationEventState Cancelled => this.Cache[CancelledId];

        private UniquelyIdentifiableSticky<CommunicationEventState> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<CommunicationEventState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            
            
            new CommunicationEventStateBuilder(this.Session)
                .WithName("Scheduled")
                .WithUniqueId(ScheduledId)
                .Build();

            new CommunicationEventStateBuilder(this.Session)
                .WithName("In Progress")
                .WithUniqueId(InProgressId)
                .Build();

            new CommunicationEventStateBuilder(this.Session)
                .WithName("Completed")
                .WithUniqueId(CompletedId)
                .Build();

            new CommunicationEventStateBuilder(this.Session)
                .WithName("Cancelled")
                .WithUniqueId(CancelledId)
                .Build();
        }
    }
}