// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Task.cs" company="Allors bvba">
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
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    public partial class WorkTask
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.WorkTask, M.WorkTask.WorkEffortState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var internalOrganisations = new Organisations(this.strategy.Session).Extent()
                .Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistTakenBy && internalOrganisations.Count() == 1)
            {
                this.TakenBy = internalOrganisations.First();
            }

            if (!this.ExistStore && this.ExistTakenBy)
            {
                var store = new Stores(this.strategy.Session).Extent()
                    .FirstOrDefault(v => Equals(v.InternalOrganisation, this.TakenBy));
                if (store != null)
                {
                    this.Store = store;
                }
            }

            if (!this.ExistWorkEffortNumber && this.ExistStore)
            {
                this.WorkEffortNumber = this.Store.DeriveNextWorkEffortNumber();
            }

            var model = new Dictionary<string, object> { { "workTask", this } };
            this.RenderPrintDocument(this.TakenBy?.SalesInvoiceTemplate, model);
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
