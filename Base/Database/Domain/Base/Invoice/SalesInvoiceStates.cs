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

        private UniquelyIdentifiableSticky<SalesInvoiceState> cache;

        public SalesInvoiceState NotPaid => this.Cache[NotPaidId];

        public SalesInvoiceState Paid => this.Cache[PaidId];

        public SalesInvoiceState PartiallyPaid => this.Cache[PartiallyPaidId];

        public SalesInvoiceState ReadyForPosting => this.Cache[ReadyForPostingId];

        public SalesInvoiceState WrittenOff => this.Cache[WrittenOffId];

        public SalesInvoiceState Cancelled => this.Cache[CancelledId];

        private UniquelyIdentifiableSticky<SalesInvoiceState> Cache => this.cache ??= new UniquelyIdentifiableSticky<SalesInvoiceState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(PaidId, v => v.Name = "Paid");
            merge(NotPaidId, v => v.Name = "Not Paid");
            merge(PartiallyPaidId, v => v.Name = "Partially Paid");
            merge(ReadyForPostingId, v => v.Name = "Ready For Posting");
            merge(WrittenOffId, v => v.Name = "Written Off");
            merge(CancelledId, v => v.Name = "Cancelled");
        }
    }
}
