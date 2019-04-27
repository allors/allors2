// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrderItemState.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using System;

    public partial class PurchaseOrderItemState
    {
        public bool IsCreated => Equals(this.UniqueId, PurchaseOrderItemStates.CreatedId);

        public bool IsAwaitingApproval => Equals(this.UniqueId, PurchaseOrderItemStates.AwaitingApprovalId);

        public bool IsCancelled => Equals(this.UniqueId, PurchaseOrderItemStates.CancelledId);

        public bool IsCancelledByOrder => Equals(this.UniqueId, PurchaseOrderItemStates.CancelledByOrderId);

        public bool IsCompleted => Equals(this.UniqueId, PurchaseOrderItemStates.CompletedId);

        public bool IsRejected => Equals(this.UniqueId, PurchaseOrderItemStates.RejectedId);

        public bool IsFinished => Equals(this.UniqueId, PurchaseOrderItemStates.FinishedId);

        public bool IsOnHold => Equals(this.UniqueId, PurchaseOrderItemStates.OnHoldId);

        public bool IsInProcess => Equals(this.UniqueId, PurchaseOrderItemStates.InProcessId);
    }
}