// <copyright file="ProductType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class ProductType
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (changeSet.HasChangedRoles(this, this.Meta.SerialisedItemCharacteristicTypes))
            {
                foreach (Part part in this.PartsWhereProductType)
                {
                    iteration.AddDependency(part, this);
                    iteration.Mark(part);

                    foreach (SerialisedItem serialisedItem in part.SerialisedItems)
                    {
                        iteration.AddDependency(serialisedItem, this);
                        iteration.Mark(serialisedItem);
                    }
                }
            }
        }
    }
}
