// <copyright file="WorkEffortInventoryAssignmentTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using System.Linq;
    using Xunit;

    public class WorkEffortInventoryAssignmentTests : DomainTest
    {
        [Fact]
        public void GivenWorkEffort_WhenAddingInventoryAssignment_ThenInventoryConsumptionCreated()
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

            this.Session.Derive(true);

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
            Assert.Equal(reasons.Consumption, transaction.Reason);

            Assert.Equal(0, part.QuantityCommittedOut);
            Assert.Equal(1, part.QuantityOnHand);
        }

        [Fact]
        public void GivenWorkEffortWithInventoryAssignment_WhenChangingPart_ThenInventoryConsumptionChange()
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

            this.Session.Derive(true);

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
            Assert.Equal(reasons.Consumption, transactions[0].Reason);

            // Re-arrange
            inventoryAssignment.InventoryItem = part2.InventoryItemsWherePart.First;

            // Act
            this.Session.Derive(true);

            // Assert
            var part1Transactions = inventoryAssignment.InventoryItemTransactions.Where(t => t.Part.Equals(part1)).ToArray();
            var part2Transactions = inventoryAssignment.InventoryItemTransactions.Where(t => t.Part.Equals(part2)).ToArray();

            Assert.Equal(0, part1Transactions.Sum(t => t.Quantity));
            Assert.Equal(10, part2Transactions.Sum(t => t.Quantity));

            Assert.Equal(10, part1.QuantityOnHand);
            Assert.Equal(0, part2.QuantityOnHand);
        }

        [Fact]
        public void GivenWorkEffortWithInventoryAssignment_WhenCancelling_ThenInventoryConsumptionCancelled()
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

            this.Session.Derive(true);

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

            var consumption = transactions.First(t => t.Reason.Equals(reasons.Consumption) && (t.Quantity > 0));
            var consumptionCancellation = transactions.First(t => t.Reason.Equals(reasons.Consumption) && (t.Quantity < 0));

            Assert.Equal(10, consumption.Quantity);
            Assert.Equal(-10, consumptionCancellation.Quantity);

            Assert.Equal(10, part.QuantityOnHand);
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

            this.Session.Derive(true);

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

            Assert.Equal(1, transactions.Count);

            var consumption = transactions.First(t => t.Reason.Equals(reasons.Consumption));

            Assert.Equal(10, consumption.Quantity);

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

            this.Session.Derive(true);

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
            var consumption = transactions.First(t => t.Reason.Equals(reasons.Consumption) && (t.Quantity > 0));
            var consumptionCancellation = transactions.First(t => t.Reason.Equals(reasons.Consumption) && (t.Quantity < 0));

            Assert.Equal(2, transactions.Count);

            Assert.Equal(10, consumption.Quantity);
            Assert.Equal(-10, consumptionCancellation.Quantity);

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

            this.Session.Derive(true);

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
            inventoryAssignment.InventoryItem = part2.InventoryItemsWherePart.First;
            inventoryAssignment.Quantity = 5;

            workEffort.Complete();
            this.Session.Derive(true);

            // Assert
            var part1Transactions = inventoryAssignment.InventoryItemTransactions.Where(t => t.Part.Equals(part1)).ToArray();
            var part2Transactions = inventoryAssignment.InventoryItemTransactions.Where(t => t.Part.Equals(part2)).ToArray();

            var part1Consumptions = part1Transactions.Where(t => t.Reason.Equals(reasons.Consumption));
            var part2Consumptions = part2Transactions.Where(t => t.Reason.Equals(reasons.Consumption));

            Assert.Equal(0, part1Consumptions.Sum(r => r.Quantity));
            Assert.Equal(5, part2Consumptions.Sum(c => c.Quantity));

            Assert.Equal(10, part1.QuantityOnHand);
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

            this.Session.Derive(true);

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

            workEffort.Reopen();
            this.Session.Derive(true);

            // Act
            inventoryAssignment.InventoryItem = part2.InventoryItemsWherePart.First;
            inventoryAssignment.Quantity = 4;

            this.Session.Derive(true);

            // Assert
            var part1Transactions = inventoryAssignment.InventoryItemTransactions.Where(t => t.Part.Equals(part1)).ToArray();
            var part2Transactions = inventoryAssignment.InventoryItemTransactions.Where(t => t.Part.Equals(part2)).ToArray();

            var part1Consumptions = part1Transactions.Where(t => t.Reason.Equals(reasons.Consumption));
            var part2Consumptions = part2Transactions.Where(t => t.Reason.Equals(reasons.Consumption));

            Assert.Equal(0, part1Consumptions.Sum(c => c.Quantity));
            Assert.Equal(4, part2Consumptions.Sum(r => r.Quantity));

            Assert.Equal(10, part1.QuantityOnHand);
            Assert.Equal(1, part2.QuantityOnHand);
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

            this.Session.Derive(true);

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
            var consumption = inventoryAssignment.InventoryItemTransactions.First(t => t.Reason.Equals(reasons.Consumption) && (t.Quantity > 0));
            Assert.Equal(5, consumption.Quantity);

            // Re-arrange
            inventoryAssignment.Quantity = 10;

            // Act
            this.Session.Derive(true);

            // Assert
            var consumptions = inventoryAssignment.InventoryItemTransactions.Where(t => t.Reason.Equals(reasons.Consumption));
            Assert.Equal(10, consumptions.Sum(r => r.Quantity));

            // Re-arrange
            workEffort.Complete();

            // Act
            this.Session.Derive(true);

            // Assert
            consumptions = inventoryAssignment.InventoryItemTransactions.Where(t => t.Reason.Equals(reasons.Consumption));

            Assert.Equal(2, inventoryAssignment.InventoryItemTransactions.Count);

            Assert.Equal(10, consumptions.Sum(r => r.Quantity));
            Assert.Equal(10, part.QuantityOnHand);
        }
    }
}
