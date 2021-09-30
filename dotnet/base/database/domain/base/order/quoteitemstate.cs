// <copyright file="QuoteItemState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class QuoteItemState
    {
        public bool IsCancelled => Equals(this.UniqueId, QuoteItemStates.CancelledId);

        public bool IsDraft => Equals(this.UniqueId, QuoteItemStates.DraftId);

        public bool IsApproved => Equals(this.UniqueId, QuoteItemStates.InProcessId);

        public bool IsAwaitingAcceptance => Equals(this.UniqueId, QuoteItemStates.AwaitingAcceptanceId);

        public bool IsAccepted => Equals(this.UniqueId, QuoteItemStates.AcceptedId);

        public bool IsOrdered => Equals(this.UniqueId, QuoteItemStates.OrderedId);

        public bool IsRejected => Equals(this.UniqueId, QuoteItemStates.RejectedId);

        public bool IsSubmitted => Equals(this.UniqueId, QuoteItemStates.SubmittedId);
    }
}
