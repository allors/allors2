// <copyright file="SalesInvoiceItems.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors;
    using Allors.Meta;

    public partial class SalesInvoiceItems
    {
        protected override void BasePrepare(Setup setup)
        {
            base.BasePrepare(setup);

            setup.AddDependency(this.ObjectType, M.SalesInvoiceItemState);
        }

        protected override void BaseSecure(Security config)
        {
            var paid = new SalesInvoiceItemStates(this.Session).Paid;
            var writtenOff = new SalesInvoiceItemStates(this.Session).WrittenOff;
            var cancelled = new SalesInvoiceItemStates(this.Session).Cancelled;
            var cancelledByOrder = new SalesInvoiceItemStates(this.Session).CancelledByInvoice;

            config.Deny(this.ObjectType, paid, Operations.Write, Operations.Execute);
            config.Deny(this.ObjectType, writtenOff, Operations.Write, Operations.Execute);
            config.Deny(this.ObjectType, cancelled, Operations.Write, Operations.Execute);
            config.Deny(this.ObjectType, cancelledByOrder, Operations.Write, Operations.Execute);
        }
    }
}
