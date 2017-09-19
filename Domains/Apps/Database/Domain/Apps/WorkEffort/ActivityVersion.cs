// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActivityVersion.cs" company="Allors bvba">
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
    public partial class ActivityVersion
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (ActivityVersionBuilder) method.Builder;
            var activity = builder.Activity;

            if (activity != null)
            {
                this.Name = activity.Name;
                this.Description = activity.Description;
                this.Priority = activity.Priority;
                this.WorkEffortPurposes = activity.WorkEffortPurposes;
                this.ActualCompletion = activity.ActualCompletion;
                this.ScheduledStart = activity.ScheduledStart;
                this.ScheduledCompletion = activity.ScheduledCompletion;
                this.ActualHours = activity.ActualHours;
                this.EstimatedHours = activity.EstimatedHours;
                this.Precendencies = activity.Precendencies;
                this.Facility = activity.Facility;
                this.DeliverablesProduced = activity.DeliverablesProduced;
                this.ActualStart = activity.ActualStart;
                this.InventoryItemsNeeded = activity.InventoryItemsNeeded;
                this.Children = activity.Children;
                this.OrderItemFulfillment = activity.OrderItemFulfillment;
                this.WorkEffortType = activity.WorkEffortType;
                this.InventoryItemsProduced = activity.InventoryItemsProduced;
                this.RequirementFulfillments = activity.RequirementFulfillments;
                this.SpecialTerms = activity.SpecialTerms;
                this.Concurrencies = activity.Concurrencies;
                this.CurrentObjectState = activity.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}