//------------------------------------------------------------------------------------------------- 
// <copyright file="WorkTaskTests.cs" company="Allors bvba">
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

using System.Linq;

namespace Allors.Domain
{
    using Xunit;
        
    public class WorkEffortInventoryAssignmentTests : DomainTest
    {
        [Fact]
        public void GivenWorkEffort_WhenAddingInventoryAssignment_ThenInventoryReservationCreated()
        {
            // Arrange
            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var reasons = new InventoryTransactionReasons(this.Session);

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();
            var part = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .Build();

            new InventoryItemTransactionBuilder(this.Session)
                .WithPart(part)
                .WithReason(new InventoryTransactionReasons(this.Session).IncomingShipment)
                .WithQuantity(11)
                .Build();

            // Act
            this.Session.Derive(true);

            // Assert
            Assert.Empty(workEffort.WorkEffortInventoryAssignmentsWhereAssignment);
            Assert.True(workEffort.WorkEffortState.IsCreated);

            // Re-arrange
            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithInventoryItem(part.InventoryItemsWherePart.First)
                .WithQuantity(10)
                .Build();

            // Act
            this.Session.Derive(true);

            // Assert
            var transactions = inventoryAssignment.InventoryItemTransactions;

            Assert.Single(transactions);
            var transaction = transactions[0];
            Assert.Equal(part, transaction.Part);
            Assert.Equal(10, transaction.Quantity);
            Assert.Equal(reasons.Reservation, transaction.Reason);

            Assert.Equal(10, part.QuantityCommittedOut);
            Assert.Equal(11, part.QuantityOnHand);
        }

        [Fact]
        public void GivenWorkEffort_WhenAddingInventoryAssignment_ThenInventory()
        {
            var reasons = new InventoryTransactionReasons(this.Session);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();
            var part = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .Build();

            this.Session.Derive(true);

            var inventoryItem = part.InventoryItemsWherePart.FirstOrDefault() as NonSerialisedInventoryItem;

            Assert.Empty(workEffort.WorkEffortInventoryAssignmentsWhereAssignment);
            Assert.True(workEffort.WorkEffortState.IsCreated);

            // Assignment when inventory qoh = 0
            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithInventoryItem(inventoryItem)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            var transactions = inventoryAssignment.InventoryItemTransactions;

            Assert.Single(transactions);
            var transaction = transactions[0];
            Assert.Equal(part, transaction.Part);
            Assert.Equal(10, transaction.Quantity);
            Assert.Equal(reasons.Reservation, transaction.Reason);

            Assert.Equal(10, inventoryItem.QuantityCommittedOut);
            Assert.Equal(0, inventoryItem.QuantityOnHand);
            Assert.Equal(0, inventoryItem.AvailableToPromise);
            Assert.Equal(0, inventoryItem.QuantityExpectedIn);

            // PurchaseOrder for part 
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();
            new SupplierRelationshipBuilder(this.Session).WithSupplier(supplier).Build();

            var order = new PurchaseOrderBuilder(this.Session).WithTakenViaSupplier(supplier).Build();

            var item1 = new PurchaseOrderItemBuilder(this.Session).WithPart(part).WithQuantityOrdered(20).Build();
            order.AddPurchaseOrderItem(item1);

            this.Session.Derive();

            order.Confirm();

            this.Session.Derive();

            Assert.Equal(10, inventoryItem.QuantityCommittedOut);
            Assert.Equal(0, inventoryItem.QuantityOnHand);
            Assert.Equal(0, inventoryItem.AvailableToPromise);
            Assert.Equal(20, inventoryItem.QuantityExpectedIn);

            order.QuickReceive();

            this.Session.Derive();

            Assert.Equal(10, inventoryItem.QuantityCommittedOut);
            Assert.Equal(20, inventoryItem.QuantityOnHand);
            Assert.Equal(10, inventoryItem.AvailableToPromise);
            Assert.Equal(0, inventoryItem.QuantityExpectedIn);

            workEffort.Complete();

            this.Session.Derive(true);

            Assert.Equal(0, inventoryItem.QuantityCommittedOut);
            Assert.Equal(10, inventoryItem.QuantityOnHand);
            Assert.Equal(10, inventoryItem.AvailableToPromise);
            Assert.Equal(0, inventoryItem.QuantityExpectedIn);
        }

