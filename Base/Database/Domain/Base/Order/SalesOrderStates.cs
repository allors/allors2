// <copyright file="SalesOrderStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SalesOrderStates
    {
        internal static readonly Guid ProvisionalId = new Guid("29abc67d-4be1-4af3-b993-64e9e36c3e6b");
        internal static readonly Guid RequestsApprovalId = new Guid("6b6f6e25-4da1-455d-9c9f-21f2d4316d66");
        internal static readonly Guid CancelledId = new Guid("8AE3813D-7866-4e1c-AB70-EE695154F8F7");
        internal static readonly Guid CompletedId = new Guid("81F80082-040C-405a-8C01-778868D57C75");
        internal static readonly Guid RejectedId = new Guid("AE2AB1DC-0E5E-4061-924C-025AB84769C0");
        internal static readonly Guid OnHoldId = new Guid("f625fb7e-893e-4f68-ab7b-2bc29a644e5b");
        internal static readonly Guid InProcessId = new Guid("ddbb678e-9a66-4842-87fd-4e628cff0a75");
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

        private UniquelyIdentifiableSticky<SalesOrderState> StateCache => this.stateCache ??= new UniquelyIdentifiableSticky<SalesOrderState>(this.Session);

        protected override void BaseSetup(Setup setup)
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
