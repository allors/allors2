
// <copyright file="LocalisedText.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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
