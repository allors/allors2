// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderItemModel.cs" company="Allors bvba">
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

using System.Collections.Generic;

namespace Allors.Domain.Print.ProductQuoteModel
{
    public class QuoteItemModel
    {
        public QuoteItemModel(QuoteItem item)
        {
            var session = item.Strategy.Session;

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

            this.Images = new Dictionary<string, byte[]>();

            if (serialisedItem.ExistPrimaryPhoto)
            {
                this.Images.Add("Primary", serialisedItem.PrimaryPhoto.MediaContent.Data);
            }

            var i = 0;
            foreach (Media productPhoto in serialisedItem.Photos)
            {
                this.Images.Add($"image{i++}", productPhoto.MediaContent.Data);
            }
        }

        public string Reference { get; }
        public string Product { get; }
        public string Description { get; }
        public string Details { get; }
        public decimal Quantity { get; }
        public string Price { get; }
        public string Amount { get; }
        public string Comment { get; }
        public Dictionary<string, byte[]> Images { get; set; }
    }
}
