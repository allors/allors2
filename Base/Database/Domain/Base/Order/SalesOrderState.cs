// <copyright file="SalesOrderState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class SalesOrderState
    {
        public bool IsProvisional => Equals(this.UniqueId, SalesOrderStates.ProvisionalId);

        public bool IsRequestsApproval => Equals(this.UniqueId, SalesOrderStates.RequestsApprovalId);

        public bool IsReadyForPosting => Equals(this.UniqueId, SalesOrderStates.ReadyForPostingId);

        public bool IsCancelled => Equals(this.UniqueId, SalesOrderStates.CancelledId);

        public bool IsCompleted => Equals(this.UniqueId, SalesOrderStates.CompletedId);

        public bool IsRejected => Equals(this.UniqueId, SalesOrderStates.RejectedId);

        public bool IsFinished => Equals(this.UniqueId, SalesOrderStates.FinishedId);

        public bool IsOnHold => Equals(this.UniqueId, SalesOrderStates.OnHoldId);

        public bool IsInProcess => Equals(this.UniqueId, SalesOrderStates.InProcessId);

        public bool IsAwaitingAcceptance => Equals(this.UniqueId, SalesOrderStates.AwaitingAcceptanceId);
    }
}
