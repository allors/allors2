
// <copyright file="PurchaseOrderItemAssignmentModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace Allors.Domain.Print.WorkTaskModel
{
    public class PurchaseOrderItemAssignmentModel
    {
        public PurchaseOrderItemAssignmentModel(WorkEffortPurchaseOrderItemAssignment assignment)
        {
            var purchaseOrderItem = assignment.PurchaseOrderItem;

            this.Name = purchaseOrderItem.Part?.Name ?? purchaseOrderItem.Description;
            this.Quantity = assignment.Quantity;
            this.UnitOfMeasure = purchaseOrderItem.Part?.UnitOfMeasure?.Abbreviation?.ToUpperInvariant() ??
                                 purchaseOrderItem.Part?.UnitOfMeasure?.Name?.ToUpperInvariant() ??
                                 "EA";
            this.UnitSellingPrice = assignment.UnitSellingPrice;
            this.SellingPrice = Math.Round(assignment.Quantity * assignment.UnitSellingPrice, 2);
        }

        public string Name { get; }

        public int Quantity { get; }

        public string UnitOfMeasure { get; }

        public decimal UnitSellingPrice { get; }

        public decimal SellingPrice { get; }
    }
}
