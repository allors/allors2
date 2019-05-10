// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductQuoteStates.cs" company="Allors bvba">
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

    public partial class QuoteState
    {
        public bool IsCreated => Equals(this.UniqueId, QuoteStates.CreatedId);

        public bool IsApproved => Equals(this.UniqueId, QuoteStates.ApprovedId);

        public bool IsOrdered => Equals(this.UniqueId, QuoteStates.OrderedId);

        public bool IsCancelled => Equals(this.UniqueId, QuoteStates.CancelledId);

        public bool IsRejected => Equals(this.UniqueId, QuoteStates.RejectedId);
    }
}