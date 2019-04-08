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
        private static readonly Guid CreatedId = new Guid("B1565CD4-D01A-4623-BF19-8C816DF96AA6");
        private static readonly Guid ApprovedId = new Guid("675D6899-1EBB-4FDB-9DC9-B8AEF0A135D2");
        private static readonly Guid OrderedId = new Guid("FE9A6F81-9935-466F-9F71-A537AF046019");
        private static readonly Guid CancelledId = new Guid("ED013479-08AF-4D02-96A7-3FC8B7BE37EF");
        private static readonly Guid RejectedId = new Guid("C897C8E8-2C01-438B-B4C9-B71AD8CCB7C4");

        private UniquelyIdentifiableSticky<QuoteState> stateCache;

        public bool IsCreated => QuoteStates.CreatedId.Equals(this.UniqueId);

        public bool IsApproved => QuoteStates.ApprovedId.Equals(this.UniqueId);

        public bool IsOrdered => QuoteStates.OrderedId.Equals(this.UniqueId);

        public bool IsCancelled => QuoteStates.CancelledId.Equals(this.UniqueId);

        public bool IsRejected => QuoteStates.RejectedId.Equals(this.UniqueId);

    }
}