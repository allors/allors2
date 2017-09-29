// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoreTests.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using Meta;
    using Xunit;

    
    public class StoreTests : DomainTest
    {
        [Fact]
        public void GivenStore_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new StoreBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithName("Organisation store");
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground);
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            builder.WithSalesInvoiceCounter( new CounterBuilder(this.DatabaseSession).Build() ).Build();
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            builder.WithFiscalYearInvoiceNumber(new FiscalYearInvoiceNumberBuilder(this.DatabaseSession).WithFiscalYear(DateTime.Today.Year).Build());
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenStore_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var internalOrganisation = Singleton.Instance(this.DatabaseSession).InternalOrganisation;

            var store = new StoreBuilder(this.DatabaseSession)
                .WithName("Organisation store")
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(0, store.CreditLimit);
            Assert.Equal(0, store.PaymentGracePeriod);
            Assert.Equal(0, store.ShipmentThreshold);
            Assert.Equal(internalOrganisation.DefaultPaymentMethod, store.DefaultPaymentMethod);
            Assert.Equal(1, store.PaymentMethods.Count);
            Assert.Equal(new Warehouses(this.DatabaseSession).FindBy(M.Warehouse.Name, "facility"), store.DefaultFacility);
        }

        [Fact]
        public void GivenStore_WhenDefaultPaymentMethodIsSet_ThenPaymentMethodIsAddedToCollectionPaymentMethods()
        {
            this.DatabaseSession.Commit();

            var netherlands = new Countries(this.DatabaseSession).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("BE23 3300 6167 6391")
                .WithBankAccount(new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("NL50RABO0109546784").WithNameOnAccount("Martien").Build())
                .Build();

            var store = new StoreBuilder(this.DatabaseSession)
                .WithName("Organisation store")
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithDefaultPaymentMethod(ownBankAccount)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(1, store.PaymentMethods.Count);
            Assert.Equal(ownBankAccount, store.PaymentMethods.First);
        }

        [Fact]
        public void GivenStoreWithoutDefaultPaymentMethod_WhenSinglePaymentMethodIsAdded_ThenDefaultPaymentMethodIsSet()
        {
            this.DatabaseSession.Commit();

            var netherlands = new Countries(this.DatabaseSession).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();

            var ownBankAccount = new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("BE23 3300 6167 6391")
                .WithBankAccount(new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("NL50RABO0109546784").WithNameOnAccount("Martien").Build())
                .Build();

            var store = new StoreBuilder(this.DatabaseSession)
                .WithName("Organisation store")
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithPaymentMethod(ownBankAccount)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(ownBankAccount, store.DefaultPaymentMethod);
        }
    }
}
