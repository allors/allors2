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
        internal static readonly Guid ReadyForPostingId = new Guid("279B6128-314E-4767-A605-F1C34931EA84");
        internal static readonly Guid CancelledId = new Guid("8AE3813D-7866-4e1c-AB70-EE695154F8F7");
        internal static readonly Guid CompletedId = new Guid("81F80082-040C-405a-8C01-778868D57C75");
        internal static readonly Guid RejectedId = new Guid("AE2AB1DC-0E5E-4061-924C-025AB84769C0");
        internal static readonly Guid OnHoldId = new Guid("f625fb7e-893e-4f68-ab7b-2bc29a644e5b");
        internal static readonly Guid PostedId = new Guid("37d344e7-5962-425c-86a9-24bf1e098448");
        internal static readonly Guid InProcessId = new Guid("ddbb678e-9a66-4842-87fd-4e628cff0a75");
        internal static readonly Guid FinishedId = new Guid("DFE75006-81FD-424a-AF58-2528A657155D");

        private UniquelyIdentifiableSticky<SalesOrderState> cache;

        public SalesOrderState Provisional => this.Cache[ProvisionalId];

        public SalesOrderState RequestsApproval => this.Cache[RequestsApprovalId];

        public SalesOrderState ReadyForPosting => this.Cache[ReadyForPostingId];

        public SalesOrderState Cancelled => this.Cache[CancelledId];

        public SalesOrderState Completed => this.Cache[CompletedId];

        public SalesOrderState Rejected => this.Cache[RejectedId];

        public SalesOrderState Finished => this.Cache[FinishedId];

        public SalesOrderState OnHold => this.Cache[OnHoldId];

        public SalesOrderState Posted => this.Cache[PostedId];

        public SalesOrderState InProcess => this.Cache[InProcessId];

        private UniquelyIdentifiableSticky<SalesOrderState> Cache => this.cache ??= new UniquelyIdentifiableSticky<SalesOrderState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(ProvisionalId, v => v.Name = "Created");
            merge(RequestsApprovalId, v => v.Name = "Requests Approval");
            merge(ReadyForPostingId, v => v.Name = "Ready For Posting");
            merge(CancelledId, v => v.Name = "Cancelled");
            merge(CompletedId, v => v.Name = "Completed");
            merge(RejectedId, v => v.Name = "Rejected");
            merge(OnHoldId, v => v.Name = "On Hold");
            merge(PostedId, v => v.Name = "Posted");
            merge(InProcessId, v => v.Name = "In Process");
            merge(FinishedId, v => v.Name = "Finished");
        }
    }
}
