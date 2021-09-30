// <copyright file="PurchaseOrderItemState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class PurchaseOrderItemState
    {
        public bool IsCreated => Equals(this.UniqueId, PurchaseOrderItemStates.CreatedId);

        public bool IsAwaitingApproval => Equals(this.UniqueId, PurchaseOrderItemStates.AwaitingApprovalId);

        public bool IsCancelled => Equals(this.UniqueId, PurchaseOrderItemStates.CancelledId);

        public bool IsCompleted => Equals(this.UniqueId, PurchaseOrderItemStates.CompletedId);

        public bool IsRejected => Equals(this.UniqueId, PurchaseOrderItemStates.RejectedId);

        public bool IsFinished => Equals(this.UniqueId, PurchaseOrderItemStates.FinishedId);

        public bool IsOnHold => Equals(this.UniqueId, PurchaseOrderItemStates.OnHoldId);

        public bool IsInProcess => Equals(this.UniqueId, PurchaseOrderItemStates.InProcessId);

        public bool IsSent => Equals(this.UniqueId, PurchaseOrderItemStates.SentId);
    }
}
