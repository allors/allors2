// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductCategory.cs" company="Allors bvba">
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

    using Meta;
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

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCatScope)
            {
                this.CatScope = new CatScopes(this.strategy.Session).Public;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var defaultLocale = Singleton.Instance(this.strategy.Session).DefaultLocale;

            if (this.LocalisedNames.Any(x => x.Locale.Equals(defaultLocale)))
            {
                this.Name = this.LocalisedNames.First(x => x.Locale.Equals(defaultLocale)).Text;
            }

            if (this.LocalisedDescriptions.Any(x => x.Locale.Equals(defaultLocale)))
            {
                this.Description = this.LocalisedDescriptions.First(x => x.Locale.Equals(defaultLocale)).Text;
            }

            if (this.ExistProductCategoriesWhereParent && this.ExistPackage)
            {
                derivation.Validation.AddError(this, M.ProductCategory.Package, ErrorMessages.ProductCategoryPackageOnlyAtLowestLevel);
            }

            foreach (ProductCategory productCategory in this.ProductCategoriesWhereAncestor)
            {
                productCategory.AppsOnDeriveAncestors(derivation);
            }

            foreach (Product product in this.ProductsWhereProductCategoriesExpanded)
            {
                product.AppsOnDeriveProductCategoriesExpanded();
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