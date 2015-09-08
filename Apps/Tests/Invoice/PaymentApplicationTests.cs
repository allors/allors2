//------------------------------------------------------------------------------------------------- 
// <copyright file="PaymentApplicationTests.cs" company="Allors bvba">
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
    public class PaymentApplicationTests : DomainTest
    {
        [Test]
        public void GivenPaymentApplication_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var billToContactMechanism = new EmailAddressBuilder(this.DatabaseSession).WithElectronicAddressString("info@allors.com").Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();
            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation)
                .Build();

            new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(billToContactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession)
                                        .WithProduct(new GoodBuilder(this.DatabaseSession).WithName("good").Build())  
                                        .WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem)
                                        .WithQuantity(1)
                                        .WithActualUnitPrice(100M)
                                        .Build())
                .Build();

            var builder = new PaymentApplicationBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithAmountApplied(0);
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenPaymentApplication_WhenDeriving_ThenAmountAppliedCannotBeLargerThenAmountReceived()
        {
            var contactMechanism = new ContactMechanisms(this.DatabaseSession).Extent().First;

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var customer = new PersonBuilder(this.DatabaseSession).WithLastName("customer").Build();
            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(customer)
                .WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation)
                .Build();

            var invoice = new SalesInvoiceBuilder(this.DatabaseSession)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantity(1).WithActualUnitPrice(1000M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem).Build())
                .Build();

            this.DatabaseSession.Derive(true);

            var receipt = new ReceiptBuilder(this.DatabaseSession)
                .WithAmount(100)
                .WithEffectiveDate(DateTime.UtcNow)
                .Build();

            var paymentApplication = new PaymentApplicationBuilder(this.DatabaseSession)
                .WithAmountApplied(200)
                .WithInvoiceItem(invoice.InvoiceItems[0])
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();
            
            receipt.AddPaymentApplication(paymentApplication);

            var derivationLog = this.DatabaseSession.Derive();
            Assert.IsTrue(derivationLog.HasErrors);
            Assert.Contains(PaymentApplications.Meta.AmountApplied, derivationLog.Errors[0].RoleTypes);
        }
    }
}