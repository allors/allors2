// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrderItemVersion.cs" company="Allors bvba">
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
    public partial class SalesOrderItemVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (SalesOrderItemVersionBuilder) method.Builder;
            var salesOrderItem = builder.SalesOrderItem;

            if (salesOrderItem != null)
            {
                this.InternalComment = salesOrderItem.InternalComment;
                this.BudgetItem = salesOrderItem.BudgetItem;
                this.QuantityOrdered = salesOrderItem.QuantityOrdered;
                this.Description = salesOrderItem.Description;
                this.CorrespondingPurchaseOrder = salesOrderItem.CorrespondingPurchaseOrder;
                this.QuoteItem = salesOrderItem.QuoteItem;
                this.AssignedDeliveryDate = salesOrderItem.AssignedDeliveryDate;
                this.OrderTerms = salesOrderItem.OrderTerms;
                this.ShippingInstruction = salesOrderItem.ShippingInstruction;
                this.Associations = salesOrderItem.Associations;
                this.Message = salesOrderItem.Message;
                this.OrderedWithFeatures = salesOrderItem.OrderedWithFeatures;
                this.RequiredProfitMargin = salesOrderItem.RequiredProfitMargin;
                this.QuantityShipNow = salesOrderItem.QuantityShipNow;
                this.RequiredMarkupPercentage = salesOrderItem.RequiredMarkupPercentage;
                this.AssignedShipToAddress = salesOrderItem.AssignedShipToAddress;
                this.QuantityReturned = salesOrderItem.QuantityReturned;
                this.SalesRep = salesOrderItem.SalesRep;
                this.AssignedShipToParty = salesOrderItem.AssignedShipToParty;
                this.ReservedFromInventoryItem = salesOrderItem.ReservedFromInventoryItem;
                this.Product = salesOrderItem.Product;
                this.ProductFeature = salesOrderItem.ProductFeature;
                this.CurrentObjectState = salesOrderItem.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}