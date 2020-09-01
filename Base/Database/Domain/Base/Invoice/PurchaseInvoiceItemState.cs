// <copyright file="PurchaseInvoiceItemState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class PurchaseInvoiceItemState
    {
        public bool IsCreated => this.UniqueId == PurchaseInvoiceItemStates.CreatedId;

        public bool IsAwaitingApproval => this.UniqueId == PurchaseInvoiceItemStates.AwaitingApprovalId;

        public bool IsPartiallyPaid => this.UniqueId == PurchaseInvoiceItemStates.PartiallyPaidId;

        public bool IsNotPaid => this.UniqueId == PurchaseInvoiceItemStates.NotPaidId;

        public bool IsPaid => this.UniqueId == PurchaseInvoiceItemStates.PaidId;

        public bool IsRejected => this.UniqueId == PurchaseInvoiceItemStates.RejectedId;

        public bool IsCancelledByInvoice => this.UniqueId == PurchaseInvoiceItemStates.CancelledByInvoiceId;

        public bool IsRevising => this.UniqueId == PurchaseInvoiceItemStates.RevisingId;
    }
}
