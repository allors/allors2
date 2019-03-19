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
        public bool Sent => this.UniqueId == SalesInvoiceItemStates.SentId;

        public bool Paid => this.UniqueId == SalesInvoiceItemStates.PaidId;

        public bool NotPaid => this.UniqueId == SalesInvoiceItemStates.NotPaidId;

        public bool PartiallyPaid => this.UniqueId == SalesInvoiceItemStates.PartiallyPaidId;

        public bool ReadyForPosting => this.UniqueId == SalesInvoiceItemStates.ReadyForPostingId;

        public bool WrittenOff => this.UniqueId == SalesInvoiceItemStates.WrittenOffId;

        public bool Cancelled => this.UniqueId == SalesInvoiceItemStates.CancelledId;
    }
}
