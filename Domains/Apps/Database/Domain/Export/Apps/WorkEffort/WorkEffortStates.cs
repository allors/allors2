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
        private static readonly Guid NeedsActionId = new Guid("0D2618A7-A3B7-40f5-88C4-30DADE4D5164");
        private static readonly Guid ConfirmedId = new Guid("61E5FCB6-7814-4FED-8CE7-28C26EB88EE6");
        private static readonly Guid DeclinedId = new Guid("0DFFC51A-4982-4DDA-808B-8F70D46F2749");
        private static readonly Guid InProgressId = new Guid("8384CC3B-492F-4EED-B305-C735D6C82433");
        private static readonly Guid CompletedId = new Guid("E8E941CD-7175-4931-AB1E-50E52DC6D720");
        private static readonly Guid FinishedId = new Guid("2CEF3B4F-2609-4A7C-BBF3-ACC76EB9472D");
        private static readonly Guid CancelledId = new Guid("D3EBD54F-35B0-4bc4-AC8E-4EC583028B0A");
        private static readonly Guid DelegatedId = new Guid("0EB0E562-5094-4F48-AAAC-EF5DF239CF07");
        private static readonly Guid InPlanningId = new Guid("007B3B09-218D-4898-AE32-A78FAEFA097E");
        private static readonly Guid PlannedId = new Guid("E120D688-150C-4B19-BD7B-28A6D7617599");
        private static readonly Guid SentId = new Guid("5A0A160B-9A48-47A1-A0E9-07A84F0A3F95");
        private static readonly Guid AcceptedId = new Guid("09148AB2-28B4-4A84-B403-17884149156C");
        private static readonly Guid TentativeId = new Guid("A2FAAC53-49EC-45F4-84F3-10C9558A366E");

        private UniquelyIdentifiableSticky<WorkEffortState> stateCache;

        public WorkEffortState NeedsAction => this.StateCache[NeedsActionId];

        public WorkEffortState Confirmed => this.StateCache[ConfirmedId];

        public WorkEffortState Declined => this.StateCache[DeclinedId];

        public WorkEffortState InProgress => this.StateCache[InProgressId];

        public WorkEffortState Completed => this.StateCache[CompletedId];

        public WorkEffortState Finished => this.StateCache[FinishedId];

        public WorkEffortState Cancelled => this.StateCache[CancelledId];

        public WorkEffortState Delagated => this.StateCache[DelegatedId];

        public WorkEffortState InPlanning => this.StateCache[InPlanningId];

        public WorkEffortState Planned => this.StateCache[PlannedId];

        public WorkEffortState Sent => this.StateCache[SentId];

        public WorkEffortState Accepted => this.StateCache[AcceptedId];

        public WorkEffortState Tentative => this.StateCache[TentativeId];

        private UniquelyIdentifiableSticky<WorkEffortState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<WorkEffortState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;
            var reasons = new InventoryTransactionReasons(this.Session);

            new WorkEffortStateBuilder(this.Session)
                .WithUniqueId(NeedsActionId)
                .WithName("NeedsAction")
                .Build();

            new WorkEffortStateBuilder(this.Session)
                .WithUniqueId(ConfirmedId)
                .WithName("Confirmed")
                .Build();

            new WorkEffortStateBuilder(this.Session)
                .WithUniqueId(DeclinedId)
                .WithName("Declined")
                .Build();

            new WorkEffortStateBuilder(this.Session)
                .WithUniqueId(InProgressId)
                .WithName("In Progress")
                .Build();

            new WorkEffortStateBuilder(this.Session)
                .WithUniqueId(CompletedId)
                .WithName("Completed")
                .Build();

            new WorkEffortStateBuilder(this.Session)
                .WithUniqueId(FinishedId)
                .WithName("Finished")
                .Build();

            new WorkEffortStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new WorkEffortStateBuilder(this.Session)
                .WithUniqueId(DelegatedId)
                .WithName("Delegated")
                .Build();

            new WorkEffortStateBuilder(this.Session)
                .WithUniqueId(InPlanningId)
                .WithName("In Planning")
                .Build();

            new WorkEffortStateBuilder(this.Session)
                .WithUniqueId(PlannedId)
                .WithName("Planned")
                .Build();

            new WorkEffortStateBuilder(this.Session)
                .WithUniqueId(SentId)
                .WithName("Sent")
                .Build();

            new WorkEffortStateBuilder(this.Session)
                .WithUniqueId(AcceptedId)
                .WithName("Accepted")
                .Build();

            new WorkEffortStateBuilder(this.Session)
                .WithUniqueId(TentativeId)
                .WithName("Tentative")
                .Build();

            // The Needs Action state is the initial and re-opened state (Cancel Consumption for Re-Open)
            this.NeedsAction.AddInventoryTransactionReasonsToCreate(reasons.Reservation);
            this.NeedsAction.AddInventoryTransactionReasonsToCancel(reasons.Consumption);

            // Work Effort States which create a Reservation (if one doesn't exist)
            this.Confirmed.AddInventoryTransactionReasonsToCreate(reasons.Reservation);
            this.Declined.AddInventoryTransactionReasonsToCreate(reasons.Reservation);
            this.InProgress.AddInventoryTransactionReasonsToCreate(reasons.Reservation);
            this.Delagated.AddInventoryTransactionReasonsToCreate(reasons.Reservation);
            this.InPlanning.AddInventoryTransactionReasonsToCreate(reasons.Reservation);
            this.Planned.AddInventoryTransactionReasonsToCreate(reasons.Reservation);
            this.Sent.AddInventoryTransactionReasonsToCreate(reasons.Reservation);
            this.Accepted.AddInventoryTransactionReasonsToCreate(reasons.Reservation);
            this.Tentative.AddInventoryTransactionReasonsToCreate(reasons.Reservation);

            // The Completed state should create a Consumption (which Decreases the Reservation)
            this.Completed.AddInventoryTransactionReasonsToCreate(reasons.Reservation);
            this.Completed.AddInventoryTransactionReasonsToCreate(reasons.Consumption);
            
            // The Cancelled state should cancel any existing Consumption and Reservation
            this.Cancelled.AddInventoryTransactionReasonsToCancel(reasons.Reservation);
            this.Cancelled.AddInventoryTransactionReasonsToCancel(reasons.Consumption);
        }
    }
}