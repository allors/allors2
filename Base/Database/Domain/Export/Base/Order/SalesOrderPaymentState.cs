// <copyright file="SalesOrderPaymentState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class SalesOrderPaymentState
    {
        public bool NotPaid => Equals(this.UniqueId, SalesOrderPaymentStates.NotPaidId);

        public bool PartiallyPaid => Equals(this.UniqueId, SalesOrderPaymentStates.PartiallyPaidId);

        public bool Paid => Equals(this.UniqueId, SalesOrderPaymentStates.PaidId);
    }
}
