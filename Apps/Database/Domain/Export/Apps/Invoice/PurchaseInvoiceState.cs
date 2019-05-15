// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseInvoiceState.cs" company="Allors bvba">
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

    public partial class PurchaseInvoiceState
    {
        public bool IsCreated => this.UniqueId == PurchaseInvoiceStates.CreatedId;

        public bool IsAwaitingApproval => this.UniqueId == PurchaseInvoiceStates.AwaitingApprovalId;

        public bool IsReceived => this.UniqueId == PurchaseInvoiceStates.ReceivedId;

        public bool IsPartiallyPaid => this.UniqueId == PurchaseInvoiceStates.PartiallyPaidId;

        public bool IsNotPaid => this.UniqueId == PurchaseInvoiceStates.NotPaidId;

        public bool IsPaid => this.UniqueId == PurchaseInvoiceStates.PaidId;

        public bool IsCancelled => this.UniqueId == PurchaseInvoiceStates.CancelledId;

        public bool IsRejected => this.UniqueId == PurchaseInvoiceStates.RejectedId;
    }
}
