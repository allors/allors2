// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseInvoiceItemObjectStates.cs" company="Allors bvba">
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

    public partial class PurchaseInvoiceItemObjectStates
    {
        public static readonly Guid InProcessId = new Guid("72366881-A6CE-455f-80FF-A0E7295F2B8C");
        public static readonly Guid ApprovedId = new Guid("A1E60D62-57FB-4c46-94E7-89D94CF5DCF3");
        public static readonly Guid ReceivedId = new Guid("6B1F51FD-C3C6-4bd3-BBF0-1FCC66F8C455");
        public static readonly Guid PaidId = new Guid("EC0FD4B0-C766-453e-98C4-36FEFEC38A69");
        public static readonly Guid ReadyForPostingId = new Guid("AD5681F9-1B01-452f-8811-AE8428D59D69");
        public static readonly Guid CancelledId = new Guid("B983C7C4-4D18-4b53-966C-371D20DC4B2A");

        private UniquelyIdentifiableCache<PurchaseInvoiceItemObjectState> stateCache;

        public PurchaseInvoiceItemObjectState InProcess
        {
            get { return this.StateCache.Get(InProcessId); }
        }

        public PurchaseInvoiceItemObjectState Approved
        {
            get { return this.StateCache.Get(ApprovedId); }
        }

        public PurchaseInvoiceItemObjectState ReadyForPosting
        {
            get { return this.StateCache.Get(ReadyForPostingId); }
        }

        public PurchaseInvoiceItemObjectState Received
        {
            get { return this.StateCache.Get(ReceivedId); }
        }

        public PurchaseInvoiceItemObjectState Paid
        {
            get { return this.StateCache.Get(PaidId); }
        }

        public PurchaseInvoiceItemObjectState Cancelled
        {
            get { return this.StateCache.Get(CancelledId); }
        }

        private UniquelyIdentifiableCache<PurchaseInvoiceItemObjectState> StateCache
        {
            get
            {
                return this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<PurchaseInvoiceItemObjectState>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(Session).EnglishGreatBritain;
            var dutchLocale = new Locales(Session).DutchNetherlands;

            new PurchaseInvoiceItemObjectStateBuilder(Session)
                .WithUniqueId(InProcessId)
                .WithName("In Process")
                .Build();

            new PurchaseInvoiceItemObjectStateBuilder(Session)
                .WithUniqueId(ApprovedId)
                .WithName("Approved")
                .Build();

            new PurchaseInvoiceItemObjectStateBuilder(Session)
                .WithUniqueId(ReceivedId)
                .WithName("Received")
                .Build();

            new PurchaseInvoiceItemObjectStateBuilder(Session)
                .WithUniqueId(ReadyForPostingId)
                .WithName("Ready For Posting")
                .Build();

            new PurchaseInvoiceItemObjectStateBuilder(Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();

            new PurchaseInvoiceItemObjectStateBuilder(Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
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
