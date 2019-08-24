// <copyright file="SalesOrderPaymentStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SalesOrderPaymentStates
    {
        internal static readonly Guid NotPaidId = new Guid("8F5E0C7D-893F-4C7F-8297-3B4BD6319D02");
        internal static readonly Guid PaidId = new Guid("0C84C6F6-3204-4f7f-9BFA-FA4CBA643177");
        internal static readonly Guid PartiallyPaidId = new Guid("F9E8E105-F84E-4550-A725-25CE6E96614E");

        private UniquelyIdentifiableSticky<SalesOrderPaymentState> stateCache;

        public SalesOrderPaymentState NotPaid => this.StateCache[NotPaidId];

        public SalesOrderPaymentState PartiallyPaid => this.StateCache[PartiallyPaidId];

        public SalesOrderPaymentState Paid => this.StateCache[PaidId];

        private UniquelyIdentifiableSticky<SalesOrderPaymentState> StateCache => this.stateCache ?? (this.stateCache = new UniquelyIdentifiableSticky<SalesOrderPaymentState>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            new SalesOrderPaymentStateBuilder(this.Session)
                .WithUniqueId(NotPaidId)
                .WithName("Not Paid")
                .Build();

            new SalesOrderPaymentStateBuilder(this.Session)
                .WithUniqueId(PartiallyPaidId)
                .WithName("Partially Paid")
                .Build();

            new SalesOrderPaymentStateBuilder(this.Session)
                .WithUniqueId(PaidId)
                .WithName("Paid")
                .Build();
        }
    }
}
