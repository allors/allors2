// <copyright file="WorkspaceRecord.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using System.Collections.Generic;
    using Meta;

    public class WorkspaceRecord : IRecord
    {
        private readonly IClass @class;
        private readonly long id;
        private readonly IReadOnlyDictionary<IRelationType, object> roleByRelationType;

        public WorkspaceRecord(IClass @class, long id, long version, IReadOnlyDictionary<IRelationType, object> roleByRelationType)
        {
            this.id = id;
            this.@class = @class;
            this.Version = version;
            this.roleByRelationType = roleByRelationType;
        }

        public WorkspaceRecord(WorkspaceRecord originalRecord, IReadOnlyDictionary<IRelationType, object> changedRoleByRoleType)
        {
            this.id = originalRecord.id;
            this.@class = originalRecord.@class;
            this.Version = ++originalRecord.Version;

            var dictionary = new Dictionary<IRelationType, object>();
            foreach (var roleType in this.@class.WorkspaceOriginRoleTypes)
            {
                var relationType = roleType.RelationType;

                if (changedRoleByRoleType != null && changedRoleByRoleType.TryGetValue(relationType, out var role))
                {
                    if (role != null)
                    {
                        dictionary.Add(relationType, role);
                    }
                }
                else if (originalRecord.roleByRelationType != null && originalRecord.roleByRelationType.TryGetValue(roleType.RelationType, out role))
                {
                    dictionary.Add(relationType, role);
                }
            }

            this.roleByRelationType = dictionary;
        }

        public long Version { get; private set; }

        public object GetRole(IRoleType roleType)
        {
            object @object = null;
            this.roleByRelationType?.TryGetValue(roleType.RelationType, out @object);
            return @object;
        }
    }
}
