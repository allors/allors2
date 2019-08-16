// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalisedText.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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
    public partial class LocalisedText
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistCatalogueWhereLocalisedDescription || this.ExistCatalogueWhereLocalisedName)
            {
                var catalogue = (Catalogue)this.CatalogueWhereLocalisedName;
                derivation.AddDependency(this, catalogue);
            }

            if (this.ExistProductCategoryWhereLocalisedDescription || this.ExistProductCategoryWhereLocalisedName)
            {
                var productCategory = (ProductCategory)this.ProductCategoryWhereLocalisedName;
                derivation.AddDependency(this, productCategory);
            }

            if (this.ExistUnifiedProductWhereLocalisedDescription || this.ExistUnifiedProductWhereLocalisedName)
            {
                var product = (Product)this.UnifiedProductWhereLocalisedName;
                derivation.AddDependency(this, product);
            }
        }
    }
}
