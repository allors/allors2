// <copyright file="PurchaseInvoiceItemStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class PurchaseInvoiceItemStates
    {
        public static readonly Guid CreatedId = new Guid("4821A8FA-4DB8-48E3-A2FA-AFB1C635C1D4");
        public static readonly Guid AwaitingApprovalId = new Guid("A91953F9-73C8-480E-8097-CA9709DF3E66");
        public static readonly Guid ReceivedId = new Guid("6B1F51FD-C3C6-4bd3-BBF0-1FCC66F8C455");
        public static readonly Guid PartiallyPaidId = new Guid("DF7A7B6E-293F-432D-BE39-B92612652E29");
        public static readonly Guid NotPaidId = new Guid("50AB37E3-A74C-44F9-92C2-6AFF42C2BB99");
        public static readonly Guid PaidId = new Guid("EC0FD4B0-C766-453e-98C4-36FEFEC38A69");
        public static readonly Guid RejectedId = new Guid("4F92EA82-7683-4417-945B-5B0434E390A2");
        public static readonly Guid CancelledId = new Guid("B983C7C4-4D18-4b53-966C-371D20DC4B2A");
        public static readonly Guid CancelledByInvoiceId = new Guid("06189E75-4026-41A2-90FD-A5F6A6711992");

        private UniquelyIdentifiableSticky<PurchaseInvoiceItemState> cache;

        public PurchaseInvoiceItemState Created => this.Cache[CreatedId];

        public PurchaseInvoiceItemState AwaitingApproval => this.Cache[AwaitingApprovalId];

        public PurchaseInvoiceItemState Received => this.Cache[ReceivedId];

        public PurchaseInvoiceItemState PartiallyPaid => this.Cache[PartiallyPaidId];

        public PurchaseInvoiceItemState NotPaid => this.Cache[NotPaidId];

        public PurchaseInvoiceItemState Paid => this.Cache[PaidId];

        public PurchaseInvoiceItemState Rejected => this.Cache[RejectedId];

        public PurchaseInvoiceItemState Cancelled => this.Cache[CancelledId];

        public PurchaseInvoiceItemState CancelledByinvoice => this.Cache[CancelledByInvoiceId];

        private UniquelyIdentifiableSticky<PurchaseInvoiceItemState> Cache => this.cache ??= new UniquelyIdentifiableSticky<PurchaseInvoiceItemState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(CreatedId, v => v.Name = "Created");
            merge(AwaitingApprovalId, v => v.Name = "Awaiting Approval");
            merge(ReceivedId, v => v.Name = "Received");
            merge(PartiallyPaidId, v => v.Name = "Partially Paid");
            merge(NotPaidId, v => v.Name = "Not Paid");
            merge(PaidId, v => v.Name = "Paid");
            merge(RejectedId, v => v.Name = "Rejected");
            merge(CancelledId, v => v.Name = "Cancelled");
            merge(CancelledByInvoiceId, v => v.Name = "Cancelled By Invoice");
        }
    }
}
