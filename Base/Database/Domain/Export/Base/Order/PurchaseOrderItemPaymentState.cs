// <copyright file="PurchaseOrderItemPaymentState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class PurchaseOrderItemPaymentState
    {
        public bool IsNotPaid => Equals(this.UniqueId, PurchaseOrderItemPaymentStates.NotPaidId);

        public bool IsPartiallyPaid => Equals(this.UniqueId, PurchaseOrderItemPaymentStates.PartiallyPaidId);

        public bool IsPaid => Equals(this.UniqueId, PurchaseOrderItemPaymentStates.PaidId);
    }
}
