// <copyright file="QuoteItemStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class QuoteItemStates
    {
        public static readonly Guid DraftId = new Guid("84AD17A3-10F7-4FDB-B76A-41BDB1EDB0E6");
        public static readonly Guid SubmittedId = new Guid("E511EA2D-6EB9-428D-A982-B097938A8FF8");
        public static readonly Guid InProcessId = new Guid("3335810C-9E26-4604-B272-D18B831E79E0");
        public static readonly Guid AwaitingApprovalId = new Guid("76155bb7-53a3-4175-b026-74274a337820");
        public static readonly Guid AwaitingAcceptanceId = new Guid("e0982b61-deb1-47cb-851b-c182f03326a1");
        public static readonly Guid AcceptedId = new Guid("6e56c9f1-7bea-4ced-a724-67e4221a5993");
        public static readonly Guid CancelledId = new Guid("6433F6F7-22D6-4142-8FC5-8941F4F0B6A8");
        public static readonly Guid RejectedId = new Guid("CA8E48EC-5EF7-4082-8347-648B41585485");
        public static readonly Guid OrderedId = new Guid("BF59B586-C35A-423B-9115-DEA1079D905F");

        private UniquelyIdentifiableSticky<QuoteItemState> cache;

        public QuoteItemState Draft => this.Cache[DraftId];

        public QuoteItemState Submitted => this.Cache[SubmittedId];

        public QuoteItemState InProcess => this.Cache[InProcessId];

        public QuoteItemState AwaitingApproval => this.Cache[AwaitingApprovalId];

        public QuoteItemState AwaitingAcceptance => this.Cache[AwaitingAcceptanceId];

        public QuoteItemState Accepted => this.Cache[AcceptedId];

        public QuoteItemState Cancelled => this.Cache[CancelledId];

        public QuoteItemState Rejected => this.Cache[RejectedId];

        public QuoteItemState Ordered => this.Cache[OrderedId];

        private UniquelyIdentifiableSticky<QuoteItemState> Cache => this.cache ??= new UniquelyIdentifiableSticky<QuoteItemState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(DraftId, v => v.Name = "Draft");

            merge(SubmittedId, v => v.Name = "Submitted");
            merge(InProcessId, v => v.Name = "In Process");
            merge(AwaitingApprovalId, v => v.Name = "Awaiting Approval");
            merge(AwaitingAcceptanceId, v => v.Name = "Awaiting Acceptance");
            merge(AcceptedId, v => v.Name = "Accepted");
            merge(CancelledId, v => v.Name = "Cancelled");
            merge(RejectedId, v => v.Name = "Rejected");
            merge(OrderedId, v => v.Name = "Ordered");
        }
    }
}
