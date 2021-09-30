// <copyright file="PurchaseInvoiceItems.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class PurchaseInvoiceItems
    {
        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.PurchaseInvoiceItemState);

        protected override void BaseSecure(Security config)
        {
            var created = new PurchaseInvoiceItemStates(this.Session).Created;
            var awaitingApproval = new PurchaseInvoiceItemStates(this.Session).AwaitingApproval;
            var paid = new PurchaseInvoiceItemStates(this.Session).Paid;
            var cancelledByinvoice = new PurchaseInvoiceItemStates(this.Session).CancelledByinvoice;
            var rejected = new PurchaseInvoiceItemStates(this.Session).Rejected;
            var revising = new PurchaseInvoiceItemStates(this.Session).Revising;

            var reject = this.Meta.Reject;
            var delete = this.Meta.Delete;

            config.Deny(this.ObjectType, created, reject);
            config.Deny(this.ObjectType, awaitingApproval, delete);

            //if (revising != null)
            //{
            //    config.Deny(this.ObjectType, revising, delete);
            //}

            config.Deny(this.ObjectType, cancelledByinvoice, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, rejected, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, paid, Operations.Execute, Operations.Write);
        }
    }
}
