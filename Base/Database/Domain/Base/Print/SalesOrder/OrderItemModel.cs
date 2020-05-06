// <copyright file="OrderItemModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.SalesOrderModel
{
    using System.Globalization;
    using Markdig;

    public class OrderItemModel
    {
        public OrderItemModel(SalesOrderItem item)
        {
            var currencyIsoCode = item.SalesOrderWhereSalesOrderItem.Currency.IsoCode;

            this.Reference = item.InvoiceItemType?.Name;
            this.Product = item.Product?.Name;

            this.Description = item.Description;
            if (item.Description != null)
            {
                this.Description = Markdown.ToPlainText(this.Description);
            }

            this.Quantity = item.QuantityOrdered;
            // TODO: Where does the currency come from?
            this.Price = currencyIsoCode + " " + item.UnitPrice.ToString("N2", new CultureInfo("nl-BE"));
            this.Amount = currencyIsoCode + " " + item.TotalExVat.ToString("N2", new CultureInfo("nl-BE"));
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
