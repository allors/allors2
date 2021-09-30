// <copyright file="SalesOrderItemStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using Meta;

    public partial class SalesOrderItemStates
    {
        internal static readonly Guid ProvisionalId = new Guid("5B0993B5-5784-4e8d-B1AD-93AFFAC9A913");
        internal static readonly Guid ReadyForPostingId = new Guid("6e4f9535-a7ce-483f-9fbd-c9fd331d355e");
        internal static readonly Guid RequestsApprovalId = new Guid("8d3a4a0a-ed27-4478-baff-ece591068712");
        internal static readonly Guid AwaitingAcceptanceId = new Guid("d3965e9b-764d-4787-87b4-82cb2acb0878");
        internal static readonly Guid OnHoldId = new Guid("3b185d51-af4a-441e-be0d-f91cfcbdb5d8");
        internal static readonly Guid InProcessId = new Guid("e08401f7-1deb-4b27-b0c5-8f034bffedb5");
        internal static readonly Guid CancelledId = new Guid("8B6FD903-B4A6-4360-A63C-9EBDFB7243AA");
        internal static readonly Guid CompletedId = new Guid("AC46B106-D266-46d7-BFD7-4196394A5AE0");
        internal static readonly Guid RejectedId = new Guid("F39F2F64-49A8-4a70-ACBC-B7F581F31EEF");
        internal static readonly Guid FinishedId = new Guid("33C0ED0C-FDFE-45ff-A008-7A638094A94A");

        private UniquelyIdentifiableSticky<SalesOrderItemState> cache;

        public SalesOrderItemState Provisional => this.Cache[ProvisionalId];

        public SalesOrderItemState ReadyForPosting => this.Cache[ReadyForPostingId];

        public SalesOrderItemState RequestsApproval => this.Cache[RequestsApprovalId];

        public SalesOrderItemState AwaitingAcceptance => this.Cache[AwaitingAcceptanceId];

        public SalesOrderItemState Cancelled => this.Cache[CancelledId];

        public SalesOrderItemState Completed => this.Cache[CompletedId];

        public SalesOrderItemState Rejected => this.Cache[RejectedId];

        public SalesOrderItemState Finished => this.Cache[FinishedId];

        public SalesOrderItemState OnHold => this.Cache[OnHoldId];

        public SalesOrderItemState InProcess => this.Cache[InProcessId];

        private UniquelyIdentifiableSticky<SalesOrderItemState> Cache => this.cache ??= new UniquelyIdentifiableSticky<SalesOrderItemState>(this.Session);

        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.InventoryTransactionReason.ObjectType);

        protected override void BaseSetup(Setup setup)
        {
            var reasons = new InventoryTransactionReasons(this.Session);

            var merge = this.Cache.Merger().Action();

            merge(ProvisionalId, v =>
            {
                v.Name = "Provisional";
            });

            merge(CancelledId, v =>
            {
                v.Name = "Cancelled";
                v.AddInventoryTransactionReasonsToCancel(reasons.Reservation);
            });

            merge(CompletedId, v =>
            {
                v.Name = "Completed";
            });

            merge(RejectedId, v =>
            {
                v.Name = "Rejected";
                v.AddInventoryTransactionReasonsToCancel(reasons.Reservation);
            });

            merge(OnHoldId, v =>
            {
                v.Name = "On Hold";
            });

            merge(InProcessId, v =>
            {
                v.Name = "In Process";
                v.AddInventoryTransactionReasonsToCreate(reasons.Reservation);
            });

            merge(FinishedId, v =>
            {
                v.Name = "Finished";
            });

            merge(ReadyForPostingId, v =>
            {
                v.Name = "Ready For Posting";
            });

            merge(RequestsApprovalId, v =>
            {
                v.Name = "Requests Approval";
            });

            merge(AwaitingAcceptanceId, v =>
            {
                v.Name = "Awaiting customer acceptance";
            });
        }
    }
}
