// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryItemExtensions.cs" company="Allors bvba">
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
    using System.Collections.Generic;
    using System.Linq;

    public static partial class InventoryItemExtensions
    {
        public static void AppsOnDerive(this InventoryItem @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!@this.ExistFacility)
            {
                if (@this.Good?.VendorProductsWhereProduct.Count == 1)
                {
                    var internalOrganisation = @this.Good?.VendorProductsWhereProduct[0].InternalOrganisation;
                    @this.Facility = internalOrganisation.DefaultFacility;
                }
            }

            if (!@this.ExistFacility)
            {
                var internalOrganisation = @this.Part?.InternalOrganisation;
                if (internalOrganisation != null)
                {
                    @this.Facility = internalOrganisation.DefaultFacility;
                }
            }
        }

        public static void AppsOnDeriveProductCategories(this InventoryItem @this, IDerivation derivation)
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

        private static void AddParentCategories(this InventoryItem @this, ProductCategory productCategory)
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
    }
}