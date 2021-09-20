// <copyright file="QuoteState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class QuoteState
    {
        public bool IsCreated => Equals(this.UniqueId, QuoteStates.CreatedId);

        public bool IsAwaitingApproval => Equals(this.UniqueId, QuoteStates.AwaitingApprovalId);

        public bool IsInProcess => Equals(this.UniqueId, QuoteStates.InProcessId);

        public bool IsAwaitingAcceptance => Equals(this.UniqueId, QuoteStates.AwaitingAcceptanceId);

        public bool IsAccepted => Equals(this.UniqueId, QuoteStates.AcceptedId);

        public bool IsOrdered => Equals(this.UniqueId, QuoteStates.OrderedId);

        public bool IsCancelled => Equals(this.UniqueId, QuoteStates.CancelledId);

        public bool IsRejected => Equals(this.UniqueId, QuoteStates.RejectedId);
    }
}
