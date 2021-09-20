// <copyright file="CurrencyTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the CurrencyTests type.</summary>

namespace Tests
{
    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Xunit;

    public class CurrencyTests : DomainTest
    {
        [Fact]
        public void GivenRateForExactDate_ConvertFromCurrency()
        {
            var today = this.Session.Now().Date;
            var fromCurrency = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "TRY");
            var toCurrency = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "GBP");

            new ExchangeRateBuilder(this.Session)
                .WithValidFrom(today.AddDays(-1))
                .WithFromCurrency(fromCurrency)
                .WithToCurrency(toCurrency)
                .WithRate(0.085M)
                .Build();

            new ExchangeRateBuilder(this.Session)
                .WithValidFrom(today)
                .WithFromCurrency(fromCurrency)
                .WithToCurrency(toCurrency)
                .WithRate(0.085945871M)
                .Build();

            this.Session.Derive();

            var amount = Currencies.ConvertCurrency(270000M, today, fromCurrency, toCurrency);
            Assert.Equal(23205.39M, amount);
        }

        [Fact]
        public void GivenRateForExactDate_ConvertFromCurrencyUsingInvertedRate()
        {
            var today = this.Session.Now().Date;
            var fromTry = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "TRY");
            var toGbp = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "GBP");

            new ExchangeRateBuilder(this.Session)
                .WithValidFrom(today)
                .WithFromCurrency(toGbp)
                .WithToCurrency(fromTry)
                .WithRate(11.635230272M)
                .Build();

            this.Session.Derive();

            var amount = Currencies.ConvertCurrency(270000M, today, fromTry, toGbp);
            Assert.Equal(23205.39M, amount);
        }

        [Fact]
        public void GivenHistoricRate_ConvertFromCurrency_UsingMostRecent()
        {
            var today = this.Session.Now().Date;
            var fromCurrency = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "TRY");
            var toCurrency = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "GBP");

            new ExchangeRateBuilder(this.Session)
                .WithValidFrom(today.AddDays(-2))
                .WithFromCurrency(fromCurrency)
                .WithToCurrency(toCurrency)
                .WithRate(0.085M)
                .Build();

            new ExchangeRateBuilder(this.Session)
                .WithValidFrom(today.AddDays(-1))
                .WithFromCurrency(fromCurrency)
                .WithToCurrency(toCurrency)
                .WithRate(0.085945871M)
                .Build();

            this.Session.Derive();

            var amount = Currencies.ConvertCurrency(270000M, today, fromCurrency, toCurrency);
            Assert.Equal(23205.39M, amount);
        }

        [Fact]
        public void GivenFutureRate_ConvertFromCurrency()
        {
            var today = this.Session.Now().Date;
            var fromCurrency = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "TRY");
            var toCurrency = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "GBP");

            new ExchangeRateBuilder(this.Session)
                .WithValidFrom(today.AddDays(1))
                .WithFromCurrency(fromCurrency)
                .WithToCurrency(toCurrency)
                .WithRate(0.085945871M)
                .Build();

            this.Session.Derive();

            var amount = Currencies.ConvertCurrency(270000M, today, fromCurrency, toCurrency);
            Assert.Equal(0M, amount);
        }
    }
}
