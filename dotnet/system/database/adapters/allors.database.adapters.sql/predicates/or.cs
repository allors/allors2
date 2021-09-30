// <copyright file="Or.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    internal sealed class Or : CompositePredicate
    {
        internal Or(ExtentFiltered extent) : base(extent)
        {
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            if (this.Include)
            {
                var atLeastOneChildIncluded = false;
                statement.Append("(");
                foreach (var filter in this.Filters)
                {
                    if (atLeastOneChildIncluded)
                    {
                        statement.Append(" OR ");
                    }

                    if (filter.BuildWhere(statement, alias))
                    {
                        atLeastOneChildIncluded = true;
                    }
                }

                statement.Append(")");
                return atLeastOneChildIncluded;
            }

            return false;
        }
    }
}
