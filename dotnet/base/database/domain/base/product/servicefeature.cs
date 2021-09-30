// <copyright file="ServiceFeature.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class ServiceFeature
    {
        public void AddToBasePrice(BasePrice basePrice) => this.AddBasePrice(basePrice);

        public void RemoveFromBasePrices(BasePrice basePrice) => this.RemoveBasePrice(basePrice);
    }
}
