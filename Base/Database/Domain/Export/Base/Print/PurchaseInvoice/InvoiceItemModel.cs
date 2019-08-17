// <copyright file="InvoiceItemModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Linq;

namespace Allors.Domain.Print.PurchaseInvoiceModel
{
    public class InvoiceItemModel
    {
        public InvoiceItemModel(PurchaseInvoiceItem item)
        {
            this.Part = item.ExistPart ? item.Part?.Name : item.Description;
            this.Description = item.ExistPart ? item.Description : string.Empty;
            this.Quantity = item.Quantity;
            // TODO: Where does the currency come from?
            var currency = "€";
            this.Price = item.UnitPrice.ToString("0.00") + " " + currency;
            this.Amount = item.TotalExVat.ToString("0.00") + " " + currency;
            this.Comment = item.Comment;
            this.SupplierProductId = item.Part?.SupplierOfferingsWherePart?.FirstOrDefault(v => v.Supplier.Equals(item.PurchaseInvoiceWherePurchaseInvoiceItem.BilledFrom))?.SupplierProductId;
        }

        public string Part { get; }
        public string Description { get; }
        public decimal Quantity { get; }
        public string Price { get; }
        public string Amount { get; }
        public string Comment { get; }
        public string SupplierProductId { get; }
    }
}
