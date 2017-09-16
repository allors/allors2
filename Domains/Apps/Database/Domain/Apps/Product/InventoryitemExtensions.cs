// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryitemExtensions.v.cs" company="Allors bvba">
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

namespace Allors.Domain
{
    public static partial class IInventoryItemExtensions
    {
        public static void AppsOnDeriveProductCategories(this IInventoryItem @this, IDerivation derivation)
        {
            @this.RemoveDerivedProductCategories();

            if (@this.ExistGood)
            {
                foreach (ProductCategory productCategory in @this.Good.ProductCategories)
                {
                    @this.AddDerivedProductCategory(productCategory);
                    @this.AddParentCategories(productCategory);
                }
            }
        }

        private static void AddParentCategories(this IInventoryItem @this, ProductCategory productCategory)
        {
            if (productCategory.ExistParents)
            {
                foreach (ProductCategory parent in productCategory.Parents)
                {
                    @this.AddDerivedProductCategory(parent);
                    @this.AddParentCategories(parent);
                }
            }
        }

        private static void AppsOnDerive(this IInventoryItem @this, ObjectOnDerive method)
        {
            if (!@this.ExistProductType)
            {
                foreach (ProductCharacteristicValue productCharacteristicValue in @this.ProductCharacteristicValues)
                {
                    productCharacteristicValue.Delete();
                }
            }
            else
            {
                var productCharacteristics = new HashSet<ProductCharacteristic>(@this.ProductType.ProductCharacteristics);
                var locales = new HashSet<Locale>(Singleton.Instance(@this.Strategy.Session).Locales);

                var currentProductCharacteristicValueByLocaleByProductCharacteristic = @this.ProductCharacteristicValues
                    .GroupBy(v => v.ProductCharacteristic)
                    .ToDictionary(g => g.Key, g => g.GroupBy(v => v.Locale).ToDictionary(h => h.Key, h => h.First()));

                foreach (ProductCharacteristicValue productCharacteristicValue in @this.ProductCharacteristicValues)
                {
                    // Delete obsolete ProductCharacteristic
                    if (!productCharacteristics.Contains(productCharacteristicValue.ProductCharacteristic) ||
                        !locales.Contains(productCharacteristicValue.Locale))
                    {
                        productCharacteristicValue.Delete();
                    }
                }

                foreach (var productCharacteristic in productCharacteristics)
                {
                    foreach (var locale in locales)
                    {
                        ProductCharacteristicValue productCharacteristicValue = null;
                        Dictionary<Locale, ProductCharacteristicValue> currentProductCharacteristicValueByLocale;
                        if (currentProductCharacteristicValueByLocaleByProductCharacteristic.TryGetValue(productCharacteristic, out currentProductCharacteristicValueByLocale))
                        {
                            currentProductCharacteristicValueByLocale.TryGetValue(locale, out productCharacteristicValue);
                        }

                        if (productCharacteristicValue == null)
                        {
                            productCharacteristicValue = new ProductCharacteristicValueBuilder(@this.Strategy.Session)
                                .WithProductCharacteristic(productCharacteristic)
                                .WithLocale(locale)
                                .Build();

                            @this.AddProductCharacteristicValue(productCharacteristicValue);
                        }
                    }
                }
            }
        }
    }
}