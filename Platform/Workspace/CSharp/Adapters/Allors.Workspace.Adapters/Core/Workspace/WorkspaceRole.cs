// <copyright file="WorkspaceRole.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using System.Linq;
    using Meta;
    using Protocol.Data;
    using Protocol.Remote.Sync;

    public class WorkspaceRole : IWorkspaceRole
    {
        internal WorkspaceRole(SyncResponseContext ctx, SyncResponseRole syncResponseRole)
        {
            var value = syncResponseRole.V;
            this.RoleType = ctx.ReadRoleType(syncResponseRole);

            var objectType = this.RoleType.ObjectType;
            if (objectType.IsUnit)
            {
                this.Value = UnitConvert.Parse(this.RoleType.ObjectType.Id, value);
            }
            else
            {
                if (this.RoleType.IsOne)
                {
                    this.Value = value != null ? long.Parse(value) : (long?)null;
                }
                else
                {
                    this.Value = value != null
                        ? value.Split(',').Select(long.Parse).ToArray()
                        : Array.Empty<long>();
                }
            }
        }

        public IRoleType RoleType { get; }

        public object Value { get; }
    }
}
