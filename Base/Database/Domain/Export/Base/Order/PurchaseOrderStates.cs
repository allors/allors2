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

        private UniquelyIdentifiableSticky<PurchaseOrderState> stateCache;

        public PurchaseOrderState Created => this.StateCache[CreatedId];

        public PurchaseOrderState AwaitingApprovalLevel1 => this.StateCache[AwaitingApprovalLevel1Id];

        public PurchaseOrderState AwaitingApprovalLevel2 => this.StateCache[AwaitingApprovalLevel2Id];

        public PurchaseOrderState Cancelled => this.StateCache[CancelledId];

        public PurchaseOrderState Completed => this.StateCache[CompletedId];

        public PurchaseOrderState Rejected => this.StateCache[RejectedId];

        public PurchaseOrderState Finished => this.StateCache[FinishedId];

        public PurchaseOrderState OnHold => this.StateCache[OnHoldId];

        public PurchaseOrderState InProcess => this.StateCache[InProcessId];

        public PurchaseOrderState Sent => this.StateCache[SentId];

        private UniquelyIdentifiableSticky<PurchaseOrderState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<PurchaseOrderState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            new PurchaseOrderStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new PurchaseOrderStateBuilder(this.Session)
                .WithUniqueId(AwaitingApprovalLevel1Id)
                .WithName("Awaiting Approval level 1")
                .Build();

            new PurchaseOrderStateBuilder(this.Session)
                .WithUniqueId(AwaitingApprovalLevel2Id)
                .WithName("Awaiting Approval level 2")
                .Build();

            new PurchaseOrderStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new PurchaseOrderStateBuilder(this.Session)
                .WithUniqueId(CompletedId)
                .WithName("Completed")
                .Build();

            new PurchaseOrderStateBuilder(this.Session)
                .WithUniqueId(RejectedId)
                .WithName("Rejected")
                .Build();

            new PurchaseOrderStateBuilder(this.Session)
                .WithUniqueId(OnHoldId)
                .WithName("On Hold")
                .Build();

            new PurchaseOrderStateBuilder(this.Session)
                .WithUniqueId(InProcessId)
                .WithName("In Process")
                .Build();

            new PurchaseOrderStateBuilder(this.Session)
                .WithUniqueId(SentId)
                .WithName("Sent")
                .Build();

            new PurchaseOrderStateBuilder(this.Session)
                .WithUniqueId(FinishedId)
                .WithName("Finished")
                .Build();
        }
    }
}
