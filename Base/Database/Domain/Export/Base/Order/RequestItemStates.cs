
// <copyright file="RequestItemStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class RequestItemStates
    {
        private static readonly Guid DraftId = new Guid("B173DFBE-9421-4697-8FFB-E46AFC724490");
        private static readonly Guid SubmittedId = new Guid("B118C185-DE34-4131-BE1F-E6162C1DEA4B");
        private static readonly Guid CancelledId = new Guid("E98A3001-C343-4925-9D95-CE370DFC98E7");
        private static readonly Guid QuotedId = new Guid("D12FF2E4-8CB2-4CA0-8864-A90819D0EE19");

        private UniquelyIdentifiableSticky<RequestItemState> stateCache;

        public RequestItemState Draft => this.StateCache[DraftId];

        public RequestItemState Submitted => this.StateCache[SubmittedId];

        public RequestItemState Cancelled => this.StateCache[CancelledId];

        public RequestItemState Quoted => this.StateCache[QuotedId];

        private UniquelyIdentifiableSticky<RequestItemState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<RequestItemState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            new RequestItemStateBuilder(this.Session)
                .WithUniqueId(DraftId)
                .WithName("Anonymous")
                .Build();

            new RequestItemStateBuilder(this.Session)
                .WithUniqueId(SubmittedId)
                .WithName("Submitted")
                .Build();

            new RequestItemStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new RequestItemStateBuilder(this.Session)
                .WithUniqueId(QuotedId)
                .WithName("quoted")
                .Build();
        }
    }
}
