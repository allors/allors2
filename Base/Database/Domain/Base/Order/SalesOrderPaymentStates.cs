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

        private UniquelyIdentifiableSticky<SalesOrderPaymentState> cache;

        public SalesOrderPaymentState NotPaid => this.Cache[NotPaidId];

        public SalesOrderPaymentState PartiallyPaid => this.Cache[PartiallyPaidId];

        public SalesOrderPaymentState Paid => this.Cache[PaidId];

        private UniquelyIdentifiableSticky<SalesOrderPaymentState> Cache => this.cache ??= new UniquelyIdentifiableSticky<SalesOrderPaymentState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(NotPaidId, v => v.Name = "Not Paid");
            merge(PartiallyPaidId, v => v.Name = "Partially Paid");
            merge(PaidId, v => v.Name = "Paid");
        }
    }
}
