// <copyright file="ReceiptTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Allors.Meta;
    using Xunit;

    public class ReceiptTests : DomainTest
    {
        private Singleton singleton;
        private Part finishedGood;
        private Good good;
        private Organisation billToCustomer;

        public ReceiptTests()
        {
            var euro = new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR");

            this.singleton = this.Session.GetSingleton();
            this.billToCustomer = new OrganisationBuilder(this.Session).WithName("billToCustomer").WithPreferredCurrency(euro).Build();
            this.good = new Goods(this.Session).FindBy(M.Good.Name, "good1");
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").WithLocale(new Locales(this.Session).EnglishGreatBritain).Build();

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(this.billToCustomer).Build();

            new SupplierOfferingBuilder(this.Session)
                .WithPart(this.finishedGood)
                .WithSupplier(supplier)
                .WithFromDate(this.Session.Now())
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithCurrency(euro)
                .WithPrice(7)
                .Build();

            new BasePriceBuilder(this.Session)
                .WithDescription("current good")
                .WithProduct(this.good)
                .WithPrice(10)
                .WithFromDate(this.Session.Now())
                .WithThroughDate(this.Session.Now().AddYears(1).AddDays(-1))
                .Build();

            this.Session.Derive();
            this.Session.Commit();
        }

        [Fact]
        public void GivenReceipt_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            this.InstantiateObjects(this.Session);

            var receipt = new ReceiptBuilder(this.Session).WithEffectiveDate(this.Session.Now()).Build();

            Assert.True(receipt.ExistUniqueId);
        }

        [Fact]
        public void GivenReceipt_WhenApplied_ThenInvoiceItemAmountPaidIsUpdated()
        {
            this.InstantiateObjects(this.Session);

            var productItem = new InvoiceItemTypes(this.Session).ProductItem;
            var contactMechanism = new ContactMechanisms(this.Session).Extent().First;

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(this.billToCustomer)
                .WithAssignedBillToContactMechanism(contactMechanism)
                .Build();

            var item1 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithQuantity(1).WithAssignedUnitPrice(100M).WithInvoiceItemType(productItem).Build();
            var item2 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithQuantity(1).WithAssignedUnitPrice(200M).WithInvoiceItemType(productItem).Build();
            var item3 = new SalesInvoiceItemBuilder(this.Session).WithProduct(this.good).WithQuantity(1).WithAssignedUnitPrice(300M).WithInvoiceItemType(productItem).Build();

            invoice.AddSalesInvoiceItem(item1);
            invoice.AddSalesInvoiceItem(item2);
            invoice.AddSalesInvoiceItem(item3);

            this.Session.Derive();

            new ReceiptBuilder(this.Session)
                .WithAmount(50)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(item2).WithAmountApplied(50).Build())
                .WithEffectiveDate(this.Session.Now())
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
                .WithEffectiveDate(this.Session.Now())
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

                .Build();

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(customer)
                .Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithAssignedBillToContactMechanism(billToContactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session)
                                        .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                                        .WithProduct(this.good)
                                        .WithQuantity(1)
                                        .WithAssignedUnitPrice(100M)
                                        .Build())
                .Build();

            this.Session.Derive();

            var receipt = new ReceiptBuilder(this.Session)
                .WithAmount(100)
                .WithEffectiveDate(this.Session.Now())
                .WithPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(invoice.SalesInvoiceItems[0]).WithAmountApplied(50).Build())
                .Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            receipt.AddPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(invoice.SalesInvoiceItems[0]).WithAmountApplied(50).Build());

            Assert.False(this.Session.Derive(false).HasErrors);

            receipt.AddPaymentApplication(new PaymentApplicationBuilder(this.Session).WithInvoiceItem(invoice.SalesInvoiceItems[0]).WithAmountApplied(1).Build());

            var derivationLog = this.Session.Derive(false);
            Assert.True(derivationLog.HasErrors);
            Assert.Contains(M.PaymentApplication.AmountApplied, derivationLog.Errors[0].RoleTypes);
        }

        private void InstantiateObjects(ISession session)
        {
            this.finishedGood = (Part)session.Instantiate(this.finishedGood);
            this.good = (Good)session.Instantiate(this.good);
            this.singleton = (Singleton)session.Instantiate(this.singleton);
            this.billToCustomer = (Organisation)session.Instantiate(this.billToCustomer);
        }
    }
}
