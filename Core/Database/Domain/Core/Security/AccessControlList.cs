// <copyright file="AccessControlList.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Allors;
    using Allors.Meta;

    /// <summary>
    /// List of permissions for an object/user combination.
    /// </summary>
    public class AccessControlList : IAccessControlList
    {
        private readonly Guid classId;

        private bool lazyLoaded;

        private AccessControl[] accessControls;
        private HashSet<long> deniedPermissions;
        private PermissionCache permissionCache;
        private Dictionary<Guid, Dictionary<Operations, long>> permissionIdByOperationByOperandTypeId;

        internal AccessControlList(IAccessControlListFactory accessControlListFactory, IObject @object)
        {
            this.AccessControlListFactory = accessControlListFactory;
            this.Object = @object as Object;
            this.classId = this.Object.Strategy.Class.Id;

            this.lazyLoaded = false;
        }

        public Object Object { get; }

        public IAccessControlListFactory AccessControlListFactory { get; }

        public IEnumerable<AccessControl> AccessControls
        {
            get
            {
                this.LazyLoad();
                return this.accessControls;
            }
        }

        public IEnumerable<long> DeniedPermissionIds
        {
            get
            {
                this.LazyLoad();
                return this.deniedPermissions;
            }
        }

        public bool CanRead(IPropertyType propertyType) => this.IsPermitted(propertyType, Operations.Read);

        public bool CanRead(IConcreteRoleType roleType) => this.IsPermitted(roleType.RoleType, Operations.Read);

        public bool CanWrite(IRoleType roleType) => this.IsPermitted(roleType, Operations.Write);

        public bool CanWrite(IConcreteRoleType roleType) => this.IsPermitted(roleType.RoleType, Operations.Write);

        public bool CanExecute(IMethodType methodType) => this.IsPermitted(methodType, Operations.Execute);

        public bool IsPermitted(IOperandType operandType, Operations operation)
        {
            if (this.Object == null)
            {
                return operation == Operations.Read;
            }

            this.LazyLoad();

            if (this.permissionIdByOperationByOperandTypeId.TryGetValue(operandType.Id, out var permissionIdByOperation))
            {
                if (permissionIdByOperation.TryGetValue(operation, out var permissionId))
                {
                    if (this.deniedPermissions?.Contains(permissionId) == true)
                    {
                        return false;
                    }

                    return this.accessControls.Any(v => this.AccessControlListFactory.EffectivePermissionIdsByAccessControl[v].Contains(permissionId));
                }
            }

            return false;
        }

        private void LazyLoad()
        {
            if (!this.lazyLoaded)
            {
                var strategy = this.Object.Strategy;
                var session = strategy.Session;

                SecurityToken[] securityTokens;
                if (this.Object is DelegatedAccessControlledObject controlledObject)
                {
                    var delegatedAccess = controlledObject.DelegateAccess();
                    securityTokens = delegatedAccess.SecurityTokens;

                    var delegatedAccessDeniedPermissions = delegatedAccess.DeniedPermissions;
                    if (delegatedAccessDeniedPermissions != null && delegatedAccessDeniedPermissions.Length > 0)
                    {
                        this.deniedPermissions = this.Object.DeniedPermissions.Count > 0 ?
                                                     new HashSet<long>(this.Object.DeniedPermissions.Union(delegatedAccessDeniedPermissions).Select(v => v.Id)) :
                                                     new HashSet<long>(delegatedAccessDeniedPermissions.Select(v => v.Id));
                    }
                    else if (this.Object.DeniedPermissions.Count > 0)
                    {
                        this.deniedPermissions = new HashSet<long>(this.Object.DeniedPermissions.Select(v => v.Id));
                    }
                }
                else
                {
                    securityTokens = this.Object.SecurityTokens;

                    if (this.Object.DeniedPermissions.Count > 0)
                    {
                        this.deniedPermissions = new HashSet<long>(this.Object.DeniedPermissions.Select(v => v.Id));
                    }
                }

                if (securityTokens == null || securityTokens.Length == 0)
                {
                    var singleton = session.GetSingleton();
                    securityTokens = strategy.IsNewInSession
                                          ? new[] { singleton.InitialSecurityToken ?? singleton.DefaultSecurityToken }
                                          : new[] { singleton.DefaultSecurityToken };
                }

                this.accessControls = securityTokens.SelectMany(v => v.AccessControls)
                    .Distinct()
                    .Where(this.AccessControlListFactory.EffectivePermissionIdsByAccessControl.ContainsKey)
                    .ToArray();

                this.permissionCache = session.GetCache<PermissionCache, PermissionCache>(() => new PermissionCache(session));
                this.permissionIdByOperationByOperandTypeId = this.permissionCache.PermissionIdByOperationByOperandTypeIdByClassId[this.classId];

                this.lazyLoaded = true;
            }
        }
    }
}
