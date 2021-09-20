// <copyright file="SalesInvoiceType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class SalesInvoiceType
    {
        public bool IsSalesInvoice => this.UniqueId == SalesInvoiceTypes.SalesInvoiceId;

        public bool IsCreditNote => this.UniqueId == SalesInvoiceTypes.CreditNoteId;
    }
}
