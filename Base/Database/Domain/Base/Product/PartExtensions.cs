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
        public static void BaseOnDerive(this Part @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            @this.SetDisplayName();
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
                var builder = new StringBuilder();

                builder.Append(@this.Name);

                foreach (SupplierOffering supplierOffering in @this.SupplierOfferingsWherePart)
                {
                    if (supplierOffering.Supplier is Organisation supplier)
                    {
                        builder.Append(", " + supplier.Name);

                        if (supplierOffering.ExistSupplierProductId)
                        {
                            builder.Append(" (" + supplierOffering.SupplierProductId + ")");
                        }
                    }
                }

                @this.DisplayName = builder.ToString();

                method.Result = true;
            }
        }
    }
}
