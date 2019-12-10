// <copyright file="PurchaseOrderItemStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class PurchaseOrderItemStates
    {
        public static readonly Guid CreatedId = new Guid("57273ADE-A813-40ba-B319-EF8D62AC92B6");
        public static readonly Guid AwaitingApprovalId = new Guid("BB3F365A-BC0D-44ff-9682-0D9FF910C637");
        public static readonly Guid CancelledId = new Guid("7342A3E6-69E4-49a7-9C2E-93574BF14072");
        public static readonly Guid CompletedId = new Guid("9B338149-43EA-4091-BBD8-C3485337FBC5");
        public static readonly Guid RejectedId = new Guid("0CD96679-4699-42de-9AB6-C4DA197F907D");
        public static readonly Guid OnHoldId = new Guid("BEB5870C-0542-42fa-B2FC-5D2BD21673B7");
        public static readonly Guid InProcessId = new Guid("9CD110AE-7787-469f-9A3E-F0000E35E588");
        public static readonly Guid SentId = new Guid("9A707175-547D-4B2F-9780-1750E7878A59");
        public static readonly Guid FinishedId = new Guid("4166228F-0ECC-444b-A45E-43794184DBB9");

        private UniquelyIdentifiableSticky<PurchaseOrderItemState> cache;

        public PurchaseOrderItemState Created => this.Cache[CreatedId];

        public PurchaseOrderItemState AwaitingApproval => this.Cache[AwaitingApprovalId];

        public PurchaseOrderItemState Cancelled => this.Cache[CancelledId];

        public PurchaseOrderItemState Completed => this.Cache[CompletedId];

        public PurchaseOrderItemState Rejected => this.Cache[RejectedId];

        public PurchaseOrderItemState Finished => this.Cache[FinishedId];

        public PurchaseOrderItemState OnHold => this.Cache[OnHoldId];

        public PurchaseOrderItemState InProcess => this.Cache[InProcessId];

        public PurchaseOrderItemState Sent => this.Cache[SentId];

        private UniquelyIdentifiableSticky<PurchaseOrderItemState> Cache => this.cache ??= new UniquelyIdentifiableSticky<PurchaseOrderItemState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(CreatedId, v => v.Name = "Created");
            merge(AwaitingApprovalId, v => v.Name = "Awaiting Approval");
            merge(CancelledId, v => v.Name = "Cancelled");
            merge(CompletedId, v => v.Name = "Completed");
            merge(RejectedId, v => v.Name = "Rejected");
            merge(OnHoldId, v => v.Name = "On Hold");
            merge(InProcessId, v => v.Name = "In Process");
            merge(SentId, v => v.Name = "Sent");
            merge(FinishedId, v => v.Name = "Finished");
        }
    }
}
