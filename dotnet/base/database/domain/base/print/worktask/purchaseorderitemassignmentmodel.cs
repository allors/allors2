// <copyright file="PurchaseOrderItemAssignmentModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.WorkTaskModel
{
    using System;
    using System.Globalization;

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
            this.UnitSellingPrice = Rounder.RoundDecimal(assignment.UnitSellingPrice, 2).ToString("N2", new CultureInfo("nl-BE"));
            this.SellingPrice = Rounder.RoundDecimal(assignment.Quantity * assignment.UnitSellingPrice, 2).ToString("N2", new CultureInfo("nl-BE"));
        }

        public string Name { get; }

        public int Quantity { get; }

        public string UnitOfMeasure { get; }

        public string UnitSellingPrice { get; }

        public string SellingPrice { get; }
    }
}