        [Fact]
        public void GivenWorkEffortWithInventoryAssignment_WhenChangingPart_ThenInventoryReservationsChange()
        {
            // Arrange
            var reasons = new InventoryTransactionReasons(this.Session);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();
            var part1 = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .Build();
            var part2 = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P2")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .Build();

            new InventoryItemTransactionBuilder(this.Session)
                .WithPart(part1)
                .WithReason(new InventoryTransactionReasons(this.Session).IncomingShipment)
                .WithQuantity(10)
                .Build();

            new InventoryItemTransactionBuilder(this.Session)
                .WithPart(part2)
                .WithReason(new InventoryTransactionReasons(this.Session).IncomingShipment)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithInventoryItem(part1.InventoryItemsWherePart.First)
                .WithQuantity(10)
                .Build();

            // Act
            this.Session.Derive(true);

            // Assert
            var transactions = inventoryAssignment.InventoryItemTransactions.ToArray();

            Assert.Single(transactions);
            Assert.Equal(part1, transactions[0].Part);
            Assert.Equal(10, transactions[0].Quantity);
            Assert.Equal(reasons.Reservation, transactions[0].Reason);

            // Re-arrange
            inventoryAssignment.InventoryItem = part2.InventoryItemsWherePart.First;

            // Act
            this.Session.Derive(true);

            // Assert
            var part1Transactions = inventoryAssignment.InventoryItemTransactions.Where(t => t.Part.Equals(part1)).ToArray();
            var part2Transactions = inventoryAssignment.InventoryItemTransactions.Where(t => t.Part.Equals(part2)).ToArray();

            Assert.Equal(0, part1Transactions.Sum(t => t.Quantity));
            Assert.Equal(10, part2Transactions.Sum(t => t.Quantity));

            Assert.Equal(0, part1.QuantityCommittedOut);
            Assert.Equal(10, part2.QuantityCommittedOut);
        }

        [Fact]
        public void GivenWorkEffortWithInventoryAssignment_WhenCancelling_ThenInventoryReservationCancelled()
        {
            // Arrange
            var reasons = new InventoryTransactionReasons(this.Session);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();
            var part = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .Build();

            new InventoryItemTransactionBuilder(this.Session)
                .WithPart(part)
                .WithReason(new InventoryTransactionReasons(this.Session).IncomingShipment)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithInventoryItem(part.InventoryItemsWherePart.First)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            // Act
            workEffort.Cancel();
            this.Session.Derive(true);

            // Assert
            var transactions = inventoryAssignment.InventoryItemTransactions;

            Assert.Equal(2, transactions.Count);

            var reservation = transactions.First(t => t.Reason.Equals(reasons.Reservation) && (t.Quantity > 0));
            var reservationCancellation = transactions.First(t => t.Reason.Equals(reasons.Reservation) && (t.Quantity < 0));

            Assert.Equal(10, reservation.Quantity);
            Assert.Equal(-10, reservationCancellation.Quantity);

            Assert.Equal(0, part.QuantityCommittedOut);
        }

        [Fact]
        public void GivenWorkEffortWithInventoryAssignment_WhenCompleting_ThenInventoryTransactionsCreated()
        {
            // Arrange
            var reasons = new InventoryTransactionReasons(this.Session);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();
            var part = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .Build();

            new InventoryItemTransactionBuilder(this.Session).WithPart(part).WithQuantity(100).WithReason(reasons.PhysicalCount).Build();

            this.Session.Derive(true);

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithInventoryItem(part.InventoryItemsWherePart.First)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            // Act
            workEffort.Complete();
            this.Session.Derive(true);

            // Assert
            var transactions = inventoryAssignment.InventoryItemTransactions;

            Assert.Equal(2, transactions.Count);

            var reservation = transactions.First(t => t.Reason.Equals(reasons.Reservation) && (t.Quantity > 0));
            var consumption = transactions.First(t => t.Reason.Equals(reasons.Consumption));

            Assert.Equal(10, reservation.Quantity);
            Assert.Equal(10, consumption.Quantity);

            Assert.Equal(0, part.QuantityCommittedOut);
            Assert.Equal(90, part.QuantityOnHand);
        }

