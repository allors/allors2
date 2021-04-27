// <copyright file="Currencies.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;

    public partial class Currencies
    {
        public static decimal ConvertCurrency(decimal amount, DateTime validFrom, Currency fromCurrency, Currency toCurrency)
        {
            if (!fromCurrency.Equals(toCurrency) )
            {
                var exchangeRate = fromCurrency.ExchangeRatesWhereFromCurrency.Where(v => v.ValidFrom.Date <= validFrom.Date && v.ToCurrency.Equals(toCurrency)).OrderByDescending(v => v.ValidFrom).FirstOrDefault();

                if (exchangeRate != null)
                {
                    return Rounder.RoundDecimal(amount * exchangeRate.Rate, 2);
                }
                else
                {
                    var invertedExchangeRate = toCurrency.ExchangeRatesWhereFromCurrency.Where(v => v.ValidFrom.Date <= validFrom.Date && v.ToCurrency.Equals(fromCurrency)).OrderByDescending(v => v.ValidFrom).FirstOrDefault();
                    if (invertedExchangeRate != null)
                    {
                        return Rounder.RoundDecimal(amount * (1 / invertedExchangeRate.Rate), 2);
                    }
                }

                return 0;
            }

            return amount;
        }
    }
}
