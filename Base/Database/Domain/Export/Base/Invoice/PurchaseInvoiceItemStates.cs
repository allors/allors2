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
        public static readonly Guid CreatedId = new Guid("4821A8FA-4DB8-48E3-A2FA-AFB1C635C1D4");
        public static readonly Guid AwaitingApprovalId = new Guid("A91953F9-73C8-480E-8097-CA9709DF3E66");
        public static readonly Guid ReceivedId = new Guid("6B1F51FD-C3C6-4bd3-BBF0-1FCC66F8C455");
        public static readonly Guid PartiallyPaidId = new Guid("DF7A7B6E-293F-432D-BE39-B92612652E29");
        public static readonly Guid NotPaidId = new Guid("50AB37E3-A74C-44F9-92C2-6AFF42C2BB99");
        public static readonly Guid PaidId = new Guid("EC0FD4B0-C766-453e-98C4-36FEFEC38A69");
        public static readonly Guid RejectedId = new Guid("4F92EA82-7683-4417-945B-5B0434E390A2");
        public static readonly Guid CancelledId = new Guid("B983C7C4-4D18-4b53-966C-371D20DC4B2A");
        public static readonly Guid CancelledByInvoiceId = new Guid("06189E75-4026-41A2-90FD-A5F6A6711992");

        private UniquelyIdentifiableSticky<PurchaseInvoiceItemState> stateCache;

        public PurchaseInvoiceItemState Created => this.StateCache[CreatedId];

        public PurchaseInvoiceItemState AwaitingApproval => this.StateCache[AwaitingApprovalId];

        public PurchaseInvoiceItemState Received => this.StateCache[ReceivedId];

        public PurchaseInvoiceItemState PartiallyPaid => this.StateCache[PartiallyPaidId];

        public PurchaseInvoiceItemState NotPaid => this.StateCache[NotPaidId];

        public PurchaseInvoiceItemState Paid => this.StateCache[PaidId];

        public PurchaseInvoiceItemState Rejected => this.StateCache[RejectedId];

        public PurchaseInvoiceItemState Cancelled => this.StateCache[CancelledId];

        public PurchaseInvoiceItemState CancelledByinvoice => this.StateCache[CancelledByInvoiceId];

        private UniquelyIdentifiableSticky<PurchaseInvoiceItemState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<PurchaseInvoiceItemState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            new PurchaseInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new PurchaseInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(AwaitingApprovalId)
                .WithName("Awaiting Approval")
                .Build();

            new PurchaseInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(ReceivedId)
                .WithName("Received")
                .Build();

            new PurchaseInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(PartiallyPaidId)
                .WithName("Partially Paid")
                .Build();

            new PurchaseInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(NotPaidId)
                .WithName("Not Paid")
                .Build();

            new PurchaseInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();

            new PurchaseInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(RejectedId)
                .WithName("Rejected")
                .Build();

            new PurchaseInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new PurchaseInvoiceItemStateBuilder(this.Session)
                .WithUniqueId(CancelledByInvoiceId)
                .WithName("Cancelled")
                .Build();
        }
    }
}
