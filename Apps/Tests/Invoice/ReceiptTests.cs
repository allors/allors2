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
    using NUnit.Framework;

    [TestFixture]
    public class ReceiptTests : DomainTest
    {
        private Good good;
        private InternalOrganisation internalOrganisation;
        private Organisation billToCustomer;

        [SetUp]
        public override void Init()
        {
            base.Init();

            var euro = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");

            this.internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            this.billToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("billToCustomer").WithPreferredCurrency(euro).Build();
            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain).Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(this.billToCustomer).WithInternalOrganisation(this.internalOrganisation).Build();

            this.good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var goodPurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(this.good)
                .WithSupplier(supplier)
                .WithFromDate(DateTime.UtcNow)
                .WithProductPurchasePrice(goodPurchasePrice)
                .Build();

            new BasePriceBuilder(this.DatabaseSession)
                .WithSpecifiedFor(this.internalOrganisation)
                .WithDescription("current good")
                .WithProduct(this.good)
                .WithPrice(10)
                .WithFromDate(DateTime.UtcNow)
                .WithThroughDate(DateTime.UtcNow.AddYears(1).AddDays(-1))
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();
        }

        [Test]
        public void GivenReceipt_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var receipt = new ReceiptBuilder(this.DatabaseSession).WithEffectiveDate(DateTime.UtcNow).Build();

            Assert.IsTrue(receipt.ExistUniqueId);
        }

        [Test]
        public void GivenReceipt_WhenApplied_ThenInvoiceItemAmountPaidIsUpdated()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var productItem = new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem;
            var contactMechanism = new ContactMechanisms(this.DatabaseSession).Extent().First;

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(this.billToCustomer)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            var item1 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithQuantity(1).WithActualUnitPrice(100M).WithSalesInvoiceItemType(productItem).Build();
            var item2 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithQuantity(1).WithActualUnitPrice(200M).WithSalesInvoiceItemType(productItem).Build();
            var item3 = new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(this.good).WithQuantity(1).WithActualUnitPrice(300M).WithSalesInvoiceItemType(productItem).Build();

            invoice.AddSalesInvoiceItem(item1);
            invoice.AddSalesInvoiceItem(item2);
            invoice.AddSalesInvoiceItem(item3);

            this.DatabaseSession.Derive(true);

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(50)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(item2).WithAmountApplied(50).Build())
                .WithEffectiveDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(0, item1.AmountPaid);
            Assert.AreEqual(50, item2.AmountPaid);
            Assert.AreEqual(0, item3.AmountPaid);

            new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(350)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(item1).WithAmountApplied(100).Build())
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(item2).WithAmountApplied(150).Build())
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(item3).WithAmountApplied(100).Build())
                .WithEffectiveDate(DateTime.UtcNow)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(100, item1.AmountPaid);
            Assert.AreEqual(200, item2.AmountPaid);
            Assert.AreEqual(100, item3.AmountPaid);
        }

        [Test]
        public void GivenReceipt_WhenDeriving_ThenAmountCanNotBeSmallerThenAmountApplied()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var billToContactMechanism = new EmailAddressBuilder(this.DatabaseSession).WithElectronicAddressString("info@allors.com").Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();
            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation)
                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(billToContactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession)
                                        .WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem)
                                        .WithProduct(this.good)
                                        .WithQuantity(1)
                                        .WithActualUnitPrice(100M)
                                        .Build())
                .Build();

            this.DatabaseSession.Derive(true);

            var receipt = new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(100)
                .WithEffectiveDate(DateTime.UtcNow)
                .WithPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice.SalesInvoiceItems[0]).WithAmountApplied(50).Build())
                .Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            receipt.AddPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice.SalesInvoiceItems[0]).WithAmountApplied(50).Build());

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            receipt.AddPaymentApplication(new PaymentApplicationBuilder(this.DatabaseSession).WithInvoiceItem(invoice.SalesInvoiceItems[0]).WithAmountApplied(1).Build());

            var derivationLog = this.DatabaseSession.Derive();
            Assert.IsTrue(derivationLog.HasErrors);
            Assert.Contains(Receipts.Meta.Amount, derivationLog.Errors[0].RoleTypes);
        }

        private void InstantiateObjects(ISession session)
        {
            this.good = (Good)session.Instantiate(this.good);
            this.internalOrganisation = (InternalOrganisation)session.Instantiate(this.internalOrganisation);
            this.billToCustomer = (Organisation)session.Instantiate(this.billToCustomer);
        }
    }
}