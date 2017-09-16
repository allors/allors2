// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductionRunVersion.cs" company="Allors bvba">
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
    public partial class ProductionRunVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (ProductionRunVersionBuilder) method.Builder;
            var productionRun = builder.ProductionRun;

            if (productionRun != null)
            {
                this.Name = productionRun.Name;
                this.Description = productionRun.Description;
                this.Priority = productionRun.Priority;
                this.WorkEffortPurposes = productionRun.WorkEffortPurposes;
                this.ActualCompletion = productionRun.ActualCompletion;
                this.ScheduledStart = productionRun.ScheduledStart;
                this.ScheduledCompletion = productionRun.ScheduledCompletion;
                this.ActualHours = productionRun.ActualHours;
                this.EstimatedHours = productionRun.EstimatedHours;
                this.Precendencies = productionRun.Precendencies;
                this.Facility = productionRun.Facility;
                this.DeliverablesProduced = productionRun.DeliverablesProduced;
                this.ActualStart = productionRun.ActualStart;
                this.InventoryItemsNeeded = productionRun.InventoryItemsNeeded;
                this.Children = productionRun.Children;
                this.OrderItemFulfillment = productionRun.OrderItemFulfillment;
                this.WorkEffortType = productionRun.WorkEffortType;
                this.InventoryItemsProduced = productionRun.InventoryItemsProduced;
                this.RequirementFulfillments = productionRun.RequirementFulfillments;
                this.SpecialTerms = productionRun.SpecialTerms;
                this.Concurrencies = productionRun.Concurrencies;
                this.CurrentObjectState = productionRun.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}