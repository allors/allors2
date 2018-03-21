// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoiceStates.cs" company="Allors bvba">
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

    public partial class SalesInvoiceStates
    {
        private static readonly Guid SentId = new Guid("C2EE7017-5023-4ac8-AD83-F2B9798FD6EA");
        private static readonly Guid PaidId = new Guid("9B4FC618-CE43-4930-B0EE-B271320FC0B4");
        private static readonly Guid PartiallyPaidId = new Guid("26BE5583-7016-4B3C-90C1-FB4BB3E2726C");
        private static readonly Guid ReadyForPostingId = new Guid("488F61FF-F474-44ba-9925-49AF7BCB0CD0");
        private static readonly Guid WrittenOffId = new Guid("04EAD707-51F5-4718-8B43-229D2D75BDE2");
        private static readonly Guid CancelledId = new Guid("3924F84A-515F-4a47-A7F3-361A50D890FB");

        private UniquelyIdentifiableSticky<SalesInvoiceState> stateCache;

        public SalesInvoiceState Sent => this.StateCache[SentId];

        public SalesInvoiceState Paid => this.StateCache[PaidId];

        public SalesInvoiceState PartiallyPaid => this.StateCache[PartiallyPaidId];

        public SalesInvoiceState ReadyForPosting => this.StateCache[ReadyForPostingId];

        public SalesInvoiceState WrittenOff => this.StateCache[WrittenOffId];

        public SalesInvoiceState Cancelled => this.StateCache[CancelledId];

        private UniquelyIdentifiableSticky<SalesInvoiceState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<SalesInvoiceState>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new SalesInvoiceStateBuilder(this.Session)
                .WithUniqueId(SentId)
                .WithName("Sent")
                .Build();

            new SalesInvoiceStateBuilder(this.Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();

            new SalesInvoiceStateBuilder(this.Session)
                .WithUniqueId(PartiallyPaidId)
                .WithName("Partially Paid")
                .Build();

            new SalesInvoiceStateBuilder(this.Session)
                .WithUniqueId(ReadyForPostingId)
                .WithName("Ready For Posting")
                .Build();

            new SalesInvoiceStateBuilder(this.Session)
                .WithUniqueId(WrittenOffId)
                .WithName("Written Off")
                .Build();

            new SalesInvoiceStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();
        }
    }
}
