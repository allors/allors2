//------------------------------------------------------------------------------------------------- 
// <copyright file="PaymentStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the HomeAddress type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    public partial class PaymentStates
    {
        private static readonly Guid UnpaidId = new Guid("FC38E48D-C8C4-4F26-A8F1-5D4E962B6F93");
        private static readonly Guid PartiallyPaidId = new Guid("1801737F-2760-4600-9243-7E6BDD8A224D");
        private static readonly Guid PaidId = new Guid("04FAD96A-2B0F-4F07-ABB7-57657A34E422");

        private UniquelyIdentifiableSticky<PaymentState> sticky;

        public Sticky<Guid, PaymentState> Sticky => this.sticky ?? (this.sticky = new UniquelyIdentifiableSticky<PaymentState>(this.Session));

        public PaymentState Unpaid => this.Sticky[UnpaidId];

        public PaymentState PartiallyPaid => this.Sticky[PartiallyPaidId];

        public PaymentState Paid => this.Sticky[PaidId];

        protected override void CoreSetup(Setup setup)
        {
            new PaymentStateBuilder(this.Session).WithUniqueId(UnpaidId).WithName("Unpaid").Build();
            new PaymentStateBuilder(this.Session).WithUniqueId(PartiallyPaidId).WithName("PartiallyPaid").Build();
            new PaymentStateBuilder(this.Session).WithUniqueId(PaidId).WithName("Paid").Build();
        }
    }
}
