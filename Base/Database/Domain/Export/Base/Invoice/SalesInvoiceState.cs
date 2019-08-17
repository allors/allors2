// <copyright file="SalesInvoiceState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SalesInvoiceState
    {
        public bool Paid => this.UniqueId == SalesInvoiceStates.PaidId;

        public bool PartiallyPaid => this.UniqueId == SalesInvoiceStates.PartiallyPaidId;

        public bool ReadyForPosting => this.UniqueId == SalesInvoiceStates.ReadyForPostingId;

        public bool WrittenOff => this.UniqueId == SalesInvoiceStates.WrittenOffId;

        public bool Cancelled => this.UniqueId == SalesInvoiceStates.CancelledId;
    }
}
