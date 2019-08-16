
// <copyright file="QuoteItemModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.ProductQuoteModel
{
    public class QuoteItemModel
    {
        public QuoteItemModel(QuoteItem item)
        {
            var product = item.Product;
            var serialisedItem = item.SerialisedItem;

            this.Reference = item.InvoiceItemType?.Name;
            this.Product = serialisedItem?.Name ?? product?.Name;
            this.Description = serialisedItem?.Description ?? product?.Description;
            this.Details = item.Details;
            this.Quantity = item.Quantity;
            // TODO: Where does the currency come from?
            var currency = "€";
            this.Price = item.UnitPrice.ToString("0.00") + " " + currency;

            // TODO: Make TotalPrice a derived field on ProductQuote
            var totalPrice = item.Quantity * item.UnitPrice;
            this.Amount = totalPrice.ToString("0.00") + " " + currency;

            this.Comment = item.Comment;
        }

        public string Reference { get; }
        public string Product { get; }
        public string Description { get; }
        public string Details { get; }
        public decimal Quantity { get; }
        public string Price { get; }
        public string Amount { get; }

        public string Comment { get; }
    }
}
