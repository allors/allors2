// <copyright file="RequestState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class RequestState
    {
        public bool IsAnonymous => Equals(this.UniqueId, RequestStates.AnonymousId);

        public bool IsSubmitted => Equals(this.UniqueId, RequestStates.SubmittedId);

        public bool IsCancelled => Equals(this.UniqueId, RequestStates.CancelledId);

        public bool IsQuoted => Equals(this.UniqueId, RequestStates.QuotedId);

        public bool IsPendingCustomer => Equals(this.UniqueId, RequestStates.PendingCustomerId);

        public bool IsRejected => Equals(this.UniqueId, RequestStates.RejectedId);
    }
}
