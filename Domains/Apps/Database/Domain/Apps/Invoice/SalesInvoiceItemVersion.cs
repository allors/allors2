// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoiceItemVersion.cs" company="Allors bvba">
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

    public partial class SalesInvoiceItemVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (SalesInvoiceItemVersionBuilder) method.Builder;
            var salesInvoiceItem = builder.SalesInvoiceItem;

            if (salesInvoiceItem != null)
            {
                this.InternalComment = salesInvoiceItem.InternalComment;
                this.InvoiceTerms = salesInvoiceItem.InvoiceTerms;
                this.InvoiceVatRateItems = salesInvoiceItem.InvoiceVatRateItems;
                this.AdjustmentFor = salesInvoiceItem.AdjustmentFor;
                this.SerializedInventoryItem = salesInvoiceItem.SerializedInventoryItem;
                this.Message = salesInvoiceItem.Message;
                this.Quantity = salesInvoiceItem.Quantity;
                this.Description = salesInvoiceItem.Description;
                this.ProductFeature = salesInvoiceItem.ProductFeature;
                this.RequiredProfitMargin = salesInvoiceItem.RequiredProfitMargin;
                this.Product = salesInvoiceItem.Product;
                this.SalesInvoiceItemType = salesInvoiceItem.SalesInvoiceItemType;
                this.SalesRep = salesInvoiceItem.SalesRep;
                this.TimeEntries = salesInvoiceItem.TimeEntries;
                this.RequiredMarkupPercentage = salesInvoiceItem.RequiredMarkupPercentage;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}