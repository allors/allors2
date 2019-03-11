// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtentSort.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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

namespace Allors.Adapters.Database.Sql
{
    using Allors.Adapters.Database.Npgsql;

    using Meta;

    public class ExtentSort
    {
        private readonly SortDirection direction;
        private readonly IRoleType roleType;
        private readonly DatabaseSession session;
        private ExtentSort subSorter;

        public ExtentSort(DatabaseSession session, IRoleType roleType, SortDirection direction)
        {
            this.session = session;
            this.roleType = roleType;
            this.direction = direction;
        }

        public void AddSort(IRoleType subSortRoleType, SortDirection subSortDirection)
        {
            if (this.subSorter == null)
            {
                this.subSorter = new ExtentSort(this.session, subSortRoleType, subSortDirection);
            }
            else
            {
                this.subSorter.AddSort(subSortRoleType, subSortDirection);
            }
        }

        public void BuildOrder(ExtentStatement statement)
        {
            if (statement.Sorter.Equals(this))
            {
                statement.Append(" ORDER BY " + statement.Schema.Column(this.roleType));
            }
            else
            {
                statement.Append(" , " + statement.Schema.Column(this.roleType));
            }

            statement.Append(this.direction == SortDirection.Ascending ? " ASC " : " DESC ");

            if (this.subSorter != null)
            {
                this.subSorter.BuildOrder(statement);
            }

            if (this.direction == SortDirection.Ascending)
            {
                this.AddAscendingAppendix(statement);
            }
            else
            {
                this.AddDescendingAppendix(statement);
            }
        }

        public void BuildOrder(ExtentStatement statement, string alias)
        {
            if (statement.Sorter.Equals(this))
            {
                statement.Append(" ORDER BY " + alias + "." + statement.Schema.Column(this.roleType));
            }
            else
            {
                statement.Append(" , " + alias + "." + statement.Schema.Column(this.roleType));
            }

            if (this.direction == SortDirection.Ascending)
            {
                statement.Append(" ASC ");
                this.AddAscendingAppendix(statement);
            }
            else
            {
                statement.Append(" DESC ");
                this.AddDescendingAppendix(statement);
            }

            if (this.subSorter != null)
            {
                this.subSorter.BuildOrder(statement, alias);
            }
        }

        public void BuildSelect(ExtentStatement statement)
        {
            statement.Append(" , " + statement.Schema.Column(this.roleType) + " ");
            if (this.subSorter != null)
            {
                this.subSorter.BuildSelect(statement);
            }
        }

        public void BuildSelect(ExtentStatement statement, string alias)
        {
            statement.Append(" , " + alias + "." + statement.Schema.Column(this.roleType) + " ");
            if (this.subSorter != null)
            {
                this.subSorter.BuildSelect(statement, alias);
            }
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