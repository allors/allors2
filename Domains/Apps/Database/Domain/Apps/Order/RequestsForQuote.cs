// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestsForQuote.cs" company="Allors bvba">
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
    using Meta;

    public partial class RequestsForQuote
    {
        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, M.RequestObjectState);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var anonymous = new RequestObjectStates(this.Session).Anonymous;
            var cancelled = new RequestObjectStates(this.Session).Cancelled;
            var quoted = new RequestObjectStates(this.Session).Quoted;
            var pendingCustomer = new RequestObjectStates(this.Session).PendingCustomer;
            var rejected = new RequestObjectStates(this.Session).Rejected;
            var submitted = new RequestObjectStates(this.Session).Submitted;

            var cancel = this.Meta.Cancel;
            var hold = this.Meta.Hold;
            var submit = this.Meta.Submit;
            var reject = this.Meta.Reject;
            var createQuote = this.Meta.CreateQuote;

            config.Deny(this.ObjectType, quoted, createQuote);
            config.Deny(this.ObjectType, submitted, submit);
            config.Deny(this.ObjectType, anonymous, hold, createQuote);
            config.Deny(this.ObjectType, pendingCustomer, hold, createQuote);
            config.Deny(this.ObjectType, rejected, reject, cancel, submit, hold, createQuote);
            config.Deny(this.ObjectType, cancelled, reject, cancel, submit, hold, createQuote);

            config.Deny(this.ObjectType, cancelled, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operations.Write);
            config.Deny(this.ObjectType, quoted, Operations.Write);
        }
    }
}