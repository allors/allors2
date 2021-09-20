// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuoteItemModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.Print.ProductQuoteModel
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Allors.Meta;

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
            this.Quantity = item.Quantity.ToString("0");
            // TODO: Where does the currency come from?
            var currency = "€";
            this.Price = item.UnitPrice.ToString("N2", new CultureInfo("nl-BE")) + " " + currency;

            this.UnitAmount = item.UnitPrice.ToString("N2", new CultureInfo("nl-BE")) + " " + currency;
            this.TotalAmount = item.TotalExVat.ToString("N2", new CultureInfo("nl-BE")) + " " + currency;

            this.Comment = item.Comment;

            if (product != null)
            {
                this.ProductCategory = string.Join(", ", product.ProductCategoriesWhereProduct.Select(v => v.Name));
            }

            var unifiedGood = product as UnifiedGood;
            var nonUnifiedGood = product as NonUnifiedGood;

            if (unifiedGood != null)
            {
                this.BrandName = unifiedGood.Brand?.Name;
                this.ModelName = unifiedGood.Model?.Name;
            }
            else if (nonUnifiedGood != null)
            {
                this.BrandName = nonUnifiedGood?.Part?.Brand?.Name;
                this.ModelName = nonUnifiedGood?.Part?.Model?.Name;
            }

            if (serialisedItem != null)
            {
                this.IdentificationNumber = serialisedItem.ItemNumber;
                this.Year = serialisedItem.ManufacturingYear.ToString();

                var hoursType = new SerialisedItemCharacteristicTypes(session).FindBy(M.SerialisedItemCharacteristicType.Name,"Hours");
                var hoursCharacteristic = serialisedItem.SerialisedItemCharacteristics.FirstOrDefault(v => v.SerialisedItemCharacteristicType.Equals(hoursType));
                if (hoursCharacteristic != null)
                {
                    this.Hours = $"{hoursCharacteristic.Value} {hoursType.UnitOfMeasure?.Abbreviation}";
                }

                if (serialisedItem.ExistPrimaryPhoto)
                {
                    this.PrimaryPhotoName = $"{item.Id}_primaryPhoto";
                    imageByImageName.Add(this.PrimaryPhotoName, serialisedItem.PrimaryPhoto.MediaContent.Data);
                }

                if (serialisedItem.AdditionalPhotos.Count > 0)
                {
                    this.SecondaryPhotoName1 = $"{item.Id}_secondaryPhoto1";
                    imageByImageName.Add(this.SecondaryPhotoName1, serialisedItem.AdditionalPhotos[0].MediaContent.Data);
                }

                if (serialisedItem.AdditionalPhotos.Count > 1)
                {
                    this.SecondaryPhotoName2 = $"{item.Id}_secondaryPhoto2";
                    imageByImageName.Add(this.SecondaryPhotoName2, serialisedItem.AdditionalPhotos[1].MediaContent.Data);
                }
            }
            else if (product != null)
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
        }

        public string PrimaryPhotoName { get; set; }

        public string SecondaryPhotoName1 { get; set; }

        public string SecondaryPhotoName2 { get; set; }

        public string Reference { get; }

        public string Product { get; }

        public string Description { get; }

        public string Details { get; }

        public string Quantity { get; }

        public string Price { get; }

        public string UnitAmount { get; }

        public string TotalAmount { get; }

        public string Comment { get; }

        public string IdentificationNumber { get; }

        public string ProductCategory { get; }

        public string BrandName { get; }

        public string ModelName { get; }

        public string Year { get; }

        public string Hours { get; }
    }
}
