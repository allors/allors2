﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PriceableExtensions.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using Meta;
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
                    @this.UnitSurcharge += surcharge;
                }
                else
                {
                    var percentage = surchargeComponent.Percentage ?? 0;
                    surcharge = decimal.Round((@this.UnitBasePrice * percentage) / 100, 2);
                    @this.UnitSurcharge += surcharge;
                }

                ////Revenuebreaks on quantity and value are mutually exclusive.
                if (priceComponent.ExistRevenueQuantityBreak || priceComponent.ExistRevenueValueBreak)
                {
                    if (revenueBreakSurcharge == 0)
                    {
                        revenueBreakSurcharge = surcharge;
                    }
                    else
                    {
                        ////Apply highest of the two. Revert the other one. 
                        if (surcharge > revenueBreakSurcharge)
                        {
                            @this.UnitSurcharge -= revenueBreakSurcharge;
                        }
                        else
                        {
                            @this.UnitSurcharge -= surcharge;
                        }
                    }
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
                    @this.UnitDiscount += discount;
                }
                else
                {
                    var percentage = discountComponent.Percentage ?? 0;
                    discount = decimal.Round((@this.UnitBasePrice * percentage) / 100, 2);
                    @this.UnitDiscount += discount;
                }

                ////Revenuebreaks on quantity and value are mutually exclusive.
                if (priceComponent.ExistRevenueQuantityBreak || priceComponent.ExistRevenueValueBreak)
                {
                    if (revenueBreakDiscount == 0)
                    {
                        revenueBreakDiscount = discount;
                    }
                    else
                    {
                        ////Apply highest of the two. Revert the other one. 
                        if (discount > revenueBreakDiscount)
                        {
                            @this.UnitDiscount -= revenueBreakDiscount;
                        }
                        else
                        {
                            @this.UnitDiscount -= discount;
                        }
                    }
                }
            }

            return revenueBreakDiscount;
        }
    }
}