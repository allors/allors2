// <copyright file="QuoteStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class QuoteStates
    {
        public static readonly Guid CreatedId = new Guid("B1565CD4-D01A-4623-BF19-8C816DF96AA6");
        public static readonly Guid AwaitingApprovalId = new Guid("3e315c74-f8e7-4538-977e-9446e220d2b1");
        public static readonly Guid InProcessId = new Guid("675D6899-1EBB-4FDB-9DC9-B8AEF0A135D2");
        public static readonly Guid AwaitingAcceptanceId = new Guid("324beb70-937f-4c4d-a7e9-2e3063c88a62");
        public static readonly Guid AcceptedId = new Guid("3943f87c-f098-49c8-89ba-12047c826777");
        public static readonly Guid OrderedId = new Guid("FE9A6F81-9935-466F-9F71-A537AF046019");
        public static readonly Guid CancelledId = new Guid("ED013479-08AF-4D02-96A7-3FC8B7BE37EF");
        public static readonly Guid RejectedId = new Guid("C897C8E8-2C01-438B-B4C9-B71AD8CCB7C4");

        private UniquelyIdentifiableSticky<QuoteState> cache;

        public QuoteState Created => this.Cache[CreatedId];

        public QuoteState AwaitingApproval => this.Cache[AwaitingApprovalId];

        public QuoteState InProcess => this.Cache[InProcessId];

        public QuoteState AwaitingAcceptance => this.Cache[AwaitingAcceptanceId];

        public QuoteState Accepted => this.Cache[AcceptedId];

        public QuoteState Ordered => this.Cache[OrderedId];

        public QuoteState Cancelled => this.Cache[CancelledId];

        public QuoteState Rejected => this.Cache[RejectedId];

        private UniquelyIdentifiableSticky<QuoteState> Cache => this.cache ??= new UniquelyIdentifiableSticky<QuoteState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(CreatedId, v => v.Name = "Created");
            merge(AwaitingApprovalId, v => v.Name = "Awaiting Approval");
            merge(InProcessId, v => v.Name = "In process");
            merge(AwaitingAcceptanceId, v => v.Name = "Awaiting customer acceptance");
            merge(AcceptedId, v => v.Name = "Accepted");
            merge(OrderedId, v => v.Name = "Ordered");
            merge(CancelledId, v => v.Name = "Cancelled");
            merge(RejectedId, v => v.Name = "Rejected");
        }
    }
}
