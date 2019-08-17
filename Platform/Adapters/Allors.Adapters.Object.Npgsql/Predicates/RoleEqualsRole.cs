// <copyright file="RoleEqualsRole.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the AllorsPredicateRoleEqualsRoleSql type.
// </summary>

namespace Allors.Adapters.Object.Npgsql
{
    using System;

    using Allors.Meta;
    using Adapters;

    internal sealed class RoleEqualsRole : Predicate
    {
        private readonly IRoleType equalsRole;
        private readonly IRoleType role;

        internal RoleEqualsRole(ExtentFiltered extent, IRoleType role, IRoleType equalsRole)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleEquals(role, equalsRole);
            this.role = role;
            this.equalsRole = equalsRole;
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            if (this.role.ObjectType.IsUnit && this.equalsRole.ObjectType.IsUnit)
            {
                statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.role.RelationType] + "=" + alias + "." + schema.ColumnNameByRelationType[this.equalsRole.RelationType]);
            }
            else if (((IComposite)this.role.ObjectType).ExistExclusiveClass && ((IComposite)this.equalsRole.ObjectType).ExistExclusiveClass)
            {
                statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.role.RelationType] + "=" + alias + "." + schema.ColumnNameByRelationType[this.equalsRole.RelationType]);
            }
            else
            {
                throw new NotImplementedException();
            }

            return this.Include;
        }

        internal override void Setup(ExtentStatement statement)
        {
            statement.UseRole(this.role);
            statement.UseRole(this.equalsRole);
        }
    }
}
