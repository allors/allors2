// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryAssignmentModel.cs" company="Allors bvba">
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
    public class InventoryAssignmentModel
    {
        public InventoryAssignmentModel(WorkEffortInventoryAssignment assignment)
        {
            this.PartId = assignment.InventoryItem.Part?.PartIdentification();
            this.PartName = assignment.InventoryItem.Part?.Name;
            this.Quantity = assignment.BillableQuantity ?? 0M;
            this.UnitOfMeasure = assignment.InventoryItem.Part?.UnitOfMeasure?.Abbreviation?.ToUpperInvariant() ??
                                 assignment.InventoryItem.Part?.UnitOfMeasure?.Name?.ToUpperInvariant() ??
                                 "EA";
            this.UnitSellingPrice = assignment.UnitSellingPrice;
            this.SellingPrice = Math.Round(assignment.Quantity * assignment.UnitSellingPrice, 2);
        }

        public string PartId { get; }
        public string PartName { get; }
        public decimal Quantity { get; }
        public string UnitOfMeasure { get; }
        public decimal UnitSellingPrice { get; }
        public decimal SellingPrice { get; }
    }
}
