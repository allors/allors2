// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleEqualsValue.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Object.SqlClient
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
                    this.obj = obj;
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

        internal override void Setup(ExtentStatement statement)
        {
            statement.UseRole(this.roleType);
        }
    }
}