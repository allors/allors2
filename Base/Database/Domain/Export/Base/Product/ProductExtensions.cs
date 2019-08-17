// <copyright file="ProductExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Linq;

namespace Allors.Domain
{
    using System;

    public static partial class ProductExtensions
    {
        public static void BaseOnPreDerive(this Product @this, ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;
            foreach (ProductCategory productCategory in @this.ProductCategoriesWhereProduct)
            {
                derivation.AddDependency(productCategory, @this);
            }
        }

        public static void AddToBasePrice(this Product @this, BasePrice basePrice) => @this.AddBasePrice(basePrice);

        public static void RemoveFromBasePrices(this Product @this, BasePrice basePrice) => @this.RemoveBasePrice(basePrice);

        public static void BaseOnDeriveVirtualProductPriceComponent(this Product @this)
        {
            if (!@this.ExistProductWhereVariant)
            {
                @this.RemoveVirtualProductPriceComponents();
            }

            if (@this.ExistVariants)
            {
                @this.RemoveVirtualProductPriceComponents();

                var priceComponents = @this.PriceComponentsWhereProduct;

                foreach (Product product in @this.Variants)
                {
                    foreach (PriceComponent priceComponent in priceComponents)
                    {
                        product.AddVirtualProductPriceComponent(priceComponent);

                        if (priceComponent is BasePrice basePrice && !priceComponent.ExistProductFeature)
                        {
                            product.AddToBasePrice(basePrice);
                        }
                    }
                }
            }
        }

        public static PriceComponent[] GetPriceComponents(this Product @this, PriceComponent[] currentPriceComponents)
        {
            var genericPriceComponents = currentPriceComponents.Where(priceComponent => !priceComponent.ExistProduct && !priceComponent.ExistPart && !priceComponent.ExistProductFeature).ToArray();

            var exclusiveProductPriceComponents = currentPriceComponents.Where(priceComponent => priceComponent.Product?.Equals(@this) == true && !priceComponent.ExistProductFeature).ToArray();

            if (exclusiveProductPriceComponents.Length > 0)
            {
                return exclusiveProductPriceComponents.Union(genericPriceComponents).ToArray();
            }

            if (@this.ExistProductWhereVariant)
            {
                return currentPriceComponents.Where(priceComponent => priceComponent.Product?.Equals(@this.ProductWhereVariant) == true && !priceComponent.ExistProductFeature).Union(genericPriceComponents).ToArray();
            }

            return genericPriceComponents;
        }
    }
}
