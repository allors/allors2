// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Allors bvba">
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
namespace Allors.Domain
{
    public partial class Program
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            var isNewVersion =
                !this.ExistCurrentVersion ||
                !object.Equals(this.ActualCompletion, this.CurrentVersion.ActualCompletion);

            var isNewStateVersion =
                !this.ExistCurrentVersion ||
                !object.Equals(this.Name, this.CurrentVersion.Name) ||
                !object.Equals(this.Description, this.CurrentVersion.Description) ||
                !object.Equals(this.Priority, this.CurrentVersion.Priority) ||
                !object.Equals(this.WorkEffortPurposes, this.CurrentVersion.WorkEffortPurposes) ||
                !object.Equals(this.ActualCompletion, this.CurrentVersion.ActualCompletion) ||
                !object.Equals(this.ScheduledStart, this.CurrentVersion.ScheduledStart) ||
                !object.Equals(this.ScheduledCompletion, this.CurrentVersion.ScheduledCompletion) ||
                !object.Equals(this.ActualHours, this.CurrentVersion.ActualHours) ||
                !object.Equals(this.EstimatedHours, this.CurrentVersion.EstimatedHours) ||
                !object.Equals(this.Precendencies, this.CurrentVersion.Precendencies) ||
                !object.Equals(this.Facility, this.CurrentVersion.Facility) ||
                !object.Equals(this.DeliverablesProduced, this.CurrentVersion.DeliverablesProduced) ||
                !object.Equals(this.ActualStart, this.CurrentVersion.ActualStart) ||
                !object.Equals(this.InventoryItemsNeeded, this.CurrentVersion.InventoryItemsNeeded) ||
                !object.Equals(this.Children, this.CurrentVersion.Children) ||
                !object.Equals(this.OrderItemFulfillment, this.CurrentVersion.OrderItemFulfillment) ||
                !object.Equals(this.WorkEffortType, this.CurrentVersion.WorkEffortType) ||
                !object.Equals(this.InventoryItemsProduced, this.CurrentVersion.InventoryItemsProduced) ||
                !object.Equals(this.RequirementFulfillments, this.CurrentVersion.RequirementFulfillments) ||
                !object.Equals(this.SpecialTerms, this.CurrentVersion.SpecialTerms) ||
                !object.Equals(this.Concurrencies, this.CurrentVersion.Concurrencies) ||
                !object.Equals(this.CurrentObjectState, this.CurrentVersion.CurrentObjectState);

            if (isNewVersion)
            {
                this.PreviousVersion = this.CurrentVersion;
                this.CurrentVersion = new ProgramVersionBuilder(this.Strategy.Session).WithProgram(this).Build();
                this.AddAllVersion(this.CurrentVersion);
            }

            if (isNewStateVersion)
            {
                this.CurrentStateVersion = CurrentVersion;
                this.AddAllStateVersion(this.CurrentStateVersion);
            }
        }
    }
}
