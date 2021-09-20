// <copyright file="PaymentApplicationTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Allors.Meta;
    using Xunit;

    public class PaymentApplicationTests : DomainTest
    {
        [Fact]
        public void GivenPaymentApplication_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var billToContactMechanism = new EmailAddressBuilder(this.Session).WithElectronicAddressString("info@allors.com").Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").Build();
            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(customer)
                .Build();

            var good1 = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithAssignedBillToContactMechanism(billToContactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session)
                                        .WithProduct(good1)
                                        .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                                        .WithQuantity(1)
                                        .WithAssignedUnitPrice(100M)
                                        .Build())
                .Build();

            var builder = new PaymentApplicationBuilder(this.Session);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenPaymentApplication_WhenDeriving_ThenAmountAppliedCannotBeLargerThenAmountReceived()
        {
            var contactMechanism = new ContactMechanisms(this.Session).Extent().First;
            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            var customer = new PersonBuilder(this.Session).WithLastName("customer").Build();
            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(customer)

                .Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithAssignedBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithAssignedUnitPrice(1000M).WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem).Build())
                .Build();

            this.Session.Derive();

            var receipt = new ReceiptBuilder(this.Session)
                .WithAmount(100)
                .WithEffectiveDate(this.Session.Now())
                .Build();

            var paymentApplication = new PaymentApplicationBuilder(this.Session)
                .WithAmountApplied(200)
                .WithInvoiceItem(invoice.InvoiceItems[0])
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            receipt.AddPaymentApplication(paymentApplication);

            var derivationLog = this.Session.Derive(false);
            Assert.True(derivationLog.HasErrors);
            Assert.Contains(M.PaymentApplication.AmountApplied, derivationLog.Errors[0].RoleTypes);
        }
    }
}
