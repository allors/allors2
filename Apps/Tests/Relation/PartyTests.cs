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
    using NUnit.Framework;

    [TestFixture]
    public class PartyTests : DomainTest
    {
        [Test]
        public void GivenParty_WhenSalesRepRelationshipIsUpdated_ThenCurrentSalesRepsAreUpdated()
        {
            var salesRep1 = new PersonBuilder(this.DatabaseSession).WithLastName("salesRep1").Build();
            var salesRep2 = new PersonBuilder(this.DatabaseSession).WithLastName("salesRep2").Build();
            var salesRep3 = new PersonBuilder(this.DatabaseSession).WithLastName("salesRep3").Build();
            var organisation = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();

            var salesRepRelationship1 = new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(organisation)
                .WithSalesRepresentative(salesRep1)
                .WithFromDate(DateTimeFactory.CreateDate(2010, 01, 01))
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, organisation.CurrentSalesReps.Count);
            Assert.Contains(salesRep1, organisation.CurrentSalesReps);

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(organisation)
                .WithSalesRepresentative(salesRep2)
                .WithFromDate(DateTimeFactory.CreateDate(2010, 01, 01))
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, organisation.CurrentSalesReps.Count);
            Assert.Contains(salesRep1, organisation.CurrentSalesReps);
            Assert.Contains(salesRep2, organisation.CurrentSalesReps);

            salesRepRelationship1.ThroughDate = DateTimeFactory.CreateDate(2010, 12, 31);
            
            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, organisation.CurrentSalesReps.Count);
            Assert.Contains(salesRep2, organisation.CurrentSalesReps);

            new SalesRepRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(organisation)
                .WithSalesRepresentative(salesRep3)
                .WithProductCategory(new ProductCategoryBuilder(this.DatabaseSession).WithDescription("category").Build())
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, organisation.CurrentSalesReps.Count);
            Assert.Contains(salesRep2, organisation.CurrentSalesReps);
            Assert.Contains(salesRep3, organisation.CurrentSalesReps);
        }

        [Test]
        public void GivenPartyWithOpenOrders_WhenDeriving_ThenOpenOrderAmountIsUpdated()
        {
            throw new Exception("TODO");

            //var organisation = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            //var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            //new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(organisation).WithInternalOrganisation(internalOrganisation).Build();

            //var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();

            //var postalAddress = new PostalAddressBuilder(this.DatabaseSession)
            //      .WithAddress1("Kleine Nieuwedijkstraat 2")
            //      .WithGeographicBoundary(mechelen)
            //      .Build();

            //new SalesOrderBuilder(this.DatabaseSession).WithBillToCustomer(organisation).WithShipToAddress(postalAddress).WithTotalIncVat(100M).Build();
            //new SalesOrderBuilder(this.DatabaseSession).WithBillToCustomer(organisation).WithShipToAddress(postalAddress).WithTotalIncVat(200M).Build();
            //new SalesOrderBuilder(this.DatabaseSession).WithBillToCustomer(organisation).WithShipToAddress(postalAddress).WithTotalIncVat(400M).WithCurrentObjectState(new SalesOrderObjectStates(this.DatabaseSession).Finished).Build();
            
            //this.DatabaseSession.Derive(true);

            //Assert.AreEqual(300M, organisation.OpenOrderAmount);
        }

        [Test]
        public void GivenPartyWithRevenue_WhenDeriving_ThenTotalRevenuesAreUpdated()
        {
            var customer = new OrganisationBuilder(this.DatabaseSession).WithName("customer").Build();
            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation");
            var productItem = new SalesInvoiceItemTypes(this.DatabaseSession).ProductItem;
            var contactMechanism = new ContactMechanisms(this.DatabaseSession).Extent().First;

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.DatabaseSession).WithRate(0).Build())
                .WithName("good")
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
                .Build();

            new SalesInvoiceBuilder(this.DatabaseSession)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithActualUnitPrice(100M).WithQuantity(1).WithSalesInvoiceItemType(productItem).Build())
                .WithInvoiceDate(date2)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            new SalesInvoiceBuilder(this.DatabaseSession)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.DatabaseSession).SalesInvoice)
                .WithBillToCustomer(customer)
                .WithSalesInvoiceItem(new SalesInvoiceItemBuilder(this.DatabaseSession).WithProduct(good).WithActualUnitPrice(100M).WithQuantity(1).WithSalesInvoiceItemType(productItem).Build())
                .WithInvoiceDate(date3)
                .WithBillToContactMechanism(contactMechanism)
                .Build();

            this.DatabaseSession.Derive(true);

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(10M, customer.LastYearsRevenue);
            Assert.AreEqual(200M, customer.YTDRevenue);
        }
    }
}
