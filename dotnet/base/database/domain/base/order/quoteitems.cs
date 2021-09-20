// <copyright file="QuoteItems.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class QuoteItems
    {
        protected override void BaseSecure(Security config)
        {
            var draft = new QuoteItemStates(this.Session).Draft;
            var cancelled = new QuoteItemStates(this.Session).Cancelled;
            var submitted = new QuoteItemStates(this.Session).Submitted;
            var approved = new QuoteItemStates(this.Session).InProcess;
            var awaitingAcceptance = new QuoteItemStates(this.Session).AwaitingAcceptance;
            var awaitingApproval = new QuoteItemStates(this.Session).AwaitingApproval;
            var accepted = new QuoteItemStates(this.Session).Accepted;
            var ordered = new QuoteItemStates(this.Session).Ordered;
            var rejected = new QuoteItemStates(this.Session).Rejected;

            var cancel = this.Meta.Cancel;
            var submit = this.Meta.Submit;
            var delete = this.Meta.Delete;

            config.Deny(this.ObjectType, submitted, submit);
            config.Deny(this.ObjectType, awaitingAcceptance, submit, delete);
            config.Deny(this.ObjectType, awaitingApproval, submit, delete);
            config.Deny(this.ObjectType, accepted, cancel, submit, delete);
            config.Deny(this.ObjectType, cancelled, cancel, submit);
            config.Deny(this.ObjectType, rejected, cancel, submit);
            config.Deny(this.ObjectType, ordered, cancel, submit, delete);

            config.Deny(this.ObjectType, cancelled, Operations.Write);
            config.Deny(this.ObjectType, awaitingApproval, Operations.Write);
            config.Deny(this.ObjectType, awaitingAcceptance, Operations.Write);
            config.Deny(this.ObjectType, accepted, Operations.Write);
            config.Deny(this.ObjectType, ordered, Operations.Write);
            config.Deny(this.ObjectType, approved, Operations.Write);
        }
    }
}
