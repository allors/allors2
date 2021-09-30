// <copyright file="PartExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;
    using System.Text;

    public static partial class PartExtensions
    {
        public static void BaseOnBuild(this Part @this, ObjectOnBuild method)
        {
            if (!@this.ExistPartWeightedAverage)
            {
                @this.PartWeightedAverage = new PartWeightedAverageBuilder(@this.Session()).Build();
            }
        }

        public static void BaseOnDerive(this Part @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            @this.SetDisplayName();
        }

        public static void BaseOnPostDerive(this Part @this, ObjectOnPostDerive method)
        {
            var builder = new StringBuilder();

            builder.Append(@this.Name);

            foreach (LocalisedText localisedText in @this.LocalisedNames)
            {
                builder.Append(string.Join(", ", localisedText.Text));
            }

            if (@this.ExistProductIdentifications)
            {
                builder.Append(string.Join(", ", @this.ProductIdentifications.Select(v => v.Identification)));
            }

            if (@this.ExistProductCategoriesWhereAllPart)
            {
                builder.Append(string.Join(", ", @this.ProductCategoriesWhereAllPart.Select(v => v.Name)));
            }

            if (@this.ExistSupplierOfferingsWherePart)
            {
                builder.Append(string.Join(", ", @this.SupplierOfferingsWherePart.Select(v => v.Supplier.PartyName)));
                builder.Append(string.Join(", ", @this.SupplierOfferingsWherePart.Select(v => v.SupplierProductId)));
                builder.Append(string.Join(", ", @this.SupplierOfferingsWherePart.Select(v => v.SupplierProductName)));
            }

            if (@this.ExistSerialisedItems)
            {
                builder.Append(string.Join(", ", @this.SerialisedItems.Select(v => v.SerialNumber)));
            }

            if (@this.ExistProductType)
            {
                builder.Append(string.Join(", ", @this.ProductType.Name));
            }

            if (@this.ExistBrand)
            {
                builder.Append(string.Join(", ", @this.Brand.Name));
            }

            if (@this.ExistModel)
            {
                builder.Append(string.Join(", ", @this.Model.Name));
            }

            foreach (PartCategory partCategory in @this.PartCategoriesWherePart)
            {
                builder.Append(string.Join(", ", partCategory.Name));
            }

            builder.Append(string.Join(", ", @this.Keywords));

            @this.SearchString = builder.ToString();
        }

        public static string PartIdentification(this Part @this)
        {
            if (@this.ProductIdentifications.Count == 0)
            {
                return null;
            }

            var partId = @this.ProductIdentifications.FirstOrDefault(g => g.ExistProductIdentificationType
                                                                         && g.ProductIdentificationType.Equals(new ProductIdentificationTypes(@this.Strategy.Session).Part));

            var goodId = @this.ProductIdentifications.FirstOrDefault(g => g.ExistProductIdentificationType
                                                                          && g.ProductIdentificationType.Equals(new ProductIdentificationTypes(@this.Strategy.Session).Good));

            var id = partId ?? goodId;
            return id?.Identification;
        }

        public static PriceComponent[] GetPriceComponents(this Part @this, PriceComponent[] currentPriceComponents)
        {
            var genericPriceComponents = currentPriceComponents.Where(priceComponent => !priceComponent.ExistPart && !priceComponent.ExistProduct && !priceComponent.ExistProductFeature).ToArray();

            var exclusivePartPriceComponents = currentPriceComponents.Where(priceComponent => priceComponent.Part?.Equals(@this) == true).ToArray();

            if (exclusivePartPriceComponents.Length > 0)
            {
                return exclusivePartPriceComponents.Union(genericPriceComponents).ToArray();
            }

            return genericPriceComponents;
        }

        public static void BaseSetDisplayName(this Part @this, PartSetDisplayName method)
        {
            if (!method.Result.HasValue)
            {
                @this.DisplayName = @this.Name;

                method.Result = true;
            }
        }
    }
}
