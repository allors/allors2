// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkTaskModel.cs" company="Allors bvba">
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

namespace Allors.Domain.Print.WorkTaskModel
{
    public class InventoryAssignmentModel
    {
        public InventoryAssignmentModel(WorkEffortInventoryAssignment assignment)
        {
            if (assignment != null)
            {
                this.PartId = assignment.Part?.PartIdentification;
                this.PartName = assignment.Part?.Name;
                this.Quantity = assignment.Quantity;
                this.UnitOfMeasure = assignment.Part?.UnitOfMeasure?.Abbreviation?.ToUpperInvariant() ??
                                     assignment.Part?.UnitOfMeasure?.Name?.ToUpperInvariant() ??
                                     "EA";
            }
        }

        public string PartId { get; }
        public string PartName { get; }
        public int Quantity { get; }
        public string UnitOfMeasure { get; }
    }
}
