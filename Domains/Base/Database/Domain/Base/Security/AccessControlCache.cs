// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccessControlCache.cs" company="Allors bvba">
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
        internal AccessControlCache(AccessControl accessControl)
        {
            this.CacheId = accessControl.CacheId;

            this.EffectiveUserIds = new HashSet<long>(accessControl.EffectiveUsers.Select(v => v.Id));

            this.OperationsByOperandTypeIdByClassId = new Dictionary<Guid, Dictionary<Guid, Operations>>();
            foreach (Permission permission in accessControl.EffectivePermissions)
            {
                var classId = permission.ConcreteClassPointer;
                if (!this.OperationsByOperandTypeIdByClassId.TryGetValue(classId, out var operationsByOperandType))
                {
                    operationsByOperandType = new Dictionary<Guid, Operations>();
                    this.OperationsByOperandTypeIdByClassId.Add(classId, operationsByOperandType);
                }

                var operandTypeId = permission.OperandTypePointer;
                var operation = permission.Operation;

                operationsByOperandType.TryGetValue(operandTypeId, out var operations);
                operations = operations | operation;

                operationsByOperandType[operandTypeId] = operations;
            }
        }

        public Guid CacheId { get; }

        public HashSet<long> EffectiveUserIds { get; }

        public Dictionary<Guid, Dictionary<Guid, Operations>> OperationsByOperandTypeIdByClassId { get; }
    }
}