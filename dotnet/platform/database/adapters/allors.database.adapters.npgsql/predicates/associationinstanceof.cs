// <copyright file="AssociationInstanceOf.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Npgsql
{
    using Adapters;
    using Allors.Meta;

    internal sealed class AssociationInstanceOf : Predicate
    {
        private readonly IAssociationType association;
        private readonly IObjectType[] instanceClasses;

        internal AssociationInstanceOf(ExtentFiltered extent, IAssociationType association, IObjectType instanceType, IObjectType[] instanceClasses)
        {
            extent.CheckAssociation(association);
            PredicateAssertions.ValidateAssociationInstanceof(association, instanceType);
            this.association = association;
            this.instanceClasses = instanceClasses;
        }

        internal override bool BuildWhere(ExtentStatement statement, string alias)
        {
            var schema = statement.Mapping;
            if (this.instanceClasses.Length == 1)
            {
                statement.Append(" (" + statement.GetJoinName(this.association) + "." + Mapping.ColumnNameForClass + " IS NOT NULL AND ");
                statement.Append(" " + statement.GetJoinName(this.association) + "." + Mapping.ColumnNameForClass + "=" + statement.AddParameter(this.instanceClasses[0].Id) + ") ");
            }
            else if (this.instanceClasses.Length > 1)
            {
                statement.Append(" ( ");
                for (var i = 0; i < this.instanceClasses.Length; i++)
                {
                    statement.Append(" (" + statement.GetJoinName(this.association) + "." + Mapping.ColumnNameForClass + " IS NOT NULL AND ");
                    statement.Append(" " + statement.GetJoinName(this.association) + "." + Mapping.ColumnNameForClass + "=" + statement.AddParameter(this.instanceClasses[i].Id) + ")");
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
            statement.UseAssociation(this.association);
            statement.UseAssociationInstance(this.association);
        }
    }
}
