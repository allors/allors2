// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoiceItemState.cs" company="Allors bvba">
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

    public partial class SalesInvoiceItemState
    {
        public bool IsPaid => this.UniqueId == SalesInvoiceItemStates.PaidId;

        public bool IsNotPaid => this.UniqueId == SalesInvoiceItemStates.NotPaidId;

        public bool IsPartiallyPaid => this.UniqueId == SalesInvoiceItemStates.PartiallyPaidId;

        public bool IsReadyForPosting => this.UniqueId == SalesInvoiceItemStates.ReadyForPostingId;

        public bool IsWrittenOff => this.UniqueId == SalesInvoiceItemStates.WrittenOffId;

        public bool IsCancelled => this.UniqueId == SalesInvoiceItemStates.CancelledId;

        public bool IsCancelledByInvoice => this.UniqueId == SalesInvoiceItemStates.CancelledByInvoiceId;
    }
}
