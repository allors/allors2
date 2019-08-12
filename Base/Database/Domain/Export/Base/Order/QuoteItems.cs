// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuoteItems.cs" company="Allors bvba">
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

    public partial class QuoteItems
    {
        protected override void BaseSecure(Security config)
        {
            

            var draft = new QuoteItemStates(this.Session).Draft;
            var cancelled = new QuoteItemStates(this.Session).Cancelled;
            var submitted = new QuoteItemStates(this.Session).Submitted;
            var ordered = new QuoteItemStates(this.Session).Ordered;
            var rejected = new QuoteItemStates(this.Session).Rejected;

            var cancel = this.Meta.Cancel;
            var submit = this.Meta.Submit;
            var delete = this.Meta.Delete;

            config.Deny(this.ObjectType, submitted, submit);
            config.Deny(this.ObjectType, cancelled, cancel, submit);
            config.Deny(this.ObjectType, rejected, cancel, submit);
            config.Deny(this.ObjectType, ordered, cancel, submit, delete);

            config.Deny(this.ObjectType, cancelled, Operations.Write);
            config.Deny(this.ObjectType, ordered, Operations.Write);
        }
    }
}