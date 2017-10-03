// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseInvoiceItemStates.cs" company="Allors bvba">
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

    public partial class PurchaseInvoiceItemStates
    {
        private static readonly Guid InProcessId = new Guid("72366881-A6CE-455f-80FF-A0E7295F2B8C");
        private static readonly Guid ApprovedId = new Guid("A1E60D62-57FB-4c46-94E7-89D94CF5DCF3");
        private static readonly Guid ReceivedId = new Guid("6B1F51FD-C3C6-4bd3-BBF0-1FCC66F8C455");
        private static readonly Guid PaidId = new Guid("EC0FD4B0-C766-453e-98C4-36FEFEC38A69");
        private static readonly Guid ReadyForPostingId = new Guid("AD5681F9-1B01-452f-8811-AE8428D59D69");
        private static readonly Guid CancelledId = new Guid("B983C7C4-4D18-4b53-966C-371D20DC4B2A");

        private UniquelyIdentifiableCache<PurchaseInvoiceItemState> stateCache;

        public PurchaseInvoiceItemState InProcess => this.StateCache.Get(InProcessId);

        public PurchaseInvoiceItemState Approved => this.StateCache.Get(ApprovedId);

        public PurchaseInvoiceItemState ReadyForPosting => this.StateCache.Get(ReadyForPostingId);

        public PurchaseInvoiceItemState Received => this.StateCache.Get(ReceivedId);

        public PurchaseInvoiceItemState Paid => this.StateCache.Get(PaidId);

        public PurchaseInvoiceItemState Cancelled => this.StateCache.Get(CancelledId);

        private UniquelyIdentifiableCache<PurchaseInvoiceItemState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<PurchaseInvoiceItemState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new PurchaseInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(InProcessId)
                .WithName("In Process")
                .Build();

            new PurchaseInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(ApprovedId)
                .WithName("Approved")
                .Build();

            new PurchaseInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(ReceivedId)
                .WithName("Received")
                .Build();

            new PurchaseInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(ReadyForPostingId)
                .WithName("Ready For Posting")
                .Build();

            new PurchaseInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();

            new PurchaseInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();
        }
    }
}
