// <copyright file="SalesInvoiceState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class SalesInvoiceState
    {
        public bool IsPaid => this.UniqueId == SalesInvoiceStates.PaidId;

        public bool IsPartiallyPaid => this.UniqueId == SalesInvoiceStates.PartiallyPaidId;

        public bool IsReadyForPosting => this.UniqueId == SalesInvoiceStates.ReadyForPostingId;

        public bool IsWrittenOff => this.UniqueId == SalesInvoiceStates.WrittenOffId;

        public bool IsCancelled => this.UniqueId == SalesInvoiceStates.CancelledId;
    }
}
