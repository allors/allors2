// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductQuoteObjectStates.cs" company="Allors bvba">
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

    public partial class ProposalObjectStates
    {
        private static readonly Guid CreatedId = new Guid("F63CF49E-B9FC-477D-8F0B-B48CA872525D");
        private static readonly Guid ApprovedId = new Guid("3CDD8631-7F76-4B81-8089-34E2791EB246");
        private static readonly Guid OrderedId = new Guid("BFB841BF-BABF-4A8D-89F6-F9AE16BB0D16");
        private static readonly Guid RejectedId = new Guid("BE7C7DA1-B347-49BE-A9D9-EDAA81824474");

        private UniquelyIdentifiableCache<ProposalObjectState> stateCache;

        public ProposalObjectState Created => this.StateCache.Get(CreatedId);

        public ProposalObjectState Approved => this.StateCache.Get(ApprovedId);

        public ProposalObjectState Ordered => this.StateCache.Get(OrderedId);

        public ProposalObjectState Rejected => this.StateCache.Get(RejectedId);

        private UniquelyIdentifiableCache<ProposalObjectState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<ProposalObjectState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            new ProposalObjectStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new ProposalObjectStateBuilder(this.Session)
                .WithUniqueId(ApprovedId)
                .WithName("Approved")
                .Build();

            new ProposalObjectStateBuilder(this.Session)
                .WithUniqueId(OrderedId)
                .WithName("Ordered")
                .Build();

            new ProposalObjectStateBuilder(this.Session)
                .WithUniqueId(RejectedId)
                .WithName("Rejected")
                .Build();
        }
    }
}