// <copyright file="ServiceExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public static partial class ServiceExtensions
    {
        public static void AddToBasePrice(this Service @this, BasePrice basePrice) => @this.AddBasePrice(basePrice);

        public static void RemoveFromBasePrices(this Service @this, BasePrice basePrice) => @this.RemoveBasePrice(basePrice);

        public static void BaseOnDerive(this Service @this, ObjectOnDerive method) => @this.BaseOnDeriveVirtualProductPriceComponent();
    }
}
