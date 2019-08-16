
// <copyright file="RoleBetween.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Memory
{
    using System;
    using Allors.Meta;
    using Adapters;

    internal sealed class RoleBetween : Predicate
    {
        private readonly ExtentFiltered extent;
        private readonly IRoleType roleType;
        private readonly object first;
        private readonly object second;

        internal RoleBetween(ExtentFiltered extent, IRoleType roleType, object first, object second)
        {
            extent.CheckForRoleType(roleType);
            PredicateAssertions.ValidateRoleBetween(roleType, first, second);

            this.extent = extent;
            this.roleType = roleType;

            this.first = first;
            this.second = second;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            var firstValue = this.first;
            var secondValue = this.second;

            var firstRole = this.first as IRoleType;
            if (firstRole != null)
            {
                firstValue = strategy.GetInternalizedUnitRole(firstRole);
            }
            else
            {
                if (this.roleType.ObjectType is IUnit)
                {
                    firstValue = this.roleType.Normalize(this.first);
                }
            }

            var secondRole = this.second as IRoleType;
            if (secondRole != null)
            {
                secondValue = strategy.GetInternalizedUnitRole(secondRole);
            }
            else
            {
                if (this.roleType.ObjectType is IUnit)
                {
                    secondValue = this.roleType.Normalize(this.second);
                }
            }

            var comparable = strategy.GetInternalizedUnitRole(this.roleType) as IComparable;

            if (comparable == null)
            {
                return ThreeValuedLogic.Unknown;
            }

            return (comparable.CompareTo(firstValue) >= 0 && comparable.CompareTo(secondValue) <= 0)
                       ? ThreeValuedLogic.True
                       : ThreeValuedLogic.False;
        }
    }
}
