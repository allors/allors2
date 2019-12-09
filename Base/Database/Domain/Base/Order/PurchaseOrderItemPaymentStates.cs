// <copyright file="PurchaseOrderItemPaymentStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class PurchaseOrderItemPaymentStates
    {
        internal static readonly Guid NotPaidId = new Guid("CAA6FCD8-3350-4113-945D-4C6A6398F317");
        internal static readonly Guid PaidId = new Guid("25AC349F-ECCD-4AB0-B032-0E5F14618083");
        internal static readonly Guid PartiallyPaidId = new Guid("B4269B24-DBCF-4548-A559-BC3D7A617D97");

        private UniquelyIdentifiableSticky<PurchaseOrderItemPaymentState> stateCache;

        public PurchaseOrderItemPaymentState NotPaid => this.StateCache[NotPaidId];

        public PurchaseOrderItemPaymentState Paid => this.StateCache[PaidId];

        public PurchaseOrderItemPaymentState PartiallyPaid => this.StateCache[PartiallyPaidId];

        private UniquelyIdentifiableSticky<PurchaseOrderItemPaymentState> StateCache => this.stateCache ??= new UniquelyIdentifiableSticky<PurchaseOrderItemPaymentState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            new PurchaseOrderItemPaymentStateBuilder(this.Session)
                .WithUniqueId(NotPaidId)
                .WithName("Not Paid")
                .Build();

            new PurchaseOrderItemPaymentStateBuilder(this.Session)
                .WithUniqueId(PartiallyPaidId)
                .WithName("Partially Paid")
                .Build();

            new PurchaseOrderItemPaymentStateBuilder(this.Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();
        }
    }
}
