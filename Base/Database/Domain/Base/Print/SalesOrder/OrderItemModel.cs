// <copyright file="OrderItemModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Markdig;

namespace Allors.Domain.Print.SalesOrderModel
{
    public class OrderItemModel
    {
        public OrderItemModel(SalesOrderItem item)
        {
            this.Reference = item.InvoiceItemType?.Name;
            this.Product = item.Product?.Name;

            this.Description = item.Description;
            if (item.Description != null)
            {
                this.Description = Markdown.ToPlainText(this.Description);
            }

            this.Quantity = item.QuantityOrdered;
            // TODO: Where does the currency come from?
            var currency = "€";
            this.Price = item.UnitPrice.ToString("0.00") + " " + currency;
            this.Amount = item.TotalExVat.ToString("0.00") + " " + currency;
            this.Comment = item.Comment;
        }

        public string Reference { get; }

        public string Product { get; }

        public string Description { get; }

        public decimal Quantity { get; }

        public string Price { get; }

        public string Amount { get; }

        public string Comment { get; }
    }
}
