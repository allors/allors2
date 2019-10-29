// <copyright file="QuoteItemStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class QuoteItemStates
    {
        private static readonly Guid DraftId = new Guid("84AD17A3-10F7-4FDB-B76A-41BDB1EDB0E6");
        private static readonly Guid SubmittedId = new Guid("E511EA2D-6EB9-428D-A982-B097938A8FF8");
        private static readonly Guid ApprovedId = new Guid("3335810C-9E26-4604-B272-D18B831E79E0");
        private static readonly Guid CancelledId = new Guid("6433F6F7-22D6-4142-8FC5-8941F4F0B6A8");
        private static readonly Guid RejectedId = new Guid("CA8E48EC-5EF7-4082-8347-648B41585485");
        private static readonly Guid OrderedId = new Guid("BF59B586-C35A-423B-9115-DEA1079D905F");

        private UniquelyIdentifiableSticky<QuoteItemState> stateCache;

        public QuoteItemState Draft => this.StateCache[DraftId];

        public QuoteItemState Submitted => this.StateCache[SubmittedId];

        public QuoteItemState Approved => this.StateCache[ApprovedId];

        public QuoteItemState Cancelled => this.StateCache[CancelledId];

        public QuoteItemState Rejected => this.StateCache[RejectedId];

        public QuoteItemState Ordered => this.StateCache[OrderedId];

        private UniquelyIdentifiableSticky<QuoteItemState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<QuoteItemState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            new QuoteItemStateBuilder(this.Session)
                .WithUniqueId(DraftId)
                .WithName("Draft")
                .Build();

            new QuoteItemStateBuilder(this.Session)
                .WithUniqueId(SubmittedId)
                .WithName("Submitted")
                .Build();

            new QuoteItemStateBuilder(this.Session)
                .WithUniqueId(ApprovedId)
                .WithName("Approved")
                .Build();

            new QuoteItemStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new QuoteItemStateBuilder(this.Session)
                .WithUniqueId(RejectedId)
                .WithName("Rejected")
                .Build();

            new QuoteItemStateBuilder(this.Session)
                .WithUniqueId(OrderedId)
                .WithName("Ordered")
                .Build();
        }
    }
}
