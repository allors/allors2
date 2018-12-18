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
        private IEnumerable<ProductCategory> AllSuperJacent
        {
            get
            {
                var superJacent = new List<ProductCategory>();

                foreach (ProductCategory parent in this.Parents)
                {
                    superJacent.Add(parent);
                    superJacent.AddRange(parent.AllSuperJacent);
                }

                return superJacent;
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

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            foreach (Object parent in this.Parents)
            {
                derivation.AddDependency(parent, this);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var defaultLocale = this.strategy.Session.GetSingleton().DefaultLocale;

            if (this.LocalisedNames.Any(x => x.Locale.Equals(defaultLocale)))
            {
                this.Name = this.LocalisedNames.First(x => x.Locale.Equals(defaultLocale)).Text;
            }

            if (this.LocalisedDescriptions.Any(x => x.Locale.Equals(defaultLocale)))
            {
                this.Description = this.LocalisedDescriptions.First(x => x.Locale.Equals(defaultLocale)).Text;
            }

            if (!this.ExistCategoryImage)
            {
                this.CategoryImage = this.strategy.Session.GetSingleton().Settings.NoImageAvailableImage;
            }

            foreach (ProductCategory productCategory in this.ProductCategoriesWhereSuperJacent)
            {
                productCategory.AppsOnDeriveSuperJacent(derivation);
            }

            foreach (Product product in this.ProductsWhereProductCategoriesExpanded)
            {
                product.AppsOnDeriveProductCategoriesExpanded();
            }

            this.AppsOnDeriveSuperJacent(derivation);
            this.AppsOnDeriveChildren(derivation);
            this.AppsOnDeriveAllProducts();
            this.AppsDeriveSerialisedItems();
        }

        public void AppsOnDeriveSuperJacent(IDerivation derivation)
        {
            this.RemoveSuperJacent();

            foreach (var superJacent in this.AllSuperJacent)
            {
                this.AddSuperJacent(superJacent);
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

        public void AppsOnDeriveAllProducts()
        {
            var allProducts = new List<Product>(this.Products);

            foreach (ProductCategory child in this.Children)
            {
                allProducts.AddRange(child.AllProducts);
            }

            this.AllProducts = allProducts.ToArray();
        }

        public void AppsDeriveSerialisedItems()
        {
            var allgoods = this.AllProducts.Where(v => v is Good).ToArray() as Good[];

            this.SerialisedItems = allgoods?.SelectMany(v => v.Part.SerialisedItems).Where(v => v.AvailableForSale).ToArray();
        }
    }
}