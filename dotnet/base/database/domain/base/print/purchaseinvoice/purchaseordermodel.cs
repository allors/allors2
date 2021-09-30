// <copyright file="PurchaseOrderModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.PurchaseInvoiceModel
{
    public class PurchaseOrderModel
    {
        public PurchaseOrderModel(PurchaseOrder order) => this.OrderNumber = order.OrderNumber;

        public string OrderNumber { get; }
    }
}
