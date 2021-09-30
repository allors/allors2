// <copyright file="InstanceOf.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using Adapters;
    using Meta;

    internal sealed class InstanceOf : Predicate
    {
        private readonly IObjectType[] instanceClasses;

        internal InstanceOf(IObjectType instanceType, IObjectType[] instanceClasses)
        {
            PredicateAssertions.ValidateInstanceof(instanceType);
            this.instanceClasses = instanceClasses;
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            if (this.instanceClasses.Length == 1)
            {
                statement.Append(alias + "." + Mapping.ColumnNameForClass + "=" + statement.AddParameter(this.instanceClasses[0].Id) + " ");
            }
            else if (this.instanceClasses.Length > 1)
            {
                statement.Append(" ( ");
                for (var i = 0; i < this.instanceClasses.Length; i++)
                {
                    statement.Append(alias + "." + Mapping.ColumnNameForClass + "=" + statement.AddParameter(this.instanceClasses[i].Id));
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
        }
    }
}
