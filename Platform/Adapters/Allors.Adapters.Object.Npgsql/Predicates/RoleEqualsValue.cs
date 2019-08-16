
// <copyright file="RoleEqualsValue.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Object.Npgsql
{
    using System;

    using Allors.Meta;
    using Adapters;

    internal sealed class RoleEqualsValue : Predicate
    {
        private readonly object obj;
        private readonly IRoleType roleType;

        internal RoleEqualsValue(ExtentFiltered extent, IRoleType roleType, object obj)
        {
            extent.CheckRole(roleType);
            PredicateAssertions.ValidateRoleEquals(roleType, obj);
            this.roleType = roleType;
            if (obj is Enum)
            {
                if (((IUnit)roleType.ObjectType).IsInteger)
                {
                    this.obj = (Int32)obj;
                }
                else
                {
                    throw new Exception("Role Object Type " + roleType.ObjectType.Name + " doesn't support enumerations.");
                }
            }
            else
            {
                this.obj = roleType.ObjectType.IsUnit ? roleType.Normalize(obj) : obj;
            }
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            if (this.roleType.ObjectType.IsUnit)
            {
                statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.roleType.RelationType] + "=" + statement.AddParameter(this.obj));
            }
            else
            {
                var allorsObject = (IObject)this.obj;

                if (this.roleType.RelationType.ExistExclusiveClasses)
                {
                    statement.Append(" (" + alias + "." + schema.ColumnNameByRelationType[this.roleType.RelationType] + " IS NOT NULL AND ");
                    statement.Append(" " + alias + "." + schema.ColumnNameByRelationType[this.roleType.RelationType] + "=" + allorsObject.Strategy.ObjectId + ")");
                }
                else
                {
                    statement.Append(" (" + this.roleType.SingularFullName + "_R." + Mapping.ColumnNameForRole + " IS NOT NULL AND ");
                    statement.Append(" " + this.roleType.SingularFullName + "_R." + Mapping.ColumnNameForRole + "=" + allorsObject.Strategy.ObjectId + ")");
                }
            }

            return this.Include;
        }

        internal override void Setup(ExtentStatement statement) => statement.UseRole(this.roleType);
    }
}
