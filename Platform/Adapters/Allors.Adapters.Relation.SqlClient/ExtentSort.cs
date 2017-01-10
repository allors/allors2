//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsExtentSortSql.cs" company="Allors bvba">
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
// <summary>Defines the AllorsExtentSortSql type.</summary>
//-------------------------------------------------------------------------------------------------

using Allors;

namespace Allors.Adapters.Relation.SqlClient
{
    using Allors.Meta;

    internal class AllorsExtentSortSql
    {
        private readonly SortDirection sortDirection;
        private readonly IRoleType roleType;
        private readonly Session session;
        private AllorsExtentSortSql subSorter;

        internal AllorsExtentSortSql(Session session, IRoleType roleType, SortDirection sortDirection)
        {
            this.session = session;
            this.roleType = roleType;
            this.sortDirection = sortDirection;
        }

        internal void AddSort(IRoleType sortRoleType, SortDirection sortDirection)
        {
            if (this.subSorter == null)
            {
                this.subSorter = new AllorsExtentSortSql(this.session, sortRoleType, sortDirection);
            }
            else
            {
                this.subSorter.AddSort(sortRoleType, sortDirection);
            }
        }

        internal void BuildOrder(AllorsExtentSortSql sorter, Mapping mapping, AllorsExtentStatementSql statement)
        {
            if (sorter.Equals(this))
            {
                statement.Append(" ORDER BY " + this.roleType.SingularFullName + "_R" + ".R");
            }
            else
            {
                statement.Append(" , " + this.roleType.SingularFullName + "_R" + ".R");
            }

            statement.Append(this.sortDirection == SortDirection.Ascending ? " ASC " : " DESC ");

            if (this.subSorter != null)
            {
                this.subSorter.BuildOrder(sorter, mapping, statement);
            }
        }

        internal void BuildSelect(AllorsExtentFilteredSql extent, Mapping mapping, AllorsExtentStatementSql statement)
        {
            statement.Append(" , " + this.roleType.SingularFullName + "_R." + Mapping.ColumnNameForRole + " ");
            if (this.subSorter != null)
            {
                this.subSorter.BuildSelect(extent, mapping, statement);
            }
        }

        internal void Setup(AllorsExtentFilteredSql extent, AllorsExtentStatementSql statement)
        {
            statement.UseRole(this.roleType);
            if (this.subSorter != null)
            {
                this.subSorter.Setup(extent, statement);
            }
        }
    }
}