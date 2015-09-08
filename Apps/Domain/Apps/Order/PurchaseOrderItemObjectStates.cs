// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrderItemObjectStates.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    public partial class PurchaseOrderItemObjectStates
    {
        public static readonly Guid CreatedId = new Guid("57273ADE-A813-40ba-B319-EF8D62AC92B6");
        public static readonly Guid RequestsApprovalId = new Guid("BB3F365A-BC0D-44ff-9682-0D9FF910C637");
        public static readonly Guid CancelledId = new Guid("7342A3E6-69E4-49a7-9C2E-93574BF14072");
        public static readonly Guid PartiallyReceivedId = new Guid("C4E6F011-3484-4773-8FFF-FE24EF6C231A");
        public static readonly Guid ReceivedId = new Guid("B2AD8B85-2C31-48fc-9963-9074C764CC7B");
        public static readonly Guid CompletedId = new Guid("9B338149-43EA-4091-BBD8-C3485337FBC5");
        public static readonly Guid PaidId = new Guid("97B335DD-AB2C-4b6d-8282-B5E3DE6490BE");
        public static readonly Guid PartiallyPaidId = new Guid("3AF95B46-77DF-4c65-8693-B1E23B8F1A20");
        public static readonly Guid RejectedId = new Guid("0CD96679-4699-42de-9AB6-C4DA197F907D");
        public static readonly Guid OnHoldId = new Guid("BEB5870C-0542-42fa-B2FC-5D2BD21673B7");
        public static readonly Guid InProcessId = new Guid("9CD110AE-7787-469f-9A3E-F0000E35E588");
        public static readonly Guid FinishedId = new Guid("4166228F-0ECC-444b-A45E-43794184DBB9");

        private UniquelyIdentifiableCache<PurchaseOrderItemObjectState> stateCache;

        public PurchaseOrderItemObjectState Created
        {
            get { return this.StateCache.Get(CreatedId); }
        }

        public PurchaseOrderItemObjectState RequestsApproval
        {
            get { return this.StateCache.Get(RequestsApprovalId); }
        }

        public PurchaseOrderItemObjectState Cancelled
        {
            get { return this.StateCache.Get(CancelledId); }
        }

        public PurchaseOrderItemObjectState Completed
        {
            get { return this.StateCache.Get(CompletedId); }
        }

        public PurchaseOrderItemObjectState Paid
        {
            get { return this.StateCache.Get(PaidId); }
        }

        public PurchaseOrderItemObjectState PartiallyPaid
        {
            get { return this.StateCache.Get(PartiallyPaidId); }
        }

        public PurchaseOrderItemObjectState PartiallyReceived
        {
            get { return this.StateCache.Get(PartiallyReceivedId); }
        }

        public PurchaseOrderItemObjectState Received
        {
            get { return this.StateCache.Get(ReceivedId); }
        }

        public PurchaseOrderItemObjectState Rejected
        {
            get { return this.StateCache.Get(RejectedId); }
        }

        public PurchaseOrderItemObjectState Finished
        {
            get { return this.StateCache.Get(FinishedId); }
        }

        public PurchaseOrderItemObjectState OnHold
        {
            get { return this.StateCache.Get(OnHoldId); }
        }

        public PurchaseOrderItemObjectState InProcess
        {
            get { return this.StateCache.Get(InProcessId); }
        }

        private UniquelyIdentifiableCache<PurchaseOrderItemObjectState> StateCache
        {
            get
            {
                return this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<PurchaseOrderItemObjectState>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(Session).EnglishGreatBritain;
            var dutchLocale = new Locales(Session).DutchNetherlands;

            new PurchaseOrderItemObjectStateBuilder(Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(Session)
                .WithUniqueId(RequestsApprovalId)
                .WithName("Requests Approval")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(Session)
                .WithUniqueId(CompletedId)
                .WithName("Completed")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(Session)
                .WithUniqueId(PartiallyReceivedId)
                .WithName("Partially Received")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(Session)
                .WithUniqueId(ReceivedId)
                .WithName("Received")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(Session)
                .WithUniqueId(PartiallyPaidId)
                .WithName("Partially Paid")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(Session)
                .WithUniqueId(RejectedId)
                .WithName("Rejected")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(Session)
                .WithUniqueId(OnHoldId)
                .WithName("On Hold")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(Session)
                .WithUniqueId(InProcessId)
                .WithName("In Process")
                .Build();

            new PurchaseOrderItemObjectStateBuilder(Session)
                .WithUniqueId(FinishedId)
                .WithName("Finished")
                .Build();
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}