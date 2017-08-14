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
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCurrentObjectState)
            {
                this.CurrentObjectState = new RequestItemObjectStates(this.Strategy.Session).Submitted;
            }
        }
        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(this, M.RequestItem.Product, M.RequestItem.ProductFeature, M.RequestItem.Description, M.RequestItem.NeededSkill, M.RequestItem.Deliverable);
            derivation.Validation.AssertExistsAtMostOne(this, M.RequestItem.Product, M.RequestItem.ProductFeature, M.RequestItem.Description, M.RequestItem.NeededSkill, M.RequestItem.Deliverable);

            this.DeriveCurrentObjectState(derivation);
        }

        private void DeriveCurrentObjectState(IDerivation derivation)
        {
            if (this.ExistCurrentObjectState && !this.CurrentObjectState.Equals(this.LastObjectState))
            {
                var currentStatus = new RequestItemStatusBuilder(this.Strategy.Session).WithRequestItemObjectState(this.CurrentObjectState).Build();
                this.AddRequestItemStatus(currentStatus);
                this.CurrentRequestItemStatus = currentStatus;
            }
        }
    }
}