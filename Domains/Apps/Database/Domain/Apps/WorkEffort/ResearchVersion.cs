// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResearchVersion.cs" company="Allors bvba">
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
    public partial class ResearchVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (ResearchVersionBuilder) method.Builder;
            var research = builder.Research;

            if (research != null)
            {
                this.Name = research.Name;
                this.Description = research.Description;
                this.Priority = research.Priority;
                this.WorkEffortPurposes = research.WorkEffortPurposes;
                this.ActualCompletion = research.ActualCompletion;
                this.ScheduledStart = research.ScheduledStart;
                this.ScheduledCompletion = research.ScheduledCompletion;
                this.ActualHours = research.ActualHours;
                this.EstimatedHours = research.EstimatedHours;
                this.Precendencies = research.Precendencies;
                this.Facility = research.Facility;
                this.DeliverablesProduced = research.DeliverablesProduced;
                this.ActualStart = research.ActualStart;
                this.InventoryItemsNeeded = research.InventoryItemsNeeded;
                this.Children = research.Children;
                this.OrderItemFulfillment = research.OrderItemFulfillment;
                this.WorkEffortType = research.WorkEffortType;
                this.InventoryItemsProduced = research.InventoryItemsProduced;
                this.RequirementFulfillments = research.RequirementFulfillments;
                this.SpecialTerms = research.SpecialTerms;
                this.Concurrencies = research.Concurrencies;
                this.CurrentObjectState = research.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}