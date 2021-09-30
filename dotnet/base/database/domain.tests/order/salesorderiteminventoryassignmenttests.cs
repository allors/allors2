// <copyright file="SalesOrderItemInventoryAssignmentTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using System.Linq;
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

            new CustomerRelationshipBuilder(this.Session).WithFromDate(this.Session.Now()).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

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
                .WithVatRegime(new VatRegimes(this.Session).BelgiumStandard)
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
                .WithAssignedShipToAddress(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .WithAssignedBillToContactMechanism(new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build())
                .Build();

            this.salesOrderItem = new SalesOrderItemBuilder(this.Session)
                .WithProduct(good)
                .WithQuantityOrdered(3)
                .WithAssignedUnitPrice(5)
                .Build();

            salesOrder.AddSalesOrderItem(this.salesOrderItem);

            this.Session.Derive();

            salesOrder.SetReadyForPosting();
            this.Session.Derive();

            salesOrder.Post();
            this.Session.Derive();

            salesOrder.Accept();
            this.Session.Derive();

            this.Session.Commit();
        }

        [Fact]
        public void GivenSalesOrderItem_WhenAddedToOrder_ThenInventoryReservationCreated()
        {
            Assert.True(this.salesOrderItem.SalesOrderItemState.IsInProcess);
            Assert.Single(this.salesOrderItem.SalesOrderItemInventoryAssignments);
            var transactions = this.salesOrderItem.SalesOrderItemInventoryAssignments.First.InventoryItemTransactions;

            Assert.Single(transactions);
            var transaction = transactions[0];
            Assert.Equal(this.part, transaction.Part);
            Assert.Equal(3, transaction.Quantity);
            Assert.Equal(this.reasons.Reservation, transaction.Reason);

            Assert.Equal(3, this.salesOrderItem.QuantityReserved);
            Assert.Equal(3, this.salesOrderItem.QuantityCommittedOut);

            Assert.Equal(3, ((NonSerialisedInventoryItem)this.part.InventoryItemsWherePart.First()).QuantityCommittedOut);
            Assert.Equal(11, ((NonSerialisedInventoryItem)this.part.InventoryItemsWherePart.First()).QuantityOnHand);

            Assert.Equal(3, this.part.QuantityCommittedOut);
            Assert.Equal(11, this.part.QuantityOnHand);
        }

        //[Fact]
        //public void GivenSalesOrderItem_WhenChangingInventoryItem_ThenInventoryReservationsChange()
        //{
        //    var facility2 = new FacilityBuilder(this.Session)
        //        .WithFacilityType(new FacilityTypes(this.Session).Warehouse)
        //        .WithName("facility 2")
        //        .WithOwner(this.InternalOrganisation)
        //        .Build();

        //    new InventoryItemTransactionBuilder(this.Session)
        //        .WithPart(this.part)
        //        .WithReason(new InventoryTransactionReasons(this.Session).IncomingShipment)
        //        .WithFacility(facility2)
        //        .WithQuantity(10)
        //        .Build();

        //    this.Session.Derive();

        //    this.salesOrderItem.ReservedFromNonSerialisedInventoryItem = (NonSerialisedInventoryItem)this.part.InventoryItemsWherePart.Single(v => v.Facility.Equals(facility2));

        //    this.Session.Derive();

        //    Assert.True(this.salesOrderItem.SalesOrderItemState.InProcess);
        //    Assert.Equal(2, this.salesOrderItem.SalesOrderItemInventoryAssignments.Count);

        //    var previousInventoryItem = (NonSerialisedInventoryItem)this.part.InventoryItemsWherePart.FirstOrDefault(v => v.Facility.Name.Equals("facility"));
        //    var currentInventoryItem = this.salesOrderItem.ReservedFromNonSerialisedInventoryItem;

        //    Assert.Equal(11, previousInventoryItem.QuantityOnHand);
        //    Assert.Equal(0, previousInventoryItem.QuantityCommittedOut);
        //    Assert.Equal(11, previousInventoryItem.AvailableToPromise);

        //    Assert.Equal(10, currentInventoryItem.QuantityOnHand);
        //    Assert.Equal(3, currentInventoryItem.QuantityCommittedOut);
        //    Assert.Equal(7, currentInventoryItem.AvailableToPromise);

        //    Assert.Equal(3, this.salesOrderItem.QuantityReserved);

        //    Assert.Equal(3, this.part.QuantityCommittedOut);
        //    Assert.Equal(21, this.part.QuantityOnHand);
        //}

        [Fact]
        public void GivenSalesOrderItem_WhenChangingQuantity_ThenInventoryReservationsChange()
        {
            this.salesOrderItem.QuantityOrdered = 6;

            this.Session.Derive();

            var transaction = this.salesOrderItem.SalesOrderItemInventoryAssignments.First.InventoryItemTransactions.Last();
            Assert.Equal(this.part, transaction.Part);
            Assert.Equal(3, transaction.Quantity);
            Assert.Equal(this.reasons.Reservation, transaction.Reason);

            var inventoryItem = (NonSerialisedInventoryItem)this.part.InventoryItemsWherePart.First();

            Assert.Equal(11, inventoryItem.QuantityOnHand);
            Assert.Equal(6, inventoryItem.QuantityCommittedOut);
            Assert.Equal(5, inventoryItem.AvailableToPromise);

            Assert.Equal(6, this.salesOrderItem.QuantityReserved);

            Assert.Equal(6, this.part.QuantityCommittedOut);
            Assert.Equal(11, this.part.QuantityOnHand);
        }
    }
}
