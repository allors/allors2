// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoiceObjectStates.cs" company="Allors bvba">
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

    public partial class SalesInvoiceObjectStates
    {
        public static readonly Guid SentId = new Guid("C2EE7017-5023-4ac8-AD83-F2B9798FD6EA");
        public static readonly Guid PaidId = new Guid("9B4FC618-CE43-4930-B0EE-B271320FC0B4");
        public static readonly Guid PartiallyPaidId = new Guid("26BE5583-7016-4B3C-90C1-FB4BB3E2726C");
        public static readonly Guid ReadyForPostingId = new Guid("488F61FF-F474-44ba-9925-49AF7BCB0CD0");
        public static readonly Guid WrittenOffId = new Guid("04EAD707-51F5-4718-8B43-229D2D75BDE2");
        public static readonly Guid CancelledId = new Guid("3924F84A-515F-4a47-A7F3-361A50D890FB");

        private UniquelyIdentifiableCache<SalesInvoiceObjectState> stateCache;

        public SalesInvoiceObjectState Sent
        {
            get { return this.StateCache.Get(SentId); }
        }

        public SalesInvoiceObjectState Paid
        {
            get { return this.StateCache.Get(PaidId); }
        }

        public SalesInvoiceObjectState PartiallyPaid
        {
            get { return this.StateCache.Get(PartiallyPaidId); }
        }

        public SalesInvoiceObjectState ReadyForPosting
        {
            get { return this.StateCache.Get(ReadyForPostingId); }
        }

        public SalesInvoiceObjectState WrittenOff
        {
            get { return this.StateCache.Get(WrittenOffId); }
        }

        public SalesInvoiceObjectState Cancelled
        {
            get { return this.StateCache.Get(CancelledId); }
        }

        private UniquelyIdentifiableCache<SalesInvoiceObjectState> StateCache
        {
            get
            {
                return this.stateCache ?? (this.stateCache = new UniquelyIdentifiableCache<SalesInvoiceObjectState>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(Session).EnglishGreatBritain;
            var dutchLocale = new Locales(Session).DutchNetherlands;

            new SalesInvoiceObjectStateBuilder(Session)
                .WithUniqueId(SentId)
                .WithName("Sent")
                .Build();

            new SalesInvoiceObjectStateBuilder(Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();

            new SalesInvoiceObjectStateBuilder(Session)
                .WithUniqueId(PartiallyPaidId)
                .WithName("Partially Paid")
                .Build();

            new SalesInvoiceObjectStateBuilder(Session)
                .WithUniqueId(ReadyForPostingId)
                .WithName("Ready For Posting")
                .Build();

            new SalesInvoiceObjectStateBuilder(Session)
                .WithUniqueId(WrittenOffId)
                .WithName("Written Off")
                .Build();

            new SalesInvoiceObjectStateBuilder(Session)
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
