// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoices.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    public partial class SalesInvoices
    {
        public static readonly SalesInvoice[] EmptyArray = new SalesInvoice[0];

        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, SalesInvoiceObjectStates.Meta.ObjectType);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
            config.GrantCustomer(this.ObjectType, Operation.Read);
            config.GrantSales(this.ObjectType, full);

            var sent = new SalesInvoiceObjectStates(Session).Sent;
            var paid = new SalesInvoiceObjectStates(Session).Paid;
            var partiallyPaid = new SalesInvoiceObjectStates(Session).PartiallyPaid;
            var writtenOff = new SalesInvoiceObjectStates(Session).WrittenOff;
            var cancelled = new SalesInvoiceObjectStates(Session).Cancelled;

            var sendId = Meta.Send;
            var cancelInvoiceId = Meta.CancelInvoice;

            config.Deny(this.ObjectType, sent, sendId, cancelInvoiceId);
            config.Deny(this.ObjectType, partiallyPaid, sendId, cancelInvoiceId);

            config.Deny(this.ObjectType, paid, Operation.Write, Operation.Execute);
            config.Deny(this.ObjectType, writtenOff, Operation.Write, Operation.Execute);
            config.Deny(this.ObjectType, cancelled, Operation.Write, Operation.Execute);
        }
    }
}
