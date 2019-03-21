//------------------------------------------------------------------------------------------------- 
// <copyright file="SalesOrderItemInventoryAssignmentTests.cs" company="Allors bvba">
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

using System;
using System.Linq;

namespace Allors.Domain
{
    using Xunit;
        
    public class SalesOrderItemInventoryAssignmentTests : DomainTest
    {
        private readonly InventoryTransactionReasons reasons;
        private readonly SalesOrderItem salesOrderItem;
        private readonly Part part;

        public SalesOrderItemInventoryAssignmentTests()
        {
            this.reasons = new InventoryTransactionReasons(this.Session);

            var customer = new PersonBuilder(this.Session).WithFirstName("Koen").Build();
            var internalOrganisation = this.InternalOrganisation;

            new CustomerRelationshipBuilder(this.Session).WithFromDate(DateTime.UtcNow).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();

            this.part = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            var good = new NonUnifiedGoodBuilder(this.Session)
                .WithProductIdentification(new ProductNumberBuilder(this.Session)
                    .WithIdentification("10101")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Good).Build())
                .WithName("good")
                .WithPart(this.part)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(21).Build())
                .Build();

            this.Session.Derive();

            new InventoryItemTransactionBuilder(this.Session)
                .WithPart(this.part)
                .WithReason(new InventoryTransactionReasons(this.Session).IncomingShipment)
                .WithQuantity(11)
                .Build();

            this.Session.Derive();

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithTakenBy(this.InternalOrganisation)
                .WithShipToCustomer(customer)
                .WithShipToAddress(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithBillToContactMechanism(new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.salesOrderItem = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            salesOrder.AddSalesOrderItem(salesOrderItem);

            this.Session.Derive();

            salesOrder.Confirm();

            this.Session.Derive();
            this.Session.Commit();
        }

        [Fact]
        public void GivenSalesOrderItem_WhenAddedToOrder_ThenInventoryReservationCreated()
        {
            Assert.True(salesOrderItem.SalesOrderItemState.InProcess);
            Assert.Single(salesOrderItem.SalesOrderItemInventoryAssignmentsWhereSalesOrderItem);
            var transactions = salesOrderItem.SalesOrderItemInventoryAssignmentsWhereSalesOrderItem.First.InventoryItemTransactions;

            Assert.Single(transactions);
            var transaction = transactions[0];
            Assert.Equal(part, transaction.Part);
            Assert.Equal(3, transaction.Quantity);
            Assert.Equal(reasons.Reservation, transaction.Reason);

            Assert.Equal(3, salesOrderItem.QuantityReserved);
            Assert.Equal(3, salesOrderItem.QuantityCommittedOut);

            Assert.Equal(3, part.QuantityCommittedOut);
            Assert.Equal(11, part.QuantityOnHand);
        }

        [Fact]
        public void GivenSalesOrderItem_WhenChangingInventoryItem_ThenInventoryReservationsChange()
        {
            var facility2 = new FacilityBuilder(this.Session)
                .WithFacilityType(new FacilityTypes(this.Session).Warehouse)
                .WithName("facility 2")
                .WithOwner(this.InternalOrganisation)
                .Build();

            new InventoryItemTransactionBuilder(this.Session)
                .WithPart(this.part)
                .WithReason(new InventoryTransactionReasons(this.Session).IncomingShipment)
                .WithFacility(facility2)
                .WithQuantity(10)
                .Build();

            this.Session.Derive();

            this.salesOrderItem.ReservedFromNonSerialisedInventoryItem = (NonSerialisedInventoryItem) this.part.InventoryItemsWherePart.Single(v => v.Facility.Equals(facility2));

            this.Session.Derive();

            Assert.True(salesOrderItem.SalesOrderItemState.InProcess);
            Assert.Equal(2, salesOrderItem.SalesOrderItemInventoryAssignmentsWhereSalesOrderItem.Count);

            var previousInventoryItem = (NonSerialisedInventoryItem) this.part.InventoryItemsWherePart.FirstOrDefault(v => v.Facility.Name.Equals("facility"));
            var currentInventoryItem = salesOrderItem.ReservedFromNonSerialisedInventoryItem;

            Assert.Equal(11, previousInventoryItem.QuantityOnHand);
            Assert.Equal(0, previousInventoryItem.QuantityCommittedOut);
            Assert.Equal(11, previousInventoryItem.AvailableToPromise);

            Assert.Equal(10, currentInventoryItem.QuantityOnHand);
            Assert.Equal(3, currentInventoryItem.QuantityCommittedOut);
            Assert.Equal(7, currentInventoryItem.AvailableToPromise);

            Assert.Equal(3, salesOrderItem.QuantityReserved);

            Assert.Equal(3, part.QuantityCommittedOut);
            Assert.Equal(21, part.QuantityOnHand);
        }

        [Fact]
        public void GivenSalesOrderItem_WhenChangingQuantity_ThenInventoryReservationsChange()
        {
            this.salesOrderItem.QuantityOrdered = 1;

            this.Session.Derive();

            Assert.True(salesOrderItem.SalesOrderItemState.InProcess);
            Assert.Single(salesOrderItem.SalesOrderItemInventoryAssignmentsWhereSalesOrderItem);

            var transaction = salesOrderItem.SalesOrderItemInventoryAssignmentsWhereSalesOrderItem.First.InventoryItemTransactions.Last();
            Assert.Equal(part, transaction.Part);
            Assert.Equal(-2, transaction.Quantity);
            Assert.Equal(reasons.Reservation, transaction.Reason);

            var inventoryItem = (NonSerialisedInventoryItem)this.part.InventoryItemsWherePart.First();

            Assert.Equal(11, inventoryItem.QuantityOnHand);
            Assert.Equal(1, inventoryItem.QuantityCommittedOut);
            Assert.Equal(10, inventoryItem.AvailableToPromise);

            Assert.Equal(1, salesOrderItem.QuantityReserved);

            Assert.Equal(1, part.QuantityCommittedOut);
            Assert.Equal(11, part.QuantityOnHand);
        }
    }
}