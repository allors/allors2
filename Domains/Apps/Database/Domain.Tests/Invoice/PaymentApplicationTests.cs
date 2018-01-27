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
    using Meta;
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

            new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(billToContactMechanism)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session)
                                        .WithProduct(new GoodBuilder(this.Session)
                                                            .WithOrganisation(this.InternalOrganisation)
                                                            .WithName("good")
                                                            .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                                                            .Build())  
                                        .WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem)
                                        .WithQuantity(1)
                                        .WithActualUnitPrice(100M)
                                        .Build())
                .Build();

            var builder = new PaymentApplicationBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithAmountApplied(0);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenPaymentApplication_WhenDeriving_ThenAmountAppliedCannotBeLargerThenAmountReceived()
        {
            var contactMechanism = new ContactMechanisms(this.Session).Extent().First;

            var good = new GoodBuilder(this.Session)
                .WithOrganisation(this.InternalOrganisation)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(0).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            var customer = new PersonBuilder(this.Session).WithLastName("customer").Build();
            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(customer)
                
                .Build();

            var invoice = new SalesInvoiceBuilder(this.Session)
                .WithBillToCustomer(customer)
                .WithBillToContactMechanism(contactMechanism)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithActualUnitPrice(1000M).WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Session).ProductItem).Build())
                .Build();

            this.Session.Derive();

            var receipt = new ReceiptBuilder(this.Session)
                .WithAmount(100)
                .WithEffectiveDate(DateTime.UtcNow)
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