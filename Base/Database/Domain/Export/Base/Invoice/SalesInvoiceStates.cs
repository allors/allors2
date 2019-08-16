

// <copyright file="SalesInvoiceStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SalesInvoiceStates
    {
        internal static readonly Guid PaidId = new Guid("9B4FC618-CE43-4930-B0EE-B271320FC0B4");
        internal static readonly Guid PartiallyPaidId = new Guid("26BE5583-7016-4B3C-90C1-FB4BB3E2726C");
        internal static readonly Guid NotPaidId = new Guid("E123505A-9DDB-4435-BA56-101A00C27A8D");
        internal static readonly Guid ReadyForPostingId = new Guid("488F61FF-F474-44ba-9925-49AF7BCB0CD0");
        internal static readonly Guid WrittenOffId = new Guid("04EAD707-51F5-4718-8B43-229D2D75BDE2");
        internal static readonly Guid CancelledId = new Guid("3924F84A-515F-4a47-A7F3-361A50D890FB");

        private UniquelyIdentifiableSticky<SalesInvoiceState> stateCache;

        public SalesInvoiceState NotPaid => this.StateCache[NotPaidId];

        public SalesInvoiceState Paid => this.StateCache[PaidId];

        public SalesInvoiceState PartiallyPaid => this.StateCache[PartiallyPaidId];

        public SalesInvoiceState ReadyForPosting => this.StateCache[ReadyForPostingId];

        public SalesInvoiceState WrittenOff => this.StateCache[WrittenOffId];

        public SalesInvoiceState Cancelled => this.StateCache[CancelledId];

        private UniquelyIdentifiableSticky<SalesInvoiceState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<SalesInvoiceState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new SalesInvoiceStateBuilder(this.Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();

            new SalesInvoiceStateBuilder(this.Session)
                .WithUniqueId(NotPaidId)
                .WithName("Not Paid")
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
