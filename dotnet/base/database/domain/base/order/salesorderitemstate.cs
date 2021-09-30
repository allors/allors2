// <copyright file="SalesOrderItemState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class SalesOrderItemState
    {
        public bool IsProvisional => Equals(this.UniqueId, SalesOrderItemStates.ProvisionalId);

        public bool IsReadyForPosting => Equals(this.UniqueId, SalesOrderItemStates.ReadyForPostingId);

        public bool IsRequestsApproval => Equals(this.UniqueId, SalesOrderItemStates.RequestsApprovalId);

        public bool IsAwaitingAcceptance => Equals(this.UniqueId, SalesOrderItemStates.AwaitingAcceptanceId);

        public bool IsInProcess => Equals(this.UniqueId, SalesOrderItemStates.InProcessId);

        public bool IsCancelled => Equals(this.UniqueId, SalesOrderItemStates.CancelledId);

        public bool IsCompleted => Equals(this.UniqueId, SalesOrderItemStates.CompletedId);

        public bool IsRejected => Equals(this.UniqueId, SalesOrderItemStates.RejectedId);

        public bool IsFinished => Equals(this.UniqueId, SalesOrderItemStates.FinishedId);

        public bool IsOnHold => Equals(this.UniqueId, SalesOrderItemStates.OnHoldId);
    }
}
