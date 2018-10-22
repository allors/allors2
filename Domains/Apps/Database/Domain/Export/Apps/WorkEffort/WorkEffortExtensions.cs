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
                foreach (WorkEffortPartyAssignment partyAssignment in @this.WorkEffortPartyAssignmentsWhereAssignment)
                {
                    derivation.AddDependency(partyAssignment, @this);
                }

                foreach (WorkEffortInventoryAssignment inventoryAssignment in @this.WorkEffortInventoryAssignmentsWhereAssignment)
                {
                    derivation.AddDependency(inventoryAssignment, @this);
                }
            }
        }

        public static void AppsOnDerive(this WorkEffort @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!@this.ExistOwner)
            {
                if (!(@this.Strategy.Session.GetUser() is Person owner))
                {
                    owner = @this.Strategy.Session.GetSingleton().Guest as Person;
                }

                @this.Owner = owner;
            }

            @this.DeriveOwnerSecurity();
        }

        public static void AppsOnBuild(this WorkEffort @this, ObjectOnBuild method)
        {
            if (!@this.ExistWorkEffortState)
            {
                @this.WorkEffortState = new WorkEffortStates(@this.Strategy.Session).NeedsAction;
            }
        }

        public static void AppsConfirm(this WorkEffort @this, WorkEffortConfirm method)
        {
            @this.WorkEffortState = new WorkEffortStates(@this.Strategy.Session).Confirmed;
        }

        public static void AppsComplete(this WorkEffort @this, WorkEffortComplete method)
        {
            @this.WorkEffortState = new WorkEffortStates(@this.Strategy.Session).Completed;
        }

        public static void AppsCancel(this WorkEffort @this, WorkEffortCancel cancel)
        {
            @this.WorkEffortState = new WorkEffortStates(@this.Strategy.Session).Cancelled;
        }

        public static void AppsReopen(this WorkEffort @this, WorkEffortReopen reopen)
        {
            @this.WorkEffortState = new WorkEffortStates(@this.Strategy.Session).NeedsAction;
        }

        private static void DeriveOwnerSecurity(this WorkEffort @this)
        {
            if (!@this.ExistOwnerAccessControl)
            {
                var ownerRole = new Roles(@this.Strategy.Session).Owner;
                @this.OwnerAccessControl = new AccessControlBuilder(@this.Strategy.Session)
                    .WithRole(ownerRole)
                    .WithSubject(@this.Owner)
                    .Build();
            }

            if (!@this.ExistOwnerSecurityToken)
            {
                @this.OwnerSecurityToken = new SecurityTokenBuilder(@this.Strategy.Session)
                    .WithAccessControl(@this.OwnerAccessControl)
                    .Build();
            }
        }
    }
}
