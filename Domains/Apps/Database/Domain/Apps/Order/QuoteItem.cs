// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuoteItem.cs" company="Allors bvba">
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
    using Meta;

    public partial class QuoteItem
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;


        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCurrentObjectState)
            {
                this.CurrentObjectState = new QuoteItemObjectStates(this.Strategy.Session).Submitted;
            }
        }
        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(this, M.QuoteItem.Product, M.QuoteItem.ProductFeature, M.QuoteItem.Deliverable);
            derivation.Validation.AssertExistsAtMostOne(this, M.QuoteItem.Product, M.QuoteItem.ProductFeature, M.QuoteItem.Deliverable);

            if (this.ExistRequestItem)
            {
                this.RequiredByDate = this.RequestItem.RequiredByDate;
            }
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            var isNewVersion =
                !this.ExistCurrentVersion ||
                !object.Equals(this.InternalComment, this.CurrentVersion.InternalComment) ||
                !object.Equals(this.Authorizer, this.CurrentVersion.Authorizer) ||
                !object.Equals(this.Deliverable, this.CurrentVersion.Deliverable) ||
                !object.Equals(this.Product, this.CurrentVersion.Product) ||
                !object.Equals(this.EstimatedDeliveryDate, this.CurrentVersion.EstimatedDeliveryDate) ||
                !object.Equals(this.UnitOfMeasure, this.CurrentVersion.UnitOfMeasure) ||
                !object.Equals(this.ProductFeature, this.CurrentVersion.ProductFeature) ||
                !object.Equals(this.UnitPrice, this.CurrentVersion.UnitPrice) ||
                !object.Equals(this.Skill, this.CurrentVersion.Skill) ||
                !object.Equals(this.WorkEffort, this.CurrentVersion.WorkEffort) ||
                !object.Equals(this.QuoteTerms, this.CurrentVersion.QuoteTerms) ||
                !object.Equals(this.Quantity, this.CurrentVersion.Quantity) ||
                !object.Equals(this.RequestItem, this.CurrentVersion.RequestItem) ||
                !object.Equals(this.CurrentObjectState, this.CurrentVersion.CurrentObjectState);

            var isNewStateVersion =
                !this.ExistCurrentVersion ||
                !object.Equals(this.CurrentObjectState, this.CurrentVersion.CurrentObjectState);

            if (isNewVersion)
            {
                this.PreviousVersion = this.CurrentVersion;
                this.CurrentVersion = new QuoteItemVersionBuilder(this.Strategy.Session).WithQuoteItem(this).Build();
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