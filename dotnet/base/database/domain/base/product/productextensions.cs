// <copyright file="ProductExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Meta;

    public static partial class ProductExtensions
    {
        public static void BaseOnPreDerive(this Product @this, ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(@this) || changeSet.IsCreated(@this) || changeSet.HasChangedRoles(@this))
            {
                foreach (ProductCategory productCategory in @this.ProductCategoriesWhereProduct)
                {
                    iteration.AddDependency(productCategory, @this);
                    iteration.Mark(productCategory);
                }
            }
        }

        public static void BaseOnDeriveVirtualProductPriceComponent(this Product @this)
        {
            if (!@this.ExistProductWhereVariant)
            {
                @this.DerivedRoles.RemoveVirtualProductPriceComponents();
            }

            if (@this.ExistVariants)
            {
                @this.DerivedRoles.RemoveVirtualProductPriceComponents();

                var priceComponents = @this.PriceComponentsWhereProduct;

                foreach (Product product in @this.Variants)
                {
                    foreach (PriceComponent priceComponent in priceComponents)
                    {
                        // HACK: DerivedRoles
                        var productDerivedRoles = (ProductDerivedRoles)product;
                        productDerivedRoles.AddVirtualProductPriceComponent(priceComponent);

                        if (priceComponent is BasePrice basePrice && !priceComponent.ExistProductFeature)
                        {
                            productDerivedRoles.AddBasePrice(basePrice);
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
