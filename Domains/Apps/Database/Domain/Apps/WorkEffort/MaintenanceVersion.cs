// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaintenanceVersion.cs" company="Allors bvba">
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
    public partial class MaintenanceVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (MaintenanceVersionBuilder) method.Builder;
            var maintenance = builder.Maintenance;

            if (maintenance != null)
            {
                this.Name = maintenance.Name;
                this.Description = maintenance.Description;
                this.Priority = maintenance.Priority;
                this.WorkEffortPurposes = maintenance.WorkEffortPurposes;
                this.ActualCompletion = maintenance.ActualCompletion;
                this.ScheduledStart = maintenance.ScheduledStart;
                this.ScheduledCompletion = maintenance.ScheduledCompletion;
                this.ActualHours = maintenance.ActualHours;
                this.EstimatedHours = maintenance.EstimatedHours;
                this.Precendencies = maintenance.Precendencies;
                this.Facility = maintenance.Facility;
                this.DeliverablesProduced = maintenance.DeliverablesProduced;
                this.ActualStart = maintenance.ActualStart;
                this.InventoryItemsNeeded = maintenance.InventoryItemsNeeded;
                this.Children = maintenance.Children;
                this.OrderItemFulfillment = maintenance.OrderItemFulfillment;
                this.WorkEffortType = maintenance.WorkEffortType;
                this.InventoryItemsProduced = maintenance.InventoryItemsProduced;
                this.RequirementFulfillments = maintenance.RequirementFulfillments;
                this.SpecialTerms = maintenance.SpecialTerms;
                this.Concurrencies = maintenance.Concurrencies;
                this.CurrentObjectState = maintenance.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}