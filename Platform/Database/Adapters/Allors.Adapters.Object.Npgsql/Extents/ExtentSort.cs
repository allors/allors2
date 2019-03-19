// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtentSort.cs" company="Allors bvba">
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

namespace Allors.Adapters.Object.Npgsql
{
    using Meta;

    internal class ExtentSort
    {
        private readonly SortDirection direction;
        private readonly IRoleType roleType;
        private readonly Session session;
        private ExtentSort subSorter;

        internal ExtentSort(Session session, IRoleType roleType, SortDirection direction)
        {
            this.session = session;
            this.roleType = roleType;
            this.direction = direction;
        }

        internal void AddSort(IRoleType subSortIRoleType, SortDirection subSortDirection)
        {
            if (this.subSorter == null)
            {
                this.subSorter = new ExtentSort(this.session, subSortIRoleType, subSortDirection);
            }
            else
            {
                this.subSorter.AddSort(subSortIRoleType, subSortDirection);
            }
        }

        internal void BuildOrder(ExtentStatement statement)
        {
            statement.Append(
                statement.Sorter.Equals(this)
                    ? $" ORDER BY {statement.Mapping.ColumnNameByRelationType[this.roleType.RelationType]}"
                    : $" , {statement.Mapping.ColumnNameByRelationType[this.roleType.RelationType]}");

            statement.Append(this.direction == SortDirection.Ascending ? " ASC NULLS FIRST " : " DESC NULLS LAST ");

            this.subSorter?.BuildOrder(statement);

            if (this.direction == SortDirection.Ascending)
            {
                this.AddAscendingAppendix(statement);
            }
            else
            {
                this.AddDescendingAppendix(statement);
            }
        }

        internal void BuildOrder(ExtentStatement statement, string alias)
        {
            statement.Append(
                statement.Sorter.Equals(this)
                    ? $" ORDER BY {alias}.{statement.Mapping.ColumnNameByRelationType[this.roleType.RelationType]}"
                    : $" , {alias}.{statement.Mapping.ColumnNameByRelationType[this.roleType.RelationType]}");

            if (this.direction == SortDirection.Ascending)
            {
                statement.Append(" ASC NULLS FIRST ");
                this.AddAscendingAppendix(statement);
            }
            else
            {
                statement.Append(" DESC NULLS LAST ");
                this.AddDescendingAppendix(statement);
            }

            this.subSorter?.BuildOrder(statement, alias);
        }

        internal void BuildSelect(ExtentStatement statement)
        {
            statement.Append($" , {statement.Mapping.ColumnNameByRelationType[this.roleType.RelationType]} ");
            this.subSorter?.BuildSelect(statement);
        }

        internal void BuildSelect(ExtentStatement statement, string alias)
        {
            statement.Append($" , {alias}.{statement.Mapping.ColumnNameByRelationType[this.roleType.RelationType]} ");
            this.subSorter?.BuildSelect(statement, alias);
        }

        private void AddAscendingAppendix(ExtentStatement statement)
        {
            var sortAppendix = this.session.Database.AscendingSortAppendix;
            if (sortAppendix != null)
            {
                statement.Append(sortAppendix + " ");
            }
        }

        private void AddDescendingAppendix(ExtentStatement statement)
        {
            var sortAppendix = this.session.Database.DescendingSortAppendix;
            if (sortAppendix != null)
            {
                statement.Append(sortAppendix + " ");
            }
        }
    }
}