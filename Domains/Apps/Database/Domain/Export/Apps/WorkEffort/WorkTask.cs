// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkTask.cs" company="Allors bvba">
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
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Meta;

    public partial class WorkTask
    {
        public new string ToString() => this.WorkEffortNumber ?? this.Name;

        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.WorkTask, M.WorkTask.WorkEffortState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var internalOrganisations = new Organisations(this.strategy.Session).Extent()
                .Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistTakenBy && internalOrganisations.Count() == 1)
            {
                this.TakenBy = internalOrganisations.First();
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            if (!this.ExistWorkEffortNumber && this.ExistTakenBy)
            {
                this.WorkEffortNumber = this.TakenBy.NextWorkEffortNumber();
            }

            var model = new Dictionary<string, object> { { "workTask", this } };

            var timeEntries = new List<TimeEntry>();

            foreach (ServiceEntry serviceEntry in this.ServiceEntriesWhereWorkEffort)
            {
                if (serviceEntry is TimeEntry timeEntry)
                {
                    timeEntries.Add(timeEntry);
                }
            }

            if (timeEntries.Count() > 0)
            {
                model.Add("timeEntries", timeEntries.ToArray());
            }

            var inventoryAssignments = this.WorkEffortInventoryAssignmentsWhereAssignment.ToArray();
            if (inventoryAssignments.Count() > 0)
            {
                model.Add("inventory", inventoryAssignments);
            }

            this.RenderPrintDocument(this.TakenBy?.WorkTaskTemplate, model);
        }

        //public void AppsDelete(DeletableDelete method)
        //{
        //    foreach (WorkEffortStatus workEffortStatus in this.WorkEffortStatuses)
        //    {
        //        workEffortStatus.Delete();
        //    }

        //    foreach (WorkEffortAssignment workEffortAssignment in this.WorkEffortAssignmentsWhereAssignment)
        //    {
        //        workEffortAssignment.Delete();
        //    }
        //}
    }
}
