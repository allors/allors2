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

    public partial class ProductQuotes
    {
        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, M.QuoteObjectState);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var created = new QuoteObjectStates(this.Session).Created;
            var approved = new QuoteObjectStates(this.Session).Approved;
            var ordered = new QuoteObjectStates(this.Session).Ordered;
            var rejected = new QuoteObjectStates(this.Session).Rejected;

            var approve = this.Meta.Approve;
            var reject = this.Meta.Reject;
            var order = this.Meta.Order;

            config.Deny(this.ObjectType, created, order);
            config.Deny(this.ObjectType, ordered, approve, reject, order);
            config.Deny(this.ObjectType, rejected, approve, reject, order);

            config.Deny(this.ObjectType, rejected, Operations.Write);
            config.Deny(this.ObjectType, ordered, Operations.Write);
        }
    }
}