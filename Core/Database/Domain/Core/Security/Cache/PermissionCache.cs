// <copyright file="PermissionCache.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Meta;

    public class PermissionCache
    {
        private static readonly PrefetchPolicy PrefetchPolicy;

        static PermissionCache() =>
            PrefetchPolicy = new PrefetchPolicyBuilder()
                .WithRule(M.Permission.ConcreteClassPointer)
                .Build();

        public PermissionCache(ISession session)
        {
            var permissions = new Permissions(session).Extent();

            session.Prefetch(PrefetchPolicy, permissions);

            this.PermissionIdByOperationByOperandTypeIdByClassId = permissions
                .GroupBy(v => v.ConcreteClass.Id)
                .ToDictionary(
                    v => v.Key,
                    w => w.GroupBy(v => v.OperandType.Id).ToDictionary(v => v.Key, x =>
                        x.ToDictionary(v => v.Operation, y => y.Id)));

            this.SortedWorkspacePermissionIdsByClassId = permissions
                .Where(v => (v.OperandType as RoleType)?.Workspace == true || (v.OperandType as MethodType)?.Workspace == true)
                .GroupBy(v => v.ConcreteClass.Id)
                .ToDictionary(
                    v => v.Key,
                    w => w.Select(x => x.Id).OrderBy(x => x).ToArray());
        }

        public Dictionary<Guid, Dictionary<Guid, Dictionary<Operations, long>>> PermissionIdByOperationByOperandTypeIdByClassId { get; }

        public Dictionary<Guid, long[]> SortedWorkspacePermissionIdsByClassId { get; }
    }
}
