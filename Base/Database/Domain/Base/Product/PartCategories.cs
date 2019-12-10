// <copyright file="PartCategories.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class PartCategories
    {
        private UniquelyIdentifiableSticky<PartCategory> cache;

        public Extent<PartCategory> RootCategories
        {
            get
            {
                var extent = this.Session.Extent(this.ObjectType);
                extent.Filter.AddNot().AddExists(this.Meta.PrimaryParent);
                return extent;
            }
        }

        private UniquelyIdentifiableSticky<PartCategory> Cache => this.cache ??= new UniquelyIdentifiableSticky<PartCategory>(this.Session);
    }
}
