// <copyright file="SalesOrderItemInvoiceState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class SalesOrderItemInvoiceState
    {
        public bool NotInvoiced => Equals(this.UniqueId, SalesOrderItemInvoiceStates.NotInvoicedId);

        public bool PartiallyInvoiced => Equals(this.UniqueId, SalesOrderItemInvoiceStates.PartiallyInvoicedId);

        public bool Invoiced => Equals(this.UniqueId, SalesOrderItemInvoiceStates.InvoicedId);
    }
}
