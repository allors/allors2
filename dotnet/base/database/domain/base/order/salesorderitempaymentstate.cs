// <copyright file="SalesOrderItemPaymentState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class SalesOrderItemPaymentState
    {
        public bool NotPaid => Equals(this.UniqueId, SalesOrderItemPaymentStates.NotPaidId);

        public bool PartiallyPaid => Equals(this.UniqueId, SalesOrderItemPaymentStates.PartiallyPaidId);

        public bool Paid => Equals(this.UniqueId, SalesOrderItemPaymentStates.PaidId);
    }
}
