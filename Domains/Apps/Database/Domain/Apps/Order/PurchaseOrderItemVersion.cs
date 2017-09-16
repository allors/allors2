// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrderItemVersion.cs" company="Allors bvba">
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
    public partial class PurchaseOrderItemVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (PurchaseOrderItemVersionBuilder) method.Builder;
            var purchaseOrderItem = builder.PurchaseOrderItem;

            if (purchaseOrderItem != null)
            {
                this.InternalComment = purchaseOrderItem.InternalComment;
                this.BudgetItem = purchaseOrderItem.BudgetItem;
                this.QuantityOrdered = purchaseOrderItem.QuantityOrdered;
                this.Description = purchaseOrderItem.Description;
                this.CorrespondingPurchaseOrder = purchaseOrderItem.CorrespondingPurchaseOrder;
                this.QuoteItem = purchaseOrderItem.QuoteItem;
                this.AssignedDeliveryDate = purchaseOrderItem.AssignedDeliveryDate;
                this.OrderTerms = purchaseOrderItem.OrderTerms;
                this.ShippingInstruction = purchaseOrderItem.ShippingInstruction;
                this.Associations = purchaseOrderItem.Associations;
                this.Message = purchaseOrderItem.Message;
                this.QuantityReceived = purchaseOrderItem.QuantityReceived;
                this.Product = purchaseOrderItem.Product;
                this.Part = purchaseOrderItem.Part;
                this.CurrentObjectState = purchaseOrderItem.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}