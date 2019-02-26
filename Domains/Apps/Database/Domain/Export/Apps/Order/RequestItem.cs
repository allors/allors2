// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestItem.cs" company="Allors bvba">
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

using System;

namespace Allors.Domain
{
    using Meta;

    public partial class RequestItem
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.RequestItem, M.RequestItem.RequestItemState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistRequestItemState)
            {
                this.RequestItemState = new RequestItemStates(this.Strategy.Session).Submitted;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(this, M.RequestItem.Product, M.RequestItem.ProductFeature, M.RequestItem.SerialisedItem, M.RequestItem.Description, M.RequestItem.NeededSkill, M.RequestItem.Deliverable);
            derivation.Validation.AssertExistsAtMostOne(this, M.RequestItem.Product, M.RequestItem.ProductFeature, M.RequestItem.Description, M.RequestItem.NeededSkill, M.RequestItem.Deliverable);
            derivation.Validation.AssertExistsAtMostOne(this, M.RequestItem.SerialisedItem, M.RequestItem.ProductFeature, M.RequestItem.Description, M.RequestItem.NeededSkill, M.RequestItem.Deliverable);

            if (!this.ExistUnitOfMeasure)
            {
                this.UnitOfMeasure = new UnitsOfMeasure(this.Strategy.Session).Piece;
            }

            if (this.RequestWhereRequestItem.RequestState.Equals(new RequestStates(this.Strategy.Session).Cancelled))
            {
                this.Cancel();
            }
        }

        public void AppsCancel(RequestItemCancel method)
        {
            this.RequestItemState = new RequestItemStates(this.Strategy.Session).Cancelled;
        }
    }
}