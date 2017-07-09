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
            var salesRep1 = new PersonBuilder(this.DatabaseSession).WithLastName("salesRep1").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var salesRep2 = new PersonBuilder(this.DatabaseSession).WithLastName("salesRep2").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var salesRep3 = new PersonBuilder(this.DatabaseSession).WithLastName("salesRep3").WithPersonRole(new PersonRoles(this.DatabaseSession).Employee).Build();
            var organisation = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();

            var salesRepRelationship1 = new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(organisation)
                .WithSalesRepresentative(salesRep1)
                .WithFromDate(DateTimeFactory.CreateDate(2010, 01, 01))
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.Equal(1, organisation.CurrentSalesReps.Count);
            Assert.Contains(salesRep1, organisation.CurrentSalesReps);

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(organisation)
                .WithSalesRepresentative(salesRep2)
                .WithFromDate(DateTimeFactory.CreateDate(2010, 01, 01))
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.Equal(2, organisation.CurrentSalesReps.Count);
            Assert.Contains(salesRep1, organisation.CurrentSalesReps);
            Assert.Contains(salesRep2, organisation.CurrentSalesReps);

            salesRepRelationship1.ThroughDate = DateTimeFactory.CreateDate(2010, 12, 31);
            
            this.DatabaseSession.Derive(true);

            Assert.Equal(1, organisation.CurrentSalesReps.Count);
            Assert.Contains(salesRep2, organisation.CurrentSalesReps);

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(organisation)
                .WithSalesRepresentative(salesRep3)
                .WithProductCategory(new ProductCategoryBuilder(this.DatabaseSession)
                                        .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("category").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                                        .Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.Equal(2, organisation.CurrentSalesReps.Count);
            Assert.Contains(salesRep2, organisation.CurrentSalesReps);
            Assert.Contains(salesRep3, organisation.CurrentSalesReps);
        }

        [Fact]
        public void GivenPartyWithOpenOrders_WhenDeriving_ThenOpenOrderAmountIsUpdated()
        {
            var organisation = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(organisation).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            var postalAddress = new PostalAddressBuilder(this.DatabaseSession)
                  .WithAddress1("Kleine Nieuwedijkstraat 2")
                  .WithGeographicBoundary(mechelen)
                  .Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(21).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("good").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive(true);

            var salesOrder1 = new SalesOrderBuilder(this.DatabaseSession).WithBillToCustomer(organisation).WithShipToAddress(postalAddress).WithComment("salesorder1").Build();
            var orderItem1 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(10)
                .Build();
            salesOrder1.AddSalesOrderItem(orderItem1);

            var salesOrder2 = new SalesOrderBuilder(this.DatabaseSession).WithBillToCustomer(organisation).WithShipToAddress(postalAddress).WithComment("salesorder2").Build();
            var orderItem2 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(10)
                .Build();
            salesOrder2.AddSalesOrderItem(orderItem2);

            var salesOrder3 = new SalesOrderBuilder(this.DatabaseSession).WithBillToCustomer(organisation).WithShipToAddress(postalAddress).WithComment("salesorder3").Build();
            var orderItem3 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithActualUnitPrice(10)
                .Build();
            salesOrder3.AddSalesOrderItem(orderItem3);
            salesOrder3.Cancel();

            this.DatabaseSession.Derive(true);

            Assert.Equal(242M, organisation.OpenOrderAmount);
        }

        [Fact]
        public void GivenPartyWithRevenue_WhenDeriving_ThenTotalRevenuesAreUpdated()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").WithOrganisationRole(new OrganisationRoles(this.DatabaseSession).Customer).Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");
            var productItem = new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem;
            var contactMechanism = new ContactMechanisms(this.DatabaseSession).Extent().First;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("goof").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive(true);

            var date1 = DateTimeFactory.CreateDate(DateTime.UtcNow.AddYears(-1).Year, 1, 1);
            var date2 = DateTimeFactory.CreateDate(DateTime.UtcNow.Year, 1, 1);
            var date3 = DateTimeFactory.CreateDate(DateTime.UtcNow.Year, 2, 1);

            new SalesInvoiceBuilder(this.DatabaseSession)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithActualUnitPrice(10M).WithQuantity(1).WithSalesInvoiceItemType(productItem).Build())
                .WithInvoiceDate(date1)
                .WithBillToContactMechanism(contactMechanism)
                .WithComment("invoice1")
                .Build();

            new SalesInvoiceBuilder(this.DatabaseSession)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithActualUnitPrice(100M).WithQuantity(1).WithSalesInvoiceItemType(productItem).Build())
                .WithInvoiceDate(date2)
                .WithBillToContactMechanism(contactMechanism)
                .WithComment("invoice2")
                .Build();

            new SalesInvoiceBuilder(this.DatabaseSession)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithActualUnitPrice(100M).WithQuantity(1).WithSalesInvoiceItemType(productItem).Build())
                .WithInvoiceDate(date3)
                .WithBillToContactMechanism(contactMechanism)
                .WithComment("invoice3")
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.Equal(10M, customer.LastYearsRevenue);
            Assert.Equal(200M, customer.YTDRevenue);
        }
    }
}
