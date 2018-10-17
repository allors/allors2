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
            var reasons = new InventoryTransactionReasons(this.Session);

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").Build();
            var part = new PartBuilder(this.Session).WithPartId("P1").Build();

            this.Session.Derive(true);

            workEffort.WorkEffortInventoryAssignmentsWhereAssignment.ShouldBeEmpty();
            workEffort.WorkEffortState.IsNeedsAction.ShouldBeTrue();

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort).WithPart(part)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            var transactions = inventoryAssignment.InventoryItemTransactions;

            transactions.Count.ShouldEqual(1);
            transactions[0].Part.ShouldEqual(part);
            transactions[0].Quantity.ShouldEqual(10);
            transactions[0].Reason.ShouldEqual(reasons.Reservation);
        }

        [Fact]
        public void GivenWorkEffortWithInventoryAssignments_WhenCancelling_ThenInventoryReservationCancelled()
        {
            var reasons = new InventoryTransactionReasons(this.Session);

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").Build();
            var part = new PartBuilder(this.Session).WithPartId("P1").Build();

            this.Session.Derive(true);

            workEffort.WorkEffortInventoryAssignmentsWhereAssignment.ShouldBeEmpty();
            workEffort.WorkEffortState.IsNeedsAction.ShouldBeTrue();

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort).WithPart(part)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            workEffort.Cancel();

            this.Session.Derive(true);

            var transactions = inventoryAssignment.InventoryItemTransactions;
            var reservation = transactions.First(t => t.Reason.Equals(reasons.Reservation) && (t.Quantity > 0));
            var reservationCancellation = transactions.First(t => t.Reason.Equals(reasons.Reservation) && (t.Quantity < 0));

            transactions.Count.ShouldEqual(2);

            reservation.Quantity.ShouldEqual(10);
            reservationCancellation.Quantity.ShouldEqual(-10);
        }

        [Fact]
        public void GivenWorkEffortWithInventoryAssignments_WhenCompleting_ThenInventoryTransactionsCreated()
        {
            var reasons = new InventoryTransactionReasons(this.Session);

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").Build();
            var part = new PartBuilder(this.Session).WithPartId("P1").Build();

            this.Session.Derive(true);

            workEffort.WorkEffortInventoryAssignmentsWhereAssignment.ShouldBeEmpty();
            workEffort.WorkEffortState.IsNeedsAction.ShouldBeTrue();

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort).WithPart(part)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            workEffort.Finish();

            this.Session.Derive(true);

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
        public void GivenWorkEffortWithInventoryAssignments_WhenCompletingThenCancelling_ThenInventoryTransactionsCancelled()
        {
            var reasons = new InventoryTransactionReasons(this.Session);

            var workEffort = new WorkTaskBuilder(this.Session).WithName("Activity").Build();
            var part = new PartBuilder(this.Session).WithPartId("P1").Build();

            this.Session.Derive(true);

            workEffort.WorkEffortInventoryAssignmentsWhereAssignment.ShouldBeEmpty();
            workEffort.WorkEffortState.IsNeedsAction.ShouldBeTrue();

            var inventoryAssignment = new WorkEffortInventoryAssignmentBuilder(this.Session)
                .WithAssignment(workEffort).WithPart(part)
                .WithQuantity(10)
                .Build();

            this.Session.Derive(true);

            workEffort.Finish();

            this.Session.Derive(true);

            workEffort.Cancel();

            this.Session.Derive(true);

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
    }
}