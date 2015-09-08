// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductCategory.cs" company="Allors bvba">
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
    using System.Collections.Generic;

    using Resources;

    public partial class ProductCategory
    {
        private IEnumerable<ProductCategory> AncestorList
        {
            get
            {
                var ancestors = new List<ProductCategory>();

                foreach (ProductCategory parent in this.Parents)
                {
                    ancestors.Add(parent);
                    ancestors.AddRange(parent.AncestorList);
                }

                return ancestors;
            }
        }

        private IEnumerable<ProductCategory> ChildList
        {
            get
            {
                var children = new List<ProductCategory>();

                foreach (ProductCategory child in this.ProductCategoriesWhereParent)
                {
                    children.Add(child);
                    children.AddRange(child.ChildList);
                }

                return children;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistProductCategoriesWhereParent && this.ExistPackage)
            {
                derivation.Log.AddError(this, ProductCategories.Meta.Package, ErrorMessages.ProductCategoryPackageOnlyAtLowestLevel);
            }

            foreach (ProductCategory productCategory in this.ProductCategoriesWhereAncestor)
            {
                productCategory.AppsOnDeriveAncestors(derivation);
            }

            foreach (Product product in this.ProductsWhereProductCategoryExpanded)
            {
                product.DeriveProductCategoryExpanded();
            }

            this.AppsOnDeriveAncestors(derivation);
            this.AppsOnDeriveChildren(derivation);
        }

        public void AppsOnDeriveAncestors(IDerivation derivation)
        {
            this.RemoveAncestors();

            foreach (var ancestor in this.AncestorList)
            {
                this.AddAncestor(ancestor);
            }

            foreach (ProductCategory productCategory in this.ProductCategoriesWhereAncestor)
            {
                productCategory.AppsOnDeriveAncestors(derivation);
            }
        }

        public void AppsOnDeriveChildren(IDerivation derivation)
        {
            this.RemoveChildren();

            foreach (var child in this.ChildList)
            {
                this.AddChild(child);
            }

            foreach (ProductCategory parent in this.Parents)
            {
                parent.AppsOnDeriveChildren(derivation);
            }
        }
    }
}