// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhaseVersion.cs" company="Allors bvba">
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
    public partial class PhaseVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (PhaseVersionBuilder) method.Builder;
            var phase = builder.Phase;

            if (phase != null)
            {
                this.Name = phase.Name;
                this.Description = phase.Description;
                this.Priority = phase.Priority;
                this.WorkEffortPurposes = phase.WorkEffortPurposes;
                this.ActualCompletion = phase.ActualCompletion;
                this.ScheduledStart = phase.ScheduledStart;
                this.ScheduledCompletion = phase.ScheduledCompletion;
                this.ActualHours = phase.ActualHours;
                this.EstimatedHours = phase.EstimatedHours;
                this.Precendencies = phase.Precendencies;
                this.Facility = phase.Facility;
                this.DeliverablesProduced = phase.DeliverablesProduced;
                this.ActualStart = phase.ActualStart;
                this.InventoryItemsNeeded = phase.InventoryItemsNeeded;
                this.Children = phase.Children;
                this.OrderItemFulfillment = phase.OrderItemFulfillment;
                this.WorkEffortType = phase.WorkEffortType;
                this.InventoryItemsProduced = phase.InventoryItemsProduced;
                this.RequirementFulfillments = phase.RequirementFulfillments;
                this.SpecialTerms = phase.SpecialTerms;
                this.Concurrencies = phase.Concurrencies;
                this.CurrentObjectState = phase.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}