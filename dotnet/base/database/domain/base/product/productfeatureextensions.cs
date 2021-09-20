// <copyright file="ProductFeatureExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;

    public static partial class ProductFeatureExtensions
    {
        public static PriceComponent[] GetPriceComponents(this ProductFeature @this, Product product, PriceComponent[] currentPriceComponents)
        {
            var genericPriceComponents = currentPriceComponents.Where(priceComponent => !priceComponent.ExistProduct && !priceComponent.ExistPart && !priceComponent.ExistProductFeature).ToArray();

            var exclusiveProductPriceComponents = currentPriceComponents.Where(priceComponent => priceComponent.ProductFeature?.Equals(@this) == true && priceComponent.Product?.Equals(product) == true).ToArray();

            if (exclusiveProductPriceComponents.Length == 0)
            {
                exclusiveProductPriceComponents = currentPriceComponents.Where(priceComponent => priceComponent.ProductFeature?.Equals(@this) == true && !priceComponent.ExistProduct).ToArray();
            }

            if (exclusiveProductPriceComponents.Length > 0)
            {
                return exclusiveProductPriceComponents.Union(genericPriceComponents).ToArray();
            }

            return genericPriceComponents;
        }
    }
}
