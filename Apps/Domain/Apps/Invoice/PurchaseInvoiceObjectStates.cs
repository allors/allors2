// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseInvoiceObjectStates.cs" company="Allors bvba">
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

    public partial class PurchaseInvoiceObjectStates
    {
        public static readonly Guid InProcessId = new Guid("C6501188-7145-4abd-85FC-BEF746C74E9E");
        public static readonly Guid ApprovedId = new Guid("5961E348-FCA0-4670-89B4-216F57C48519");
        public static readonly Guid ReceivedId = new Guid("FC9EC85B-2419-4c97-92F6-45F5C6D3DF61");
        public static readonly Guid PaidId = new Guid("2982C8BE-657E-4594-BCAF-98997AFEA9F8");
        public static readonly Guid ReadyForPostingId = new Guid("C6939E28-AEF9-4db9-B4C2-B6445BA6D881");
        public static readonly Guid CancelledId = new Guid("60650051-F1F1-4dd6-90C8-5E744093D2EE");

        private UniquelyIdentifiableCache<PurchaseInvoiceObjectState> stateCache;

        public PurchaseInvoiceObjectState InProcess
        {
            get { return this.StateCache.Get(InProcessId); }
        }

        public PurchaseInvoiceObjectState Approved
        {
            get { return this.StateCache.Get(ApprovedId); }
        }

        public PurchaseInvoiceObjectState ReadyForPosting
        {
            get { return this.StateCache.Get(ReadyForPostingId); }
        }

        public PurchaseInvoiceObjectState Received
        {
            get { return this.StateCache.Get(ReceivedId); }
        }

        public PurchaseInvoiceObjectState Paid
        {
            get { return this.StateCache.Get(PaidId); }
        }

        public PurchaseInvoiceObjectState Cancelled
        {
            get { return this.StateCache.Get(CancelledId); }
        }

        private UniquelyIdentifiableCache<PurchaseInvoiceObjectState> StateCache
        {
            get
            {
                return this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<PurchaseInvoiceObjectState>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(Session).EnglishGreatBritain;
            var dutchLocale = new Locales(Session).DutchNetherlands;

            new PurchaseInvoiceObjectStateBuilder(Session)
                .WithUniqueId(InProcessId)
                .WithName("In Process")
                .Build();

            new PurchaseInvoiceObjectStateBuilder(Session)
                .WithUniqueId(ApprovedId)
                .WithName("Approved")
                .Build();

            new PurchaseInvoiceObjectStateBuilder(Session)
                .WithUniqueId(ReceivedId)
                .WithName("Received")
                .Build();

            new PurchaseInvoiceObjectStateBuilder(Session)
                .WithUniqueId(ReadyForPostingId)
                .WithName("Ready For Posting")
                .Build();

            new PurchaseInvoiceObjectStateBuilder(Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();

            new PurchaseInvoiceObjectStateBuilder(Session)
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
