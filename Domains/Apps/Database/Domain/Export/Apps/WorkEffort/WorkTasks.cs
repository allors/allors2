// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkTasks.cs" company="Allors bvba">
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
    using Meta;

    public partial class WorkTasks
    {
        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var created = new WorkEffortStates(this.Session).Created;
            var inProgress = new WorkEffortStates(this.Session).InProgress;
            var cancelled = new WorkEffortStates(this.Session).Cancelled;
            var completed = new WorkEffortStates(this.Session).Completed;
            var finished = new WorkEffortStates(this.Session).Finished;

            var cancel = this.Meta.Cancel;
            var reopen = this.Meta.Reopen;
            var complete = this.Meta.Complete;
            var invoice = this.Meta.Invoice;

            config.Deny(this.ObjectType, created, reopen, complete, invoice);
            config.Deny(this.ObjectType, inProgress, reopen);
            config.Deny(this.ObjectType, cancelled, cancel, invoice, complete);
            config.Deny(this.ObjectType, completed, reopen, complete);

            config.Deny(this.ObjectType, cancelled, Operations.Write);
            config.Deny(this.ObjectType, completed, Operations.Write);
            config.Deny(this.ObjectType, finished, Operations.Execute, Operations.Write);

            config.Deny(M.TimeEntry, cancelled, Operations.Write);
            config.Deny(M.TimeEntry, finished, Operations.Write);
            config.Deny(M.TimeEntry, completed, Operations.Write);
            config.Deny(M.WorkEffortAssignmentRate, cancelled, Operations.Write);
            config.Deny(M.WorkEffortAssignmentRate, finished, Operations.Write);
            config.Deny(M.WorkEffortAssignmentRate, completed, Operations.Write);
            config.Deny(M.WorkEffortInventoryAssignment, cancelled, Operations.Write);
            config.Deny(M.WorkEffortInventoryAssignment, finished, Operations.Write);
            config.Deny(M.WorkEffortInventoryAssignment, completed, Operations.Write);
            config.Deny(M.WorkEffortPartyAssignment, cancelled, Operations.Write);
            config.Deny(M.WorkEffortPartyAssignment, finished, Operations.Write);
            config.Deny(M.WorkEffortPartyAssignment, completed, Operations.Write);
            config.Deny(M.WorkEffortPurchaseOrderItemAssignment, cancelled, Operations.Write);
            config.Deny(M.WorkEffortPurchaseOrderItemAssignment, finished, Operations.Write);
            config.Deny(M.WorkEffortPurchaseOrderItemAssignment, completed, Operations.Write);
            config.Deny(M.WorkEffortFixedAssetAssignment, cancelled, Operations.Write);
            config.Deny(M.WorkEffortFixedAssetAssignment, finished, Operations.Write);
            config.Deny(M.WorkEffortFixedAssetAssignment, completed, Operations.Write);
        }
    }
}