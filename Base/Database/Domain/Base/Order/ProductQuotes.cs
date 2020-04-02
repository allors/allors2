// <copyright file="ProductQuotes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class ProductQuotes
    {
        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.QuoteState);

        protected override void BaseSecure(Security config)
        {
            var created = new QuoteStates(this.Session).Created;
            var approved = new QuoteStates(this.Session).Approved;
            var awaitingAcceptance = new QuoteStates(this.Session).AwaitingAcceptance;
            var accepted = new QuoteStates(this.Session).Accepted;
            var ordered = new QuoteStates(this.Session).Ordered;
            var rejected = new QuoteStates(this.Session).Rejected;
            var cancelled = new QuoteStates(this.Session).Cancelled;

            var approve = this.Meta.Approve;
            var send = this.Meta.Send;
            var accept = this.Meta.Accept;
            var reopen = this.Meta.Reopen;
            var reject = this.Meta.Reject;
            var order = this.Meta.Order;
            var cancel = this.Meta.Cancel;

            config.Deny(this.ObjectType, created, order, reopen, send, accept);
            config.Deny(this.ObjectType, approved, approve, accept, order, reopen);
            config.Deny(this.ObjectType, awaitingAcceptance, approve, order, send, reopen, reject);
            config.Deny(this.ObjectType, accepted, approve, send, accept, reject, reopen);
            config.Deny(this.ObjectType, ordered, approve, reject, order, cancel, reopen, send, accept);
            config.Deny(this.ObjectType, rejected, approve, reject, order, send, accept, cancel);
            config.Deny(this.ObjectType, cancelled, cancel, reject, order, send, accept, approve);

            config.Deny(this.ObjectType, approved, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operations.Write);
            config.Deny(this.ObjectType, awaitingAcceptance, Operations.Write);
            config.Deny(this.ObjectType, accepted, Operations.Write);
            config.Deny(this.ObjectType, ordered, Operations.Write);
            config.Deny(this.ObjectType, cancelled, Operations.Write);
        }
    }
}
