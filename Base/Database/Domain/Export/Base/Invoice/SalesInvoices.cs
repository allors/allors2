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

        protected override void BasePrepare(Setup setup)
        {
            base.BasePrepare(setup);

            setup.AddDependency(this.ObjectType, M.SalesInvoiceState);
        }

        protected override void BaseSecure(Security config)
        {
            

            var notPaid = new SalesInvoiceStates(this.Session).NotPaid;
            var paid = new SalesInvoiceStates(this.Session).Paid;
            var partiallyPaid = new SalesInvoiceStates(this.Session).PartiallyPaid;
            var writtenOff = new SalesInvoiceStates(this.Session).WrittenOff;
            var cancelled = new SalesInvoiceStates(this.Session).Cancelled;
            var readyForPosting = new SalesInvoiceStates(this.Session).ReadyForPosting;

            var send = this.Meta.Send;
            var cancelInvoice = this.Meta.CancelInvoice;
            var writeOff = this.Meta.WriteOff;
            var reopen = this.Meta.Reopen;
            var credit = this.Meta.Credit;
            var setPaid = this.Meta.SetPaid;
            var delete = this.Meta.Delete;

            config.Deny(this.ObjectType, readyForPosting, reopen, credit, setPaid);
            config.Deny(this.ObjectType, notPaid, send, cancelInvoice, reopen, delete);
            config.Deny(this.ObjectType, partiallyPaid, send, cancelInvoice, reopen, delete);
            config.Deny(this.ObjectType, paid, send, writeOff, cancelInvoice, reopen, setPaid, delete);
            config.Deny(this.ObjectType, writtenOff, send, cancelInvoice, writeOff, credit, setPaid, delete);
            config.Deny(this.ObjectType, cancelled, send, cancelInvoice, writeOff, credit, setPaid, delete);

            config.Deny(this.ObjectType, notPaid, Operations.Write);
            config.Deny(this.ObjectType, partiallyPaid, Operations.Write);
            config.Deny(this.ObjectType, paid, Operations.Write, Operations.Execute);
            config.Deny(this.ObjectType, writtenOff, Operations.Write);
            config.Deny(this.ObjectType, cancelled, Operations.Write);
        }
    }
}
