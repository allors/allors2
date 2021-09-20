// <copyright file="ExtentSort.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Npgsql
{
    using Allors.Meta;

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
