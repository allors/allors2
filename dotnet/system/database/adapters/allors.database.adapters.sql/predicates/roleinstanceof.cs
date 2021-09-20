// <copyright file="RoleInstanceof.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using Adapters;
    using Meta;

    internal sealed class RoleInstanceof : Predicate
    {
        private readonly IObjectType[] instanceClasses;
        private readonly IRoleType role;

        internal RoleInstanceof(ExtentFiltered extent, IRoleType role, IObjectType instanceType, IObjectType[] instanceClasses)
        {
            extent.CheckRole(role);
            PredicateAssertions.ValidateRoleInstanceOf(role, instanceType);
            this.role = role;
            this.instanceClasses = instanceClasses;
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            if (this.instanceClasses.Length == 1)
            {
                statement.Append(" (" + statement.GetJoinName(this.role) + "." + Mapping.ColumnNameForClass + " IS NOT NULL AND ");
                statement.Append(" " + statement.GetJoinName(this.role) + "." + Mapping.ColumnNameForClass + "=" + statement.AddParameter(this.instanceClasses[0].Id) + ")");
            }
            else if (this.instanceClasses.Length > 1)
            {
                statement.Append(" ( ");
                for (var i = 0; i < this.instanceClasses.Length; i++)
                {
                    statement.Append(" (" + statement.GetJoinName(this.role) + "." + Mapping.ColumnNameForClass + " IS NOT NULL AND ");
                    statement.Append(" " + statement.GetJoinName(this.role) + "." + Mapping.ColumnNameForClass + "=" + statement.AddParameter(this.instanceClasses[i].Id) + ")");
                    if (i < this.instanceClasses.Length - 1)
                    {
                        statement.Append(" OR ");
                    }
                }

                statement.Append(" ) ");
            }

            return this.Include;
        }

        internal override void Setup(ExtentStatement statement)
        {
            statement.UseRole(this.role);
            statement.UseRoleInstance(this.role);
        }
    }
}
