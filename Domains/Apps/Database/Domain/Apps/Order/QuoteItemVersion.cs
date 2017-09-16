// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuoteItemVersion.cs" company="Allors bvba">
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
    public partial class QuoteItemVersion
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (QuoteItemVersionBuilder) method.Builder;
            var quoteItem = builder.QuoteItem;

            if (quoteItem != null)
            {
                this.InternalComment = quoteItem.InternalComment;
                this.Authorizer = quoteItem.Authorizer;
                this.Deliverable = quoteItem.Deliverable;
                this.Product = quoteItem.Product;
                this.EstimatedDeliveryDate = quoteItem.EstimatedDeliveryDate;
                this.UnitOfMeasure = quoteItem.UnitOfMeasure;
                this.ProductFeature = quoteItem.ProductFeature;
                this.UnitPrice = quoteItem.UnitPrice;
                this.Skill = quoteItem.Skill;
                this.WorkEffort = quoteItem.WorkEffort;
                this.QuoteTerms = quoteItem.QuoteTerms;
                this.Quantity = quoteItem.Quantity;
                this.RequestItem = quoteItem.RequestItem;
                this.CurrentObjectState = quoteItem.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}