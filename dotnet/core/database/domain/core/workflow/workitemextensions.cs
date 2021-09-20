// <copyright file="WorkItemExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;

    public static partial class WorkItemExtensions
    {
        public static void CoreOnPreDerive(this WorkItem @this, ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(@this) || changeSet.IsCreated(@this) || changeSet.HasChangedRoles(@this))
            {
                foreach (var task in @this.TasksWhereWorkItem.Where(v => !v.ExistDateClosed))
                {
                    iteration.AddDependency(task, @this);
                    iteration.Mark(task);
                }
            }
        }
    }
}
