// <copyright file="PurchaseInvoiceState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class PurchaseInvoiceState
    {
        public bool IsCreated => this.UniqueId == PurchaseInvoiceStates.CreatedId;

        public bool IsAwaitingApproval => this.UniqueId == PurchaseInvoiceStates.AwaitingApprovalId;

        public bool IsPartiallyPaid => this.UniqueId == PurchaseInvoiceStates.PartiallyPaidId;

        public bool IsNotPaid => this.UniqueId == PurchaseInvoiceStates.NotPaidId;

        public bool IsPaid => this.UniqueId == PurchaseInvoiceStates.PaidId;

        public bool IsCancelled => this.UniqueId == PurchaseInvoiceStates.CancelledId;

        public bool IsRejected => this.UniqueId == PurchaseInvoiceStates.RejectedId;

        public bool IsRevising => this.UniqueId == PurchaseInvoiceStates.RevisingId;
    }
}
