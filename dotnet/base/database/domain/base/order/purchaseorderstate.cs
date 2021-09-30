// <copyright file="PurchaseOrderState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class PurchaseOrderState
    {
        public bool IsCreated => Equals(this.UniqueId, PurchaseOrderStates.CreatedId);

        public bool IsAwaitingApprovalLevel1 => Equals(this.UniqueId, PurchaseOrderStates.AwaitingApprovalLevel1Id);

        public bool IsAwaitingApprovalLevel2 => Equals(this.UniqueId, PurchaseOrderStates.AwaitingApprovalLevel2Id);

        public bool IsCancelled => Equals(this.UniqueId, PurchaseOrderStates.CancelledId);

        public bool IsCompleted => Equals(this.UniqueId, PurchaseOrderStates.CompletedId);

        public bool IsRejected => Equals(this.UniqueId, PurchaseOrderStates.RejectedId);

        public bool IsOnHold => Equals(this.UniqueId, PurchaseOrderStates.OnHoldId);

        public bool IsInProcess => Equals(this.UniqueId, PurchaseOrderStates.InProcessId);

        public bool IsSent => Equals(this.UniqueId, PurchaseOrderStates.SentId);

        public bool IsFinished => Equals(this.UniqueId, PurchaseOrderStates.FinishedId);
    }
}
