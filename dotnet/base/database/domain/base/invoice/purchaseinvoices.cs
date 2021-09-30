// <copyright file="PurchaseInvoices.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;
    using Allors.Meta;

    public partial class PurchaseInvoices
    {
        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.PurchaseInvoiceState);

        protected override void BaseSecure(Security config)
        {
            var created = new PurchaseInvoiceStates(this.Session).Created;
            var awaitingApproval = new PurchaseInvoiceStates(this.Session).AwaitingApproval;
            var notPaid = new PurchaseInvoiceStates(this.Session).NotPaid;
            var partiallyPaid = new PurchaseInvoiceStates(this.Session).PartiallyPaid;
            var paid = new PurchaseInvoiceStates(this.Session).Paid;
            var cancelled = new PurchaseInvoiceStates(this.Session).Cancelled;
            var rejected = new PurchaseInvoiceStates(this.Session).Rejected;
            var revising = new PurchaseInvoiceStates(this.Session).Revising;

            var approve = this.Meta.Approve;
            var reject = this.Meta.Reject;
            var confirm = this.Meta.Confirm;
            var cancel = this.Meta.Cancel;
            var reopen = this.Meta.Reopen;
            var createSalesInvoice = this.Meta.CreateSalesInvoice;
            var delete = this.Meta.Delete;
            var setPaid = this.Meta.SetPaid;
            var revise = this.Meta.Revise;
            var finishRevising = this.Meta.FinishRevising;

            config.Deny(this.ObjectType, created, approve, reject, reopen, createSalesInvoice, setPaid, revise, finishRevising);
            config.Deny(this.ObjectType, cancelled, approve, reject, confirm, cancel, setPaid, createSalesInvoice, delete, revise, finishRevising);
            config.Deny(this.ObjectType, rejected, approve, reject, confirm, cancel, setPaid, createSalesInvoice, delete, revise, finishRevising);
            config.Deny(this.ObjectType, awaitingApproval, confirm, cancel, reopen, setPaid, delete, revise, finishRevising);
            config.Deny(this.ObjectType, notPaid, cancel, reject, approve, confirm, reopen, createSalesInvoice, delete, finishRevising);
            config.Deny(this.ObjectType, partiallyPaid, cancel, reject, approve, confirm, reopen, createSalesInvoice, delete, finishRevising);
            config.Deny(this.ObjectType, paid, cancel, reject, approve, confirm, reopen, createSalesInvoice, delete, finishRevising);

            if (revising != null)
            {
                config.Deny(this.ObjectType, revising, cancel, reject, approve, confirm, reopen, createSalesInvoice, delete, setPaid, revise);
            }

            var except = new HashSet<IOperandType>
            {
                this.Meta.ElectronicDocuments.RoleType,
                this.Meta.Print,
            };

            config.DenyExcept(this.ObjectType, notPaid, except, Operations.Write);
            config.DenyExcept(this.ObjectType, partiallyPaid, except, Operations.Write);
            config.DenyExcept(this.ObjectType, paid, except, Operations.Write);
            config.DenyExcept(this.ObjectType, cancelled, except, Operations.Write);
            config.DenyExcept(this.ObjectType, rejected, except, Operations.Write);
        }
    }
}
