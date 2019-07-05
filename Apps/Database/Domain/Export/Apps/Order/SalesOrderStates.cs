// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrderStates.cs" company="Allors bvba">
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

    public partial class SalesOrderStates
    {
        internal static readonly Guid ProvisionalId = new Guid("29ABC67D-4BE1-4af3-B993-64E9E36C3E6B");
        internal static readonly Guid RequestsApprovalId = new Guid("6B6F6E25-4DA1-455d-9C9F-21F2D4316D66");
        internal static readonly Guid CancelledId = new Guid("8AE3813D-7866-4e1c-AB70-EE695154F8F7");
        internal static readonly Guid CompletedId = new Guid("81F80082-040C-405a-8C01-778868D57C75");
        internal static readonly Guid RejectedId = new Guid("AE2AB1DC-0E5E-4061-924C-025AB84769C0");
        internal static readonly Guid OnHoldId = new Guid("F625FB7E-893E-4f68-AB7B-2BC29A644E5B");
        internal static readonly Guid InProcessId = new Guid("DDBB678E-9A66-4842-87FD-4E628CFF0A75");
        internal static readonly Guid FinishedId = new Guid("DFE75006-81FD-424a-AF58-2528A657155D");

        private UniquelyIdentifiableSticky<SalesOrderState> stateCache;

        public SalesOrderState Provisional => this.StateCache[ProvisionalId];

        public SalesOrderState RequestsApproval => this.StateCache[RequestsApprovalId];

        public SalesOrderState Cancelled => this.StateCache[CancelledId];

        public SalesOrderState Completed => this.StateCache[CompletedId];

        public SalesOrderState Rejected => this.StateCache[RejectedId];

        public SalesOrderState Finished => this.StateCache[FinishedId];

        public SalesOrderState OnHold => this.StateCache[OnHoldId];

        public SalesOrderState InProcess => this.StateCache[InProcessId];

        private UniquelyIdentifiableSticky<SalesOrderState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<SalesOrderState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            new SalesOrderStateBuilder(this.Session)
                .WithUniqueId(ProvisionalId)
                .WithName("Created")
                .Build();

            new SalesOrderStateBuilder(this.Session)
                .WithUniqueId(RequestsApprovalId)
                .WithName("Requests Approval")
                .Build();

            new SalesOrderStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new SalesOrderStateBuilder(this.Session)
                .WithUniqueId(CompletedId)
                .WithName("Completed")
                .Build();

            new SalesOrderStateBuilder(this.Session)
                .WithUniqueId(RejectedId)
                .WithName("Rejected")
                .Build();

            new SalesOrderStateBuilder(this.Session)
                .WithUniqueId(OnHoldId)
                .WithName("On Hold")
                .Build();

            new SalesOrderStateBuilder(this.Session)
                .WithUniqueId(InProcessId)
                .WithName("In Process")
                .Build();

            new SalesOrderStateBuilder(this.Session)
                .WithUniqueId(FinishedId)
                .WithName("Finished")
                .Build();
        }
    }
}