// <copyright file="QuoteState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class QuoteState
    {
        public bool IsCreated => Equals(this.UniqueId, QuoteStates.CreatedId);

        public bool IsApproved => Equals(this.UniqueId, QuoteStates.ApprovedId);

        public bool IsOrdered => Equals(this.UniqueId, QuoteStates.OrderedId);

        public bool IsSent => Equals(this.UniqueId, QuoteStates.SentId);

        public bool IsCancelled => Equals(this.UniqueId, QuoteStates.CancelledId);

        public bool IsRejected => Equals(this.UniqueId, QuoteStates.RejectedId);
    }
}
