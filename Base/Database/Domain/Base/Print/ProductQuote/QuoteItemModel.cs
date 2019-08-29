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
using System.Linq;

namespace Allors.Domain.Print.ProductQuoteModel
{
    public class QuoteItemModel
    {
        public QuoteItemModel(QuoteItem item, Dictionary<string, byte[]> imageByImageName)
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
            this.ProductCategory = string.Join(", ", product.ProductCategoriesWhereProduct.Select(v => v.Name));

            var unifiedGood = product as UnifiedGood;
            var nonUnifiedGood = product as NonUnifiedGood;

            if (unifiedGood != null)
            {
                this.BrandName = unifiedGood.Brand?.Name;
                this.ModelName = unifiedGood.Model?.Name;
            }
            else
            {
                this.BrandName = nonUnifiedGood.Part?.Brand?.Name;
                this.ModelName = nonUnifiedGood.Part?.Model?.Name;
            }

            if (serialisedItem == null)
            {
                this.IdentificationNumber = product.ProductIdentifications.FirstOrDefault(v => v.ProductIdentificationType.Equals(new ProductIdentificationTypes(session).Good)).Identification;

                if (product.ExistPrimaryPhoto)
                {
                    this.PrimaryPhotoName = $"{item.Id}_primaryPhoto";
                    imageByImageName.Add(this.PrimaryPhotoName, product.PrimaryPhoto.MediaContent.Data);
                }

                if (product.Photos.Count > 0)
                {
                    this.SecondaryPhotoName1 = $"{item.Id}_secondaryPhoto1";
                    imageByImageName.Add(this.SecondaryPhotoName1, product.Photos[0].MediaContent.Data);
                }

                if (product.Photos.Count > 1)
                {
                    this.SecondaryPhotoName2 = $"{item.Id}_secondaryPhoto2";
                    imageByImageName.Add(this.SecondaryPhotoName2, product.Photos[0].MediaContent.Data);
                }
            }
            else
            {
                this.IdentificationNumber = serialisedItem.ItemNumber;
                this.Year = serialisedItem.ManufacturingYear.ToString();

                if (serialisedItem.ExistPrimaryPhoto)
                {
                    this.PrimaryPhotoName = $"{item.Id}_primaryPhoto";
                    imageByImageName.Add(this.PrimaryPhotoName, serialisedItem.PrimaryPhoto.MediaContent.Data);
                }

                if (serialisedItem.Photos.Count > 0)
                {
                    this.SecondaryPhotoName1 = $"{item.Id}_secondaryPhoto1";
                    imageByImageName.Add(this.SecondaryPhotoName1, serialisedItem.Photos[0].MediaContent.Data);
                }

                if (serialisedItem.Photos.Count > 1)
                {
                    this.SecondaryPhotoName2 = $"{item.Id}_secondaryPhoto2";
                    imageByImageName.Add(this.SecondaryPhotoName2, serialisedItem.Photos[1].MediaContent.Data);
                }
            }
        }

        public string PrimaryPhotoName { get; set; }

        public string SecondaryPhotoName1 { get; set; }

        public string SecondaryPhotoName2 { get; set; }

        public string Reference { get; }

        public string Product { get; }

        public string Description { get; }

        public string Details { get; }

        public decimal Quantity { get; }

        public string Price { get; }

        public string Amount { get; }

        public string Comment { get; }

        public string IdentificationNumber { get; }

        public string ProductCategory { get; }

        public string BrandName { get; }

        public string ModelName { get; }

        public string Year { get; }
    }
}
