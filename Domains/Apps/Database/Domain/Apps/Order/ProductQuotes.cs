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

            setup.AddDependency(this.ObjectType, M.QuoteState);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var created = new QuoteStates(this.Session).Created;
            var approved = new QuoteStates(this.Session).Approved;
            var ordered = new QuoteStates(this.Session).Ordered;
            var rejected = new QuoteStates(this.Session).Rejected;

            var approve = this.Meta.Approve;
            var reject = this.Meta.Reject;
            var order = this.Meta.Order;
            var addNewQuoteItem = this.Meta.AddNewQuoteItem;

            config.Deny(this.ObjectType, created, order);
            config.Deny(this.ObjectType, ordered, approve, reject, order, addNewQuoteItem);
            config.Deny(this.ObjectType, rejected, approve, reject, order, addNewQuoteItem);

            config.Deny(this.ObjectType, rejected, Operations.Write);
            config.Deny(this.ObjectType, ordered, Operations.Write);
        }
    }
}