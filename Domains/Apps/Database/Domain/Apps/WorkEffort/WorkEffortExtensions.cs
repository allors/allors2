// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkEffortExtensions.cs" company="Allors bvba">
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
    public static partial class WorkEffortExtensions
    {
        public static void AppsOnPreDerive(this WorkEffort @this, ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (derivation.ChangeSet.Associations.Contains(@this.Id))
            {
                if (@this.ExistWorkEffortAssignmentsWhereAssignment)
                {
                    foreach (WorkEffortAssignment workEffortAssignment in @this.WorkEffortAssignmentsWhereAssignment)
                    {
                        derivation.AddDependency(workEffortAssignment, @this);
                    }
                }
            }
        }

        public static void AppsOnBuild(this WorkEffort @this, ObjectOnBuild method)
        {
            if (!@this.ExistCurrentObjectState)
            {
                @this.CurrentObjectState = new WorkEffortObjectStates(@this.Strategy.Session).NeedsAction;
            }
        }

        public static void AppsConfirm(this WorkEffort @this, WorkEffortConfirm method)
        {
            @this.CurrentObjectState = new WorkEffortObjectStates(@this.Strategy.Session).Confirmed;
        }

        public static void AppsFinish(this WorkEffort @this, WorkEffortFinish method)
        {
            @this.CurrentObjectState = new WorkEffortObjectStates(@this.Strategy.Session).Completed;
        }

        public static void AppsCancel(this WorkEffort @this, WorkEffortCancel cancel)
        {
            @this.CurrentObjectState = new WorkEffortObjectStates(@this.Strategy.Session).Cancelled;
        }

        public static void AppsReopen(this WorkEffort @this, WorkEffortReopen reopen)
        {
            @this.CurrentObjectState = new WorkEffortObjectStates(@this.Strategy.Session).NeedsAction;
        }
    }
}
