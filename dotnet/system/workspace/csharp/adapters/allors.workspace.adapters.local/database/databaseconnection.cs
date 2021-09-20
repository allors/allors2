// <copyright file="v.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Local
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Database;
    using Database.Security;
    using Database.Services;
    using Meta;
    using Ranges;
    using AccessControl = AccessControl;
    using IRoleType = Database.Meta.IRoleType;

    public class DatabaseConnection : Adapters.DatabaseConnection
    {
        private readonly Dictionary<long, AccessControl> accessControlById;
        private readonly IPermissionsCache permissionCache;
        private readonly ConcurrentDictionary<long, DatabaseRecord> recordsById;

        private readonly Func<IWorkspaceServices> servicesBuilder;
        private readonly IRanges<long> recordRanges;

        public DatabaseConnection(Configuration configuration, IDatabase database, Func<IWorkspaceServices> servicesBuilder, Func<IRanges<long>> rangesFactory) : base(configuration, new IdGenerator())
        {
            this.Database = database;
            this.servicesBuilder = servicesBuilder;
            this.recordRanges = rangesFactory();

            this.recordsById = new ConcurrentDictionary<long, DatabaseRecord>();
            this.permissionCache = this.Database.Services.Get<IPermissionsCache>();
            this.accessControlById = new Dictionary<long, AccessControl>();
        }

        public long UserId { get; set; }

        internal IDatabase Database { get; }

        internal void Sync(IEnumerable<IObject> objects, IAccessControl accessControl)
        {
            foreach (var @object in objects)
            {
                var id = @object.Id;
                var databaseClass = @object.Strategy.Class;
                var roleTypes = databaseClass.DatabaseRoleTypes.Where(w => w.RelationType.WorkspaceNames.Length > 0);

                var workspaceClass = (IClass)this.Configuration.MetaPopulation.FindByTag(databaseClass.Tag);
                var roleByRoleType = roleTypes.ToDictionary(w => ((IRelationType)this.Configuration.MetaPopulation.FindByTag(w.RelationType.Tag)).RoleType, w => this.GetRole(@object, w));

                var acl = accessControl[@object];

                var accessControls = acl.Grants
                    ?.Select(this.GetAccessControl)
                    .ToArray() ?? Array.Empty<AccessControl>();

                this.recordsById[id] = new DatabaseRecord(workspaceClass, id, @object.Strategy.ObjectVersion, roleByRoleType, this.recordRanges.Load(acl.Revocations.Select(v => v.Strategy.ObjectId)), accessControls);
            }
        }

        public override IWorkspace CreateWorkspace() => new Workspace(this, this.servicesBuilder(), this.recordRanges);

        public override Adapters.DatabaseRecord GetRecord(long id)
        {
            this.recordsById.TryGetValue(id, out var databaseObjects);
            return databaseObjects;
        }

        public override long GetPermission(IClass @class, IOperandType operandType, Operations operation)
        {
            var classId = this.Database.MetaPopulation.FindByTag(@class.Tag).Id;
            var operandId = this.Database.MetaPopulation.FindByTag(operandType.OperandTag).Id;

            long permission;
            var permissionCacheEntry = this.permissionCache.Get(classId);

            switch (operation)
            {
                case Operations.Read:
                    permissionCacheEntry.RoleReadPermissionIdByRelationTypeId.TryGetValue(operandId, out permission);
                    break;
                case Operations.Write:
                    permissionCacheEntry.RoleWritePermissionIdByRelationTypeId.TryGetValue(operandId, out permission);
                    break;
                case Operations.Execute:
                    permissionCacheEntry.MethodExecutePermissionIdByMethodTypeId.TryGetValue(operandId, out permission);
                    break;
                case Operations.Create:
                    throw new NotSupportedException("Create is not supported");
                default:
                    throw new ArgumentOutOfRangeException($"Unknown operation {operation}");
            }

            return permission;
        }

        internal IEnumerable<IObject> ObjectsToSync(Pull pull) =>
            pull.DatabaseObjects.Where(v =>
            {
                if (this.recordsById.TryGetValue(v.Id, out var databaseRoles))
                {
                    return v.Strategy.ObjectVersion != databaseRoles.Version;
                }

                return true;
            });

        private AccessControl GetAccessControl(IGrant grant)
        {
            if (!this.accessControlById.TryGetValue(grant.Strategy.ObjectId, out var acessControl))
            {
                acessControl = new AccessControl();
                this.accessControlById.Add(grant.Strategy.ObjectId, acessControl);
            }

            if (acessControl.Version == grant.Strategy.ObjectVersion)
            {
                return acessControl;
            }

            acessControl.Version = grant.Strategy.ObjectVersion;
            acessControl.PermissionIds = this.recordRanges.Import(grant.Permissions.Select(v => v.Id));

            return acessControl;
        }

        private object GetRole(IObject @object, IRoleType roleType)
        {
            if (roleType.ObjectType.IsUnit)
            {
                return @object.Strategy.GetUnitRole(roleType);
            }

            if (roleType.IsOne)
            {
                return @object.Strategy.GetCompositeRole(roleType)?.Id;
            }

            return this.recordRanges.Load(@object.Strategy.GetCompositesRole<IObject>(roleType).Select(v => v.Id));
        }
    }
}
