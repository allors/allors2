// <copyright file="WorkEffortStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using Meta;

    public partial class WorkEffortStates
    {
        private static readonly Guid CreatedId = new Guid("C082CD60-5C5F-4948-BDB1-06BD9C385751");
        private static readonly Guid InProgressId = new Guid("7A83DF7B-9918-4B10-8F99-48896F9DB105");
        private static readonly Guid CancelledId = new Guid("F9FC3CD0-44C9-4343-98FD-494C4D6C9988");
        private static readonly Guid CompletedId = new Guid("4D942F82-3B8F-4248-9EBC-22B1E5F05D93");
        private static readonly Guid FinishedId = new Guid("6A9716A1-8174-4B26-86EB-22A265B74E78");

        private UniquelyIdentifiableSticky<WorkEffortState> cache;

        public WorkEffortState Created => this.Cache[CreatedId];

        public WorkEffortState InProgress => this.Cache[InProgressId];

        public WorkEffortState Completed => this.Cache[CompletedId];

        public WorkEffortState Finished => this.Cache[FinishedId];

        public WorkEffortState Cancelled => this.Cache[CancelledId];

        private UniquelyIdentifiableSticky<WorkEffortState> Cache => this.cache ??= new UniquelyIdentifiableSticky<WorkEffortState>(this.Session);

        protected override void BasePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.InventoryTransactionReason);

        protected override void BaseSetup(Setup setup)
        {
            var reasons = new InventoryTransactionReasons(this.Session);

            var merge = this.Cache.Merger().Action();

            merge(CreatedId, v => v.Name = "Created");
            merge(InProgressId, v => v.Name = "In Progress");
            merge(CompletedId, v => v.Name = "Completed");
            merge(FinishedId, v => v.Name = "Finished");
            merge(CancelledId, v => v.Name = "Cancelled");


            // The created state is the initial and re-opened state (Cancel Consumption for Re-Open)
            //this.Created.AddInventoryTransactionReasonsToCreate(reasons.Reservation);
            //this.Created.AddInventoryTransactionReasonsToCancel(reasons.Consumption);
            this.Created.AddInventoryTransactionReasonsToCreate(reasons.Consumption);

            // The inprogress state is the initial and re-opened state (Cancel Consumption for Re-Open)
            this.InProgress.AddInventoryTransactionReasonsToCreate(reasons.Consumption);

            // The Delivered state should create a Consumption (which Decreases the Reservation)
            //this.Completed.AddInventoryTransactionReasonsToCreate(reasons.Reservation);
            this.Completed.AddInventoryTransactionReasonsToCreate(reasons.Consumption);

            // The Cancelled state should cancel any existing Consumption and Reservation
            //this.Cancelled.AddInventoryTransactionReasonsToCancel(reasons.Reservation);
            this.Cancelled.AddInventoryTransactionReasonsToCancel(reasons.Consumption);
        }
    }
}
