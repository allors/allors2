// <copyright file="AndPredicate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    internal sealed class AndPredicate : CompositePredicate
    {
        internal AndPredicate(ExtentFiltered extent) : base(extent)
        {
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            if (this.Include)
            {
                var root = this.Extent.Filter == null || this.Extent.Filter.Equals(this);
                if (root)
                {
                    var wherePresent = !this.Extent.ObjectType.ExistExclusiveDatabaseClass;
                    statement.Append(wherePresent ? " AND " : " WHERE ");
                }
                else
                {
                    statement.Append("(");
                }

                var atLeastOneChildIncluded = false;
                foreach (var filter in this.Filters)
                {
                    if (atLeastOneChildIncluded)
                    {
                        statement.Append(" AND ");
                    }

                    if (filter.BuildWhere(statement, alias))
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
