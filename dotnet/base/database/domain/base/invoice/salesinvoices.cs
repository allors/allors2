
// <copyright file="SalesInvoices.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;
    using Allors.Meta;

    public partial class SalesInvoices
    {
        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.SalesInvoiceState);

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

            config.Deny(this.ObjectType, readyForPosting, reopen, credit, setPaid, writeOff);
            config.Deny(this.ObjectType, notPaid, send, cancelInvoice, reopen, delete);
            config.Deny(this.ObjectType, partiallyPaid, send, cancelInvoice, reopen, delete);
            config.Deny(this.ObjectType, paid, send, writeOff, cancelInvoice, reopen, setPaid, delete);
            config.Deny(this.ObjectType, writtenOff, send, cancelInvoice, writeOff, credit, setPaid, delete, reopen);
            config.Deny(this.ObjectType, cancelled, send, cancelInvoice, writeOff, credit, setPaid, delete);

            var except = new HashSet<IOperandType>
            {
                this.Meta.ElectronicDocuments.RoleType,
                this.Meta.Print,
                this.Meta.Credit,
            };

            config.DenyExcept(this.ObjectType, notPaid, except, Operations.Write);
            config.DenyExcept(this.ObjectType, partiallyPaid, except, Operations.Write);
            config.DenyExcept(this.ObjectType, paid, except, Operations.Write, Operations.Execute);
            config.DenyExcept(this.ObjectType, writtenOff, except, Operations.Write);
            config.DenyExcept(this.ObjectType, cancelled, except, Operations.Write);
        }
    }
}
