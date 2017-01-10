//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsPredicateRoleUnitEqualsValueSql.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
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
// <summary>Defines the AllorsPredicateRoleUnitEqualsValueSql type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Relation.SQLite
{
    using System;

    using Allors.Meta;
    using Adapters;

    internal sealed class AllorsPredicateRoleUnitEqualsValueSql : AllorsPredicateSql
    {
        private readonly object obj;
        private readonly IRoleType roleType;

        internal AllorsPredicateRoleUnitEqualsValueSql(AllorsExtentFilteredSql extent, IRoleType roleType, Object obj)
        {
            extent.CheckRole(roleType);
            PredicateAssertions.ValidateRoleEquals(roleType, obj);
            this.roleType = roleType;
            if (obj is Enum)
            {
                var unitType = roleType.ObjectType as IUnit;
                if (unitType != null && unitType.IsInteger)
                {
                    this.obj = (int)obj;
                }
                else
                {
                    throw new Exception("Role Object Type " + roleType.ObjectType.SingularName + " doesn't support enumerations.");
                }
            }
            else
            {
                this.obj = roleType.ObjectType is IUnit ? roleType.Normalize(obj) : obj;
            }
        }

        internal override bool BuildWhere(AllorsExtentFilteredSql extent, Mapping mapping, AllorsExtentStatementSql statement, IObjectType type, string alias)
        {
            statement.Append(" " + this.roleType.SingularFullName + "_R." + Mapping.ColumnNameForRole + "=" + statement.AddParameter(this.obj));
            return this.Include;
        }

        internal override void Setup(AllorsExtentFilteredSql extent, AllorsExtentStatementSql statement)
        {
            statement.UseRole(this.roleType);
        }
    }
}