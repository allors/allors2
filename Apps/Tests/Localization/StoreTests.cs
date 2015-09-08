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
    using NUnit.Framework;

    [TestFixture]
    public class StoreTests : DomainTest
    {
        [Test]
        public void GivenStore_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new StoreBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithName("Organisation store");
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground);
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            builder.WithSalesInvoiceCounter( new CounterBuilder(this.DatabaseSession).Build() ).Build();
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            builder.WithFiscalYearInvoiceNumber(new FiscalYearInvoiceNumberBuilder(this.DatabaseSession).WithFiscalYear(DateTime.Today.Year).Build());
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenStore_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");

            var store = new StoreBuilder(this.DatabaseSession)
                .WithName("Organisation store")
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, store.SalesOrderTemplates.Count);
            Assert.AreEqual(1, store.SalesInvoiceTemplates.Count);
            Assert.AreEqual(0, store.CreditLimit);
            Assert.AreEqual(0, store.PaymentGracePeriod);
            Assert.AreEqual(0, store.ShipmentThreshold);
            Assert.AreEqual(internalOrganisation, store.Owner);
            Assert.AreEqual(internalOrganisation.DefaultPaymentMethod, store.DefaultPaymentMethod);
            Assert.AreEqual(1, store.PaymentMethods.Count);
            Assert.AreEqual(new Warehouses(this.DatabaseSession).FindBy(Warehouses.Meta.Name, "facility"), store.DefaultFacility);
        }

        [Test]
        public void GivenStore_WhenDefaultPaymentMethodIsSet_ThenPaymentMethodIsAddedToCollectionPaymentMethods()
        {
            Singleton.Instance(this.DatabaseSession).RemoveDefaultInternalOrganisation();
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

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, store.PaymentMethods.Count);
            Assert.AreEqual(ownBankAccount, store.PaymentMethods.First);
        }

        [Test]
        public void GivenStoreWithoutDefaultPaymentMethod_WhenSinglePaymentMethodIsAdded_ThenDefaultPaymentMethodIsSet()
        {
            Singleton.Instance(this.DatabaseSession).RemoveDefaultInternalOrganisation();
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

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(ownBankAccount, store.DefaultPaymentMethod);
        }
    }
}
