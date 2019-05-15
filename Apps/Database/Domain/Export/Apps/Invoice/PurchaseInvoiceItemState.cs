// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseInvoiceItemState.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using System;

    public partial class PurchaseInvoiceItemState
    {
        public bool IsCreated => this.UniqueId == PurchaseInvoiceItemStates.CreatedId;

        public bool IsAwaitingApproval => this.UniqueId == PurchaseInvoiceItemStates.AwaitingApprovalId;

        public bool IsReceived => this.UniqueId == PurchaseInvoiceItemStates.ReceivedId;

        public bool IsPartiallyPaid => this.UniqueId == PurchaseInvoiceItemStates.PartiallyPaidId;

        public bool IsNotPaid  => this.UniqueId == PurchaseInvoiceItemStates.NotPaidId;

        public bool IsPaid => this.UniqueId == PurchaseInvoiceItemStates.PaidId;

        public bool IsRejected => this.UniqueId == PurchaseInvoiceItemStates.RejectedId;

        public bool IsCancelled => this.UniqueId == PurchaseInvoiceItemStates.CancelledId;

        public bool IsCancelledByInvoice => this.UniqueId == PurchaseInvoiceItemStates.CancelledByInvoiceId;
    }
}
