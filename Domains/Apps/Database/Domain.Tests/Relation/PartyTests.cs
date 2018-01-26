// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyTests.cs" company="Allors bvba">
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

using System.Linq;

namespace Allors.Domain
{
    using System;
    using Meta;
    using Xunit;

    
    public class PartyTests : DomainTest
    {
        [Fact]
        public void GivenParty_WhenSalesRepRelationshipIsUpdated_ThenCurrentSalesRepsAreUpdated()
        {
            var salesRep1 = new PersonBuilder(this.Session).WithLastName("salesRep1").Build();
            var salesRep2 = new PersonBuilder(this.Session).WithLastName("salesRep2").Build();
            var salesRep3 = new PersonBuilder(this.Session).WithLastName("salesRep3").Build();
            var organisation = new OrganisationBuilder(this.Session).WithName("customer").Build();

            var salesRepRelationship1 = new SalesRepRelationshipBuilder(this.Session)
                .WithCustomer(organisation)
                .WithSalesRepresentative(salesRep1)
                .WithFromDate(DateTimeFactory.CreateDate(2010, 01, 01))
                .Build();

            this.Session.Derive();

            Assert.Equal(1, organisation.CurrentSalesReps.Count);
            Assert.Contains(salesRep1, organisation.CurrentSalesReps);

            new SalesRepRelationshipBuilder(this.Session)
                .WithCustomer(organisation)
                .WithSalesRepresentative(salesRep2)
                .WithFromDate(DateTimeFactory.CreateDate(2010, 01, 01))
                .Build();

            this.Session.Derive();

            Assert.Equal(2, organisation.CurrentSalesReps.Count);
            Assert.Contains(salesRep1, organisation.CurrentSalesReps);
            Assert.Contains(salesRep2, organisation.CurrentSalesReps);

            salesRepRelationship1.ThroughDate = DateTimeFactory.CreateDate(2010, 12, 31);
            
            this.Session.Derive();

            Assert.Equal(1, organisation.CurrentSalesReps.Count);
            Assert.Contains(salesRep2, organisation.CurrentSalesReps);

            new SalesRepRelationshipBuilder(this.Session)
                .WithCustomer(organisation)
                .WithSalesRepresentative(salesRep3)
                .WithProductCategory(new ProductCategoryBuilder(this.Session)
                                        .WithName("category")
                                        .Build())
                .Build();

            this.Session.Derive();

            Assert.Equal(2, organisation.CurrentSalesReps.Count);
            Assert.Contains(salesRep2, organisation.CurrentSalesReps);
            Assert.Contains(salesRep3, organisation.CurrentSalesReps);
        }

        [Fact]
        public void GivenPartyWithOpenOrders_WhenDeriving_ThenOpenOrderAmountIsUpdated()
        {
            var organisation = new OrganisationBuilder(this.Session).WithName("customer").Build();

            var customerRelationship = new CustomerRelationshipBuilder(this.Session).WithCustomer(organisation).Build();
            var partyFinancial = organisation.PartyFinancials.First(v => Equals(v.InternalOrganisation, customerRelationship.InternalOrganisation));

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            var postalAddress = new PostalAddressBuilder(this.Session)
                  .WithAddress1("Kleine Nieuwedijkstraat 2")
                  .WithGeographicBoundary(mechelen)
                  .Build();

            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(21).Build())
                .WithName("good")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.Session.Derive();

            var salesOrder1 = new SalesOrderBuilder(this.Session).WithBillToCustomer(organisation).WithShipToAddress(postalAddress).WithComment("salesorder1").Build();
            var orderItem1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(10)
                .Build();
            salesOrder1.AddSalesOrderItem(orderItem1);

            this.Session.Derive();

            var salesOrder2 = new SalesOrderBuilder(this.Session).WithBillToCustomer(organisation).WithShipToAddress(postalAddress).WithComment("salesorder2").Build();
            var orderItem2 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(10)
                .Build();
            salesOrder2.AddSalesOrderItem(orderItem2);

            this.Session.Derive();

            var salesOrder3 = new SalesOrderBuilder(this.Session).WithBillToCustomer(organisation).WithShipToAddress(postalAddress).WithComment("salesorder3").Build();
            var orderItem3 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(10)
                .Build();
            salesOrder3.AddSalesOrderItem(orderItem3);
            salesOrder3.Cancel();

            this.Session.Derive();

            Assert.Equal(242M, partyFinancial.OpenOrderAmount);
        }

        [Fact]
        public void GivenPartyWithRevenue_WhenDeriving_ThenTotalRevenuesAreUpdated()
        {
            var customer = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var productItem = new SalesInvoiceItemTypes(this.Session).ProductItem;
            var contactMechanism = new ContactMechanisms(this.Session).Extent().First;

            var customerRelationship = new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithFromDate(DateTime.Now.AddYears(-2)).Build();
            var partyFinancial = customer.PartyFinancials.First(v => Equals(v.InternalOrganisation, customerRelationship.InternalOrganisation));

            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(0).Build())
                .WithName("goof")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .Build();

            this.Session.Derive();

            var date1 = DateTimeFactory.CreateDate(DateTime.UtcNow.AddYears(-1).Year, 1, 1);
            var date2 = DateTimeFactory.CreateDate(DateTime.UtcNow.Year, 1, 1);
            var date3 = DateTimeFactory.CreateDate(DateTime.UtcNow.Year, 2, 1);

            new SalesInvoiceBuilder(this.Session)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithActualUnitPrice(10M).WithQuantity(1).WithSalesInvoiceItemType(productItem).Build())
                .WithInvoiceDate(date1)
                .WithBillToContactMechanism(contactMechanism)
                .WithComment("invoice1")
                .Build();

            new SalesInvoiceBuilder(this.Session)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithActualUnitPrice(100M).WithQuantity(1).WithSalesInvoiceItemType(productItem).Build())
                .WithInvoiceDate(date2)
                .WithBillToContactMechanism(contactMechanism)
                .WithComment("invoice2")
                .Build();

            new SalesInvoiceBuilder(this.Session)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Session).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.Session).WithProduct(good).WithActualUnitPrice(100M).WithQuantity(1).WithSalesInvoiceItemType(productItem).Build())
                .WithInvoiceDate(date3)
                .WithBillToContactMechanism(contactMechanism)
                .WithComment("invoice3")
                .Build();

            this.Session.Derive();

            Assert.Equal(10M, partyFinancial.LastYearsRevenue);
            Assert.Equal(200M, partyFinancial.YTDRevenue);
        }
    }
}
