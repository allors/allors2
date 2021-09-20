// <copyright file="SalesInvoiceItemStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SalesInvoiceItemStates
    {
        public static readonly Guid PaidId = new Guid("875AD2E4-BF44-46f4-9CD6-5F5C5BD43ADC");
        public static readonly Guid PartiallyPaidId = new Guid("2C6A00F7-466A-4689-A7E0-2D5660804B15");
        public static readonly Guid NotPaidId = new Guid("9926746F-C1DC-4968-BA85-9D461260DD0C");
        public static readonly Guid ReadyForPostingId = new Guid("9585A2C8-5B4D-4063-A8E7-E1310DFE439D");
        public static readonly Guid WrittenOffId = new Guid("F4408FD5-CCA3-44ea-BC00-4FFECC5D1EB9");
        public static readonly Guid CancelledByInvoiceId = new Guid("3EE18D08-9AEA-445D-8E19-0616E4A61B0E");

        private UniquelyIdentifiableSticky<SalesInvoiceItemState> cache;

        public SalesInvoiceItemState NotPaid => this.Cache[NotPaidId];

        public SalesInvoiceItemState PartiallyPaid => this.Cache[PartiallyPaidId];

        public SalesInvoiceItemState Paid => this.Cache[PaidId];

        public SalesInvoiceItemState ReadyForPosting => this.Cache[ReadyForPostingId];

        public SalesInvoiceItemState WrittenOff => this.Cache[WrittenOffId];

        public SalesInvoiceItemState CancelledByInvoice => this.Cache[CancelledByInvoiceId];

        private UniquelyIdentifiableSticky<SalesInvoiceItemState> Cache => this.cache ??= new UniquelyIdentifiableSticky<SalesInvoiceItemState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(NotPaidId, v => v.Name = "Not Paid");
            merge(PartiallyPaidId, v => v.Name = "Partially Paid");
            merge(PaidId, v => v.Name = "Paid");
            merge(ReadyForPostingId, v => v.Name = "Ready For Posting");
            merge(WrittenOffId, v => v.Name = "Written Off");
            merge(CancelledByInvoiceId, v => v.Name = "Cancelled By Invoice");
        }
    }
}
