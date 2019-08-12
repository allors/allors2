// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuoteStates.cs" company="Allors bvba">
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

    public partial class QuoteStates
    {
        public static readonly Guid CreatedId = new Guid("B1565CD4-D01A-4623-BF19-8C816DF96AA6");
        public static readonly Guid ApprovedId = new Guid("675D6899-1EBB-4FDB-9DC9-B8AEF0A135D2");
        public static readonly Guid OrderedId = new Guid("FE9A6F81-9935-466F-9F71-A537AF046019");
        public static readonly Guid CancelledId = new Guid("ED013479-08AF-4D02-96A7-3FC8B7BE37EF");
        public static readonly Guid RejectedId = new Guid("C897C8E8-2C01-438B-B4C9-B71AD8CCB7C4");

        private UniquelyIdentifiableSticky<QuoteState> stateCache;

        public QuoteState Created => this.StateCache[CreatedId];

        public QuoteState Approved => this.StateCache[ApprovedId];

        public QuoteState Ordered => this.StateCache[OrderedId];

        public QuoteState Cancelled => this.StateCache[CancelledId];

        public QuoteState Rejected => this.StateCache[RejectedId];

        private UniquelyIdentifiableSticky<QuoteState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<QuoteState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            

            new QuoteStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new QuoteStateBuilder(this.Session)
                .WithUniqueId(ApprovedId)
                .WithName("Approved")
                .Build();

            new QuoteStateBuilder(this.Session)
                .WithUniqueId(OrderedId)
                .WithName("Ordered")
                .Build();

            new QuoteStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new QuoteStateBuilder(this.Session)
                .WithUniqueId(RejectedId)
                .WithName("Rejected")
                .Build();
        }
    }
}