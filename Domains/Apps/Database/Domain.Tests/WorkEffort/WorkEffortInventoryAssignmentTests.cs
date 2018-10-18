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

namespace Allors.Domain
{
    using System.Linq;
    using Should;
    using Xunit;
        
    public class WorkEffortInventoryAssignmentTests : DomainTest
    {
        [Fact]
        public void GivenNewWorkEffort_WhenAddingInventoryAssignment_ThenInventoryReservationCreated()
        {
            // Arrange
            var reasons = new InventoryTransactionReasons(this.Session);

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").Build();
            var part = new PartBuilder(this.Session).WithPartId("P1").Build();

            // Act
            this.Session.Derive(true);

            // Assert
            workEffort.WorkEffortInventoryAssignmentsWhereAssignment.ShouldBeEmpty();
            workEffort.WorkEffortState.IsNeedsAction.ShouldBeTrue();

            // Re-arrange
            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithPart(part)
                .WithQuantity(10)
                .Build();

            // Act
            this.Session.Derive(true);

            // Assert
            var transactions = inventoryAssignment.InventoryItemTransactions;

            transactions.Count.ShouldEqual(1);
            transactions[0].Part.ShouldEqual(part);
            transactions[0].Quantity.ShouldEqual(10);
            transactions[0].Reason.ShouldEqual(reasons.Reservation);
        }

        [Fact]
        public void GivenNewWorkEffortWithInventoryAssignment_WhenChangingPart_ThenOldInventoryCancelledAndNewInventoryCreated()
        {
            // Arrange
            var reasons = new InventoryTransactionReasons(this.Session);

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").Build();
            var part1 = new PartBuilder(this.Session).WithPartId("P1").Build();
            var part2 = new PartBuilder(this.Session).WithPartId("P2").Build();

            this.Session.Derive(true);

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithPart(part1)
                .WithQuantity(10)
                .Build();

            // Act
            this.Session.Derive(true);

            // Assert
            var transactions = inventoryAssignment.InventoryItemTransactions.ToArray();

            transactions.Length.ShouldEqual(1);
            transactions[0].Part.ShouldEqual(part1);
            transactions[0].Quantity.ShouldEqual(10);
            transactions[0].Reason.ShouldEqual(reasons.Reservation);

            // Re-arrange
            inventoryAssignment.Part = part2;

            // Act
            this.Session.Derive(true);

            // Assert
            var part1Transactions = inventoryAssignment.InventoryItemTransactions.Where(t => t.Part.Equals(part1)).ToArray();
            var part2Transactions = inventoryAssignment.InventoryItemTransactions.Where(t => t.Part.Equals(part2)).ToArray();

            part1Transactions.Sum(t => t.Quantity).ShouldEqual(0);
            part2Transactions.Sum(t => t.Quantity).ShouldEqual(10);
        }

        [Fact]
        public void GivenWorkEffortWithInventoryAssignment_WhenCancelling_ThenInventoryReservationCancelled()
        {
            // Arrange
            var reasons = new InventoryTransactionReasons(this.Session);

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").Build();
            var part = new PartBuilder(this.Session).WithPartId("P1").Build();

            this.Session.Derive(true);

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithPart(part)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            // Act
            workEffort.Cancel();
            this.Session.Derive(true);

            // Assert
            var transactions = inventoryAssignment.InventoryItemTransactions;
            var reservation = transactions.First(t => t.Reason.Equals(reasons.Reservation) && (t.Quantity > 0));
            var reservationCancellation = transactions.First(t => t.Reason.Equals(reasons.Reservation) && (t.Quantity < 0));

            transactions.Count.ShouldEqual(2);

            reservation.Quantity.ShouldEqual(10);
            reservationCancellation.Quantity.ShouldEqual(-10);
        }

        [Fact]
        public void GivenWorkEffortWithInventoryAssignment_WhenCompleting_ThenInventoryTransactionsCreated()
        {
            // Arrange
            var reasons = new InventoryTransactionReasons(this.Session);

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").Build();
            var part = new PartBuilder(this.Session).WithPartId("P1").Build();

            this.Session.Derive(true);

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithPart(part)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            // Act
            workEffort.Finish();
            this.Session.Derive(true);

            // Assert
            var transactions = inventoryAssignment.InventoryItemTransactions;
            var reservation = transactions.First(t => t.Reason.Equals(reasons.Reservation) && (t.Quantity > 0));
            var reservationCancellation = transactions.First(t => t.Reason.Equals(reasons.Reservation) && (t.Quantity < 0));
            var consumption = transactions.First(t => t.Reason.Equals(reasons.Consumption));

            transactions.Count.ShouldEqual(3);

            reservation.Quantity.ShouldEqual(10);
            reservationCancellation.Quantity.ShouldEqual(-10);
            consumption.Quantity.ShouldEqual(10);
        }

