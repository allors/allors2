// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkEffortStates.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using System;

    public partial class WorkEffortStates
    {
        private static readonly Guid CreatedId = new Guid("C082CD60-5C5F-4948-BDB1-06BD9C385751");
        private static readonly Guid InProgressId = new Guid("7A83DF7B-9918-4B10-8F99-48896F9DB105");
        private static readonly Guid CancelledId = new Guid("F9FC3CD0-44C9-4343-98FD-494C4D6C9988");
        private static readonly Guid CompletedId = new Guid("4D942F82-3B8F-4248-9EBC-22B1E5F05D93");
        private static readonly Guid FinishedId = new Guid("6A9716A1-8174-4B26-86EB-22A265B74E78");

        private UniquelyIdentifiableSticky<WorkEffortState> stateCache;

        public WorkEffortState Created => this.StateCache[CreatedId];

        public WorkEffortState InProgress => this.StateCache[InProgressId];

        public WorkEffortState Completed => this.StateCache[CompletedId];

        public WorkEffortState Finished => this.StateCache[FinishedId];

        public WorkEffortState Cancelled => this.StateCache[CancelledId];


        private UniquelyIdentifiableSticky<WorkEffortState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<WorkEffortState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var reasons = new InventoryTransactionReasons(this.Session);

            new WorkEffortStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new WorkEffortStateBuilder(this.Session)
                .WithUniqueId(InProgressId)
                .WithName("In Progress")
                .Build();

            new WorkEffortStateBuilder(this.Session)
                .WithUniqueId(CompletedId)
                .WithName("Delivered")
                .Build();

            new WorkEffortStateBuilder(this.Session)
                .WithUniqueId(FinishedId)
                .WithName("Finished")
                .Build();

            new WorkEffortStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            // The created state is the initial and re-opened state (Cancel Consumption for Re-Open)
            this.Created.AddInventoryTransactionReasonsToCreate(reasons.Reservation);
            this.Created.AddInventoryTransactionReasonsToCancel(reasons.Consumption);

            // The inprogress state is the initial and re-opened state (Cancel Consumption for Re-Open)
            this.InProgress.AddInventoryTransactionReasonsToCreate(reasons.Reservation);

            // The Delivered state should create a Consumption (which Decreases the Reservation)
            this.Completed.AddInventoryTransactionReasonsToCreate(reasons.Reservation);
            this.Completed.AddInventoryTransactionReasonsToCreate(reasons.Consumption);
            
            // The Cancelled state should cancel any existing Consumption and Reservation
            this.Cancelled.AddInventoryTransactionReasonsToCancel(reasons.Reservation);
            this.Cancelled.AddInventoryTransactionReasonsToCancel(reasons.Consumption);
        }
    }
}