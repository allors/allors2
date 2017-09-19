// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkTaskVersion.cs" company="Allors bvba">
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
    public partial class WorkTaskVersion
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (WorkTaskVersionBuilder) method.Builder;
            var workTask = builder.WorkTask;

            if (workTask != null)
            {
                this.Name = workTask.Name;
                this.Description = workTask.Description;
                this.Priority = workTask.Priority;
                this.WorkEffortPurposes = workTask.WorkEffortPurposes;
                this.ActualCompletion = workTask.ActualCompletion;
                this.ScheduledStart = workTask.ScheduledStart;
                this.ScheduledCompletion = workTask.ScheduledCompletion;
                this.ActualHours = workTask.ActualHours;
                this.EstimatedHours = workTask.EstimatedHours;
                this.Precendencies = workTask.Precendencies;
                this.Facility = workTask.Facility;
                this.DeliverablesProduced = workTask.DeliverablesProduced;
                this.ActualStart = workTask.ActualStart;
                this.InventoryItemsNeeded = workTask.InventoryItemsNeeded;
                this.Children = workTask.Children;
                this.OrderItemFulfillment = workTask.OrderItemFulfillment;
                this.WorkEffortType = workTask.WorkEffortType;
                this.InventoryItemsProduced = workTask.InventoryItemsProduced;
                this.RequirementFulfillments = workTask.RequirementFulfillments;
                this.SpecialTerms = workTask.SpecialTerms;
                this.Concurrencies = workTask.Concurrencies;
                this.CurrentObjectState = workTask.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}