// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccessControlList.cs" company="Allors bvba">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Allors;
    using Allors.Meta;

    /// <summary>
    /// List of permissions for an object/user combination.
    /// </summary>
    public class AccessControlList : IAccessControlList
    {
        private readonly AccessControlledObject @object;
        private readonly User user;
        private readonly ISession session;

        private readonly Guid classId;

        private IList<Dictionary<Guid, Operations>> operationsByOperandTypeIds;
        private Permission[] deniedPermissions;

        private bool lazyLoaded;

        public AccessControlList(IObject obj, User user)
        {
            this.user = user;
            this.session = this.user.Strategy.Session;
            this.@object = (AccessControlledObject)obj;
            this.classId = obj.Strategy.Class.Id;

            this.lazyLoaded = false;
        }

        public User User => this.user;

        public bool CanRead(PropertyType propertyType)
        {
            return this.IsPermitted(propertyType, Operations.Read);
        }

        public bool CanWrite(RoleType roleType)
        {
            return this.IsPermitted(roleType, Operations.Write);
        }

        public bool CanExecute(MethodType methodType)
        {
            return this.IsPermitted(methodType, Operations.Execute);
        }
        
        public bool IsPermitted(OperandType operandType, Operations operation)
        {
            return this.IsPermitted(operandType.Id, operation);
        }

        private bool IsPermitted(Guid operandTypeId, Operations operation)
        {
            this.LazyLoad();

            if (this.deniedPermissions.Length > 0)
            {
                if (this.deniedPermissions.Any(deniedPermission => deniedPermission.OperandTypePointer.Equals(operandTypeId) && deniedPermission.Operation.Equals(operation)))
                {
                    return false;
                }
            }

            if (this.operationsByOperandTypeIds != null)
            {
                foreach (var operationsByOperandTypeId in this.operationsByOperandTypeIds)
                {
                    Operations operations;
                    if (operationsByOperandTypeId.TryGetValue(operandTypeId, out operations))
                    {
                        if ((operations & operation) == operation)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
         }

        private void LazyLoad()
        {
            if (!this.lazyLoaded)
            {
                this.deniedPermissions = this.@object.DeniedPermissions.ToArray();

                SecurityToken[] securityTokens = this.@object.SecurityTokens;
                if (securityTokens.Length == 0)
                {
                    var singleton = Singleton.Instance(this.session);
                    securityTokens = this.@object.Strategy.IsNewInSession ? 
                        new[] { singleton.InitialSecurityToken ?? singleton.DefaultSecurityToken } : 
                        new[] { singleton.DefaultSecurityToken };
                }

                var caches = securityTokens.SelectMany(v => v.AccessControls).Select(v=>v.Cache).Where(v => v.EffectiveUserIds.Contains(this.user.Id));
                foreach (var cache in caches)
                {
                    var operationsByOperandTypeIdByClassId = cache.OperationsByOperandTypeIdByClassId;

                    Dictionary<Guid, Operations> operationsByClassId;
                    if (operationsByOperandTypeIdByClassId.TryGetValue(this.classId, out operationsByClassId))
                    {
                        if (this.operationsByOperandTypeIds == null)
                        {
                            this.operationsByOperandTypeIds = new List<Dictionary<Guid, Operations>>();
                        }

                        this.operationsByOperandTypeIds.Add(operationsByClassId);
                    }
                }

                this.lazyLoaded = true;
            }
        }
    }
}