// <copyright file="RequestItemStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class RequestItemStates
    {
        public static readonly Guid DraftId = new Guid("B173DFBE-9421-4697-8FFB-E46AFC724490");
        public static readonly Guid SubmittedId = new Guid("B118C185-DE34-4131-BE1F-E6162C1DEA4B");
        public static readonly Guid CancelledId = new Guid("E98A3001-C343-4925-9D95-CE370DFC98E7");
        public static readonly Guid RejectedId = new Guid("bb9a7a95-5230-4ed1-993a-6bc6896baa1e");
        public static readonly Guid QuotedId = new Guid("D12FF2E4-8CB2-4CA0-8864-A90819D0EE19");

        private UniquelyIdentifiableSticky<RequestItemState> cache;

        public RequestItemState Draft => this.Cache[DraftId];

        public RequestItemState Submitted => this.Cache[SubmittedId];

        public RequestItemState Cancelled => this.Cache[CancelledId];

        public RequestItemState Rejected => this.Cache[RejectedId];

        public RequestItemState Quoted => this.Cache[QuotedId];

        private UniquelyIdentifiableSticky<RequestItemState> Cache => this.cache ??= new UniquelyIdentifiableSticky<RequestItemState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(DraftId, v => v.Name = "Draft");
            merge(SubmittedId, v => v.Name = "Submitted");
            merge(CancelledId, v => v.Name = "Cancelled");
            merge(RejectedId, v => v.Name = "Rejected");
            merge(QuotedId, v => v.Name = "Quoted");
        }
    }
}
