// <copyright file="ProductCategories.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class ProductCategories
    {
        private UniquelyIdentifiableSticky<ProductCategory> cache;

        public Extent<ProductCategory> RootCategories
        {
            get
            {
                var extent = this.Session.Extent(this.ObjectType);
                extent.Filter.AddNot().AddExists(this.Meta.PrimaryParent);
                return extent;
            }
        }

        private UniquelyIdentifiableSticky<ProductCategory> Cache => this.cache ??= new UniquelyIdentifiableSticky<ProductCategory>(this.Session);
    }
}
