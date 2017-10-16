//------------------------------------------------------------------------------------------------- 
// <copyright file="ReceiptTests.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
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
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using Meta;
    using Xunit;

    public class ReceiptTests : DomainTest
    {
        private Singleton Singleton;
        private Good good;
        private Organisation billToCustomer;
        
        public ReceiptTests()
        {
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");

            this.Singleton = this.Session.GetSingleton();
            this.billToCustomer = new OrganisationBuilder(this.Session).WithName("billToCustomer").WithPreferredCurrency(euro).WithOrganisationRole(new OrganisationRoles(this.Session).Customer).Build();
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").WithLocale(new Locales(this.Session).EnglishGreatBritain).WithOrganisationRole(new OrganisationRoles(this.Session).Supplier).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(this.billToCustomer).Build();

            this.good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(21).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("good").WithLocale(this.Session.GetSingleton().DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var goodPurchasePrice = new ProductPurchasePriceBuilder(this.Session)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            new SupplierOfferingBuilder(this.Session)
                .WithProduct(this.good)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithProductPurchasePrice(goodPurchasePrice)
                .Build();

            new BasePriceBuilder(this.Session)
                .WithDescription("current good")
                .WithProduct(this.good)
                .WithPrice(10)
                .WithFromDate(DateTime.UtcNow)
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();
        }

        [Fact]
        public void GivenReceipt_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            this.InstantiateObjects(this.Session);

            var receipt = new ReceiptBuilder(this.Session).WithEffectiveDate(DateTime.UtcNow).Build();

            Assert.True(receipt.ExistUniqueId);
        }

        [Fact]
        public void GivenReceipt_WhenApplied_ThenInvoiceItemAmountPaidIsUpdated()
        {
            this.InstantiateObjects(this.Session);

            var productItem = new SalesInvoiceItemTypes(this.Session).ProductItem;
            var contactMechanism = new ContactMechanisms(this.Session).Extent().First;

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(this.billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(productItem).Build();
            var item2 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithQuantity(1).WithActualUnitPrice(200M).WithSalesInvoiceItemType(productItem).Build();
            var item3 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithQuantity(1).WithActualUnitPrice(300M).WithSalesInvoiceItemType(productItem).Build();

            invoice.AddSalesInvoiceItem(item1);
            invoice.AddSalesInvoiceItem(item2);
            invoice.AddSalesInvoiceItem(item3);

            this.Session.Derive();

            new ReceiptBuilder(this.Session)
                .WithAmount(50)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(item2).WithAmountApplied(50).Build())
                .WithEffectiveDate(DateTime.UtcNow)
                .Build();

            this.Session.Derive();

            Assert.Equal(0, item1.AmountPaid);
            Assert.Equal(50, item2.AmountPaid);
            Assert.Equal(0, item3.AmountPaid);

            new ReceiptBuilder(this.Session)
                .WithAmount(350)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(item1).WithAmountApplied(100).Build())
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(item2).WithAmountApplied(150).Build())
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(item3).WithAmountApplied(100).Build())
                .WithEffectiveDate(DateTime.UtcNow)
                .Build();

            this.Session.Derive();

            Assert.Equal(100, item1.AmountPaid);
            Assert.Equal(200, item2.AmountPaid);
            Assert.Equal(100, item3.AmountPaid);
        }

        [Fact]
        public void GivenReceipt_WhenDeriving_ThenAmountCanNotBeSmallerThenAmountApplied()
        {
            this.InstantiateObjects(this.Session);

            var billToContactMechanism = new EmailAddressBuilder(this.Session).WithElectronicAddressString("info@allors.com").Build();

            var customer = new PersonBuilder(this.Session)
                .WithLastName("customer")
                .WithPersonRole(new PersonRoles(this.Session).Customer)
                .Build();

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(customer)
                .Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(billToContactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session)
                                        .WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem)
                                        .WithProduct(this.good)
                                        .WithQuantity(1)
                                        .WithActualUnitPrice(100M)
                                        .Build())
                .Build();

            this.Session.Derive();

            var receipt = new ReceiptBuilder(this.Session)
                .WithAmount(100)
                .WithEffectiveDate(DateTime.UtcNow)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(invoice.SalesInvoiceItems[0]).WithAmountApplied(50).Build())
                .Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            receipt.AddPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(invoice.SalesInvoiceItems[0]).WithAmountApplied(50).Build());

            Assert.False(this.Session.Derive(false).HasErrors);

            receipt.AddPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(invoice.SalesInvoiceItems[0]).WithAmountApplied(1).Build());

            var derivationLog = this.Session.Derive(false);
            Assert.True(derivationLog.HasErrors);
            Assert.Contains(M.Receipt.Amount.RoleType, derivationLog.Errors[0].RoleTypes);
        }

        private void InstantiateObjects(ISession session)
        {
            this.good = (Good)session.Instantiate(this.good);
            this.Singleton = (Singleton)session.Instantiate(this.Singleton);
            this.billToCustomer = (Organisation)session.Instantiate(this.billToCustomer);
        }
    }
}