        [Fact]
        public void GivenWorkEffortWithInventoryAssignment_WhenCompletingThenCancelling_ThenInventoryTransactionsCancelled()
        {
            // Arrange
            var reasons = new InventoryTransactionReasons(this.Session);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();
            var part = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .Build();

            new InventoryItemTransactionBuilder(this.Session)
                .WithPart(part)
                .WithReason(new InventoryTransactionReasons(this.Session).IncomingShipment)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithInventoryItem(part.InventoryItemsWherePart.First)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            // Act
            workEffort.Complete();
            this.Session.Derive(true);

            workEffort.Cancel();
            this.Session.Derive(true);

            // Assert
            var transactions = inventoryAssignment.InventoryItemTransactions;
            var reservation = transactions.First(t => t.Reason.Equals(reasons.Reservation) && (t.Quantity > 0));
            var reservationCancellation = transactions.First(t => t.Reason.Equals(reasons.Reservation) && (t.Quantity < 0));
            var consumption = transactions.First(t => t.Reason.Equals(reasons.Consumption) && (t.Quantity > 0));
            var consumptionCancellation = transactions.First(t => t.Reason.Equals(reasons.Consumption) && (t.Quantity < 0));

            Assert.Equal(4, transactions.Count);

            Assert.Equal(10, reservation.Quantity);
            Assert.Equal(-10, reservationCancellation.Quantity);
            Assert.Equal(10, consumption.Quantity);
            Assert.Equal(-10, consumptionCancellation.Quantity);

            Assert.Equal(0, part.QuantityCommittedOut);
            Assert.Equal(10, part.QuantityOnHand);
        }

        [Fact]
        public void GivenWorkEffortWithInventoryAssignment_WhenChangingPartAndQuantityAndFinishing_ThenOldInventoryCancelledAndNewInventoryCreated()
        {
            // Arrange
            var reasons = new InventoryTransactionReasons(this.Session);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();
            var part1 = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .Build();
            var part2 = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P2")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .Build();

            new InventoryItemTransactionBuilder(this.Session)
                .WithPart(part1)
                .WithReason(new InventoryTransactionReasons(this.Session).IncomingShipment)
                .WithQuantity(10)
                .Build();

            new InventoryItemTransactionBuilder(this.Session)
                .WithPart(part2)
                .WithReason(new InventoryTransactionReasons(this.Session).IncomingShipment)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithInventoryItem(part1.InventoryItemsWherePart.First)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            // Act
            inventoryAssignment.InventoryItem= part2.InventoryItemsWherePart.First;
            inventoryAssignment.Quantity = 5;

            workEffort.Complete();
            this.Session.Derive(true);

            // Assert
            var part1Transactions = inventoryAssignment.InventoryItemTransactions.Where(t => t.Part.Equals(part1)).ToArray();
            var part2Transactions = inventoryAssignment.InventoryItemTransactions.Where(t => t.Part.Equals(part2)).ToArray();

            var part1Reservations = part1Transactions.Where(t => t.Reason.Equals(reasons.Reservation));
            var part2Reservations = part2Transactions.Where(t => t.Reason.Equals(reasons.Reservation));
            var part2Consumption = part2Transactions.Where(t => t.Reason.Equals(reasons.Consumption));

            Assert.Equal(0, part1Reservations.Sum(r => r.Quantity));
            Assert.Equal(5, part2Reservations.Sum(r => r.Quantity));
            Assert.Equal(5, part2Consumption.Sum(c => c.Quantity));

            Assert.Equal(0, part1.QuantityCommittedOut);
            Assert.Equal(10, part1.QuantityOnHand);
            Assert.Equal(0, part2.QuantityCommittedOut);
            Assert.Equal(5, part2.QuantityOnHand);
        }

