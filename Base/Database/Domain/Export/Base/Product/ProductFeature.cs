// <copyright file="ProductFeature.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial interface ProductFeature
    {
        void AddToBasePrice(BasePrice basePrice);

        void RemoveFromBasePrices(BasePrice basePrice);
    }
}
