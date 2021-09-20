// <copyright file="StoreTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>

namespace Allors.Domain
{
    using System;
    using Allors.Meta;
    using Xunit;

    public class StoreTests : DomainTest
    {
        [Fact]
        public void GivenStore_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new StoreBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("Organisation store");
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDefaultCarrier(new Carriers(this.Session).Fedex);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            builder.WithSalesInvoiceNumberCounter(new CounterBuilder(this.Session).Build()).Build();
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            builder.WithFiscalYearsStoreSequenceNumber(new FiscalYearStoreSequenceNumbersBuilder(this.Session).WithFiscalYear(DateTime.Today.Year).Build());
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenStore_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var internalOrganisation = this.InternalOrganisation;

            var store = new StoreBuilder(this.Session)
                .WithName("Organisation store")
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .Build();

            this.Session.Derive();

            Assert.Equal(0, store.CreditLimit);
            Assert.Equal(0, store.PaymentGracePeriod);
            Assert.Equal(0, store.ShipmentThreshold);
            Assert.Equal(internalOrganisation.DefaultCollectionMethod, store.DefaultCollectionMethod);
            Assert.Single(store.CollectionMethods);
            Assert.Equal(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse), store.DefaultFacility);
        }

        [Fact]
        public void GivenStore_WhenDefaultPaymentMethodIsSet_ThenPaymentMethodIsAddedToCollectionPaymentMethods()
        {
            this.Session.Commit();

            var netherlands = new Countries(this.Session).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;

            var bank = new BankBuilder(this.Session).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.Session)
                .WithDescription("BE23 3300 6167 6391")
                .WithBankAccount(new BankAccountBuilder(this.Session).WithBank(bank).WithCurrency(euro).WithIban("NL50RABO0109546784").WithNameOnAccount("Martien").Build())
                .Build();

            var store = new StoreBuilder(this.Session)
                .WithName("Organisation store")
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithDefaultCollectionMethod(ownBankAccount)
                .Build();

            this.Session.Derive();

            Assert.Single(store.CollectionMethods);
            Assert.Equal(ownBankAccount, store.CollectionMethods.First);
        }

        [Fact]
        public void GivenStoreWithoutDefaultPaymentMethod_WhenSinglePaymentMethodIsAdded_ThenDefaultPaymentMethodIsSet()
        {
            this.Session.Commit();

            var netherlands = new Countries(this.Session).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;

            var bank = new BankBuilder(this.Session).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.Session)
                .WithDescription("BE23 3300 6167 6391")
                .WithBankAccount(new BankAccountBuilder(this.Session).WithBank(bank).WithCurrency(euro).WithIban("NL50RABO0109546784").WithNameOnAccount("Martien").Build())
                .Build();

            var store = new StoreBuilder(this.Session)
                .WithName("Organisation store")
                .WithDefaultCarrier(new Carriers(this.Session).Fedex)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.Session).Ground)
                .WithCollectionMethod(ownBankAccount)
                .Build();

            this.Session.Derive();

            Assert.Equal(ownBankAccount, store.DefaultCollectionMethod);
        }
    }
}
