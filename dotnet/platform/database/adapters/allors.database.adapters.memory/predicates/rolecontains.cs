// <copyright file="RoleContains.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using Adapters;
    using Allors.Meta;

    internal sealed class RoleContains : Predicate
    {
        private readonly IRoleType roleType;
        private readonly IObject containedObject;

        internal RoleContains(ExtentFiltered extent, IRoleType roleType, IObject containedObject)
        {
            extent.CheckForRoleType(roleType);
            PredicateAssertions.ValidateRoleContains(roleType, containedObject);

            this.roleType = roleType;
            this.containedObject = containedObject;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy)
        {
            var roles = strategy.GetCompositeRoles(this.roleType.RelationType);
            if (roles != null)
            {
                return roles.Contains(this.containedObject) ? ThreeValuedLogic.True : ThreeValuedLogic.False;
            }

            return ThreeValuedLogic.False;
        }
    }
}
