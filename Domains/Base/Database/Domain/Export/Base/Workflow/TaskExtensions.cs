// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskExtensions.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    public static partial class TaskExtensions
    {
        public static void BaseOnBuild(this Task @this, ObjectOnBuild method)
        {
            if (!@this.ExistDateCreated)
            {
                @this.DateCreated = @this.Strategy.Session.Now();
            }
        }

        public static void BaseOnPreDerive(this Task @this, ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            foreach (TaskAssignment taskAssignment in @this.TaskAssignmentsWhereTask)
            {
                derivation.AddDependency(taskAssignment, @this);
            }
        }

        public static void BaseDelete(this Task @this, DeletableDelete method)
        {
            foreach (TaskAssignment taskAssignment in @this.TaskAssignmentsWhereTask)
            {
                taskAssignment.Delete();
            }
        }

        public static void AssignPerformer(this Task @this)
        {
            var currentUser = @this.Strategy.Session.GetUser() as Person;
            @this.Performer = currentUser;
        }

        public static void AssignParticipants(this Task @this, IEnumerable<Person> participants)
        {
            var session = @this.Strategy.Session;

            var participantSet = new HashSet<User>(participants.Where(v => v != null).Distinct());

            @this.Participants = participantSet.ToArray();

            // Manage Security
            var singleton = session.GetSingleton();
            var securityTokens = new HashSet<SecurityToken> {singleton.DefaultSecurityToken};
            var ownerSecurityTokens = participantSet.Where(v => v.ExistOwnerSecurityToken).Select(v => v.OwnerSecurityToken);
            securityTokens.UnionWith(ownerSecurityTokens);
            @this.SecurityTokens = securityTokens.ToArray();

            // Manage TaskAssignments
            foreach (var currentTaskAssignement in @this.TaskAssignmentsWhereTask.ToArray())
            {
                var user = currentTaskAssignement.User;
                if (!participantSet.Contains(user))
                {
                    currentTaskAssignement.Delete();
                }
                else
                {
                    participantSet.Remove(user);
                }
            }

            foreach (var user in participantSet)
            {
                new TaskAssignmentBuilder(session)
                    .WithTask(@this)
                    .WithUser(user)
                    .Build();
            }
        }
    }
}
