// <copyright file="SalesOrderInvoiceState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class SalesOrderInvoiceState
    {
        public bool NotInvoiced => Equals(this.UniqueId, SalesOrderInvoiceStates.NotInvoicedId);

        public bool PartiallyInvoiced => Equals(this.UniqueId, SalesOrderInvoiceStates.PartiallyInvoicedId);

        public bool Invoiced => Equals(this.UniqueId, SalesOrderInvoiceStates.InvoicedId);
    }
}
