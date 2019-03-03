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

            Assert.Single(organisation.CurrentSalesReps);
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

            Assert.Single(organisation.CurrentSalesReps);
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
            var store = this.Session.Extent<Store>().First;
            store.IsImmediatelyPicked = false;

            var organisation = new OrganisationBuilder(this.Session).WithName("customer").Build();
            var customerRelationship = new CustomerRelationshipBuilder(this.Session).WithCustomer(organisation).Build();

            this.Session.Derive();

            var partyFinancial = organisation.PartyFinancialRelationshipsWhereParty.First(v => Equals(v.InternalOrganisation, customerRelationship.InternalOrganisation));

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            var postalAddress = new PostalAddressBuilder(this.Session)
                  .WithAddress1("Kleine Nieuwedijkstraat 2")
                  .WithGeographicBoundary(mechelen)
                  .Build();

            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            this.Session.Derive();

            var salesOrder1 = new SalesOrderBuilder(this.Session).WithBillToCustomer(organisation).WithShipToAddress(postalAddress).WithComment("salesorder1").Build();
            var orderItem1 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithAssignedUnitPrice(10)
                .Build();
            salesOrder1.AddSalesOrderItem(orderItem1);

            this.Session.Derive();

            var salesOrder2 = new SalesOrderBuilder(this.Session).WithBillToCustomer(organisation).WithShipToAddress(postalAddress).WithComment("salesorder2").Build();
            var orderItem2 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithAssignedUnitPrice(10)
                .Build();
            salesOrder2.AddSalesOrderItem(orderItem2);

            this.Session.Derive();

            var salesOrder3 = new SalesOrderBuilder(this.Session).WithBillToCustomer(organisation).WithShipToAddress(postalAddress).WithComment("salesorder3").Build();
            var orderItem3 = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(10)
                .WithAssignedUnitPrice(10)
                .Build();
            salesOrder3.AddSalesOrderItem(orderItem3);
            salesOrder3.Cancel();

            this.Session.Derive();

            Assert.Equal(242M, partyFinancial.OpenOrderAmount);
        }
    }
}
