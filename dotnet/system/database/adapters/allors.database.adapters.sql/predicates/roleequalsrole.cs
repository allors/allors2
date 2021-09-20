// <copyright file="RoleEqualsRole.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the AllorsPredicateRoleEqualsRoleSql type.
// </summary>

namespace Allors.Database.Adapters.Sql
{
    using System;
    using Adapters;
    using Meta;

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

                if (this.role.ObjectType.Tag == UnitTags.String)
                {
                    statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.role.RelationType] + $" {schema.StringCollation} =" + alias + "." + schema.ColumnNameByRelationType[this.equalsRole.RelationType] + $" {schema.StringCollation}");
                }
                else
                {
                    statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.role.RelationType] + "=" + alias + "." + schema.ColumnNameByRelationType[this.equalsRole.RelationType]);
                }
            }
            else if (((IComposite)this.role.ObjectType).ExistExclusiveDatabaseClass && ((IComposite)this.equalsRole.ObjectType).ExistExclusiveDatabaseClass)
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
