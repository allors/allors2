// <copyright file="RequestsForQuote.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class RequestsForQuote
    {
        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.RequestState);

        protected override void BaseSecure(Security config)
        {
            var anonymous = new RequestStates(this.Session).Anonymous;
            var cancelled = new RequestStates(this.Session).Cancelled;
            var quoted = new RequestStates(this.Session).Quoted;
            var pendingCustomer = new RequestStates(this.Session).PendingCustomer;
            var rejected = new RequestStates(this.Session).Rejected;
            var submitted = new RequestStates(this.Session).Submitted;

            var cancel = this.Meta.Cancel;
            var hold = this.Meta.Hold;
            var submit = this.Meta.Submit;
            var reject = this.Meta.Reject;
            var createQuote = this.Meta.CreateQuote;
            var delete = this.Meta.Delete;

            config.Deny(this.ObjectType, quoted, cancel, hold, submit, reject, createQuote, delete);
            config.Deny(this.ObjectType, submitted, submit);
            config.Deny(this.ObjectType, anonymous, hold, createQuote);
            config.Deny(this.ObjectType, pendingCustomer, hold, createQuote, delete);
            config.Deny(this.ObjectType, rejected, reject, cancel, submit, hold, createQuote);
            config.Deny(this.ObjectType, cancelled, reject, cancel, submit, hold, createQuote);

            config.Deny(this.ObjectType, cancelled, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operations.Write);
            config.Deny(this.ObjectType, quoted, Operations.Write);
        }
    }
}
