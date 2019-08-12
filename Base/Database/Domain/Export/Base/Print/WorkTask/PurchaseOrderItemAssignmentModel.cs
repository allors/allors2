// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrderItemAssignmentModel.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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
