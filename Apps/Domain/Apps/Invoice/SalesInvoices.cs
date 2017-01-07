// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoices.cs" company="Allors bvba">
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

    public partial class SalesInvoices
    {
        public static readonly SalesInvoice[] EmptyArray = new SalesInvoice[0];

        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, M.SalesInvoiceObjectState);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var sent = new SalesInvoiceObjectStates(this.Session).Sent;
            var paid = new SalesInvoiceObjectStates(this.Session).Paid;
            var partiallyPaid = new SalesInvoiceObjectStates(this.Session).PartiallyPaid;
            var writtenOff = new SalesInvoiceObjectStates(this.Session).WrittenOff;
            var cancelled = new SalesInvoiceObjectStates(this.Session).Cancelled;

            var sendId = this.Meta.Send;
            var cancelInvoiceId = this.Meta.CancelInvoice;

            config.Deny(this.ObjectType, sent, sendId, cancelInvoiceId);
            config.Deny(this.ObjectType, partiallyPaid, sendId, cancelInvoiceId);

            config.Deny(this.ObjectType, paid, Operations.Write, Operations.Execute);
            config.Deny(this.ObjectType, writtenOff, Operations.Write, Operations.Execute);
            config.Deny(this.ObjectType, cancelled, Operations.Write, Operations.Execute);
        }
    }
}
