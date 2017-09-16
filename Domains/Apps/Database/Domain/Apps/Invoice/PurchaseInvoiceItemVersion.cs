// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseInvoiceItemVersion.cs" company="Allors bvba">
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

    public partial class PurchaseInvoiceItemVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (PurchaseInvoiceItemVersionBuilder) method.Builder;
            var purchaseInvoiceItem = builder.PurchaseInvoiceItem;

            if (purchaseInvoiceItem != null)
            {
                this.InternalComment = purchaseInvoiceItem.InternalComment;
                this.InvoiceTerms = purchaseInvoiceItem.InvoiceTerms;
                this.InvoiceVatRateItems = purchaseInvoiceItem.InvoiceVatRateItems;
                this.AdjustmentFor = purchaseInvoiceItem.AdjustmentFor;
                this.SerializedInventoryItem = purchaseInvoiceItem.SerializedInventoryItem;
                this.Message = purchaseInvoiceItem.Message;
                this.Quantity = purchaseInvoiceItem.Quantity;
                this.Description = purchaseInvoiceItem.Description;
                this.PurchaseInvoiceItemType = purchaseInvoiceItem.PurchaseInvoiceItemType;
                this.Part = purchaseInvoiceItem.Part;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}