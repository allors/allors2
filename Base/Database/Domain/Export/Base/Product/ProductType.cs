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
            var derivation = method.Derivation;

            if (derivation.HasChangedRole(this, this.Meta.SerialisedItemCharacteristicTypes))
            {
                foreach (Part part in this.PartsWhereProductType)
                {
                    derivation.AddDependency(part, this);

                    foreach (SerialisedItem serialisedItem in part.SerialisedItems)
                    {
                        derivation.AddDependency(serialisedItem, this);
                    }
                }
            }
        }
    }
}
