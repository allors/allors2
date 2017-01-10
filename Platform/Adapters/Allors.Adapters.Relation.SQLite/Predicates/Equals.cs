//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsPredicateRoleCompositeEqualsSql.cs" company="Allors bvba">
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
// <summary>Defines the AllorsPredicateRoleCompositeEqualsSql type.</summary>
//-------------------------------------------------------------------------------------------------

using Allors;

namespace Allors.Adapters.Relation.SQLite
{
    using Allors.Meta;
    using Adapters;

    internal sealed class AllorsPredicateEqualsSql : AllorsPredicateSql
    {
        private readonly IObject obj;

        internal AllorsPredicateEqualsSql(AllorsExtentFilteredSql extent, IObject obj)
        {
            PredicateAssertions.ValidateEquals(obj);
            this.obj = obj;
        }

        internal override void Setup(AllorsExtentFilteredSql extent, AllorsExtentStatementSql statement)
        {
        }

        internal override bool BuildWhere(AllorsExtentFilteredSql extent, Mapping mapping, AllorsExtentStatementSql statement, IObjectType type, string alias)
        {
            statement.Append(" (" + alias + "." + Mapping.ColumnNameForObject + "=" + statement.AddParameter(obj) + ") ");
            return Include;
        }

    }
}