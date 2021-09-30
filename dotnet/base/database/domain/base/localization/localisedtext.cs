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
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {

                if (this.ExistCatalogueWhereLocalisedDescription || this.ExistCatalogueWhereLocalisedName)
                {
                    var catalogue = (Catalogue)this.CatalogueWhereLocalisedName;
                    iteration.AddDependency(this, catalogue);
                    iteration.Mark(catalogue);
                }

                if (this.ExistProductCategoryWhereLocalisedDescription || this.ExistProductCategoryWhereLocalisedName)
                {
                    var productCategory = (ProductCategory)this.ProductCategoryWhereLocalisedName;
                    iteration.AddDependency(this, productCategory);
                    iteration.Mark(productCategory);
                }

                if (this.ExistUnifiedProductWhereLocalisedDescription || this.ExistUnifiedProductWhereLocalisedName)
                {
                    var product = this.UnifiedProductWhereLocalisedName;
                    iteration.AddDependency(this, product);
                    iteration.Mark(product);
                }
            }
        }
    }
}