        [Fact]
        public void GivenWorkEffortWithInventoryAssignment_WhenCompletingThenCancelling_ThenInventoryTransactionsCancelled()
        {
            // Arrange
            var reasons = new InventoryTransactionReasons(this.Session);

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").Build();
            var part = new PartBuilder(this.Session).WithPartId("P1").Build();

            this.Session.Derive(true);

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithPart(part)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            // Act
            workEffort.Finish();
            this.Session.Derive(true);

            workEffort.Cancel();
            this.Session.Derive(true);

            // Assert
            var transactions = inventoryAssignment.InventoryItemTransactions;
            var reservation = transactions.First(t => t.Reason.Equals(reasons.Reservation) && (t.Quantity > 0));
            var reservationCancellation = transactions.First(t => t.Reason.Equals(reasons.Reservation) && (t.Quantity < 0));
            var consumption = transactions.First(t => t.Reason.Equals(reasons.Consumption) && (t.Quantity > 0));
            var consumptionCancellation = transactions.First(t => t.Reason.Equals(reasons.Consumption) && (t.Quantity < 0));

            transactions.Count.ShouldEqual(4);

            reservation.Quantity.ShouldEqual(10);
            reservationCancellation.Quantity.ShouldEqual(-10);
            consumption.Quantity.ShouldEqual(10);
            consumptionCancellation.Quantity.ShouldEqual(-10);
        }

        [Fact]
        public void GivenNewWorkEffortWithInventoryAssignment_WhenChangingPartQuantityState_ThenOldInventoryCancelledAndNewInventoryCreated()
        {
            // Arrange
            var reasons = new InventoryTransactionReasons(this.Session);

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").Build();
            var part1 = new PartBuilder(this.Session).WithPartId("P1").Build();
            var part2 = new PartBuilder(this.Session).WithPartId("P2").Build();

            this.Session.Derive(true);

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithPart(part1)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            // Act
            inventoryAssignment.Part = part2;
            inventoryAssignment.Quantity = 5;

            workEffort.Finish();
            this.Session.Derive(true);

            // Assert
            var part1Transactions = inventoryAssignment.InventoryItemTransactions.Where(t => t.Part.Equals(part1)).ToArray();
            var part2Transactions = inventoryAssignment.InventoryItemTransactions.Where(t => t.Part.Equals(part2)).ToArray();

            var part1Reservations = part1Transactions.Where(t => t.Reason.Equals(reasons.Reservation));
            var part2Reservations = part2Transactions.Where(t => t.Reason.Equals(reasons.Reservation));
            var part2Consumption = part2Transactions.Where(t => t.Reason.Equals(reasons.Consumption));

            part1Reservations.Sum(r => r.Quantity).ShouldEqual(0);
            part2Reservations.Sum(r => r.Quantity).ShouldEqual(0);
            part2Consumption.Sum(c => c.Quantity).ShouldEqual(5);
        }

        [Fact]
        public void GivenWorkEffortWithInventoryAssignment_WhenChangingQuantity_ThenInventoryTransactionsCreated()
        {
            // Arrage
            var reasons = new InventoryTransactionReasons(this.Session);

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").Build();
            var part = new PartBuilder(this.Session).WithPartId("P1").Build();

            this.Session.Derive(true);

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort)
                .WithPart(part)
                .WithQuantity(5)
                .Build();

            // Act
            this.Session.Derive(true);

            // Assert
            var reservation = inventoryAssignment.InventoryItemTransactions.First(t => t.Reason.Equals(reasons.Reservation) && (t.Quantity > 0));
            reservation.Quantity.ShouldEqual(5);

            // Re-arrange
            inventoryAssignment.Quantity = 10;

            // Act
            this.Session.Derive(true);

            // Assert
            var reservations = inventoryAssignment.InventoryItemTransactions.Where(t => t.Reason.Equals(reasons.Reservation));
            reservations.Sum(r => r.Quantity).ShouldEqual(10);

            // Re-arrange
            workEffort.Finish();

            // Act
            this.Session.Derive(true);

            // Assert
            reservations = inventoryAssignment.InventoryItemTransactions.Where(t => t.Reason.Equals(reasons.Reservation));
            var consumption = inventoryAssignment.InventoryItemTransactions.First(t => t.Reason.Equals(reasons.Consumption));

            inventoryAssignment.InventoryItemTransactions.Count.ShouldEqual(4);

            reservations.Sum(r => r.Quantity).ShouldEqual(0);
            consumption.Quantity.ShouldEqual(10);
        }
    }
}