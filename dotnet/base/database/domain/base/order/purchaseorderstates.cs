// <copyright file="PurchaseOrderStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class PurchaseOrderStates
    {
        public static readonly Guid CreatedId = new Guid("69946F6D-718E-463d-AB36-BF4E3B970210");
        public static readonly Guid AwaitingApprovalLevel1Id = new Guid("B870154D-1B95-41E8-BF78-4CF64D23E4E6");
        public static readonly Guid AwaitingApprovalLevel2Id = new Guid("713C551D-3A39-44A8-AB81-BEEF55173F5A");
        public static readonly Guid CancelledId = new Guid("FC345C8F-7BCC-4571-B353-BF8AF27C57A8");
        public static readonly Guid CompletedId = new Guid("FDF3A1F6-605A-4f62-B463-7900D6782E56");
        public static readonly Guid RejectedId = new Guid("B11913F7-4FFD-44a8-8DDF-36200E910B37");
        public static readonly Guid OnHoldId = new Guid("D6819EB6-9141-4e83-BBC5-787CBA6E0932");
        public static readonly Guid InProcessId = new Guid("7752F5C5-B19B-4339-A937-0BAD768142A8");
        public static readonly Guid SentId = new Guid("4F037C5D-5365-4A04-9FD8-193209A60B12");
        public static readonly Guid FinishedId = new Guid("A62C1773-C42C-456c-92F3-5FC67382D9A3");

        private UniquelyIdentifiableSticky<PurchaseOrderState> cache;

        public PurchaseOrderState Created => this.Cache[CreatedId];

        public PurchaseOrderState AwaitingApprovalLevel1 => this.Cache[AwaitingApprovalLevel1Id];

        public PurchaseOrderState AwaitingApprovalLevel2 => this.Cache[AwaitingApprovalLevel2Id];

        public PurchaseOrderState Cancelled => this.Cache[CancelledId];

        public PurchaseOrderState Completed => this.Cache[CompletedId];

        public PurchaseOrderState Rejected => this.Cache[RejectedId];

        public PurchaseOrderState Finished => this.Cache[FinishedId];

        public PurchaseOrderState OnHold => this.Cache[OnHoldId];

        public PurchaseOrderState InProcess => this.Cache[InProcessId];

        public PurchaseOrderState Sent => this.Cache[SentId];

        private UniquelyIdentifiableSticky<PurchaseOrderState> Cache => this.cache ??= new UniquelyIdentifiableSticky<PurchaseOrderState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(CreatedId, v => v.Name = "Created");
            merge(AwaitingApprovalLevel1Id, v => v.Name = "Awaiting Approval level 1");
            merge(AwaitingApprovalLevel2Id, v => v.Name = "Awaiting Approval level 2");
            merge(CancelledId, v => v.Name = "Cancelled");
            merge(CompletedId, v => v.Name = "Completed");
            merge(RejectedId, v => v.Name = "Rejected");
            merge(OnHoldId, v => v.Name = "On Hold");
            merge(InProcessId, v => v.Name = "In Process");
            merge(SentId, v => v.Name = "Sent");
            merge(FinishedId, v => v.Name = "Finished");
        }
    }
}
