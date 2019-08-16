
// <copyright file="SalesInvoices.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using Meta;

    public partial class SalesInvoices
    {
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
