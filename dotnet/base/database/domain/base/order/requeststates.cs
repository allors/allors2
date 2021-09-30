// <copyright file="RequestStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class RequestStates
    {
        public static readonly Guid AnonymousId = new Guid("2F054949-E30C-4954-9A3C-191559DE8315");
        public static readonly Guid SubmittedId = new Guid("DB03407D-BCB1-433A-B4E9-26CEA9A71BFD");
        public static readonly Guid CancelledId = new Guid("6B839C79-689D-412C-9C27-B553CF350662");
        public static readonly Guid QuotedId = new Guid("79A9BAF5-D16F-4FA7-A2A0-CA20BF79833F");
        public static readonly Guid PendingCustomerId = new Guid("671FDA2F-5AA6-4EA5-B5D6-C914F0911690");
        public static readonly Guid RejectedId = new Guid("26B1E962-9799-4C53-AE36-20E8490F757A");

        private UniquelyIdentifiableSticky<RequestState> cache;

        public RequestState Anonymous => this.Cache[AnonymousId];

        public RequestState Submitted => this.Cache[SubmittedId];

        public RequestState Cancelled => this.Cache[CancelledId];

        public RequestState Quoted => this.Cache[QuotedId];

        public RequestState PendingCustomer => this.Cache[PendingCustomerId];

        public RequestState Rejected => this.Cache[RejectedId];

        private UniquelyIdentifiableSticky<RequestState> Cache => this.cache ??= new UniquelyIdentifiableSticky<RequestState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(AnonymousId, v => v.Name = "Anonymous");
            merge(SubmittedId, v => v.Name = "Submitted");
            merge(CancelledId, v => v.Name = "Cancelled");
            merge(QuotedId, v => v.Name = "Quoted");
            merge(PendingCustomerId, v => v.Name = "Pending Customer");
            merge(RejectedId, v => v.Name = "Rejected");
        }
    }
}
