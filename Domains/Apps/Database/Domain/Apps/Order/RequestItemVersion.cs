// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestItemVersion.cs" company="Allors bvba">
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
    public partial class RequestItemVersion
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (RequestItemVersionBuilder) method.Builder;
            var requestItem = builder.RequestItem;

            if (requestItem != null)
            {
                this.InternalComment= requestItem.InternalComment;
                this.Description = requestItem.Description;
                this.Quantity = requestItem.Quantity;
                this.UnitOfMeasure = requestItem.UnitOfMeasure;
                this.Requirements = requestItem.Requirements;
                this.Deliverable = requestItem.Deliverable;
                this.UnitOfMeasure = requestItem.UnitOfMeasure;
                this.ProductFeature = requestItem.ProductFeature;
                this.NeededSkill = requestItem.NeededSkill;
                this.Product = requestItem.Product;
                this.MaximumAllowedPrice = requestItem.MaximumAllowedPrice;
                this.RequiredByDate = requestItem.RequiredByDate;
                this.CurrentObjectState = requestItem.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}