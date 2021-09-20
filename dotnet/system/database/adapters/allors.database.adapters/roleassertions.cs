// <copyright file="RoleAssertions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters
{
    using System;
    using Meta;

    public static class RoleAssertions
    {
        public static void UnitRoleChecks(this IRoleType roleType, IStrategy strategy)
        {
            if (!roleType.AssociationType.ObjectType.IsAssignableFrom(strategy.Class))
            {
                throw new ArgumentException(strategy.Class + " is not a valid association object type for " + roleType + ".");
            }

            if (roleType.ObjectType is IComposite)
            {
                throw new ArgumentException(roleType.ObjectType + " on roleType " + roleType + " is not a unit type.");
            }
        }

        public static void CompositeRoleChecks(this IRoleType roleType, IStrategy strategy) => CompositeSharedChecks(roleType, strategy, null);

        public static void CompositeRoleChecks(this IRoleType roleType, IStrategy strategy, IObject role)
        {
            CompositeSharedChecks(roleType, strategy, role);
            if (!roleType.IsOne)
            {
                throw new ArgumentException("RelationType " + roleType + " has multiplicity many.");
            }
        }

        public static void CompositeRolesChecks(this IRoleType roleType, IStrategy strategy)
        {
            CompositeSharedChecks(roleType, strategy, null);
            if (!roleType.IsMany)
            {
                throw new ArgumentException("RelationType " + roleType + " has multiplicity one.");
            }
        }

        public static void CompositeRolesChecks(this IRoleType roleType, IStrategy strategy, IObject role)
        {
            CompositeSharedChecks(roleType, strategy, role);
            if (!roleType.IsMany)
            {
                throw new ArgumentException("RelationType " + roleType + " has multiplicity one.");
            }
        }

        private static void CompositeSharedChecks(this IRoleType roleType, IStrategy strategy, IObject role)
        {
            if (!roleType.AssociationType.ObjectType.IsAssignableFrom(strategy.Class))
            {
                throw new ArgumentException(strategy.Class + " has no roleType with role " + roleType + ".");
            }

            if (role != null)
            {
                if (!strategy.Transaction.Equals(role.Strategy.Transaction))
                {
                    throw new ArgumentException(role + " is from different transaction");
                }

                if (role.Strategy.IsDeleted)
                {
                    throw new ArgumentException(roleType + " on object " + strategy + " is removed.");
                }

                if (!(roleType.ObjectType is IComposite compositeType))
                {
                    throw new ArgumentException(role + " has no CompositeType");
                }

                if (!compositeType.IsAssignableFrom(role.Strategy.Class))
                {
                    throw new ArgumentException(role.Strategy.Class + " is not compatible with type " + roleType.ObjectType + " of role " + roleType + ".");
                }
            }
        }
    }
}
