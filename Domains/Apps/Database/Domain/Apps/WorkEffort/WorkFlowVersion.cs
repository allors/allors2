// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkFlowVersion.cs" company="Allors bvba">
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
    public partial class WorkFlowVersion
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (WorkFlowVersionBuilder)method.Builder;
            var workFlow = builder.WorkFlow;

            if (workFlow != null)
            {
                this.Name = workFlow.Name;
                this.Description = workFlow.Description;
                this.Priority = workFlow.Priority;
                this.WorkEffortPurposes = workFlow.WorkEffortPurposes;
                this.ActualCompletion = workFlow.ActualCompletion;
                this.ScheduledStart = workFlow.ScheduledStart;
                this.ScheduledCompletion = workFlow.ScheduledCompletion;
                this.ActualHours = workFlow.ActualHours;
                this.EstimatedHours = workFlow.EstimatedHours;
                this.Precendencies = workFlow.Precendencies;
                this.Facility = workFlow.Facility;
                this.DeliverablesProduced = workFlow.DeliverablesProduced;
                this.ActualStart = workFlow.ActualStart;
                this.InventoryItemsNeeded = workFlow.InventoryItemsNeeded;
                this.Children = workFlow.Children;
                this.OrderItemFulfillment = workFlow.OrderItemFulfillment;
                this.WorkEffortType = workFlow.WorkEffortType;
                this.InventoryItemsProduced = workFlow.InventoryItemsProduced;
                this.RequirementFulfillments = workFlow.RequirementFulfillments;
                this.SpecialTerms = workFlow.SpecialTerms;
                this.Concurrencies = workFlow.Concurrencies;
                this.CurrentObjectState = workFlow.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}