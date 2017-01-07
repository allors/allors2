// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrderObjectStates.cs" company="Allors bvba">
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

    public partial class PurchaseOrderObjectStates
    {
        private static readonly Guid ProvisionalId = new Guid("69946F6D-718E-463d-AB36-BF4E3B970210");
        private static readonly Guid RequestsApprovalId = new Guid("DA8A94F3-FC5C-4e92-B466-0F47047B2E97");
        private static readonly Guid CancelledId = new Guid("FC345C8F-7BCC-4571-B353-BF8AF27C57A8");
        private static readonly Guid PartiallyReceivedId = new Guid("77ED251D-B004-41e7-B0C4-9769CF7AE73E");
        private static readonly Guid ReceivedId = new Guid("BCCB68CE-A517-44c6-ADDA-DBEB0464D575");
        private static readonly Guid CompletedId = new Guid("FDF3A1F6-605A-4f62-B463-7900D6782E56");
        private static readonly Guid PaidId = new Guid("4BCF3FA8-5B30-482b-A762-2BF43721E045");
        private static readonly Guid PartiallyPaidId = new Guid("CB502944-27D9-4aad-9DAC-5D1F5A344D08");
        private static readonly Guid RejectedId = new Guid("B11913F7-4FFD-44a8-8DDF-36200E910B37");
        private static readonly Guid OnHoldId = new Guid("D6819EB6-9141-4e83-BBC5-787CBA6E0932");
        private static readonly Guid InProcessId = new Guid("7752F5C5-B19B-4339-A937-0BAD768142A8");
        private static readonly Guid FinishedId = new Guid("A62C1773-C42C-456c-92F3-5FC67382D9A3");

        private UniquelyIdentifiableCache<PurchaseOrderObjectState> stateCache;

        public PurchaseOrderObjectState Provisional => this.StateCache.Get(ProvisionalId);

        public PurchaseOrderObjectState RequestsApproval => this.StateCache.Get(RequestsApprovalId);

        public PurchaseOrderObjectState Cancelled => this.StateCache.Get(CancelledId);

        public PurchaseOrderObjectState Completed => this.StateCache.Get(CompletedId);

        public PurchaseOrderObjectState Paid => this.StateCache.Get(PaidId);

        public PurchaseOrderObjectState PartiallyPaid => this.StateCache.Get(PartiallyPaidId);

        public PurchaseOrderObjectState PartiallyReceived => this.StateCache.Get(PartiallyReceivedId);

        public PurchaseOrderObjectState Received => this.StateCache.Get(ReceivedId);

        public PurchaseOrderObjectState Rejected => this.StateCache.Get(RejectedId);

        public PurchaseOrderObjectState Finished => this.StateCache.Get(FinishedId);

        public PurchaseOrderObjectState OnHold => this.StateCache.Get(OnHoldId);

        public PurchaseOrderObjectState InProcess => this.StateCache.Get(InProcessId);

        private UniquelyIdentifiableCache<PurchaseOrderObjectState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<PurchaseOrderObjectState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new PurchaseOrderObjectStateBuilder(this.Session)
                .WithUniqueId(ProvisionalId)
                .WithName("Created")
                .Build();

            new PurchaseOrderObjectStateBuilder(this.Session)
                .WithUniqueId(RequestsApprovalId)
                .WithName("Requests Approval")
                .Build();

            new PurchaseOrderObjectStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new PurchaseOrderObjectStateBuilder(this.Session)
                .WithUniqueId(CompletedId)
                .WithName("Completed")
                .Build();

            new PurchaseOrderObjectStateBuilder(this.Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();

            new PurchaseOrderObjectStateBuilder(this.Session)
                .WithUniqueId(PartiallyReceivedId)
                .WithName("Partially Received")
                .Build();

            new PurchaseOrderObjectStateBuilder(this.Session)
                .WithUniqueId(ReceivedId)
                .WithName("Received")
                .Build();

            new PurchaseOrderObjectStateBuilder(this.Session)
                .WithUniqueId(PartiallyPaidId)
                .WithName("Partially Paid")
                .Build();

            new PurchaseOrderObjectStateBuilder(this.Session)
                .WithUniqueId(RejectedId)
                .WithName("Rejected")
                .Build();

            new PurchaseOrderObjectStateBuilder(this.Session)
                .WithUniqueId(OnHoldId)
                .WithName("On Hold")
                .Build();

            new PurchaseOrderObjectStateBuilder(this.Session)
                .WithUniqueId(InProcessId)
                .WithName("In Process")
                .Build();

            new PurchaseOrderObjectStateBuilder(this.Session)
                .WithUniqueId(FinishedId)
                .WithName("Finished")
                .Build();
        }
    }
}