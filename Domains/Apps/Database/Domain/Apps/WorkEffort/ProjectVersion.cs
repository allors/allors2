// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectVersion.cs" company="Allors bvba">
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
    public partial class ProjectVersion
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (ProjectVersionBuilder) method.Builder;
            var project = builder.Project;

            if (project != null)
            {
                this.Name = project.Name;
                this.Description = project.Description;
                this.Priority = project.Priority;
                this.WorkEffortPurposes = project.WorkEffortPurposes;
                this.ActualCompletion = project.ActualCompletion;
                this.ScheduledStart = project.ScheduledStart;
                this.ScheduledCompletion = project.ScheduledCompletion;
                this.ActualHours = project.ActualHours;
                this.EstimatedHours = project.EstimatedHours;
                this.Precendencies = project.Precendencies;
                this.Facility = project.Facility;
                this.DeliverablesProduced = project.DeliverablesProduced;
                this.ActualStart = project.ActualStart;
                this.InventoryItemsNeeded = project.InventoryItemsNeeded;
                this.Children = project.Children;
                this.OrderItemFulfillment = project.OrderItemFulfillment;
                this.WorkEffortType = project.WorkEffortType;
                this.InventoryItemsProduced = project.InventoryItemsProduced;
                this.RequirementFulfillments = project.RequirementFulfillments;
                this.SpecialTerms = project.SpecialTerms;
                this.Concurrencies = project.Concurrencies;
                this.CurrentObjectState = project.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}