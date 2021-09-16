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

        private AccessControl[] accessControls;
        private Restriction[] restrictions;

        private bool lazyLoaded;
        private PermissionCache permissionCache;
        private Dictionary<Guid, Dictionary<Operations, long>> permissionIdByOperationByOperandTypeId;

        internal AccessControlList(IAccessControlLists accessControlLists, IObject @object)
        {
            this.AccessControlLists = accessControlLists;
            this.Object = (Object)@object;
            this.classId = this.Object.Strategy.Class.Id;

            this.lazyLoaded = false;
        }

        public AccessControl[] AccessControls
        {
            get
            {
                this.LazyLoad();
                return this.accessControls;
            }
        }

        public Restriction[] Restrictions
        {
            get
            {
                this.LazyLoad();
                return this.restrictions;
            }
        }

        public Object Object { get; }

        private IAccessControlLists AccessControlLists { get; }

        public bool CanExecute(IMethodType methodType) => this.IsPermitted(methodType, Operations.Execute);

        public bool CanRead(IPropertyType propertyType) => this.IsPermitted(propertyType, Operations.Read);

        public bool CanRead(IConcreteRoleType roleType) => this.IsPermitted(roleType.RoleType, Operations.Read);

        public bool CanWrite(IRoleType roleType) => this.IsPermitted(roleType, Operations.Write);

        public bool CanWrite(IConcreteRoleType roleType) => this.IsPermitted(roleType.RoleType, Operations.Write);

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
                    // TODO: Optimize
                    if (this.restrictions?.Any(v => v.DeniedPermissions.Select(w => w.Id).Contains(permissionId)) == true)
                    {
                        return false;
                    }

                    return this.accessControls.Any(v => this.AccessControlLists.EffectivePermissionIdsByAccessControl[v].Contains(permissionId));
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
                    if (securityTokens != null && securityTokens.Any(v => v == null))
                    {
                        securityTokens = securityTokens.Where(v => v != null).ToArray();
                    }

                    var delegatedAccessRestrictions = delegatedAccess.Restrictions;
                    if (delegatedAccessRestrictions != null && delegatedAccessRestrictions.Length > 0)
                    {
                        this.restrictions = this.Object.Restrictions.Count > 0 ?
                                                     this.Object.Restrictions.Union(delegatedAccessRestrictions).ToArray() :
                                                     delegatedAccessRestrictions;
                    }
                    else if (this.Object.Restrictions.Count > 0)
                    {
                        this.restrictions = this.Object.Restrictions;
                    }
                }
                else
                {
                    securityTokens = this.Object.SecurityTokens;

                    if (this.Object.Restrictions.Count > 0)
                    {
                        this.restrictions = this.Object.Restrictions;
                    }
                }

                if (securityTokens == null || securityTokens.Length == 0)
                {
                    var tokens = new SecurityTokens(session);
                    securityTokens = strategy.IsNewInSession
                                          ? new[] { tokens.InitialSecurityToken ?? tokens.DefaultSecurityToken }
                                          : new[] { tokens.DefaultSecurityToken };
                }

                this.accessControls = securityTokens.SelectMany(v => v.AccessControls)
                    .Distinct()
                    .Where(this.AccessControlLists.EffectivePermissionIdsByAccessControl.ContainsKey)
                    .ToArray();

                this.permissionCache = session.GetCache<PermissionCache, PermissionCache>(() => new PermissionCache(session));
                this.permissionIdByOperationByOperandTypeId = this.permissionCache.PermissionIdByOperationByOperandTypeIdByClassId[this.classId];

                this.lazyLoaded = true;
            }
        }
    }
}
