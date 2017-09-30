// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrderItemStates.cs" company="Allors bvba">
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

    public partial class PurchaseOrderItemStates
    {
        private static readonly Guid CreatedId = new Guid("57273ADE-A813-40ba-B319-EF8D62AC92B6");
        private static readonly Guid RequestsApprovalId = new Guid("BB3F365A-BC0D-44ff-9682-0D9FF910C637");
        private static readonly Guid CancelledId = new Guid("7342A3E6-69E4-49a7-9C2E-93574BF14072");
        private static readonly Guid PartiallyReceivedId = new Guid("C4E6F011-3484-4773-8FFF-FE24EF6C231A");
        private static readonly Guid ReceivedId = new Guid("B2AD8B85-2C31-48fc-9963-9074C764CC7B");
        private static readonly Guid CompletedId = new Guid("9B338149-43EA-4091-BBD8-C3485337FBC5");
        private static readonly Guid PaidId = new Guid("97B335DD-AB2C-4b6d-8282-B5E3DE6490BE");
        private static readonly Guid PartiallyPaidId = new Guid("3AF95B46-77DF-4c65-8693-B1E23B8F1A20");
        private static readonly Guid RejectedId = new Guid("0CD96679-4699-42de-9AB6-C4DA197F907D");
        private static readonly Guid OnHoldId = new Guid("BEB5870C-0542-42fa-B2FC-5D2BD21673B7");
        private static readonly Guid InProcessId = new Guid("9CD110AE-7787-469f-9A3E-F0000E35E588");
        private static readonly Guid FinishedId = new Guid("4166228F-0ECC-444b-A45E-43794184DBB9");

        private UniquelyIdentifiableCache<PurchaseOrderItemState> stateCache;

        public PurchaseOrderItemState Created => this.StateCache.Get(CreatedId);

        public PurchaseOrderItemState RequestsApproval => this.StateCache.Get(RequestsApprovalId);

        public PurchaseOrderItemState Cancelled => this.StateCache.Get(CancelledId);

        public PurchaseOrderItemState Completed => this.StateCache.Get(CompletedId);

        public PurchaseOrderItemState Paid => this.StateCache.Get(PaidId);

        public PurchaseOrderItemState PartiallyPaid => this.StateCache.Get(PartiallyPaidId);

        public PurchaseOrderItemState PartiallyReceived => this.StateCache.Get(PartiallyReceivedId);

        public PurchaseOrderItemState Received => this.StateCache.Get(ReceivedId);

        public PurchaseOrderItemState Rejected => this.StateCache.Get(RejectedId);

        public PurchaseOrderItemState Finished => this.StateCache.Get(FinishedId);

        public PurchaseOrderItemState OnHold => this.StateCache.Get(OnHoldId);

        public PurchaseOrderItemState InProcess => this.StateCache.Get(InProcessId);

        private UniquelyIdentifiableCache<PurchaseOrderItemState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<PurchaseOrderItemState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new PurchaseOrderItemStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new PurchaseOrderItemStateBuilder(this.Session)
                .WithUniqueId(RequestsApprovalId)
                .WithName("Requests Approval")
                .Build();

            new PurchaseOrderItemStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new PurchaseOrderItemStateBuilder(this.Session)
                .WithUniqueId(CompletedId)
                .WithName("Completed")
                .Build();

            new PurchaseOrderItemStateBuilder(this.Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();

            new PurchaseOrderItemStateBuilder(this.Session)
                .WithUniqueId(PartiallyReceivedId)
                .WithName("Partially Received")
                .Build();

            new PurchaseOrderItemStateBuilder(this.Session)
                .WithUniqueId(ReceivedId)
                .WithName("Received")
                .Build();

            new PurchaseOrderItemStateBuilder(this.Session)
                .WithUniqueId(PartiallyPaidId)
                .WithName("Partially Paid")
                .Build();

            new PurchaseOrderItemStateBuilder(this.Session)
                .WithUniqueId(RejectedId)
                .WithName("Rejected")
                .Build();

            new PurchaseOrderItemStateBuilder(this.Session)
                .WithUniqueId(OnHoldId)
                .WithName("On Hold")
                .Build();

            new PurchaseOrderItemStateBuilder(this.Session)
                .WithUniqueId(InProcessId)
                .WithName("In Process")
                .Build();

            new PurchaseOrderItemStateBuilder(this.Session)
                .WithUniqueId(FinishedId)
                .WithName("Finished")
                .Build();
        }
    }
}