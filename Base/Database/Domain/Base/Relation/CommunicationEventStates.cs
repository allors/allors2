// <copyright file="CommunicationEventStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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

        private UniquelyIdentifiableSticky<CommunicationEventState> Cache => this.cache ??= new UniquelyIdentifiableSticky<CommunicationEventState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(ScheduledId, v => v.Name = "Scheduled");
            merge(InProgressId, v => v.Name = "In Progress");
            merge(CompletedId, v => v.Name = "Completed");
            merge(CancelledId, v => v.Name = "Cancelled");
        }
    }
}
