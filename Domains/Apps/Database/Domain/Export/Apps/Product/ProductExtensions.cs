// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductExtensions.cs" company="Allors bvba">
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

using System.Linq;

namespace Allors.Domain
{
    using System;

    public static partial class ProductExtensions
    {
        public static void AppsOnDerive(this Product @this, ObjectOnDerive method)
        {
            @this.AppsOnDeriveProductCategoriesExpanded();
        }

        public static void AddToBasePrice(this Product @this, BasePrice basePrice)
        {
            @this.AddBasePrice(basePrice);
        }

        public static void RemoveFromBasePrices(this Product @this, BasePrice basePrice)
        {
            @this.RemoveBasePrice(basePrice);
        }

        public static void AppsOnDeriveVirtualProductPriceComponent(this Product @this)
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

                        var basePrice = priceComponent as BasePrice;
                        if (basePrice != null && !priceComponent.ExistProductFeature)
                        {
                            product.AddToBasePrice(basePrice);
                        }
                    }
                }
            }
        }

        public static void AppsOnDeriveProductCategoriesExpanded(this Product @this)
        {
            @this.RemoveProductCategoriesExpanded();

            foreach (ProductCategory productCategory in @this.ProductCategoriesWhereProduct)
            {
                @this.AddProductCategoriesExpanded(productCategory);
                foreach (ProductCategory superJacent in productCategory.SuperJacent)
                {
                    @this.AddProductCategoriesExpanded(superJacent);
                    superJacent.AppsOnDeriveAllProducts();
                    superJacent.AppsDeriveAllSerialisedItemsForSale();
                    superJacent.AppsDeriveAllNonSerialisedInventoryItemsForSale();
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