// <copyright file="Currencies.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class Currencies
    {
        public static decimal ConvertCurrency(decimal amount, Currency fromCurrency, Currency toCurrency)
        {
            if (!fromCurrency.Equals(toCurrency))
            {
                foreach (UnitOfMeasureConversion unitOfMeasureConversion in fromCurrency.UnitOfMeasureConversions)
                {
                    if (unitOfMeasureConversion.ToUnitOfMeasure.Equals(toCurrency))
                    {
                        return Math.Round(amount * unitOfMeasureConversion.ConversionFactor, 2);
                    }
                }
            }

            return amount;
        }
    }
}
