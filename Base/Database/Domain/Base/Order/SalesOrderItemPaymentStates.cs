// <copyright file="SalesOrderItemPaymentStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SalesOrderItemPaymentStates
    {
        internal static readonly Guid NotPaidId = new Guid("2B859188-A3FA-4E53-8841-B316A81CD3BC");
        internal static readonly Guid PaidId = new Guid("086840CD-F7A6-4c04-A565-1D0AE07FED00");
        internal static readonly Guid PartiallyPaidId = new Guid("110F12F8-8AC6-40fb-8208-7697A36E88D7");

        private UniquelyIdentifiableSticky<SalesOrderItemPaymentState> cache;

        public SalesOrderItemPaymentState NotPaid => this.Cache[NotPaidId];

        public SalesOrderItemPaymentState Paid => this.Cache[PaidId];

        public SalesOrderItemPaymentState PartiallyPaid => this.Cache[PartiallyPaidId];

        private UniquelyIdentifiableSticky<SalesOrderItemPaymentState> Cache => this.cache ??= new UniquelyIdentifiableSticky<SalesOrderItemPaymentState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(NotPaidId, v => v.Name = "Not Paid");
            merge(PartiallyPaidId, v => v.Name = "Partially Paid");
            merge(PaidId, v => v.Name = "Paid");
        }
    }
}
