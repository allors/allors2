// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductExtensions.v.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public static partial class ProductExtensions
    {
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

            if (@this.ExistPrimaryProductCategory)
            {
                @this.AddProductCategoriesExpanded(@this.PrimaryProductCategory);
                foreach (ProductCategory ancestor in @this.PrimaryProductCategory.Ancestors)
                {
                    @this.AddProductCategoriesExpanded(ancestor);
                }
            }

            foreach (ProductCategory productCategory in @this.ProductCategories)
            {
                @this.AddProductCategoriesExpanded(productCategory);
                foreach (ProductCategory ancestor in productCategory.Ancestors)
                {
                    @this.AddProductCategoriesExpanded(ancestor);
                }
            }
        }
    }
}