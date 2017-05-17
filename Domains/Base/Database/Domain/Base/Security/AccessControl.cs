// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccessControl.cs" company="Allors bvba">
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

    public class AccessControlCache
    {
        public long ObjectVersion { get; }

        public HashSet<long> EffectiveUserIds { get; }

        public Dictionary<Guid, Dictionary<Guid, Operations>> OperationsByOperandTypeIdByClassId { get; }

        internal AccessControlCache(AccessControl accessControl)
        {
            this.ObjectVersion = accessControl.Strategy.ObjectVersion;

            this.EffectiveUserIds = new HashSet<long>(accessControl.EffectiveUsers.Select(v => v.Id));

            this.OperationsByOperandTypeIdByClassId = new Dictionary<Guid, Dictionary<Guid, Operations>>();
            foreach (Permission permission in accessControl.EffectivePermissions)
            {
                var classId = permission.ConcreteClassPointer;
                Dictionary<Guid, Operations> operationsByOperandType;
                if (!OperationsByOperandTypeIdByClassId.TryGetValue(classId, out operationsByOperandType))
                {
                    operationsByOperandType = new Dictionary<Guid, Operations>();
                    OperationsByOperandTypeIdByClassId.Add(classId, operationsByOperandType);
                }

                var operandTypeId = permission.OperandTypePointer;
                var operation = permission.Operation;

                Operations operations;
                operationsByOperandType.TryGetValue(operandTypeId, out operations);
                operations = operations | operation;

                operationsByOperandType[operandTypeId] = operations;
            }
        }
    }

    public partial class AccessControl
    {
        private string CacheKey => $"{nameof(AccessControl)}[{this.Id}].Cache";

        public AccessControlCache Cache
        {
            get
            {
                var database = this.strategy.Session.Database;
                var key = this.CacheKey;
                var cache = (AccessControlCache)database[key];
                if (cache == null || !this.strategy.ObjectVersion.Equals(cache.ObjectVersion))
                {
                    cache = new AccessControlCache(this);
                    database[key] = cache;
                }

                return cache;
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var database = this.strategy.Session.Database;
            database[this.CacheKey] = null;

            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(this, Meta.Subjects, Meta.SubjectGroups);

            this.EffectiveUsers = this.SubjectGroups.SelectMany(v => v.Members).Union(this.Subjects).ToArray();
            this.EffectivePermissions = this.Role?.Permissions;
        }

        public void WarmUp()
        {
            var database = this.strategy.Session.Database;
            database[this.CacheKey] = new AccessControlCache(this);
        }
    }
}