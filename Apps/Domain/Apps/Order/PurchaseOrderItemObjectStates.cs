// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrderItemObjectStates.cs" company="Allors bvba">
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

    public partial class PurchaseOrderItemObjectStates
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

        private UniquelyIdentifiableCache<PurchaseOrderItemObjectState> stateCache;

        public PurchaseOrderItemObjectState Created => this.StateCache.Get(CreatedId);

        public PurchaseOrderItemObjectState RequestsApproval => this.StateCache.Get(RequestsApprovalId);

        public PurchaseOrderItemObjectState Cancelled => this.StateCache.Get(CancelledId);

        public PurchaseOrderItemObjectState Completed => this.StateCache.Get(CompletedId);

        public PurchaseOrderItemObjectState Paid => this.StateCache.Get(PaidId);

        public PurchaseOrderItemObjectState PartiallyPaid => this.StateCache.Get(PartiallyPaidId);

        public PurchaseOrderItemObjectState PartiallyReceived => this.StateCache.Get(PartiallyReceivedId);

        public PurchaseOrderItemObjectState Received => this.StateCache.Get(ReceivedId);

        public PurchaseOrderItemObjectState Rejected => this.StateCache.Get(RejectedId);

        public PurchaseOrderItemObjectState Finished => this.StateCache.Get(FinishedId);

        public PurchaseOrderItemObjectState OnHold => this.StateCache.Get(OnHoldId);

        public PurchaseOrderItemObjectState InProcess => this.StateCache.Get(InProcessId);

        private UniquelyIdentifiableCache<PurchaseOrderItemObjectState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<PurchaseOrderItemObjectState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new PurchaseOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(RequestsApprovalId)
                .WithName("Requests Approval")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(CompletedId)
                .WithName("Completed")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(PartiallyReceivedId)
                .WithName("Partially Received")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(ReceivedId)
                .WithName("Received")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(PartiallyPaidId)
                .WithName("Partially Paid")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(RejectedId)
                .WithName("Rejected")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(OnHoldId)
                .WithName("On Hold")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(InProcessId)
                .WithName("In Process")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(this.Session)
                .WithUniqueId(FinishedId)
                .WithName("Finished")
                .Build();
        }
    }
}