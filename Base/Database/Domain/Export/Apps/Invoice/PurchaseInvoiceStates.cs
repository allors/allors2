// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseInvoiceStates.cs" company="Allors bvba">
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

    public partial class PurchaseInvoiceStates
    {
        public static readonly Guid CreatedId = new Guid("102F4080-1D12-4090-9196-F42C094C38CA");
        public static readonly Guid AwaitingApprovalId = new Guid("FE3A30A9-0174-4534-A11E-E772112E9760");
        public static readonly Guid ReceivedId = new Guid("FC9EC85B-2419-4c97-92F6-45F5C6D3DF61");
        public static readonly Guid PartiallyPaidId = new Guid("9D917078-7ACD-4F04-AE6B-24E33755E9B1");
        public static readonly Guid NotPaidId = new Guid("3D811CFE-3EC0-4975-80B8-012A42B2B3E2");
        public static readonly Guid PaidId = new Guid("2982C8BE-657E-4594-BCAF-98997AFEA9F8");
        public static readonly Guid CancelledId = new Guid("60650051-F1F1-4dd6-90C8-5E744093D2EE");
        public static readonly Guid RejectedId = new Guid("26E27DDC-0782-4C29-B4BE-FF1E7AEE788A");

        private UniquelyIdentifiableSticky<PurchaseInvoiceState> stateCache;

        public PurchaseInvoiceState Created => this.StateCache[CreatedId];

        public PurchaseInvoiceState AwaitingApproval => this.StateCache[AwaitingApprovalId];

        public PurchaseInvoiceState Received => this.StateCache[ReceivedId];

        public PurchaseInvoiceState PartiallyPaid => this.StateCache[PartiallyPaidId];

        public PurchaseInvoiceState NotPaid => this.StateCache[NotPaidId];

        public PurchaseInvoiceState Paid => this.StateCache[PaidId];

        public PurchaseInvoiceState Cancelled => this.StateCache[CancelledId];

        public PurchaseInvoiceState Rejected => this.StateCache[RejectedId];

        private UniquelyIdentifiableSticky<PurchaseInvoiceState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<PurchaseInvoiceState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            

            new PurchaseInvoiceStateBuilder(this.Session)
                .WithUniqueId(CreatedId)
                .WithName("Created")
                .Build();

            new PurchaseInvoiceStateBuilder(this.Session)
                .WithUniqueId(AwaitingApprovalId)
                .WithName("AwaitingApproval")
                .Build();

            new PurchaseInvoiceStateBuilder(this.Session)
                .WithUniqueId(ReceivedId)
                .WithName("Received")
                .Build();

            new PurchaseInvoiceStateBuilder(this.Session)
                .WithUniqueId(PartiallyPaidId)
                .WithName("Partially Paid")
                .Build();

            new PurchaseInvoiceStateBuilder(this.Session)
                .WithUniqueId(NotPaidId)
                .WithName("Not Paid")
                .Build();

            new PurchaseInvoiceStateBuilder(this.Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();

            new PurchaseInvoiceStateBuilder(this.Session)
                .WithUniqueId(CancelledId)
                .WithName("Cancelled")
                .Build();

            new PurchaseInvoiceStateBuilder(this.Session)
                .WithUniqueId(RejectedId)
                .WithName("Rejected")
                .Build();
        }
    }
}
