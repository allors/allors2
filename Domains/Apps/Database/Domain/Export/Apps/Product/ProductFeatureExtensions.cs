// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductFeatureExtensions.cs" company="Allors bvba">
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