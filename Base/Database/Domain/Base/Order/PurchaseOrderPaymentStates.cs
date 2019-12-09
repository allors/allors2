// <copyright file="PurchaseOrderPaymentStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class PurchaseOrderPaymentStates
    {
        public static readonly Guid NotPaidId = new Guid("3DCF7D7D-4CD1-4D68-A909-7BEB672A2179");
        public static readonly Guid PaidId = new Guid("4BCF3FA8-5B30-482b-A762-2BF43721E045");
        public static readonly Guid PartiallyPaidId = new Guid("CB502944-27D9-4aad-9DAC-5D1F5A344D08");

        private UniquelyIdentifiableSticky<PurchaseOrderPaymentState> stateCache;

        public PurchaseOrderPaymentState NotPaid => this.StateCache[NotPaidId];

        public PurchaseOrderPaymentState Paid => this.StateCache[PaidId];

        public PurchaseOrderPaymentState PartiallyPaid => this.StateCache[PartiallyPaidId];

        private UniquelyIdentifiableSticky<PurchaseOrderPaymentState> StateCache => this.stateCache ??= new UniquelyIdentifiableSticky<PurchaseOrderPaymentState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            new PurchaseOrderPaymentStateBuilder(this.Session)
                .WithUniqueId(NotPaidId)
                .WithName("Not Paid")
                .Build();

            new PurchaseOrderPaymentStateBuilder(this.Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();

            new PurchaseOrderPaymentStateBuilder(this.Session)
                .WithUniqueId(PartiallyPaidId)
                .WithName("Partially Paid")
                .Build();
        }
    }
}
