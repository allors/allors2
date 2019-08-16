
// <copyright file="RoleLessThan.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Memory
{
    using System;
    using Allors.Meta;
    using Adapters;

    internal sealed class RoleLessThan : Predicate
    {
        private readonly ExtentFiltered extent;
        private readonly IRoleType roleType;
        private readonly object compare;

        internal RoleLessThan(ExtentFiltered extent, IRoleType roleType, object compare)
        {
            extent.CheckForRoleType(roleType);
            PredicateAssertions.ValidateRoleLessThan(roleType, compare);

            this.extent = extent;
            this.roleType = roleType;
            this.compare = compare;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            var compareValue = this.compare;

            var compareRole = this.compare as IRoleType;
            if (compareRole != null)
            {
                compareValue = strategy.GetInternalizedUnitRole(compareRole);
            }
            else
            {
                if (this.roleType.ObjectType is IUnit)
                {
                    compareValue = this.roleType.Normalize(this.compare);
                }
            }

            var comparable = strategy.GetInternalizedUnitRole(this.roleType) as IComparable;

            if (comparable == null)
            {
                return ThreeValuedLogic.Unknown;
            }

            return comparable.CompareTo(compareValue) < 0
                       ? ThreeValuedLogic.True
                       : ThreeValuedLogic.False;
        }
    }
}
