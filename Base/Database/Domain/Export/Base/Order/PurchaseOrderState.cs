// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductQuoteStates.cs" company="Allors bvba">
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