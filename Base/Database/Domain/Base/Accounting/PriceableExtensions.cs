// <copyright file="PriceableExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    using Allors.Meta;

    internal static class PriceableExtensions
    {
        public static decimal SetUnitSurcharge(this Priceable @this, PriceComponent priceComponent, decimal revenueBreakSurcharge)
        {
            if (priceComponent.Strategy.Class.Equals(M.SurchargeComponent.ObjectType))
            {
                var surchargeComponent = (SurchargeComponent)priceComponent;
                decimal surcharge;

                if (surchargeComponent.Price.HasValue)
                {
                    surcharge = surchargeComponent.Price.Value;
                    @this.DerivedRoles.UnitSurcharge += surcharge;
                }
                else
                {
                    var percentage = surchargeComponent.Percentage ?? 0;
                    surcharge = @this.UnitBasePrice * percentage / 100;
                    @this.DerivedRoles.UnitSurcharge += surcharge;
                }
            }

            return revenueBreakSurcharge;
        }

        internal static decimal SetUnitDiscount(this Priceable @this, PriceComponent priceComponent, decimal revenueBreakDiscount)
        {
            if (priceComponent.Strategy.Class.Equals(M.DiscountComponent.ObjectType))
            {
                var discountComponent = (DiscountComponent)priceComponent;
                decimal discount;

                if (discountComponent.Price.HasValue)
                {
                    discount = discountComponent.Price.Value;
                    @this.DerivedRoles.UnitDiscount += discount;
                }
                else
                {
                    var percentage = discountComponent.Percentage ?? 0;
                    discount = @this.UnitBasePrice * percentage / 100;
                    @this.DerivedRoles.UnitDiscount += discount;
                }
            }

            return revenueBreakDiscount;
        }
    }
}
