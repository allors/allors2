//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsPredicateAndSql.cs" company="Allors bvba">
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
// <summary>Defines the AllorsPredicateAndSql type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Relation.SQLite
{
    using Allors.Meta;

    internal sealed class AllorsPredicateAndSql : AllorsPredicateCompositeSql
    {
        public AllorsPredicateAndSql(AllorsExtentFilteredSql extent) : base(extent)
        {
        }

        internal override bool BuildWhere(AllorsExtentFilteredSql extent, Mapping mapping, AllorsExtentStatementSql statement, IObjectType type, string alias)
        {
            if (this.Include)
            {
                var root = extent.Filter.Equals(this);
                statement.Append(root ? " AND " : "(");

                var atLeastOneChildIncluded = false;
                foreach (var filter in this.filters)
                {
                    if (atLeastOneChildIncluded)
                    {
                        statement.Append(" AND ");
                    }

                    if (filter.BuildWhere(extent, mapping, statement, type, alias))
                    {
                        atLeastOneChildIncluded = true;
                    }
                }

                if (!root)
                {
                    statement.Append(")");
                }

                return atLeastOneChildIncluded;
            }

            return false;
        }
    }
}