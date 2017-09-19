// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgramVersion.cs" company="Allors bvba">
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
    public partial class ProgramVersion
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (ProgramVersionBuilder) method.Builder;
            var program = builder.Program;

            if (program != null)
            {
                this.Name = program.Name;
                this.Description = program.Description;
                this.Priority = program.Priority;
                this.WorkEffortPurposes = program.WorkEffortPurposes;
                this.ActualCompletion = program.ActualCompletion;
                this.ScheduledStart = program.ScheduledStart;
                this.ScheduledCompletion = program.ScheduledCompletion;
                this.ActualHours = program.ActualHours;
                this.EstimatedHours = program.EstimatedHours;
                this.Precendencies = program.Precendencies;
                this.Facility = program.Facility;
                this.DeliverablesProduced = program.DeliverablesProduced;
                this.ActualStart = program.ActualStart;
                this.InventoryItemsNeeded = program.InventoryItemsNeeded;
                this.Children = program.Children;
                this.OrderItemFulfillment = program.OrderItemFulfillment;
                this.WorkEffortType = program.WorkEffortType;
                this.InventoryItemsProduced = program.InventoryItemsProduced;
                this.RequirementFulfillments = program.RequirementFulfillments;
                this.SpecialTerms = program.SpecialTerms;
                this.Concurrencies = program.Concurrencies;
                this.CurrentObjectState = program.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}