        [Fact]
        public void GivenWorkEffortWithInventoryAssignment_WhenChangingPartAndQuantityAndReopening_ThenOldInventoryCancelledAndNewInventoryCreated()
        {
            // Arrange
            var reasons = new InventoryTransactionReasons(this.Session);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();
            var part1 = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .Build();
            var part2 = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P2")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .Build();

            new InventoryItemTransactionBuilder(this.Session)
                .WithPart(part1)
                .WithReason(new InventoryTransactionReasons(this.Session).IncomingShipment)
                .WithQuantity(10)
                .Build();

            new InventoryItemTransactionBuilder(this.Session)
                .WithPart(part2)
                .WithReason(new InventoryTransactionReasons(this.Session).IncomingShipment)
                .WithQuantity(5)
                .Build();

            this.Session.Derive(true);

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithInventoryItem(part1.InventoryItemsWherePart.First)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            workEffort.Complete();
            this.Session.Derive(true);

            // Act
            inventoryAssignment.InventoryItem = part2.InventoryItemsWherePart.First;
            inventoryAssignment.Quantity = 5;

            workEffort.Reopen();
            this.Session.Derive(true);

            // Assert
            var part1Transactions = inventoryAssignment.InventoryItemTransactions.Where(t => t.Part.Equals(part1)).ToArray();
            var part2Transactions = inventoryAssignment.InventoryItemTransactions.Where(t => t.Part.Equals(part2)).ToArray();

            var part1Reservations = part1Transactions.Where(t => t.Reason.Equals(reasons.Reservation));
            var part1Consumption = part1Transactions.Where(t => t.Reason.Equals(reasons.Consumption));
            var part2Reservations = part2Transactions.Where(t => t.Reason.Equals(reasons.Reservation));

            Assert.Equal(0, part1Reservations.Sum(r => r.Quantity));
            Assert.Equal(0, part1Consumption.Sum(c => c.Quantity));
            Assert.Equal(5, part2Reservations.Sum(r => r.Quantity));

            Assert.Equal(0, part1.QuantityCommittedOut);
            Assert.Equal(10, part1.QuantityOnHand);

            Assert.Equal(5, part2.QuantityCommittedOut);
            Assert.Equal(5, part2.QuantityOnHand);
        }

        [Fact]
        public void GivenWorkEffortWithInventoryAssignment_WhenChangingQuantity_ThenInventoryTransactionsCreated()
        {
            // Arrage
            var reasons = new InventoryTransactionReasons(this.Session);

            var customer = new OrganisationBuilder(this.Session).WithName("Org1").Build();
            var internalOrganisation = new Organisations(this.Session).Extent().First(o => o.IsInternalOrganisation);
            new CustomerRelationshipBuilder(this.Session).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").WithCustomer(customer).WithTakenBy(internalOrganisation).Build();
            var part = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("P1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .Build();

            new InventoryItemTransactionBuilder(this.Session)
                .WithPart(part)
                .WithReason(new InventoryTransactionReasons(this.Session).IncomingShipment)
                .WithQuantity(20)
                .Build();

            this.Session.Derive(true);

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithInventoryItem(part.InventoryItemsWherePart.First)
                .WithQuantity(5)
                .Build();

            // Act
            this.Session.Derive(true);

            // Assert
            var reservation = inventoryAssignment.InventoryItemTransactions.First(t => t.Reason.Equals(reasons.Reservation) && (t.Quantity > 0));
            Assert.Equal(5, reservation.Quantity);

            // Re-arrange
            inventoryAssignment.Quantity = 10;

            // Act
            this.Session.Derive(true);

            // Assert
            var reservations = inventoryAssignment.InventoryItemTransactions.Where(t => t.Reason.Equals(reasons.Reservation));
            Assert.Equal(10, reservations.Sum(r => r.Quantity));

            // Re-arrange
            workEffort.Complete();

            // Act
            this.Session.Derive(true);

            // Assert
            reservations = inventoryAssignment.InventoryItemTransactions.Where(t => t.Reason.Equals(reasons.Reservation));
            var consumption = inventoryAssignment.InventoryItemTransactions.First(t => t.Reason.Equals(reasons.Consumption));

            Assert.Equal(3, inventoryAssignment.InventoryItemTransactions.Count);

            Assert.Equal(10, reservations.Sum(r => r.Quantity));
            Assert.Equal(10, consumption.Quantity);

            Assert.Equal(0, part.QuantityCommittedOut);
            Assert.Equal(10, part.QuantityOnHand);
        }
    }
}