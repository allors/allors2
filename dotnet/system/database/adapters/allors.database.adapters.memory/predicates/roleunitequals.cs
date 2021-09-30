// <copyright file="RoleUnitEquals.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using System;
    using Adapters;
    using Meta;

    internal sealed class RoleUnitEquals : Predicate
    {
        private readonly ExtentFiltered extent;
        private readonly IRoleType roleType;
        private readonly object equals;

        internal RoleUnitEquals(ExtentFiltered extent, IRoleType roleType, object equals)
        {
            extent.CheckForRoleType(roleType);
            PredicateAssertions.ValidateRoleEquals(roleType, equals);

            this.extent = extent;
            this.roleType = roleType;
            if (equals is Enum)
            {
                if (roleType.ObjectType is IUnit unitType && unitType.IsInteger)
                {
                    this.equals = (int)equals;
                }
                else
                {
                    throw new Exception("Role Object Type " + roleType.ObjectType.SingularName + " doesn't support enumerations.");
                }
            }
            else
            {
                this.equals = equals;
            }
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            var value = strategy.GetInternalizedUnitRole(this.roleType);

            if (value == null)
            {
                return ThreeValuedLogic.Unknown;
            }

            var equalsValue = this.equals;

            if (this.equals is IRoleType)
            {
                var equalsRole = (IRoleType)this.equals;
                equalsValue = strategy.GetInternalizedUnitRole(equalsRole);
            }
            else if (this.roleType.ObjectType is IUnit)
            {
                equalsValue = this.roleType.Normalize(this.@equals);
            }

            if (equalsValue == null)
            {
                return ThreeValuedLogic.False;
            }

            return value.Equals(equalsValue)
                       ? ThreeValuedLogic.True
                       : ThreeValuedLogic.False;
        }
    }
}
