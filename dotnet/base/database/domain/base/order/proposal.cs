// <copyright file="Proposal.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Meta;

    public partial class Proposal
    {
        public bool IsDeletable =>
         (this.QuoteState.Equals(new QuoteStates(this.Strategy.Session).Created)
             || this.QuoteState.Equals(new QuoteStates(this.Strategy.Session).Cancelled)
             || this.QuoteState.Equals(new QuoteStates(this.Strategy.Session).Rejected))
         && this.QuoteItems.All(v => v.IsDeletable);

        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.Proposal, M.Proposal.QuoteState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void BaseOnDerive(ObjectOnDerive method) => this.Sync(this.Strategy.Session);

        public void BaseOnPostDerive(ObjectOnPostDerive method)
        {
            var deletePermission = new Permissions(this.Strategy.Session).Get(this.Meta.ObjectType, this.Meta.Delete, Operations.Execute);
            if (this.IsDeletable)
            {
                this.RemoveDeniedPermission(deletePermission);
            }
            else
            {
                this.AddDeniedPermission(deletePermission);
            }
        }

        private void Sync(ISession session)
        {
            // session.Prefetch(this.SyncPrefetch, this);
            foreach (QuoteItem quoteItem in this.QuoteItems)
            {
                quoteItem.Sync(this);
            }
        }
    }
}
