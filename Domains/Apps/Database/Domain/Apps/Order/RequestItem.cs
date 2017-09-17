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
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            var isNewVersion =
                !this.ExistCurrentVersion ||
                !object.Equals(this.InternalComment, this.CurrentVersion.InternalComment) ||
                !object.Equals(this.Description, this.CurrentVersion.Description) ||
                !object.Equals(this.Quantity, this.CurrentVersion.Quantity) ||
                !object.Equals(this.UnitOfMeasure, this.CurrentVersion.UnitOfMeasure) ||
                !object.Equals(this.Requirements, this.CurrentVersion.Requirements) ||
                !object.Equals(this.Deliverable, this.CurrentVersion.Deliverable) ||
                !object.Equals(this.ProductFeature, this.CurrentVersion.ProductFeature) ||
                !object.Equals(this.NeededSkill, this.CurrentVersion.NeededSkill) ||
                !object.Equals(this.Product, this.CurrentVersion.Product) ||
                !object.Equals(this.MaximumAllowedPrice, this.CurrentVersion.MaximumAllowedPrice) ||
                !object.Equals(this.RequiredByDate, this.CurrentVersion.RequiredByDate) ||
                !object.Equals(this.CurrentObjectState, this.CurrentVersion.CurrentObjectState);

            var isNewStateVersion =
                !this.ExistCurrentVersion ||
                !object.Equals(this.CurrentObjectState, this.CurrentVersion.CurrentObjectState);

            if (isNewVersion)
            {
                this.PreviousVersion = this.CurrentVersion;
                this.CurrentVersion = new RequestItemVersionBuilder(this.Strategy.Session).WithRequestItem(this).Build();
                this.AddAllVersion(this.CurrentVersion);
            }

            if (isNewStateVersion)
            {
                this.CurrentStateVersion = CurrentVersion;
                this.AddAllStateVersion(this.CurrentStateVersion);
            }
        }
    }
}