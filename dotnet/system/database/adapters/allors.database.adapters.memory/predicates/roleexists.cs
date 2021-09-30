// <copyright file="RoleExists.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using Adapters;
    using Meta;

    internal sealed class RoleExists : Predicate
    {
        private readonly IRoleType roleType;

        internal RoleExists(ExtentFiltered extent, IRoleType roleType)
        {
            extent.CheckForRoleType(roleType);
            PredicateAssertions.ValidateRoleExists(roleType);

            this.roleType = roleType;
        }

        internal override ThreeValuedLogic Evaluate(Strategy strategy) => strategy.ExistRole(this.roleType) ? ThreeValuedLogic.True : ThreeValuedLogic.False;
    }
}
