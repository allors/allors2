// <copyright file="PriceComponentExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;

    public static class PriceComponentExtensions
    {
        public static void BaseOnDerive(this PriceComponent @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var internalOrganisations = new Organisations(@this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!@this.ExistPricedBy && internalOrganisations.Count() == 1)
            {
                @this.PricedBy = internalOrganisations.First();
            }
        }
    }
}
