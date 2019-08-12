// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrderItemStates.cs" company="Allors bvba">
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

    public partial class SalesOrderItemStates
    {
        internal static readonly Guid CreatedId = new Guid("5B0993B5-5784-4e8d-B1AD-93AFFAC9A913");
        internal static readonly Guid CancelledId = new Guid("8B6FD903-B4A6-4360-A63C-9EBDFB7243AA");
        internal static readonly Guid CompletedId = new Guid("AC46B106-D266-46d7-BFD7-4196394A5AE0");
        internal static readonly Guid RejectedId = new Guid("F39F2F64-49A8-4a70-ACBC-B7F581F31EEF");
        internal static readonly Guid OnHoldId = new Guid("3B185D51-AF4A-441e-BE0D-F91CFCBDB5C8");
        internal static readonly Guid InProcessId = new Guid("E08401F7-1DEB-4b27-B0C5-8F034BFFEBD5");
        internal static readonly Guid FinishedId = new Guid("33C0ED0C-FDFE-45ff-A008-7A638094A94A");

        private UniquelyIdentifiableSticky<SalesOrderItemState> stateCache;

        public SalesOrderItemState Created => this.StateCache[CreatedId];

        public SalesOrderItemState Cancelled => this.StateCache[CancelledId];

        public SalesOrderItemState Completed => this.StateCache[CompletedId];

        public SalesOrderItemState Rejected => this.StateCache[RejectedId];

        public SalesOrderItemState Finished => this.StateCache[FinishedId];

        public SalesOrderItemState OnHold => this.StateCache[OnHoldId];

        public SalesOrderItemState InProcess => this.StateCache[InProcessId];

        private UniquelyIdentifiableSticky<SalesOrderItemState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<SalesOrderItemState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            

            var reasons = new InventoryTransactionReasons(this.Session);

            new SalesOrderItemStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new SalesOrderItemStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new SalesOrderItemStateBuilder(this.Session)
                .WithUniqueId(CompletedId)
                .WithName("Completed")
                .Build();

            new SalesOrderItemStateBuilder(this.Session)
                .WithUniqueId(RejectedId)
                .WithName("Rejected")
                .Build();

            new SalesOrderItemStateBuilder(this.Session)
                .WithUniqueId(OnHoldId)
                .WithName("On Hold")
                .Build();

            new SalesOrderItemStateBuilder(this.Session)
                .WithUniqueId(InProcessId)
                .WithName("In Process")
                .Build();

            new SalesOrderItemStateBuilder(this.Session)
                .WithUniqueId(FinishedId)
                .WithName("Finished")
                .Build();

            this.Created.AddInventoryTransactionReasonsToCreate(reasons.Reservation);

            this.InProcess.AddInventoryTransactionReasonsToCreate(reasons.Reservation);

            this.Completed.AddInventoryTransactionReasonsToCreate(reasons.Reservation);

            this.Cancelled.AddInventoryTransactionReasonsToCancel(reasons.Reservation);

            this.Rejected.AddInventoryTransactionReasonsToCancel(reasons.Reservation);
        }
    }
}